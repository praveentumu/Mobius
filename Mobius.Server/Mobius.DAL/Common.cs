using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Mobius.Entity;
using Mobius.CoreLibrary;
using System.Data.Common;
using System.Data;

namespace Mobius.DAL
{
    public partial class MobiusDAL : IMobiusDAL
    {

        /// <summary>
        /// This method would get the master entries from DB                        
        /// </summary>
        /// <param name="masterCollection"></param>
        /// <param name="dependentValue"></param>
        /// <param name="masterDataCollection"></param>
        /// <returns></returns>
        public Result GetMasterData(MasterCollection masterCollection, int dependedValue, out List<MasterData> masterDataCollection)
        {
          
            masterDataCollection = new List<MasterData>();
            try
            {
                this.Result.IsSuccess = true;
                using (DbCommand dbCommand = _dataAccessManager.GetStoredProcCommand("GetMasterData"))
                {
                    _dataAccessManager.AddInParameter(dbCommand, "MasterDataSelected", DbType.Int32, masterCollection.GetHashCode());
                    if (dependedValue == 0)
                        _dataAccessManager.AddInParameter(dbCommand, "DependentValue", DbType.Int32, DBNull.Value);
                    else
                        _dataAccessManager.AddInParameter(dbCommand, "DependentValue", DbType.Int32, dependedValue);


                    using (DataSet dataSet = _dataAccessManager.ExecuteDataSet(dbCommand))
                    {
                        //}

                        if (dataSet != null && dataSet.Tables.Count > 0)
                        {
                            if (dataSet.Tables[0].Rows.Count > 0)
                            {
                                MasterData masterData;
                                foreach (DataRow row in dataSet.Tables[0].Rows)
                                {
                                    masterData = new MasterData();
                                    if (row["ID"] != DBNull.Value)
                                        masterData.Code = Convert.ToString(row["ID"]);

                                    if (row["Name"] != DBNull.Value)
                                        masterData.Description = Convert.ToString(row["Name"]);

                                    if (row.Table.Columns.Contains("Description") && row["Description"] != DBNull.Value)
                                    {
                                        masterData.Details = Convert.ToString(row["Description"]);
                                    }

                                    masterDataCollection.Add(masterData);
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                this.Result.IsSuccess = false;
                this.Result.SetError(ErrorCode.UnknownException, ex.Message);
            }
            return this.Result;
        }
    }
}
