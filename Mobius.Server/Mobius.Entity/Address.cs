using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Mobius.CoreLibrary;


namespace Mobius.Entity
{
    [Serializable]
    public class Address 
    {
        private City _City = null;
        public Address()
        {
            this.Id = 0;
        }
        public Address(int id)
        {
            this.Id = id;
        }

        public int Id { get; set; }

        public string AddressLine1 { get; set; }

        public string AddressLine2 { get; set; }

        public string MPIID { get; set; }

        public City City
        {
            get
            { return _City != null ? _City : _City = new City(); }
            set
            {
                if (_City == null) _City = new City();
                _City = value;
            }
        }

        public string Zip { get; set; }

        //public bool Status { get; set; }
        public AddressStatus AddressStatus { get; set; }

        public ActionType Action { get; set; }
    }
}
