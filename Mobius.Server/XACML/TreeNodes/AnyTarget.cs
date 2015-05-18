using System;
using System.Windows.Forms;

using pol = Lagash.Xacml.Core.Policy;

namespace Lagash.Xacml.ControlCenter.TreeNodes
{
	/// <summary>
	/// 
	/// </summary>
	public class AnyTarget : NoBoldNode
	{
		/// <summary>
		/// 
		/// </summary>
		public AnyTarget()
		{
			this.Text = "AnyTarget";
			this.SelectedImageIndex = 4;
			this.ImageIndex = 4;
		}
	}
}
