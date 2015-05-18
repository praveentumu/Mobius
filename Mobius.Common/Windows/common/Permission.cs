using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.IO;
using System.Xml.Serialization;
using FirstGenesis.Mobius.Common;

namespace FirstGenesis.Mobius.Common.DataTypes
{
    public enum PermissionType
    {
        View    = 1,
        Add     = 2,
        Modify  = 4,
        Delete  = 8,
        Share   = 16
    }

    public class Permission : MSerializable
    {
        List<Constraint> _constraints = new List<Constraint>();
        PermissionType  _type;

        public PermissionType  Type
        {
            get { return _type; }
            set { _type = value; }
        }

        public Permission() 
        {
        }

        public Permission(PermissionType type) 
        {
            _type = type; 
        }
        public List<Constraint> Constraints
        {
            get { return _constraints; }
            set { _constraints = value; }
        }

        public string GetPermissionName()
        {
            return _type.ToString();
        }

        public override string ToString()
        {
            StringBuilder toString = new StringBuilder();

            toString.Append(base.ToString());

            foreach(Constraint constraint in _constraints)
                toString.Append(constraint.ToString());

            toString.Append(_type.ToString());

            return  toString.ToString();
           
        }
       
    }
}
