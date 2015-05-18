
using System;
using System.Collections.Generic;
using Mobius.CoreLibrary;
namespace Mobius.Entity
{
    [Serializable]
    public class Demographics : Name
    {
        private string _SSN = string.Empty;
        private string _DOB = string.Empty;
        private string _DeceasedTime = string.Empty;
        private Gender _gender = Gender.Unspecified;
        private string _localMPIID = string.Empty;
        private Name _mothersMaidenName = null;
        private List<Telephone> _telephones = null;
        private List<Address> _address = null;
        private List<Address> _BirthPlaceAddress = null;

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
        public Gender Gender
        {
            get { return _gender; }
            set { _gender = value; }
        }

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
        public List<Address> Addresses
        {
            get
            { return _address != null ? _address : _address = new List<Address>(); }
            set
            {
                if (_address == null) _address = new List<Address>();
                _address = value;
            }
        }



        public List<Address> BirthPlaceAddress
        {
            get
            { return _BirthPlaceAddress != null ? _BirthPlaceAddress : _BirthPlaceAddress = new List<Address>(); }
            set
            {
                if (_BirthPlaceAddress == null) _BirthPlaceAddress = new List<Address>();
                _BirthPlaceAddress = value;
            }
        }


        /// <summary>
        /// 
        /// </summary>
        public List<Telephone> Telephones
        {
            get
            { return _telephones != null ? _telephones : _telephones = new List<Telephone>(); }
            set
            {
                if (_telephones == null) _telephones = new List<Telephone>();
                _telephones = value;
            }
        }
    }
}
