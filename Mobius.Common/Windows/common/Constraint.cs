using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.Xml.Serialization;
using System.IO;
using FirstGenesis.Mobius.Common;
using System.Runtime.InteropServices;

namespace FirstGenesis.Mobius.Common.DataTypes
{
    public enum ConstraintType
    {
        Expiry = 1,
        Category = 2,
        Count = 4
    }

    public class CValue
    {
        public System.Int16 intVal;
        public DateTime  dateTimeVal;
        public string  strVal;

        public override string ToString()
        {
            StringBuilder toString = new StringBuilder();
            toString.Append(base.ToString());

            toString.Append(intVal.ToString());
            toString.Append(dateTimeVal.ToString());
            toString.Append(strVal);

            return toString.ToString();
        }
    }

    [XmlRoot("Constraint")]
    public class Constraint : MSerializable
    {
      
        CValue _value = new CValue();

        string _description;
        string _name;
        ConstraintType _type;

        public Constraint()
        {

        }

        public ConstraintType Type
        {
            get { return _type; }
            set { _type = value; }
        }
        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }

        public string Description
        {
            get { return _description; }
            set { _description = value; }
        }

        public CValue Value
        {
            get { return _value; }
            set { _value = value; }
        }

        public override string ToString()
        {
            StringBuilder toString = new StringBuilder();

            toString.Append(base.ToString());

            toString.Append(_value.ToString());
            toString.Append(_description);
            toString.Append(_name);
            toString.Append(_type.ToString());

            return toString.ToString();

        }
      
    }

}
