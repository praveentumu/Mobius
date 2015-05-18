using System;
using System.Windows.Forms;

using pol = Lagash.Xacml.Core.Policy;

namespace Lagash.Xacml.ControlCenter.TreeNodes
{
	/// <summary>
	/// 
	/// </summary>
	public class AnyAction : NoBoldNode
	{
		/// <summary>
		/// 
		/// </summary>
		public AnyAction()
		{
			this.Text = "AnyAction";
			this.SelectedImageIndex = 2;
			this.ImageIndex = 2;
		}

	}
}
