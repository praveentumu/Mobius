using System;
using System.Windows.Forms;

using pol = Lagash.Xacml.Core.Policy;

namespace Lagash.Xacml.ControlCenter.TreeNodes
{
	/// <summary>
	/// 
	/// </summary>
	public class Condition : NoBoldNode
	{
		/// <summary>
		/// 
		/// </summary>
		private pol.ConditionElementReadWrite _condition;

		/// <summary>
		/// 
		/// </summary>
		/// <param name="condition"></param>
		public Condition( pol.ConditionElementReadWrite condition )
		{
			_condition = condition;

			this.Text = "Condition";
		}

		/// <summary>
		/// 
		/// </summary>
		public pol.ConditionElementReadWrite ConditionDefinition
		{
			get{ return _condition; }
		}
	}
}
