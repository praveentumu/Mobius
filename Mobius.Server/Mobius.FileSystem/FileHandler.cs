using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.CodeDom.Compiler;
using Mobius.CoreLibrary;

namespace Mobius.FileSystem
{
    public class FileHandler
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="docbyteData"></param>
        /// <param name="path"></param>
        /// <returns></returns>
        public static bool SaveDocument(byte[] docbyteData, string path)
        {
            bool bDocsaved = false;
            using (FileStream fileStream = new FileStream(path, FileMode.OpenOrCreate, FileAccess.Write))
            {
                BinaryWriter bw = new BinaryWriter(fileStream);
                bw.Write(docbyteData);
                fileStream.Close();
                bw.Close();
                bDocsaved = true;
            }
            return bDocsaved;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="DocumentID"></param>
        /// <param name="filepathLocation"></param>
        /// <returns></returns>
        public static byte[] LoadDocument(string DocumentID, string filepathLocation)
        {
            byte[] DocBytes = null;
            string filePath = filepathLocation + "\\" + DocumentID + ".xml";
            using (FileStream fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read))
            {
                BinaryReader binaryReader = new BinaryReader(fileStream);
                long numBytes = new FileInfo(filePath).Length;
                DocBytes = binaryReader.ReadBytes((int)numBytes);
                fileStream.Close();
                binaryReader.Close();
            }
            return DocBytes;
        }

        /// <summary>
        /// This method will create new folder, if not exists
        /// </summary>
        /// <param name="path"> A string specifying the path on which to create the DirectoryInfo</param>
        /// <returns></returns>
        public static bool CreateFolder(string path)
        {
            //Check path is exists
            DirectoryInfo directoryInfo = new DirectoryInfo(path);

            if (!directoryInfo.Exists)
            {
                directoryInfo = Directory.CreateDirectory(path);
            }

            return directoryInfo.Exists;
        }

        public static bool CheckDocumentExists(string DocumentID, string filepathLocation)
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
