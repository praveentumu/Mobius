
using System;
using System.Runtime.Serialization;
using Mobius.CoreLibrary;
using System.Collections.Generic;
namespace MobiusServiceLibrary
{

    [DataContract]
    public class Demographics : Name
    {
        private string _SSN = string.Empty;
        private string _DOB = string.Empty;
        private string _localMPIID = string.Empty;
        private string _mothersMaidenName = string.Empty;


        /// <summary>
        /// 
        /// </summary>
        [DataMember(EmitDefaultValue = false)]
        public string SSN
        {
            get { return _SSN; }
            set { _SSN = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        [DataMember(EmitDefaultValue = false)]
        public string DOB
        {
            get { return _DOB; }
            set { _DOB = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        [DataMember(EmitDefaultValue = false)]
        public Gender Gender
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
            get { return _localMPIID; }
            set { _localMPIID = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        [DataMember(EmitDefaultValue = false)]
        public Name MothersMaidenName
        {
            get;
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        [DataMember(EmitDefaultValue = false)]
        public List<string> Street { get; set; }
        /// <summary>
        /// 
        /// </summary>
        [DataMember(EmitDefaultValue = false)]
        public List<string> Zip { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [DataMember(EmitDefaultValue = false)]
        public List<string> City
        {
            get;
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        [DataMember(EmitDefaultValue = false)]
        public List<string> State
        {
            get;
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        [DataMember(EmitDefaultValue = false)]
        public List<string> ContractNumbers { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public string BirthPlaceStreet { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public string BirthPlaceState { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public string BirthPlaceCity { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public string BirthPlaceZip { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public string BirthPlaceCountry { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public string DeceasedDate { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public List<string> Country { get; set; }
    }
}
