using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;
using System.Xml.Schema;
using System.Xml;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

namespace FirstGenesis.Mobius.Common.DataTypes
{
    public class MSerializable
    {
        public string Serialize()
        {
            String XmlizedString = null;
            MemoryStream memoryStream = new MemoryStream();
            XmlSerializer xs = new XmlSerializer(this.GetType());
            XmlTextWriter xmlTextWriter = new XmlTextWriter(memoryStream, Encoding.UTF8);
            xs.Serialize(xmlTextWriter, this);
            memoryStream = (MemoryStream)xmlTextWriter.BaseStream;
            XmlizedString = UTF8ByteArrayToString(memoryStream.ToArray());
            return XmlizedString;
        }
        /// <summary>
        /// Deserializes xml string and creates an object from it.
        /// </summary>
        /// <param name="xmlString"></param>
        /// <returns>Deserialized object</returns>
        public object DeSerialize(string xmlString)
        {
            try
            {
                XmlSerializer xs = new XmlSerializer(this.GetType());
                MemoryStream memoryStream = new MemoryStream(StringToUTF8ByteArray(xmlString));
                XmlTextWriter xmlTextWriter = new XmlTextWriter(memoryStream, Encoding.UTF8);
                return xs.Deserialize(memoryStream);
            }
            catch
            {
                return null;
            }
        }
        public static object DeSerialize(string xmlString, Type type)
        {
            try
            {
                XmlSerializer xs = new XmlSerializer(type);
                MemoryStream memoryStream = new MemoryStream(StringToUTF8ByteArray(xmlString));
                XmlTextWriter xmlTextWriter = new XmlTextWriter(memoryStream, Encoding.UTF8);
                return xs.Deserialize(memoryStream);
            }
            catch(Exception)
            {
                return null;
            }
        }
        private static String UTF8ByteArrayToString(Byte[] characters)
        {
            UTF8Encoding encoding = new UTF8Encoding();
            String constructedString = encoding.GetString(characters);
            return (constructedString);
        }
        private static Byte[] StringToUTF8ByteArray(String pXmlString)
        {
            UTF8Encoding encoding = new UTF8Encoding();
            Byte[] byteArray = encoding.GetBytes(pXmlString);
            return byteArray;
        }
        /// <summary>
        /// This method used to remove default Namespace from serialized XML file
        /// </summary>
        /// <param name="idnFile">identification File</param>
        /// <param name="strRoot">Customize root element(for Owner,Device)</param>
        /// <returns></returns>
        public string SerializeObject(object idnFile, string strRoot)
        {
            try
            {
                String xmlString = null;
                MemoryStream memoryStream = new MemoryStream();
                XmlRootAttribute obj = new XmlRootAttribute(strRoot);
                XmlSerializerNamespaces nsSerializer = null;
                nsSerializer = new XmlSerializerNamespaces();
                nsSerializer.Add("", "");
                XmlSerializer xs = new XmlSerializer(typeof(IdnFile), obj);
                XmlTextWriter xmlTextWriter = new XmlTextWriter(memoryStream, Encoding.UTF8);
                xs.Serialize(xmlTextWriter, idnFile, nsSerializer);
                memoryStream = (MemoryStream)xmlTextWriter.BaseStream;
                xmlString = UTF8ByteArrayToString(memoryStream.ToArray());
                return xmlString;
            }
            catch(Exception)
            {
                return null;

            }

        }
    }

    [XmlRoot("HashTable", Namespace = "http://www.mobius.com/", IsNullable = true)]
    public class MyHashTable : IXmlSerializable
    {
        private Hashtable _hash = new Hashtable();
        internal static string nameSpace = "";

        public Hashtable Hash
        {
            get { return _hash; }
            set { _hash = value; }
        }

        void IXmlSerializable.WriteXml(XmlWriter writer)
        {
            IDictionaryEnumerator enumerator = _hash.GetEnumerator();
            while (enumerator.MoveNext())
            {
                writer.WriteStartElement(enumerator.Key.ToString());
                writer.WriteString(enumerator.Value.ToString());
                writer.WriteEndElement();
            }
        }

        internal static MyHashTable ReadHashTable(XmlReader r, MyHashTable myHashTable)
        {
            string key = "";
            string value = "";
            bool isHash = true;
            while (r.Read())
            {
                if (isHash)
                {
                    switch (r.NodeType)
                    {
                        case XmlNodeType.Element:
                            key = r.Name;
                            break;
                        case XmlNodeType.Text:
                            value = r.Value;
                            if (key != "")
                            {
                                myHashTable.Hash.Add(key, value);
                            }
                            key = "";
                            value = "";
                            break;
                        case XmlNodeType.EndElement:
                            key = r.Name;
                            if (key == "HashTable")
                            {
                                isHash = false;
                            }
                            break;
                    }
                }
            }
            return myHashTable;
        }

        void IXmlSerializable.ReadXml(XmlReader reader)
        {
            ReadHashTable(reader, this);
        }

        XmlSchema IXmlSerializable.GetSchema() { return null; }

    }

}