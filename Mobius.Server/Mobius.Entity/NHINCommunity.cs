
namespace Mobius.Entity
{
    using System;
    using System.Xml.Serialization;

    [XmlType("NHINCommunity")]
    public class MobiusNHINCommunity
    {
        private int _id = 0;
        private string _CommunityName = String.Empty;
        private string _CommunityIdentifier = String.Empty;
        private string _CommunityDescription = String.Empty;
        private bool _IsHomeCommunity = true;
        private bool _Active = true;


        /// <summary>
        /// ID 
        /// </summary>
        public int ID
        {
            get { return _id; }
            set { _id = value; }
        }
        /// <summary>
        /// Every center will have a Name, which we will refer as Community Name.
        /// </summary>
        public string CommunityName
        {
            get { return _CommunityName; }
            set { _CommunityName = value; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string CommunityDescription
        {
            get { return _CommunityDescription; }
            set { _CommunityDescription = value; }
        }

        /// <summary>
        /// Every center will have a unique identifier, which we will refer as Community Identifier.
        /// </summary>    
        public string CommunityIdentifier
        {
            get { return _CommunityIdentifier; }
            set { _CommunityIdentifier = value; }
        }

        /// <summary>
        /// This particular member represents has Home Community.
        /// </summary>
        public bool IsHomeCommunity
        {
            get { return _IsHomeCommunity; }
            set { _IsHomeCommunity = value; }
        }

        /// <summary>
        /// This particular member represents is Active.
        /// </summary>
        public bool Active
        {
            get { return _Active; }
            set { _Active = value; }
        }
    }
}
