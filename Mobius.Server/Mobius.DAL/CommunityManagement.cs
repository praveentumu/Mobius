namespace Mobius.DAL
{
    #region Namespace
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Data.Common;
    using Mobius.CoreLibrary;
    using Mobius.Entity;
    #endregion

    public partial class MobiusDAL
    {

        #region GetNhinCommunity
        /// <summary>
        /// This method will return all ACTIVE Nhin communities ONLY
        /// </summary>
        /// <param name="NHINCommunities"></param>
        /// <returns></returns>
        public Result GetNhinCommunity(out List<MobiusNHINCommunity> NHINCommunities)
        {
            NHINCommunities = null;
            DbCommand dbCommand = null;
            IDataReader reader = null;
            try
            {
                NHINCommunities = new List<MobiusNHINCommunity>();
                MobiusNHINCommunity NHINCommunity = null;
                dbCommand = _dataAccessManager.GetStoredProcCommand("GetNHINCommunities");
                reader = _dataAccessManager.ExecuteReader(dbCommand);

                while (reader.Read())
                {
                    NHINCommunity = new MobiusNHINCommunity();
                    if (reader["Community"] != DBNull.Value)
                        NHINCommunity.CommunityName = Convert.ToString(reader["Community"]);
                    if (reader["CommunityIdentifier"] != DBNull.Value)
                        NHINCommunity.CommunityIdentifier = Convert.ToString(reader["CommunityIdentifier"]);
                    if (reader["CommunityDescription"] != DBNull.Value)
                        NHINCommunity.CommunityDescription = Convert.ToString(reader["CommunityDescription"]);

                    if (reader["IsHomeCommunity"] != DBNull.Value)
                    {
                        NHINCommunity.IsHomeCommunity = Convert.ToBoolean(reader["IsHomeCommunity"]);
                    }
                    NHINCommunities.Add(NHINCommunity);
                }
                this.Result.IsSuccess = true;
            }
            catch (Exception ex)
            {
                this.Result.IsSuccess = false;
                this.Result.SetError(ErrorCode.UnknownException, ex.Message);
            }
            //Added for issue id #138
            finally
            {
                if (dbCommand != null)
                {
                    dbCommand.Dispose();
                }
                if (reader != null)
                {
                    reader.Dispose();
                }
            }
            return this.Result;

        }


        /// <summary>
        /// This method returns list of all communities present in the systems, irrespective they are active/inactive.
        /// </summary>
        /// <param name="NHINCommunities"></param>
        /// <returns></returns>
        public List<MobiusNHINCommunity> GetAllNhinCommunities()
        {

            List<MobiusNHINCommunity> NHINCommunities = new List<MobiusNHINCommunity>();
            DbCommand dbCommand = null;
            IDataReader reader = null;
            try
            {

                MobiusNHINCommunity NHINCommunity = null;
                dbCommand = _dataAccessManager.GetStoredProcCommand("GetAllNHINCommunities");
                reader = _dataAccessManager.ExecuteReader(dbCommand);

                while (reader.Read())
                {
                    NHINCommunity = new MobiusNHINCommunity();
                    if (reader["ID"] != DBNull.Value)
                        NHINCommunity.ID = Convert.ToInt32(reader["ID"]);

                    if (reader["CommunityIdentifier"] != DBNull.Value)
                        NHINCommunity.CommunityIdentifier = Convert.ToString(reader["CommunityIdentifier"]);

                    if (reader["CommunityDescription"] != DBNull.Value)
                        NHINCommunity.CommunityDescription = Convert.ToString(reader["CommunityDescription"]);

                    if (reader["IsHomeCommunity"] != DBNull.Value)
                        NHINCommunity.IsHomeCommunity = Convert.ToBoolean(reader["IsHomeCommunity"]);

                    if (reader["Active"] != DBNull.Value)
                        NHINCommunity.Active = Convert.ToBoolean(reader["Active"]);

                    NHINCommunities.Add(NHINCommunity);
                }
                this.Result.IsSuccess = true;
            }
            catch (Exception ex)
            {
                this.Result.IsSuccess = false;
                this.Result.SetError(ErrorCode.UnknownException, ex.Message);
            }
            //Added for issue id #138
            finally
            {
                if (dbCommand != null)
                {
                    dbCommand.Dispose();
                }
                if (reader != null)
                {
                    reader.Dispose();
                }
            }
            return NHINCommunities;

        }

        #endregion

        #region Add NhinCommunity

        public Result AddNhinCommunities(List<MobiusNHINCommunity> lsNHINCommunity)
        {
            string checkRecord = string.Empty;
            List<string> lstExistingRecords = new List<string>();
            DbCommand dbCommand = null;
            try
            {
                foreach (MobiusNHINCommunity NHINCommunity in lsNHINCommunity)
                {
                    dbCommand = _dataAccessManager.GetStoredProcCommand("AddNHINCommunities");
                    _dataAccessManager.AddInParameter(dbCommand, "@CommunityIdentifier", DbType.String, NHINCommunity.CommunityIdentifier);
                    _dataAccessManager.AddInParameter(dbCommand, "@CommunityDescription", DbType.String, NHINCommunity.CommunityDescription);
                    _dataAccessManager.AddInParameter(dbCommand, "@Active", DbType.Boolean, NHINCommunity.Active);
                    _dataAccessManager.AddInParameter(dbCommand, "@IsHomeCommunity", DbType.Boolean, NHINCommunity.IsHomeCommunity);
                    _dataAccessManager.AddOutParameter(dbCommand, "@RecordExists", DbType.String, 50);
                    _dataAccessManager.ExecuteNonQuery(dbCommand);
                    checkRecord = dbCommand.Parameters["@RecordExists"].Value.ToString();
                    if (!string.IsNullOrWhiteSpace(checkRecord))
                        lstExistingRecords.Add(checkRecord);
                }
                string records = string.Empty;
                if (lstExistingRecords.Count > 0)
                {
                    records = string.Join(",", lstExistingRecords);
                    if (lstExistingRecords.Count == lsNHINCommunity.Count)
                    {
                        this.Result.IsSuccess = false;
                        this.Result.SetError(ErrorCode.All_Exist_Records_Not_Imported, String.Format(Helper.GetErrorMessage(ErrorCode.All_Exist_Records_Not_Imported), records));
                        return this.Result;
                    }
                    else
                    {
                        this.Result.IsSuccess = false;
                        this.Result.SetError(ErrorCode.Some_Exist_Records_Not_Imported, String.Format(Helper.GetErrorMessage(ErrorCode.Some_Exist_Records_Not_Imported), records));
                        return this.Result;
                    }
                }

                else
                {
                    this.Result.IsSuccess = true;
                    this.Result.SetError(ErrorCode.Records_Imported_Successfully);
                    return this.Result;
                }
            }
            catch (Exception ex)
            {
                this.Result.IsSuccess = false;
                this.Result.SetError(ErrorCode.UnknownException, ex.Message);
            }
            //Added for issue id #138
            finally
            {
                if (dbCommand != null)
                {
                    dbCommand.Dispose();
                }

            }
            return this.Result;

        }

        #endregion

        #region Update NhinCommunity

        public Result UpdateNhinCommunity(MobiusNHINCommunity NHINCommunity)
        {
            int checkRecord = 0;
            DbCommand dbCommand = null;
            try
            {
                dbCommand = _dataAccessManager.GetStoredProcCommand("UpdateNHINCommunities");
                _dataAccessManager.AddInParameter(dbCommand, "@CommunityIdentifier", DbType.String, NHINCommunity.CommunityIdentifier);
                _dataAccessManager.AddInParameter(dbCommand, "@CommunityDescription", DbType.String, NHINCommunity.CommunityDescription);
                _dataAccessManager.AddInParameter(dbCommand, "@Active", DbType.Boolean, NHINCommunity.Active);
                _dataAccessManager.AddInParameter(dbCommand, "@IsHomeCommunity", DbType.Boolean, NHINCommunity.IsHomeCommunity);
                _dataAccessManager.AddInParameter(dbCommand, "@ID", DbType.Int32, NHINCommunity.ID);
                _dataAccessManager.AddOutParameter(dbCommand, "@RecordExists", DbType.Boolean, 1);
                _dataAccessManager.ExecuteNonQuery(dbCommand);
                checkRecord = Convert.ToInt32(dbCommand.Parameters["@RecordExists"].Value);

                if (checkRecord == 0)
                {
                    this.Result.IsSuccess = false;
                    this.Result.SetError(ErrorCode.Duplicate_Entry);
                    return this.Result;
                }
                else
                {
                    this.Result.IsSuccess = true;
                    this.Result.SetError(ErrorCode.Record_Successfully_Updated);
                    return this.Result;

                }

            }
            catch (Exception ex)
            {
                this.Result.IsSuccess = false;
                this.Result.SetError(ErrorCode.UnknownException, ex.Message);
            }
            //Added for issue id #138
            finally
            {
                if (dbCommand != null)
                {
                    dbCommand.Dispose();
                }

            }
            return this.Result;

        }

        #endregion

        #region Delete NhinCommunity
        public Result DeleteNhinCommunity(int ID)
        {
            DbCommand dbCommand = null;
            try
            {
                dbCommand = _dataAccessManager.GetStoredProcCommand("DeleteNHINCommunities");
                _dataAccessManager.AddInParameter(dbCommand, "@ID", DbType.Int32, ID);
                _dataAccessManager.ExecuteNonQuery(dbCommand);
                this.Result.IsSuccess = true;

            }
            catch (Exception ex)
            {
                this.Result.IsSuccess = false;
                this.Result.SetError(ErrorCode.UnknownException, ex.Message);
            }
            //Added for issue id #138
            finally
            {
                if (dbCommand != null)
                {
                    dbCommand.Dispose();
                }

            }
            return this.Result;

        }
        #endregion Delete NhinCommunity
    }
}
