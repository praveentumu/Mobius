using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace MobiusServiceLibrary
{

    [DataContract]
    public class NHINCommunity
    {

        private string _CommunityName = String.Empty;
        private string _CommunityIdentifier = String.Empty;
        private string _CommunityDescription = String.Empty;
        private bool _IsHomeCommunity = true;


        /// <summary>
        /// Every center will have a Name, which we will refer as Community Name.
        /// </summary>
        [DataMember]
        public string CommunityName
        {
            get { return _CommunityName; }
            set { _CommunityName = value; }
        }
        /// <summary>
        /// 
        /// </summary>
        [DataMember]
        public string CommunityDescription
        {
            get { return _CommunityDescription; }
            set { _CommunityDescription = value; }
        }

        /// <summary>
        /// Every center will have a unique identifier, which we will refer as Community Identifier.
        /// </summary>    
        [DataMember]
        public string CommunityIdentifier
        {
            get { return _CommunityIdentifier; }
            set { _CommunityIdentifier = value; }
        }

        /// <summary>
        /// This particular member represents has Home Community.
        /// </summary>
        [DataMember]
        public bool IsHomeCommunity
        {
            get { return _IsHomeCommunity; }
            set { _IsHomeCommunity = value; }
        }
    }

}
