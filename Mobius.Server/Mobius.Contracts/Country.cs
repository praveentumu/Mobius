using System;
using System.Runtime.Serialization;

namespace MobiusServiceLibrary
{
     
    [DataContract]
    public class Country
    {

        /// <summary>
        /// 
        /// </summary>
        [DataMember]
        public string CountryName
        {
            get;
            set;
        }
    }
}
