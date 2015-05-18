
namespace Mobius.ServiceLibrary
{
    #region nnamespace
    using System.Runtime.Serialization;
    using PatientDiscovery;
    using System;
    using System.Collections;
    #endregion
    //[Serializable]
    [DataContract(Name = "AdapterComponentMpiService", Namespace = "urn:gov:hhs:fha:nhinc:adaptercomponentmpi")]
    [KnownType(typeof(IntegrityCheckAlgorithm))]
    [KnownType(typeof(ArrayList))]
    [KnownType(typeof(CompressionAlgorithm))]
    [KnownType(typeof(BinaryDataEncoding))]
    [KnownType(typeof(Community_PRPA_IN201306UV02ResponseType))]
    [KnownType(typeof(PRPA_MT201310UV02Person))]
    [KnownType(typeof(COCT_MT090303UV01AssignedDevice))]                        
    [KnownType(typeof(ENXP_explicit))]
    [KnownType(typeof(ENXP_explicit))]
    [KnownType(typeof(PN_explicit))]
    [KnownType(typeof(en_explicitsuffix))]
    [KnownType(typeof(en_explicitprefix))]
    [KnownType(typeof(en_explicitgiven))]
    [KnownType(typeof(en_explicitfamily))]
    [KnownType(typeof(en_explicitdelimiter))]
    [KnownType(typeof(PRPA_MT201310UV02Person))]
    public class PRPA_IN201306UV02ResponseType : Community_PRPA_IN201306UV02ResponseType
    {
    }
}
