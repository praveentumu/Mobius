using System;
using System.Windows.Forms;

using pol = Lagash.Xacml.Core.Policy;

namespace Lagash.Xacml.ControlCenter.TreeNodes
{
	/// <summary>
	/// 
	/// </summary>
	public class Target : NoBoldNode
	{
		/// <summary>
		/// 
		/// </summary>
		private pol.TargetElementReadWrite _target;

		/// <summary>
		/// 
		/// </summary>
		/// <param name="target"></param>
		public Target( pol.TargetElementReadWrite target )
		{
			_target = target;

			this.Text = "Target";
			this.SelectedImageIndex = 4;
			this.ImageIndex = 4;

			FillTargetItems( _target.Subjects );
			FillTargetItems( _target.Resources );
			FillTargetItems( _target.Actions );
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="targetItems"></param>
		protected void FillTargetItems( pol.TargetItemsBaseReadWrite targetItems )
		{
			if( targetItems.IsAny )
			{
				if( targetItems is pol.ActionsElementReadWrite )
				{
					this.Nodes.Add( new AnyAction() );
				}
				else if( targetItems is pol.SubjectsElementReadWrite )
				{
					this.Nodes.Add( new AnySubject() );
				}
				else if( targetItems is pol.ResourcesElementReadWrite )
				{
					this.Nodes.Add( new AnyResource() );
				}
			}
			else
			{
				foreach( pol.TargetItemBaseReadWrite targetItem in targetItems.ItemsList )
				{
					this.Nodes.Add( new TargetItem( targetItem ) );
				}
			}
		}

		/// <summary>
		/// 
		/// </summary>
		public pol.TargetElementReadWrite TargetDefinition
		{
			get{ return _target; }
		}
	}
}
