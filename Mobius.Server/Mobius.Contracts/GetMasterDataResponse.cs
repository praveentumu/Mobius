using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Mobius.CoreLibrary;
using System.Runtime.Serialization;
using Mobius.CoreLibrary;

namespace MobiusServiceLibrary
{
    [DataContract]
    public class GetMasterDataResponse
    {

        private Result _result = null;
        private List<MasterData> _MasterData = null;

        /// <summary>
        /// 
        /// </summary>
        [DataMember]
        public Result Result
        {
            get { return _result != null ? _result : _result = new Result(); }
            set { _result = value; }
        }

        /// <summary>
        /// Get or set the Master data
        /// </summary>
        [DataMember]
        public List<MasterData> MasterDataCollection
        {
            get { return _MasterData != null ? _MasterData : _MasterData = new List<MasterData>(); }
            set
            {
                if (_MasterData == null) _MasterData = new List<MasterData>();
                _MasterData = value;
            }
        }

    }
}
