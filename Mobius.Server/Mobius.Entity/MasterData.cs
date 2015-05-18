using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Mobius.Entity
{
    /// <summary>
    /// This class will hold the master data value
    /// </summary>
    public class MasterData
    {
        /// <summary>
        /// get or set the code
        /// </summary>
        public string Code
        { get; set; }

        /// <summary>
        /// get or set the description of code
        /// </summary>
        public string Description
        { get; set; }

        public string Details
        {
            get;
            set;
        }
    }
}
