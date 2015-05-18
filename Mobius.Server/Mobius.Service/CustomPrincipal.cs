using System.ServiceModel;
using System.IdentityModel.Claims;

namespace Mobius.Server.MobiusHISEService
{
    public class CustomPrincipal : ServiceAuthorizationManager
    {
        protected override bool CheckAccessCore(OperationContext operationContext)
        {
            // Extact the action URI from the OperationContext. We will use this to match against the claims
            // in the AuthorizationContext
            string action = operationContext.RequestContext.RequestMessage.Headers.Action;

            if (OperationContext.Current.EndpointDispatcher.ContractName == "IMobiusSecured")
            {
                if (OperationContext.Current.ServiceSecurityContext != null)
                {
                    foreach (ClaimSet claimSet in OperationContext.Current.ServiceSecurityContext.AuthorizationContext.ClaimSets)
                    {
                        if (claimSet.Issuer == ClaimSet.System)
                        {
                            // Iterate through claims of type "Mobius"
                            foreach (Claim c in claimSet.FindClaims("Mobius", Rights.PossessProperty))
                            {
                                // If the Claim resource matches the action URI then return true to allow access
                                if (action == c.Resource.ToString())
                                {
                                    return true;
                                }
                            }
                        }
                    }
                    return false;
                }
            }
            // If we get here, return false, denying access.
            return true;

        }
    }
}
