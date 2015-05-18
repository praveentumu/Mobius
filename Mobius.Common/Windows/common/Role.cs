using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;


namespace FirstGenesis.Mobius.Common.DataTypes
{
    public enum RoleType
    {
        AHLTAMobileUser = 0x00000000,
        AHLTAUser = 0x00000001,
        Patient = 0x00000002,
        Others = 0x00000003
    }

    public class ROLE_INFO : MSerializable
    {
        string _name;
        RoleType _roleType;
        string _description;
        string _publicKey;

        [XmlIgnore]
        public string PublicKey
        {
            get { return _publicKey; }
            set { _publicKey = value; }
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
        public RoleType RoleType
        {
            get { return _roleType; }
            set { _roleType = value; }
        }


    }

}