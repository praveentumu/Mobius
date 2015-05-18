using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Security.Principal;

/// <summary>
/// Summary description for CustomPrincipal
/// </summary>
public class CustomPrincipal : IPrincipal
{
    private IIdentity _identity;
    private string[] _roles;

    public CustomPrincipal(IIdentity identity, string[] roles)
	{
        _identity = identity;
        _roles = new string[roles.Length];
        roles.CopyTo(_roles, 0);
        Array.Sort(_roles);

	}

    // IPrincipal Implementation
    public bool IsInRole(string role)
    {
        return Array.BinarySearch(_roles, role) >= 0 ? true : false;
    }

    public IIdentity Identity
    {
        get
        {
            return _identity;
        }
    }

    // Checks whether a principal is in all of the specified set of roles
    public bool IsInAllRoles( params string [] roles )
    {
      foreach (string searchrole in roles )
      {
        if (Array.BinarySearch(_roles, searchrole) < 0 )
          return false;
      }
      return true;
    }

    // Checks whether a principal is in any of the specified set of roles
    public bool IsInAnyRoles( params string [] roles )
    {
      foreach (string searchrole in roles )
      {
        if (Array.BinarySearch(_roles, searchrole ) > 0 )
          return true;
      }
      return false;
    }
}
