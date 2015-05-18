using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;
using System.IO;

namespace FirstGenesis.Mobius.Common.DataTypes
{
    public class USBDevice
    {
        public string SerialId;
        public List<string> DriveLetters = new List<string>();
        public Dictionary<string, string> DeviceAttributes = new Dictionary<string, string>();
        public string SelectedDrive;
        public string PatientName;

        public override string ToString()
        {
            return SelectedDrive + " (EIC for " + PatientName + ")";
        }
    }


    public class FILE_INFO
    {
        string _fileName;
        string _data;
        public string FileName
        {
            get { return _fileName; }
            set { _fileName = value; }
        }
        public string Data
        {
            get { return _data; }
            set { _data = value; }
        }
    }


    [XmlRoot(ElementName = "EICFileList", Namespace = "http://www.mobius.com")]
    public class EICDeviceFileList : MSerializable
    {
        List<FILE_INFO> _fileList = new List<FILE_INFO>();
        string _signature = "";

        public string Signature
        {
            get { return _signature; }
            set { _signature = value; }
        }
        public List<FILE_INFO> FileList
        {
            get { return _fileList; }
            set { _fileList = value; }
        }

        public static EICDeviceFileList LoadFromFile(string fileName)
        {
             // read the data from xml file;

            try
            {

                TextReader reader = new StreamReader(fileName);
                string xmlString = reader.ReadToEnd();
                reader.Close();

                if (xmlString.Length > 0)
                {
                    EICDeviceFileList deviceFileList = (EICDeviceFileList)MSerializable.DeSerialize(xmlString, typeof(EICDeviceFileList));
                    return deviceFileList;
                }
            }
            catch
            {

            }
            return null;
        }
    }

    #region Class definition
    [Serializable]
    public class Device : MSerializable
    {
        Registration registrationInfo = new Registration();        
        EIC_INFO deviceInfo = new EIC_INFO();
        OwnerInfo ownerInfo = new OwnerInfo();
        private string signature;
        public Registration RegistrationInfo
        {
            get { return registrationInfo; }
            set { registrationInfo = value; }
        }
        public EIC_INFO DeviceInfo
        {
            get { return deviceInfo; }
            set { deviceInfo = value; }
        }
        public OwnerInfo OwnerInfo
        {
            get { return ownerInfo; }
            set { ownerInfo = value; }
        }
        
        public string Signature
        {
            get { return signature; }
            set { signature = value; }
        }
        
    }
    [Serializable]
    public class Registration
    {
        private string registrationId;
        RegisterBy registeredBy=new RegisterBy();
        public string RegistrationId
        {
            get { return registrationId; }
            set { registrationId = value; }
        }
        public RegisterBy RegisteredBy
	    {
        get { return registeredBy; }
        set { registeredBy = value; }
	    }
	
    }
    [Serializable]
    public class OwnerInfo
    {
        public string patientId;
        FACILITY_INFO facility = new FACILITY_INFO();
        public string PatientId
        {
            get { return patientId; }
            set { patientId = value; }
        }
        public FACILITY_INFO FacilityInfo
        {
            get { return facility; }
            set { facility = value; }
        }
        

    }
    [Serializable]
    public class RegisterBy
    {   
        private string Id;
        private string name;
        public string ID
        {
            get { return Id; }
            set { Id = value; }
        }        
        public string Name
        {
            get { return name; }
            set { name = value; }
        }
    }
    #endregion



}
    


