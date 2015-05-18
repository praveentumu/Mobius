using System;
using System.Windows.Forms;

using pol = Lagash.Xacml.Core.Policy;

namespace Lagash.Xacml.ControlCenter.TreeNodes
{
	/// <summary>
	/// 
	/// </summary>
	public class AnySubject : NoBoldNode
	{
		/// <summary>
		/// 
		/// </summary>
		public AnySubject()
		{
			this.Text = "AnySubject";
			this.SelectedImageIndex = 1;
			this.ImageIndex = 1;
		}

	}
}
