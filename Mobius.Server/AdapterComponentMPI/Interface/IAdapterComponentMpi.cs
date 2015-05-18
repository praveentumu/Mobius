

namespace Mobius.ServiceLibrary
{
    #region namespace
    using System.ServiceModel;
    using PatientDiscovery;
    using System.ServiceModel.Web;
    using Mobius.ServiceLibrary;
    using System;
    #endregion

    #region ServiceContract
    //
    /// <summary>
    /// IAdapterComponentMpi interface
    /// </summary>
    [ServiceContract(Name = "AdapterComponentMpiPortType",Namespace="urn:gov:hhs:fha:nhinc:adaptercomponentmpi"), XmlSerializerFormat(Style = OperationFormatStyle.Document,    Use = OperationFormatUse.Literal, SupportFaults = true)
    ]
    [ServiceKnownType(typeof(IntegrityCheckAlgorithm))]
    [ServiceKnownType(typeof(CompressionAlgorithm))]
    [ServiceKnownType(typeof(BinaryDataEncoding))]
    [ServiceKnownType(typeof(Community_PRPA_IN201306UV02ResponseType))]
    [ServiceKnownType(typeof(PRPA_MT201310UV02Person))]
    [ServiceKnownType(typeof(COCT_MT090303UV01AssignedDevice))]
    [ServiceKnownType(typeof(ENXP_explicit))]
    [ServiceKnownType(typeof(PN_explicit))]
     [ServiceKnownType(typeof(en_explicitsuffix))]
     [ServiceKnownType(typeof(en_explicitprefix))]
     [ServiceKnownType(typeof(en_explicitgiven))]
     [ServiceKnownType(typeof(en_explicitfamily))]
     [ServiceKnownType(typeof(en_explicitdelimiter))]
    [ServiceKnownType(typeof(PRPA_MT201310UV02Person))]
    //[ServiceKnownType(typeof(Arraylist
  
    public interface IAdapterComponentMpi
    {
        /// <summary>
        /// FindCandidates
        /// </summary>
        /// <param name="reqType">RespondingGateway_PRPA_IN201305UV02RequestType class</param>
        /// <returns>PRPA_IN201306UV02ResponseType class</returns>
        [OperationContract(Action="urn:gov:hhs:fha:nhinc:adaptercomponentmpi:FindCandidatesRequest"), XmlSerializerFormat(Style = OperationFormatStyle.Document, Use = OperationFormatUse.Literal, SupportFaults = true)
        ]
        PRPA_IN201306UV02ResponseType FindCandidates(RespondingGateway_PRPA_IN201305UV02RequestType RespondingGateway_PRPA_IN201305UV02Request);
    }
    #endregion
}
