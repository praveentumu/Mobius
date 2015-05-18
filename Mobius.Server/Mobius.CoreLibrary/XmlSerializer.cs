
namespace Mobius.CoreLibrary
{
    using System;
    using System.IO;
    using System.Security.Permissions;
    using System.Text;
    using System.Xml;
    using System.Xml.Serialization;

    /// <summary>
    /// Settings used by the serializer
    /// </summary>
    public enum XmlTestSerializerSetting
    {
        /// <summary>
        /// Instruct the serializer to Omit the Encoding Identifier
        /// </summary>
        OmitEncodingIdentifier,

        /// <summary>
        /// Instruct the Serializer to omit the XML Declaration
        /// </summary>
        OmitXmlDeclaration,

        /// <summary>
        /// Instruct the Serializer to omit the Byte Order Mark (BOM)
        /// </summary>
        OmitBom,

        /// <summary>
        /// Instruct the serializer to format the output by indenting
        /// </summary>
        Indent
    }


    /// <summary>
    /// Generalized class to support XML serialization
    /// The Public Read/Only Property [String XML] will return the serialized object as an XML string 
    /// The default constructor will return Indented UTF-8 text with no XML declaration
    /// This is typically useful for testing the serialization and deserialization of objects under test or development
    /// </summary>
    public sealed class XmlSerializerHelper : IDisposable
    {

        private MemoryStream            ms                     = null;
        private XmlSerializerNamespaces nameSpaces             = null;
        private XmlWriterSettings       writerSettings         = null;
        private XmlWriter               writer                 = null;
        private XmlSerializer           serializer             = null;
        private Encoding                encoder                = null;
        private bool                    emitEncodingIdentifier = true;
        private bool                    removeBom              = false;


        /// <summary>
        /// The default Constructor
        /// </summary>
        /// <param name="objectType">The type of the object to be serialized/deserailized</param>
        /// <param name="target">the non null object to be used</param>
        /// <exception cref="ArgumentException">Returned if either parameter is null</exception> 
        /// <exception cref="ApplicationException">Returned on any other exception and wraps the original exception</exception> 
        internal XmlSerializerHelper(Type objectType, object target)
        {

            nameSpaces             = null;
            removeBom              = true;
            emitEncodingIdentifier = false;


            if (null == objectType)
                throw new ArgumentException("objectType Parameter is NULL");

            if (null == target)
                throw new ArgumentException("target Parameter is NULL");

            
            encoder = new UTF8Encoding(false, false);
            

            //
            // create the XML Writer Settings using the settings specifiec by the caller
            // 
            writerSettings                    = new XmlWriterSettings();
            writerSettings.ConformanceLevel   = ConformanceLevel.Document;
            writerSettings.OmitXmlDeclaration = true;
            writerSettings.Indent             = true;
            writerSettings.IndentChars        = " ";
            writerSettings.Encoding           = Encoding.UTF8;
            writerSettings.NewLineHandling    = NewLineHandling.Entitize;

            //
            // create the memory stream, writer and serializer
            // based on the user's settings
            // 
            ms         = new MemoryStream();
            writer     = XmlWriter.Create(ms, writerSettings);
            serializer = new XmlSerializer(objectType);

            try
            {

                //
                // serialize the target object into the memory stream 
                // using the writer created above.
                // 
                serializer.Serialize(writer, target);

            }
            catch (Exception exception)
            {
                //
                // if there was any failure - throw an application exception
                // 
                throw new ApplicationException("Could not serialize the target object", exception);
            }


        }
        
        /// <summary>
        /// Constructs the string from the array of bytes in unicode format
        /// </summary>
        /// <param name="p_sXML">String to be converted as an array of bytes</param>
        /// <returns>Byte Array</returns>
        private static Byte[] StringToUnicodeEncodingByteArray(string p_sXML)
        {
            UnicodeEncoding encoding = new UnicodeEncoding();
            Byte[] byteArray = encoding.GetBytes(p_sXML);
            return byteArray;
        }

               

