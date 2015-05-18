using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Mobius.Entity
{
    [Serializable]
    public class City
    {

        private State _State = null;
        /// <summary>
        /// 
        /// </summary>
        public string CityName
        {
            get;
            set;
        }
        
        /// <summary>
        /// 
        /// </summary>
        public State State
        {
            get
            { return _State != null ? _State : _State = new State(); }
            set
            {
                if (_State == null) _State = new State();
                _State = value;
            }
        }
    }
}
