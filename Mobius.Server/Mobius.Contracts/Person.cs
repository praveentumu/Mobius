using System;
using System.Collections.Generic;
using Mobius.CoreLibrary;
using System.Runtime.Serialization;

namespace MobiusServiceLibrary
{
    #region Person

     
    [DataContract]
    public class Patient
    {


        private List<Telephone> _telephone = null;
        private List<Address> _address = null;
        private List<string> _given = new List<string>();
        private List<string> _middleName = new List<string>();
        private List<string> _family = new List<string>();
        private List<int> _id = new List<int>();

        public Patient()
        { }
        [DataMember(EmitDefaultValue = false)]
        public string CSR
        {
            get;
            set;
        }

        [DataMember(EmitDefaultValue = false)]
        public string EmailAddress
        {
            get;
            set;
        }
        [DataMember(EmitDefaultValue = false)]
        public string PatientId
        {
            get;
            set;
        }
        [DataMember(EmitDefaultValue = false)]
        public string CommunityId
        {
            get;
            set;
        }
        [DataMember]
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
        [DataMember]
        public List<Telephone> Telephones
        {
            get
            { return _telephone != null ? _telephone : _telephone = new List<Telephone>(); }
            set
            {
                if (_telephone == null) _telephone = new List<Telephone>();
                _telephone = value;
            }
        }
        [DataMember]
        public List<int> IDNames
        {
            get { return _id; }
            set { _id = value; }
        }
        /// <summary>
        /// 
        /// </summary>
        [DataMember]
        public List<string> GivenName
        {
            get { return _given; }
            set { _given = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        [DataMember]
        public List<string> MiddleName
        {
            get { return _middleName; }
            set { _middleName = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        [DataMember]
        public List<string> FamilyName
        {
            get { return _family; }
            set { _family = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        [DataMember(EmitDefaultValue = false)]
        public string SSN
        {
            get;
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        [DataMember(EmitDefaultValue = false)]
        public string DOB
        {
            get;
            set;
        }


        /// 
        /// </summary>
        [DataMember]
        public List<string> Prefix
        {
            get;
            set;
        }


        /// 
        /// </summary>
        [DataMember]
        public List<string> Suffix
        {
            get;
            set;
        }


        /// <summary>
        /// 
        /// </summary>
        [DataMember(EmitDefaultValue = false)]
        public string LocalMPIID
        {
            get;
            set;
        }


        [DataMember(EmitDefaultValue = false)]
        public string RemoteMPIID
        {
            get;
            set;
        }

       
        [DataMember]
        public Name MothersMaidenName
        {
            get;
            set;
        }


        [DataMember(EmitDefaultValue = false)]
        public Gender Gender
        {
            get;
            set;
        }

        [DataMember(EmitDefaultValue = false)]
        public string Password
        {
            get;
            set;
        }

        [DataMember(EmitDefaultValue = false)]
        public string BirthPlaceAddress { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public string BirthPlaceCity { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public string BirthPlaceState { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public string BirthPlaceCountry { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public string BirthPlaceZip { get; set; }

        [DataMember]
        public List<ActionType> Action
        {
            get;
            set;
        }
    }

    #endregion
}
