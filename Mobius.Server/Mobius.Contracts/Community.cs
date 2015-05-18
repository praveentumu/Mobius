using System;
using System.Runtime.Serialization;
namespace MobiusServiceLibrary
{
     
    [DataContract]
    public class Community
    {
        private string communityIdentifier = String.Empty;

        /// <summary>
        /// Every center will have a unique identifier, which we will refer as Community Identifier.
        /// </summary>   
        [DataMember]
        public string CommunityIdentifier
        {
            get { return communityIdentifier; }
            set { communityIdentifier = value; }
        }


    }
}
