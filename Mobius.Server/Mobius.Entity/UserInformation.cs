using Mobius.CoreLibrary;
using System;
using System.Xml.Serialization;

namespace Mobius.Entity
{

    [XmlType("UserInformation")]
    public class UserInfo
    {      
        public int? Id { get; set; }
      
        public UserType UserType { get; set; }
      
        public Name Name { get; set; }
      
        public string MPIID { get; set; }
      
        public string CommunityId { get; set; }
      
        public string PublicKey { get; set; }
      
        public bool IsOptIn { get; set; }
      
        public string Role { get; set; }
      
        public string EmailAddress { get; set; }

        public string UserRoleCode { get; set; }
    }

}
