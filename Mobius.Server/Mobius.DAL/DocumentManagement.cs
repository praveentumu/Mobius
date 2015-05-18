using System;
using System.Data;
using System.Data.Common;
using FirstGenesis.Mobius.Server.DataAccessLayer;
using Mobius.CoreLibrary;
using Mobius.Entity;
using System.Collections.Generic;

namespace Mobius.DAL
{
    public partial class MobiusDAL
    {

        #region Private variable
        DataAccessManager _dataAccessManager = DataAccessManager.GetInstance;
        #endregion Private variable

        /// <summary>
        /// This method will returns document meta data on the bases of patient-Id and community-Id 
        /// </summary>
        /// <param name="patientId"></param>
        /// <param name="communityId"></param>
        /// <param name="documents"></param>
        /// <returns></returns>
        public Result GetDocumentMetaData(string patientId, string communityId, out List<MobiusDocument> documents)
        {
            documents = new List<MobiusDocument>();
            MobiusDocument document = null;
            try
            {
                using (DbCommand dbCommand = _dataAccessManager.GetStoredProcCommand("GetPatientDocumentMetaData"))
                {
                    _dataAccessManager.AddInParameter(dbCommand, "@PatientID", DbType.String, patientId);
                    _dataAccessManager.AddInParameter(dbCommand, "@CommunityId", DbType.String, communityId);

                    using (IDataReader reader = _dataAccessManager.ExecuteReader(dbCommand))
                    {
                        while (reader.Read())
                        {
                            document = new MobiusDocument();

                            if (reader["OriginalDocumentID"] != DBNull.Value)
                                document.DocumentUniqueId = Convert.ToString(reader["OriginalDocumentID"]);

                            if (reader["DocumentTitle"] != DBNull.Value)
                                document.DocumentTitle = Convert.ToString(reader["DocumentTitle"]);

                            if (reader["Author"] != DBNull.Value)
                                document.Author = Convert.ToString(reader["Author"]);

                            if (reader["CreatedDate"] != DBNull.Value)
                                document.CreatedOn = Convert.ToString(reader["CreatedDate"]);

                            if (reader["DataSource"] != DBNull.Value)
                                document.DataSource = Convert.ToString(reader["DataSource"]);

                            if (reader["IsShared"] != DBNull.Value)
                                document.IsShared = Convert.ToBoolean(reader["IsShared"]);

                            if (reader["PatientID"] != DBNull.Value)
                                document.SourcePatientId = Convert.ToString(reader["PatientID"]);

                            if (reader["SourceRepositoryID"] != DBNull.Value)
                                document.RepositoryUniqueId = Convert.ToString(reader["SourceRepositoryID"]);

                            if (reader["CommunityDescription"] != DBNull.Value)                            
                                document.Community = Convert.ToString(reader["CommunityDescription"]);

                            if (reader["XACMLDocumentId"] != DBNull.Value)
                                document.XACMLDocumentId = Convert.ToString(reader["XACMLDocumentId"]);
                            

                            documents.Add(document);
                        }
                    }
                }

                this.Result.IsSuccess = true;
            }

            catch (Exception ex)
            {
                this.Result.IsSuccess = false;
                this.Result.SetError(ErrorCode.UnknownException, ex.Message);
            }
            finally
            {
                document = null;
            }
            return this.Result;
        }

