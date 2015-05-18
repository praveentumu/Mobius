using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Mobius.Entity
{
    public class IndividualProvider : Provider
    {
        private string _gender;
        private string _email;
        private string _firstName;
        private string _middleName;
        private string _lastName;

        public IndividualProvider()
        {

        }

        public new string Gender
        {
            get { return _gender; }
            set { _gender = value; }
        }

        public new string Email
        {
            get { return _email; }
            set { _email = value; }
        }
        public new string FirstName
        {
            get { return _firstName; }
            set { _firstName = value; }
        }
        public new string MiddleName
        {
            get { return _middleName; }
            set { _middleName = value; }
        }
        public new string LastName
        {
            get { return _lastName; }
            set { _lastName = value; }
        }


    }
}