        public static String SerializeObject(Object p_Object, Encoding encoding = null)
        {
            String sXMLRet = string.Empty;

            MemoryStream memoryStream = null;
            XmlWriterSettings xset = null;
            XmlSerializer xs = null;
            XmlWriter xmlWriter = null;
            try
            {
                if (p_Object != null)
                {

                    encoding  = encoding == null ? Encoding.Unicode : encoding;                    
                    memoryStream = new MemoryStream();
                    xset = new XmlWriterSettings();
                    xset.Indent = true;
                    xset.Encoding = encoding;
                    xs = new XmlSerializer(p_Object.GetType());
                    xmlWriter = XmlWriter.Create(memoryStream, xset);
                    xs.Serialize(xmlWriter, p_Object);
                    string s = xmlWriter.ToString();
                    if (encoding == Encoding.Unicode)
                    {
                        UnicodeEncoding unicodeEncoding = new UnicodeEncoding();
                        sXMLRet = unicodeEncoding.GetString(memoryStream.ToArray());
                    }
                    else
                    {
                        sXMLRet = Encoding.UTF8.GetString(memoryStream.ToArray());                        
                    }
                }
            }
            catch (Exception exception)
            {
                //
                // if there was any failure - throw an application exception
                // 
                throw new ApplicationException("Could not serialize the target object", exception);
            }
            finally
            {
                xmlWriter = null;
                xs = null;
                xset = null;
                memoryStream = null;
            }
            return sXMLRet;
        }

        /// <summary>
        /// Converts an object data into its XML string representation.
        /// </summary>
        /// <param name="p_Object">Object to convert</param>
        /// <returns>XML string</returns>
        /// <remarks >Kind-a like ToXMLString() method</remarks>
        public static byte[] ObjectToByteArray(Object p_Object)
        {
            MemoryStream memoryStream = null;
            XmlWriterSettings xset = null;
            XmlSerializer xs = null;
            XmlWriter xmlWriter = null;
            try
            {
                if (p_Object != null)
                {
                    memoryStream = new MemoryStream();
                    xset = new XmlWriterSettings();
                    xset.Indent = true;
                    xset.Encoding = Encoding.Unicode;
                    xs = new XmlSerializer(p_Object.GetType());
                    xmlWriter = XmlWriter.Create(memoryStream, xset);
                    xs.Serialize(xmlWriter, p_Object);
                    return memoryStream.ToArray();
                }
            }
            catch (Exception exception)
            {
                //
                // if there was any failure - throw an application exception
                // 
                throw new ApplicationException("Could not serialize the target object", exception);
            }
            finally
            {
                xmlWriter = null;
                xs = null;
                xset = null;
                memoryStream = null;
            }
            return memoryStream.ToArray();
        }


        public static XmlElement ToXMLElement(object p_objSCClass)
        {
            XmlElement retElement = null;

            try
            {
                if (p_objSCClass != null)
                {
                    XmlDocument xDoc = new XmlDocument();
                    string sXMLString = XmlSerializerHelper.SerializeObject(p_objSCClass).Trim();
                    if (!string.IsNullOrEmpty(sXMLString))
                    {
                        xDoc.LoadXml(sXMLString);
                        retElement = xDoc.DocumentElement;
                    }
                }
            }
            catch (Exception)
            {
                retElement = null;
            }

            return retElement;
        }


        ///// <summary>
        ///// Converts an object data into its XML string representation removing app-specific namespaces.
        ///// </summary>
        ///// <param name="p_Object">Object to convert</param>
        ///// <returns>XML string</returns>
        ///// <remarks >Kind-a like ToXMLString() method</remarks>
        //public static String SimpleSerializeObject(Object p_Object)
        //{
        //    String sXMLRet = string.Empty;

        //    MemoryStream memoryStream = null;
        //    XmlWriterSettings xset = null;
        //    XmlSerializer xs = null;
        //    XmlWriter xmlWriter = null;
        //    try
        //    {
        //        if (p_Object != null)
        //        {
        //            memoryStream = new MemoryStream();
        //            xset = new XmlWriterSettings();
        //            xset.Indent = true;
        //            xset.Encoding = Encoding.Unicode;

        //            xs = new XmlSerializer(p_Object.GetType());

        //            xmlWriter = XmlWriter.Create(memoryStream, xset);
        //            xs.Serialize(xmlWriter, p_Object);
        //            sXMLRet = UnicodeEncodingByteArrayToString(memoryStream.ToArray());                    

        //        }
        //    }
        //    catch (Exception e)
        //    {
        //        throw new Exception(":XMLUtils.SerializeObject()- " + e.Message, e);
        //    }
        //    finally
        //    {
        //        xmlWriter = null;
        //        xs = null;
        //        xset = null;
        //        memoryStream = null;
        //    }
        //    return sXMLRet;
        //}


