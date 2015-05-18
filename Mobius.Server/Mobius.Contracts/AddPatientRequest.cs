using System.Runtime.Serialization;

namespace MobiusServiceLibrary
{
    [DataContract]
    public class AddPatientRequest
    {       
        [DataMember]
       public Patient Patient
        {
            get;
            set;
        }

    
    }
    
}
