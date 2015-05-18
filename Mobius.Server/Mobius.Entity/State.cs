using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Mobius.Entity
{
    [Serializable]
    public class State
    {
        private Country _Country = null;

        /// <summary>
        /// 
        /// </summary>
        public string StateName 
        { 
            get; 
            set; 
        }

        /// <summary>
        /// 
        /// </summary>
        public Country Country 
        {
            get
            { return _Country != null ? _Country : _Country = new Country(); }
            set
            {
                if (_Country == null) _Country = new Country();
                _Country = value;
            }
        }
    }
}
