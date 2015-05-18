using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Mobius.CoreLibrary;

namespace Mobius.Entity
{
    [Serializable]
   public class Telephone
    {
        public Telephone()
        {
            this.Id = 0;
        }

        public Telephone(int id)
        {
            this.Id = id;
        }

        public int Id { get; set; }

        public string Type { get; set; }

        public string Number { get; set; }

        public string Extensionnumber { get; set; }

        public bool Status { get; set; }

        public string MPIID { get; set; }

        public ActionType Action { get; set; }
    }
}
