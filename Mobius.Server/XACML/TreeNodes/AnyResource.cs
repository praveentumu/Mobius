using System;
using System.Windows.Forms;

using pol = Lagash.Xacml.Core.Policy;

namespace Lagash.Xacml.ControlCenter.TreeNodes
{
	/// <summary>
	/// 
	/// </summary>
	public class AnyResource : NoBoldNode
	{
		/// <summary>
		/// 
		/// </summary>
		public AnyResource()
		{
			this.Text = "AnyResource";
			this.SelectedImageIndex = 3;
			this.ImageIndex = 3;
		}

	}
}
