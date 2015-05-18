using System;
using System.Runtime.Serialization;

namespace MobiusServiceLibrary
{
     
    [DataContract]
    public class State
    {
        /// <summary>
        /// 
        /// </summary>
        [DataMember]
        public string StateName
        {
            get;
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        [DataMember]
        public Country Country
        {
            get;
            set;
        }
    }
}
