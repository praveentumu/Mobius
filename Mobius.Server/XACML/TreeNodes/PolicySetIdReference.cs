using System;
using System.Windows.Forms;

using pol = Lagash.Xacml.Core.Policy;

namespace Lagash.Xacml.ControlCenter.TreeNodes
{
	/// <summary>
	/// 
	/// </summary>
	public class PolicySetIdReference : NoBoldNode
	{
		/// <summary>
		/// 
		/// </summary>
		private pol.PolicySetIdReferenceElementReadWrite _policySetIdReference;

		/// <summary>
		/// 
		/// </summary>
		/// <param name="policySetIdReference"></param>
		public PolicySetIdReference( pol.PolicySetIdReferenceElementReadWrite policySetIdReference )
		{
			_policySetIdReference = policySetIdReference;

			this.Text = string.Format( "PolicySetIdReference: {0}", policySetIdReference.PolicySetId );
		}
	}
}