        /// <summary>
        /// To retrieve the list of all modules for share document purpose.
        /// </summary>
        /// <param name="c32Sections"></param>
        /// <returns></returns>
        public Result GetC32Sections_TODO(out List<C32Section> c32Sections)
        {
            c32Sections = new List<C32Section>();
            try
            {
                IDataReader reader = null;
                C32Section C32Section = null;
                Component Section = null;
                int ModuleId = 0;
                // Fetch C32 Modules 
                using (DbCommand dbCommand = _dataAccessManager.GetStoredProcCommand("GetC32Modules"))
                {
                    reader = _dataAccessManager.ExecuteReader(dbCommand);
                }

                //Fill the Modules and sections.
                while (reader.Read())
                {
                    if (ModuleId != Convert.ToInt32(reader["ModuleId"]))
                    {
                        C32Section = new C32Section();
                        if (reader["ModuleId"] != DBNull.Value)
                            ModuleId = C32Section.Id = Convert.ToInt32(reader["ModuleId"]);
                        if (reader["ModuleName"] != DBNull.Value)
                            C32Section.Name = Convert.ToString(reader["ModuleName"]);
                        if (reader["Optionality"] != DBNull.Value)
                            C32Section.Optionality = Convert.ToString(reader["Optionality"]);
                        if (reader["Repeatable"] != DBNull.Value)
                            C32Section.Repeatable = Convert.ToBoolean(reader["Repeatable"]);
                        if (reader["DisplayOrder"] != DBNull.Value)
                            C32Section.DisplayOrder = Convert.ToInt32(reader["DisplayOrder"]);
                        if (reader["LOINCCode"] != DBNull.Value)
                            C32Section.LONICCode = Convert.ToString(reader["LOINCCode"]);
                        if (reader["TemplateId"] != DBNull.Value)
                            C32Section.TemplateId = Convert.ToString(reader["TemplateId"]);
                        c32Sections.Add(C32Section);
                    }
                    if (reader["SectionId"] != DBNull.Value)
                    {
                        Section = new Component();
                        Section.Id = Convert.ToInt32(reader["SectionId"]);
                        if (reader["ModuleName"] != DBNull.Value)
                            Section.Name = Convert.ToString(reader["ModuleName"]);
                        if (reader["SectionOptionality"] != DBNull.Value)
                            Section.Optionality = Convert.ToString(reader["SectionOptionality"]);
                        if (reader["SectionRepeatable"] != DBNull.Value)
                            Section.Optionality = Convert.ToString(reader["SectionRepeatable"]);
                        if (reader["SectionDisplayOrder"] != DBNull.Value)
                            Section.DisplayOrder = Convert.ToInt32(reader["SectionDisplayOrder"]);
                        if (reader["SectionLOINCCode"] != DBNull.Value)
                            Section.LONICCode = Convert.ToString(reader["SectionLOINCCode"]);
                        if (reader["SectionTemplateId"] != DBNull.Value)
                            Section.TemplateId = Convert.ToString(reader["SectionTemplateId"]);
                        C32Section.ChildSections.Add(Section);
                    }
                }

                this.Result.IsSuccess = true;
            }
            catch (Exception ex)
            {

                this.Result.IsSuccess = false;
                this.Result.SetError(ErrorCode.UnknownException, ex.Message);
            }
            return this.Result;
        }

        /// <summary>
        /// To retrieve the list of all modules for share document purpose.
        /// </summary>
        /// <param name="c32Sections"></param>
        /// <returns></returns>
        public Result GetC32Sections(out List<C32Section> c32Sections)
        {  
            c32Sections = new List<C32Section>();
            try
            {
                IDataReader reader = null;
                C32Section C32Section = null;
                Component Section = null;                
                int ModuleId = 0;              
                
                // Fetch C32 Modules 
                using (DbCommand dbCommand = _dataAccessManager.GetStoredProcCommand("GetC32Modules"))
                {
                    reader = _dataAccessManager.ExecuteReader(dbCommand);
                }

                //Fill the Modules and sections.
                while (reader.Read())
                {
                    if (ModuleId != Convert.ToInt32(reader["ModuleId"]))
                    {
                        C32Section = new C32Section();
                        if (reader["ModuleId"] != DBNull.Value)
                            ModuleId = C32Section.Id = Convert.ToInt32(reader["ModuleId"]);
                        if (reader["ModuleName"] != DBNull.Value)
                            C32Section.Name = Convert.ToString(reader["ModuleName"]);
                        if (reader["Optionality"] != DBNull.Value)
                            C32Section.Optionality = Convert.ToString(reader["Optionality"]);
                        //if (reader["Repeatable"] != DBNull.Value)
                            //C32Section.Repeatable = Convert.ToString(reader["Repeatable"]);
                        if (reader["DisplayOrder"] != DBNull.Value)
                            C32Section.DisplayOrder = Convert.ToInt32(reader["DisplayOrder"]);
                        if (reader["LOINCCode"] != DBNull.Value)
                            C32Section.LONICCode = Convert.ToString(reader["LOINCCode"]);
                        if (reader["TemplateId"] != DBNull.Value)
                            C32Section.TemplateId = Convert.ToString(reader["TemplateId"]);
                        c32Sections.Add(C32Section);
                    }
                    if (reader["SectionId"] != DBNull.Value)
                    {
                        Section = new Component();
                        Section.Id = Convert.ToInt32(reader["SectionId"]);
                        if (reader["ModuleName"] != DBNull.Value)
                            Section.Name = Convert.ToString(reader["SectionName"]);
                        if (reader["SectionOptionality"] != DBNull.Value)
                            Section.Optionality = Convert.ToString(reader["SectionOptionality"]);
                        if (reader["SectionRepeatable"] != DBNull.Value)
                            Section.Optionality = Convert.ToString(reader["SectionRepeatable"]);
                        if (reader["SectionDisplayOrder"] != DBNull.Value)
                            Section.DisplayOrder = Convert.ToInt32(reader["SectionDisplayOrder"]);
                        if (reader["SectionLOINCCode"] != DBNull.Value)
                            Section.LONICCode = Convert.ToString(reader["SectionLOINCCode"]);
                        if (reader["SectionTemplateId"] != DBNull.Value)
                            Section.TemplateId = Convert.ToString(reader["SectionTemplateId"]);
                        C32Section.ChildSections.Add(Section);
                    }
                }

                this.Result.IsSuccess = true;
            }
            catch (Exception ex)
            {
                this.Result.IsSuccess = false;
                this.Result.SetError(ErrorCode.UnknownException, ex.Message);
            }
            return this.Result;
        }

