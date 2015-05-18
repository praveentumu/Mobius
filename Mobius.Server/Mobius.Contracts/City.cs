using System;
using System.Runtime.Serialization;

namespace MobiusServiceLibrary
{
     
    [DataContract]
    public class City
    {
        
        
        /// <summary>
        /// 
        /// </summary>
        [DataMember]
        public string CityName 
        { 
            get; 
            set; 
        }
        /// <summary>
        /// 
        /// </summary>
        [DataMember]
        public State State 
        { 
            get; set; 
        }
    }
}