        /// <summary>
        /// Converts an XML string into the given object data type.
        /// </summary>
        /// <param name="p_sXML">XML string</param>
        /// <param name="p_objType">Object data type</param>
        /// <returns>Object representation of the XML string</returns>
        public static Object DeserializeObject(string p_sXML, Type p_objType)
        {
            object objRet = null;
            XmlSerializer xs = null;
            MemoryStream memoryStream = null;
            TextReader textReader = null;
            string sXML = (p_sXML != null) ? p_sXML : string.Empty;

            try
            {
                if (!string.IsNullOrEmpty(sXML))
                {
                    xs = new XmlSerializer(p_objType);
                    memoryStream = new MemoryStream(StringToUnicodeEncodingByteArray(sXML));
                    textReader = new StreamReader(memoryStream, Encoding.Unicode);
                    objRet = xs.Deserialize(textReader);
                }

            }
            catch (Exception exception)
            {
                //
                // if there was any failure - throw an application exception
                // 
                throw new ApplicationException("Could not deserialize the target object", exception);
            }
            finally
            {
                memoryStream = null;
                xs = null;
                textReader = null;
            }

            return objRet;
        }

        /// <summary>
        /// This constuctor is used to fine tune how the XML is to be formatted
        /// 
        /// </summary>
        /// <param name="objectType">The type of the object to be serialized/deserailized</param>
        /// <param name="target">the non null object to be used</param>
        /// <param name="encoding">the type of encoding to be used UTF-8 Unicode etc.</param>
        /// <param name="nameSpaces">name spaces to be used in the schema</param>
        /// <param name="settings">a parmameter array of setting flags</param>
        /// <exception cref="ArgumentException">Returned if either parameter is null</exception> 
        /// <exception cref="ApplicationException">Returned on any other exception and wraps the original exception</exception> 
        public XmlSerializerHelper(
            Type                              objectType,
            object                            target,
            Encoding                          encoding,
            XmlSerializerNamespaces           nameSpaces,
            params XmlTestSerializerSetting[] settings)
        {

            bool indent             = false;
            bool omitXmlDeclaration = false;

            if (null == objectType)
                throw new ArgumentException("objectType Parameter is NULL");

            if (null == target)
                throw new ArgumentException("target Parameter is NULL");


            //
            // save the nameSpaces for this object
            // 
            if (null != nameSpaces)
            {

                if (nameSpaces.Count == 0)
                {
                    this.nameSpaces = new XmlSerializerNamespaces();
                    this.nameSpaces.Add("", "");
                }
                else
                {
                    this.nameSpaces = nameSpaces;
                }

            }
            else
            {
                this.nameSpaces = null;
            }


            //
            // process the settings param[] array
            // 
            if (null != settings)
            {
                foreach (XmlTestSerializerSetting setting in settings)
                {

                    if (setting == XmlTestSerializerSetting.Indent)
                    {
                        indent = true;  // the caller specified indent
                    }
                    else if (setting == XmlTestSerializerSetting.OmitBom)
                    {
                        removeBom = true;  // the caller specified removing the Byte Order Mark (BOM)
                    }
                    else if (setting == XmlTestSerializerSetting.OmitEncodingIdentifier)
                    {
                        emitEncodingIdentifier = false; // the caller specified removing the Encoding Identifier
                    }
                    else if (setting == XmlTestSerializerSetting.OmitXmlDeclaration)
                    {
                        omitXmlDeclaration = true; // the caller specified removing the Xml declaration
                    }

                }
            }


            //
            // protect against null
            //
            if (null != encoding)
            {

                //
                // build an encoder based on the callers encoding parameter
                //
                if (encoding.Equals(Encoding.ASCII))
                {
                    encoder = new ASCIIEncoding();
                }
                else if (encoding.Equals(Encoding.Unicode))
                {
                    encoder = new UnicodeEncoding(false, true, false);
                }
                else if (encoding.Equals(Encoding.UTF32))
                {
                    encoder = new UTF32Encoding(false, true, false);
                }
                else if (encoding.Equals(Encoding.UTF7))
                {
                    encoder = new UTF7Encoding(true);
                }
                else
                {
                    encoder = new UTF8Encoding(emitEncodingIdentifier, false);
                }

            }
            else
            {
                encoding = Encoding.UTF8;
                encoder  = new UTF8Encoding(emitEncodingIdentifier, false);
            }




            //
            // create the XML Writer Settings using the settings specifiec by the caller
            // 
            writerSettings                    = new XmlWriterSettings();
            writerSettings.ConformanceLevel   = ConformanceLevel.Document;
            writerSettings.OmitXmlDeclaration = omitXmlDeclaration;
            writerSettings.Indent             = indent;
            writerSettings.IndentChars        = " ";
            writerSettings.Encoding           = encoding;
            writerSettings.NewLineHandling    = NewLineHandling.Entitize;

            //
            // create the memory stream, writer and serializer
            // based on the user's settings
            // 
            ms         = new MemoryStream();
            writer     = XmlWriter.Create(ms, writerSettings);
            serializer = new XmlSerializer(objectType);

            try
            {

                //
                // serialize the target object into the memory stream 
                // using the writer created above.
                // 
                if (null != this.nameSpaces)
                {
                    serializer.Serialize(writer, target, this.nameSpaces);
                }
                else
                {
                    serializer.Serialize(writer, target);
                }

            }
            catch (Exception exception)
            {
                //
                // if there was any failure - throw an application exception
                // 
                throw new ApplicationException("Could not serialize the target object", exception);
            }


        }