        /// <summary>
        /// This method returns document metadata for the specified documentID
        /// </summary>
        /// <param name="documentId"></param>
        /// <returns></returns>
        public Result GetDocumentMetaData(string documentId, out MobiusDocument document)
        {           
            document = null;
            try
            {
                this.Result.IsSuccess = true;
                using (DbCommand dbCommand = _dataAccessManager.GetStoredProcCommand("GetDocumentMetaData"))
                {
                    _dataAccessManager.AddInParameter(dbCommand, "@DocumentID", DbType.String, documentId);
                    using (IDataReader reader = _dataAccessManager.ExecuteReader(dbCommand))
                    {
                        while (reader.Read())
                        {
                            document = new MobiusDocument();

                            if (reader["OriginalDocumentID"] != DBNull.Value)
                                document.DocumentUniqueId = Convert.ToString(reader["OriginalDocumentID"]);

                            if (reader["PatientID"] != DBNull.Value)
                                document.SourcePatientId = Convert.ToString(reader["PatientID"]);

                            if (reader["DocumentTitle"] != DBNull.Value)
                                document.DocumentTitle = Convert.ToString(reader["DocumentTitle"]);

                            if (reader["Author"] != DBNull.Value)
                                document.Author = Convert.ToString(reader["Author"]);

                            if (reader["CreatedDate"] != DBNull.Value)
                                document.CreatedOn = Convert.ToString(reader["CreatedDate"]);

                            if (reader["DataSource"] != DBNull.Value)
                                document.DataSource = Convert.ToString(reader["DataSource"]);

                            if (reader["IsShared"] != DBNull.Value)
                                document.IsShared = Convert.ToBoolean(reader["IsShared"]);

                            if (reader["SourceCommunityID"] != DBNull.Value)
                                document.SourceCommunityId = Convert.ToString(reader["SourceCommunityID"]);

                            if (reader["SourceCommunityID"] != DBNull.Value)
                                document.SourceCommunityId = Convert.ToString(reader["SourceCommunityID"]);

                            if (reader["XACMLDocumentId"] != DBNull.Value)
                                document.XACMLDocumentId = Convert.ToString(reader["XACMLDocumentId"]);


                            if (reader["SourceRepositoryID"] != DBNull.Value)
                                document.RepositoryUniqueId = Convert.ToString(reader["SourceRepositoryID"]);

                            if (reader["Reposed"] != DBNull.Value)
                                document.Reposed = Convert.ToBoolean(reader["Reposed"]);

                            if (reader["Location"] != DBNull.Value)
                                document.Location = Convert.ToString(reader["Location"]);
                            else
                                document.Location = "";
                            if (reader["CommunityDescription"] != DBNull.Value) //SourceRepositoryID
                                document.Community = Convert.ToString(reader["CommunityDescription"]);

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

        /// <summary>
        /// this method updates document metadata
        /// </summary>
        /// <param name="documentId"></param>
        /// <param name="location"></param>
        /// <returns></returns>
        public bool UpdateDocumentMetadata(string documentId, string location)
        {            
            int returnValue = 0;
            try
            {
                using (DbCommand dbCommand = _dataAccessManager.GetStoredProcCommand("UpdateDocumentMetadata"))
                {
                    _dataAccessManager.AddInParameter(dbCommand, "@DocumentID", DbType.String, documentId);
                    _dataAccessManager.AddInParameter(dbCommand, "@Location", DbType.String, location);
                    returnValue = Convert.ToInt32(_dataAccessManager.ExecuteNonQuery(dbCommand));
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return (returnValue == 1 ? true : false);
        }

        /// <summary>
        /// Save C32Document MetaData
        /// </summary>
        /// <param name="documentMetadata"></param>
        /// <returns></returns>
        public bool SaveC32DocumentMetaData(DocumentMetadata documentMetadata)
        {            
            int returnValue = 0;
            try
            {
                //dbCommand = _dataAccessManager.GetStoredProcCommand("AddReferPatient");
                using (DbCommand dbCommand = _dataAccessManager.GetStoredProcCommand("SaveDocumentDetail"))
                {
                    _dataAccessManager.AddInParameter(dbCommand, "@OriginalDocumentID", DbType.String, documentMetadata.OriginalDocumentId);
                    _dataAccessManager.AddInParameter(dbCommand, "@PatientID", DbType.String, documentMetadata.PatientId);
                    _dataAccessManager.AddInParameter(dbCommand, "@UploadedBy", DbType.String, documentMetadata.UploadedBy);
                    _dataAccessManager.AddInParameter(dbCommand, "@DocumentTitle", DbType.String, documentMetadata.DocumentTitle);
                    _dataAccessManager.AddInParameter(dbCommand, "@Author", DbType.String, documentMetadata.Author);
                    _dataAccessManager.AddInParameter(dbCommand, "@CreatedDate", DbType.String, documentMetadata.CreatedDate);
                    _dataAccessManager.AddInParameter(dbCommand, "@DocumentSource", DbType.String, documentMetadata.DocumentSource);
                    _dataAccessManager.AddInParameter(dbCommand, "@SourceCommunityID", DbType.String, documentMetadata.SourceCommunityId);
                    _dataAccessManager.AddInParameter(dbCommand, "@SourceRepositoryID", DbType.String, documentMetadata.SourceRepositryId);
                    _dataAccessManager.AddInParameter(dbCommand, "@FacilityID", DbType.String, documentMetadata.FacilityId);
                    _dataAccessManager.AddInParameter(dbCommand, "@Reposed", DbType.Boolean, documentMetadata.Reposed);
                    _dataAccessManager.AddInParameter(dbCommand, "@FilePath", DbType.String, documentMetadata.FilePath);
                    _dataAccessManager.AddInParameter(dbCommand, "@FileName", DbType.String, documentMetadata.FileName);
                    _dataAccessManager.AddInParameter(dbCommand, "@XACMLfileName", DbType.String, documentMetadata.XACMLfileName);

                    returnValue = Convert.ToInt32(_dataAccessManager.ExecuteNonQuery(dbCommand));
                }
            }
            catch (Exception)
            {
                //TODO
            }
            return (returnValue == 1 ? true : false);
        }

        /// <summary>
        /// Save C32Document MetaData
        /// </summary>
        /// <param name="documentMetadata"></param>
        /// <returns></returns>
        public bool SaveDocumentMetadata(DocumentMetadata documentMetadata)
        {
            bool saved = false;
            int returnValue = 0;
            try
            {
               using (DbCommand dbCommand = _dataAccessManager.GetStoredProcCommand("SaveDocumentMetadata"))
               {
                _dataAccessManager.AddInParameter(dbCommand, "originalDocumentID", DbType.String, documentMetadata.OriginalDocumentId);
                _dataAccessManager.AddInParameter(dbCommand, "patientID", DbType.String, documentMetadata.PatientId);
                _dataAccessManager.AddInParameter(dbCommand, "uploadedBy", DbType.String, documentMetadata.UploadedBy);

                _dataAccessManager.AddInParameter(dbCommand, "documentTitle", DbType.String, documentMetadata.DocumentTitle);
                _dataAccessManager.AddInParameter(dbCommand, "author", DbType.String, documentMetadata.Author);
                _dataAccessManager.AddInParameter(dbCommand, "createdDate", DbType.String, documentMetadata.CreatedDate);
                _dataAccessManager.AddInParameter(dbCommand, "dataSource", DbType.String, documentMetadata.DocumentSource);

                _dataAccessManager.AddInParameter(dbCommand, "sourceCommunityID", DbType.String, documentMetadata.SourceCommunityId);
                _dataAccessManager.AddInParameter(dbCommand, "sourceRepositryID", DbType.String, documentMetadata.SourceRepositryId);
                _dataAccessManager.AddInParameter(dbCommand, "facilityID", DbType.String, documentMetadata.FacilityId);
                _dataAccessManager.AddInParameter(dbCommand, "reposed", DbType.Boolean, documentMetadata.Reposed);

                _dataAccessManager.AddInParameter(dbCommand, "location", DbType.String, documentMetadata.FilePath);
                _dataAccessManager.AddInParameter(dbCommand, "xacmlDocumentId", DbType.String, documentMetadata.XacmlDocumentId);
                _dataAccessManager.AddInParameter(dbCommand, "isShared", DbType.Boolean, documentMetadata.IsShared);
                _dataAccessManager.AddInParameter(dbCommand, "sharedId", DbType.String, documentMetadata.SharedId);
                _dataAccessManager.AddInParameter(dbCommand, "fileName", DbType.String, documentMetadata.FileName);
                returnValue = Convert.ToInt32(_dataAccessManager.ExecuteNonQuery(dbCommand));
                if (returnValue == 1)
                    saved = true;
                return saved;
               }
            }
            catch (Exception)
            {
                //TODO
            }
            return saved;
        }


        /// <summary>
        ///  Delete C32DocumentMetaData
        /// </summary>
        /// <param name="originalDocumentID"></param>
        public void DeleteC32DocumentMetaData(string originalDocumentID)
        {            
            try
            {

                using (DbCommand dbCommand = _dataAccessManager.GetStoredProcCommand("DeleteDocumentDetail"))
                {
                    _dataAccessManager.AddInParameter(dbCommand, "@OriginalDocumentID", DbType.String, originalDocumentID);
                    _dataAccessManager.ExecuteNonQuery(dbCommand);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }


        /// <summary>
        /// Save sharing details 
        /// </summary>
        /// <param name="originalDocumentId"></param>
        /// <param name="ruleStartDate"></param>
        /// <param name="ruleEndDate"></param>
        /// <param name="subject"></param>
        /// <param name="purposeOfUse"></param>
        /// <returns></returns>
        public bool SaveSharingDetails(string originalDocumentId, string ruleStartDate, string ruleEndDate, List<string> lstSubject, string purposeOfUse)
        {
            bool savedandupdated = false;
            int returnValue = 0;
            try
            {
                foreach (string tempSubjecet in lstSubject)
                {
                    using (DbCommand dbCommand = _dataAccessManager.GetStoredProcCommand("SavePatientSharedInfo"))
                    {
                        _dataAccessManager.AddInParameter(dbCommand, "@OriginalDocumentID", DbType.String, originalDocumentId);
                        _dataAccessManager.AddInParameter(dbCommand, "@RuleStartDate", DbType.String, ruleStartDate);
                        _dataAccessManager.AddInParameter(dbCommand, "@RuleEndDate", DbType.String, ruleEndDate);
                        _dataAccessManager.AddInParameter(dbCommand, "@Subject", DbType.String, tempSubjecet);
                        _dataAccessManager.AddInParameter(dbCommand, "@PurposeforUse", DbType.String, purposeOfUse);
                        returnValue = Convert.ToInt32(_dataAccessManager.ExecuteNonQuery(dbCommand));
                        if (returnValue == 2)
                        {
                            savedandupdated = true;
                        }
                        else
                        {
                            savedandupdated = false;
                            break;
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message.ToString());
            }
            return savedandupdated;
        }

        /// <summary>
        /// This method will return the text against the passed Role Id or Value
        /// </summary>
        /// <param name="subject">Logged in user role value</param>
        /// <returns>return the description against the passed Role Id or Value</returns>
        public string GetRole(int subject)
        {
            string userRoleDescription = string.Empty;
            try
            {
                using (DbCommand command = DataAccessManager.GetInstance.GetStoredProcCommand("GetRoles"))
                {
                    DataAccessManager.GetInstance.AddInParameter(command, "@Subject", DbType.Int32, subject);
                    using (DataSet roleData = DataAccessManager.GetInstance.ExecuteDataSet(command))
                    {
                        if (roleData.Tables.Count > 0 && roleData.Tables[0].Rows[0]["Role"] != null)
                        {
                            userRoleDescription = roleData.Tables[0].Rows[0]["Role"].ToString();
                        }
                    }
                }
            }
            catch (Exception)
            {
                userRoleDescription = "";
            }
            return userRoleDescription;
        }



    }
}