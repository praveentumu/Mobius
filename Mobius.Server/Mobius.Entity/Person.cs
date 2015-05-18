using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Mobius.CoreLibrary;

namespace Mobius.Entity
{
    #region Person Entity

    [Serializable]
    public class Patient
    {
        private string _FacilityID;

        private string _patientId;
        private string _emailAddress;
        private string _communityId;
        private string _serialNumber;
        private string _publicKey;
        private string _CSR;
        private List<Telephone> _telephone = null;
        private List<Address> _address = null;
        private List<string> _given = new List<string>();
        private List<string> _Suffix = new List<string>();
        private List<string> _Prefix = new List<string>();
        private List<ActionType> _Actions = new List<ActionType>();

        private List<string> _middleName = new List<string>();
        private List<string> _family = new List<string>();
        private string _SSN = String.Empty;
        private string _DOB = String.Empty;
        private string _DeceasedTime = String.Empty;
        private string _gender = String.Empty;
        private string _localMPIID = String.Empty;
        private Name _mothersMaidenName = null;
        private List<int> _id = new List<int>();
        private string _CertificateCreationDate;
        private string _CertificateExpirationDate;
        public Patient()
        { }

        public string CreatedOn
        {
            get { return _CertificateCreationDate; }
            set { _CertificateCreationDate = value; }
        }
        public string ExpiryOn
        {
            get { return _CertificateExpirationDate; }
            set { _CertificateExpirationDate = value; }
        }


        public List<ActionType> Action
        {
            get { return _Actions; }
            set { _Actions = value; }
        }



        public string RemoteMPIID
        {
            get;
            set;
        }

        public string PatientId
        {
            get { return _patientId; }
            set { _patientId = value; }
        }
        public string EmailAddress
        {
            get { return _emailAddress; }
            set { _emailAddress = value; }
        }


        public string CommunityId
        {
            get { return _communityId; }
            set { _communityId = value; }
        }
        public string SerialNumber
        {
            get { return _serialNumber; }
            set { _serialNumber = value; }
        }
        public string PublicKey
        {
            get { return _publicKey; }
            set { _publicKey = value; }
        }
        public string CSR
        {
            get { return _CSR; }
            set { _CSR = value; }
        }
        public List<Address> PatientAddress
        {
            get
            { return _address != null ? _address : _address = new List<Address>(); }
            set
            {
                if (_address == null) _address = new List<Address>();
                _address = value;
            }
        }
        public List<Telephone> Telephone
        {
            get
            { return _telephone != null ? _telephone : _telephone = new List<Telephone>(); }
            set
            {
                if (_telephone == null) _telephone = new List<Telephone>();
                _telephone = value;
            }
        }

        public List<int> IDNames
        {
            get { return _id; }
            set { _id = value; }
        }
        /// <summary>
        /// 
        /// </summary>
        public List<string> GivenName
        {
            get { return _given; }
            set { _given = value; }
        }


        /// <summary>
        /// 
        /// </summary>
        public List<string> Prefix
        {
            get { return _Prefix; }
            set { _Prefix = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        public List<string> Suffix
        {
            get { return _Suffix; }
            set { _Suffix = value; }
        }


        /// <summary>
        /// 
        /// </summary>
        public List<string> MiddleName
        {
            get { return _middleName; }
            set { _middleName = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        public List<string> FamilyName
        {
            get { return _family; }
            set { _family = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        public string SSN
        {
            get { return _SSN; }
            set { _SSN = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        public string DOB
        {
            get { return _DOB; }
            set { _DOB = value; }
        }


        public string DeceasedTime
        {
            get { return _DeceasedTime; }
            set { _DeceasedTime = value; }
        }


        /// <summary>
        /// 
        /// </summary>
        //public string Gender
        //{
        //    get { return _gender; }
        //    set { _gender = value; }
        //}

        /// <summary>
        /// 
        /// </summary>
        public string LocalMPIID
        {
            get { return _localMPIID; }
            set { _localMPIID = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        public Name MothersMaidenName
        {
            get
            { return _mothersMaidenName != null ? _mothersMaidenName : _mothersMaidenName = new Name(); }
            set
            {
                if (_mothersMaidenName == null) _mothersMaidenName = new Name();
                _mothersMaidenName = value;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        public Gender Gender
        {
            get;
            set;
        }


        /// <summary>
        /// 
        /// </summary>
        public string Password
        {
            get;
            set;
        }


        public string BirthPlaceAddress { get; set; }

        public string BirthPlaceCity { get; set; }

        public string BirthPlaceState { get; set; }

        public string BirthPlaceCountry { get; set; }

        public string BirthPlaceZip { get; set; }
    }

    #endregion
}
