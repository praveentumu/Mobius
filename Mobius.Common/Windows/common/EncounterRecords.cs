using System;
using System.Collections.Generic;
using System.Text;
using FirstGenesis.Mobius.Common.DataTypes;


namespace FirstGenesis.Mobius.Common
{
    #region Class definition
    /// <summary>
    /// Encounter Record Information
    /// </summary>    
    [Serializable]
    public class EncounterInfo:MSerializable
    {
        string _EncounterID;
        string _Title;
        string _Description;
        string _OwnerID;        
        string _PatientID;
        DateTime _DateCreated;
        string _Category;
        DateTime _DateSynchronized;
        string _Data;
        DateTime _DateModified;        
        string _Signature;
        //New Added Fields        
        string _FacilityName;
        int _FacilityID;         
        string _MedicID;
        

        public string EncounterID
        {
            get
            {
                return _EncounterID;
            }
            set
            {
                _EncounterID = value;
            }
        }

        public string Title
        {
            get
            {
                return _Title;
            }
            set
            {
                _Title = value;
            }
        }

        public string Description
        {
            get
            {
                return _Description;
            }
            set
            {
                _Description = value;
            }
        }

        public string OwnerID
        {
            get
            {
                return _OwnerID;
            }
            set
            {
                _OwnerID = value;
            }
        }

        public string PatientID
        {
            get
            {
                return _PatientID;
            }
            set
            {
                _PatientID = value;
            }
        }

        public DateTime DateCreated
        {
            get
            {
                return _DateCreated;
            }
            set
            {
                _DateCreated = value;
            }
        }

        public string Category
        {
            get
            {
                return _Category;
            }
            set
            {
                _Category = value;
            }
        }

        public DateTime DateSynchronized
        {
            get
            {
                return _DateSynchronized;
            }
            set
            {
                _DateSynchronized = value;
            }
        }

        public string Data
        {
            get
            {
                return _Data;
            }
            set
            {
                _Data = value;
            }
        }
        
        public DateTime DateModified
        {
            get
            {
                return _DateModified;
            }
            set
            {
                _DateModified = value;
            }
        }

        public string Signature
        {
            get
            {
                return _Signature;
            }
            set
            {
                _Signature = value;
            }
        }
        //New Added Properties        
        public string FacilityName
        {
            get
            {
                return _FacilityName;
            }
            set
            {
                _FacilityName = value;
            }
        }

        public int FacilityID
        {
            get
            {
                return _FacilityID;
            }
            set
            {
                _FacilityID = value;
            }
        }
        //Not decided
        public string MedicID
        {
            get
            {
                return _MedicID;
            }
            set
            {
                _MedicID = value;
            }
        }
        

    }

    /// <summary>
    /// Identification File Configuration
    /// </summary>    
    [Serializable]
    public class IdnFile : MSerializable
    {
        private string _GUID;

        private string _PublicKey;

        private string _PrivateKey;

        private string _Signature;

        public string ID
        {
            get
            {
                return _GUID;
            }
            set
            {
                _GUID = value;
            }
        }

        public string PublicKey
        {
            get
            {
                return _PublicKey;
            }
            set
            {
                _PublicKey = value;
            }
        }

        public string PrivateKey
        {
            get
            {
                return _PrivateKey;
            }
            set
            {
                _PrivateKey = value;
            }
        }

        public string Signature
        {
            get
            {
                return _Signature;
            }
            set
            {
                _Signature = value;
            }
        }

    }

    #endregion
}