        /// <summary>
        /// This method will use the object type provided in the constructor to deserialize the
        /// XML string provided in the xml parameter to create an CLR object
        /// </summary>
        /// <param name="xml">An XML string representing the type of object to be deserialized</param>
        /// <returns>an object of the type specified in the constructor - NULL on any failure</returns>
        public object Deserialize(string xml)
        {

            object       returnObject = null;
            StringReader sr           = null;

            try
            {

                //
                // create a stream object
                // to use during the deserilization process
                // 
                sr = new StringReader(xml);

                //
                // deserialize the object using the StringReader
                // 
                returnObject = serializer.Deserialize(sr);


            }
            catch
            {

                // 
                // hide all failures
                // and simply return NULL
                // 
                returnObject = null;

            }


            return (returnObject);


        }


        /// <summary>
        /// Returns an XML string formatted as indicated in the 
        /// constructor or an Empty String on any failure
        /// </summary>
        public string Xml
        {

            get
            {

                //
                // local storage for the string
                // that is to be returned
                // 
                string xml = string.Empty;

                try
                {

                    //
                    // try to get the string from the 
                    // memory stream that was created in the constructor
                    // 
                    xml = encoder.GetString(ms.GetBuffer());

                    //
                    // if the caller wants the Byte Order Mark (BOM)
                    // removed - do it here.
                    // 
                    if (removeBom)
                    {
                        xml = CutOffBom(xml);
                    }

                    //
                    // clean up any noise characters on either end of the resulting string
                    // 
                    xml = CutOffNoise(xml);

                }
                catch
                {
                    // hide any exceptions and just return an empty string
                }

                return (xml);

            }

        }


        /// <summary>
        /// private method to cutt of the Byte Order Mark
        /// </summary>
        /// <param name="xmlString">the XML to operate on</param>
        /// <returns>the XML that results from removing the BOM</returns>
        [ReflectionPermission(SecurityAction.Deny)]
        private static string CutOffBom(string xmlString)
        {
            //
            // find the start of the start of the printable characters
            // 
            int startXml = xmlString.IndexOf('<');

            //
            // if the first printable characters are somewhere other
            // than the forst byte we need to chop off everything before that point
            // because that will be the un-printable BOM bytes
            // 
            if (startXml > 0)
            {
                return (xmlString.Substring(startXml));
            }
            else
            {
                return (xmlString);
            }


        }


        /// <summary>
        /// This method will chop off everything after the closing tag
        /// usually nulls or line feed carriage returns etc.
        /// </summary>
        /// <param name="xmlString"></param>
        /// <returns></returns>
        [ReflectionPermission(SecurityAction.Deny)]
        private static string CutOffNoise(string xmlString)
        {

            //
            // find the end of the interesting text in the string
            //
            int endIndex = xmlString.LastIndexOf(">") + 1;

            //
            // chop off everything past that point
            // 
            if (endIndex > 0)
            {
                return (xmlString.Substring(0, endIndex));
            }
            else
            {
                return (xmlString);
            }


        }




        #region IDisposable Members

        /// <summary>
        /// Implimentation of IDisposable
        /// </summary>
        public void Dispose()
        {
            if (null != ms)
            {
                ms.Close();
            }
        }

        #endregion

    }

}

