using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace FirstGenesis.Mobius.Common
{
   public class FileHandler
    {
        public FileHandler()
        {

        }
        public bool SaveDocument(byte[] docbyteData, string location)
        {
            bool bDocsaved = false;
            FileStream fs = new FileStream(location, FileMode.OpenOrCreate, FileAccess.Write);
            BinaryWriter bw = new BinaryWriter(fs);
            bw.Write(docbyteData);
            bw.Close();
            bDocsaved = true;
            return bDocsaved;
        }
        public byte[] LoadDocument(string DocumentID, string filepathLocation)
        {
            byte[] DocBytes = null;
            string filePath = filepathLocation + "\\" + DocumentID + ".xml";
            //XmlDocument x = new XmlDocument();
            //x.Load(filePath);

            //DocBytes = Encoding.Default.GetBytes(x.OuterXml);

            FileStream fs = new FileStream(filePath, FileMode.Open, FileAccess.Read);
            BinaryReader br = new BinaryReader(fs);
            long numBytes = new FileInfo(filePath).Length;
            DocBytes = br.ReadBytes((int)numBytes);
            return DocBytes;
        }

        public bool CheckDocumentExists(string DocumentID, string filepathLocation)
        {
            bool bdocExists = false;
            if (File.Exists(filepathLocation + "\\" + DocumentID + ".xml"))
            {
                bdocExists = true;
            }
            else
            {
                bdocExists = false;
            }

            return bdocExists;
        }
    }
}
