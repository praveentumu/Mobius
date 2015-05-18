using System;
using System.Windows.Forms;
using pol = Lagash.Xacml.Core.Policy;

namespace Lagash.Xacml.ControlCenter.TreeNodes
{
	/// <summary>
	/// 
	/// </summary>
	public class Obligations : NoBoldNode
	{
		/// <summary>
		/// 
		/// </summary>
		private pol.ObligationCollectionReadWrite _obligations;
		

		/// <summary>
		/// 
		/// </summary>
		/// <param name="obligations"></param>
		public Obligations( pol.ObligationCollectionReadWrite obligations )
		{
			_obligations = obligations;

			this.Text = "Obligations";
		}

		/// <summary>
		/// 
		/// </summary>
		public pol.ObligationCollectionReadWrite ObligationDefinition
		{
			get{ return _obligations; }
		}
	}
}
