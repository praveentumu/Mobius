using System.Runtime.Serialization;

namespace MobiusServiceLibrary
{
    /// <summary>
    /// This class will hold the master data value
    /// </summary>
    [DataContract]
    public class MasterData
    {
        /// <summary>
        /// get or set the code
        /// </summary>
        [DataMember]
        public string Code
        { get; set; }

        /// <summary>
        /// get or set the description of code
        /// </summary>
        [DataMember]
        public string Description
        { get; set; }


    }
}
