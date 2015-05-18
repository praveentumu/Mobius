using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using Mobius.CoreLibrary;

namespace MobiusServiceLibrary
{
    [DataContract]
    public class UserInformation
    {
        [DataMember]
        public int? Id { get; set; }
        [DataMember]
        public UserType UserType { get; set; }
        [DataMember]
        public Name Name { get; set; }
        
        [DataMember]
        public string MPIID { get; set; }
        [DataMember]
        public string CommunityId { get; set; }
        [DataMember]
        public string PublicKey { get; set; }
        [DataMember]
        public bool IsOptIn { get; set; }
        [DataMember]
        public string Role { get; set; }
        [DataMember]
        public string EmailAddress { get; set; }        
    }
}
