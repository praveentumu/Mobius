
namespace Mobius.EventNotification
{
    using System;
    using System.Collections.Generic;
    using System.Data;// data access manager and DBtype
    using System.Data.Common;// dbcommand
    using FirstGenesis.Mobius.Server.DataAccessLayer;
    using Mobius.CoreLibrary;


    internal class ActionHandler
    {

        internal void Process(int eventActionId, string actionType)
        {
            EventHandler eventHandler = null;
            EventAction eventAction = null;
            int eventId;
            EventActionData eventData = new EventActionData();
            eventHandler = new EventHandler();
            //Get the EventAction records from EventActions tables based on PK 
            eventId = GetEventAction(eventActionId);
            //Get the Event information based on EventActions.EventId
            eventData = GetEventData(eventId);

            eventAction = eventHandler.GetEventActionDetails(eventActionId, actionType);

            //Based on Action type execute the method(s) for email......
            foreach (Action action in eventAction.EventActions)
            {
                switch (action.Name)
                {
                    case ActionType.Mail:
                        this.SendMailNotification(action, eventData, eventActionId);
                        break;
                    case ActionType.Audit:
                        this.CreateAuditEvent(eventData, eventId, eventActionId);
                        break;

                }
            }

            //If anything wrong/error Log entry into EvetAction tables
            //and on success update records.

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="eventActionId"></param>
        /// <param name="eventActionDataId"></param>
        /// <returns></returns>
        internal int CreateEventAction(int eventActionId, int eventActionDataId)
        {
            DbCommand dbCommand;
            int recordInserted = 0;

            DataAccessManager dataAccessManager = DataAccessManager.GetInstance;
            dbCommand = dataAccessManager.GetStoredProcCommand("InsertEventActions");
            dataAccessManager.AddInParameter(dbCommand, "@EventActionId", DbType.Int32, eventActionId);
            dataAccessManager.AddInParameter(dbCommand, "@EventActionDataId", DbType.Int32, eventActionDataId);
            dataAccessManager.AddOutParameter(dbCommand, "@RecordId", DbType.Int32, 0);
            recordInserted = dataAccessManager.ExecuteNonQuery(dbCommand);
            if (recordInserted == 1)
                return (int)dbCommand.Parameters["@RecordId"].Value;
            else
                return -1;



        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="action"></param>
        /// <param name="eventData"></param>
        /// <param name="eventActionsId"></param>
        internal void SendMailNotification(Action action, EventActionData eventData, int eventActionsId)
        {
            string template = string.Empty;
            List<string> ccReciepent = null;

            if (action != null)
            {
                template = action.Template;

                //case of Emergency Access
                if (eventData.Event == EventType.EmergencyOverride)
                {
                    ccReciepent = CreateConsentTable(ref eventData, template.Contains("#ConsentTable#"));
                }

                IDictionary<string, object> data = eventData.GetTemplateData() as IDictionary<string, object>;

                // to replace all the key values from the template by EventData
                foreach (KeyValuePair<string, object> keyPair in data)
                {
                    template = template.Replace("#" + keyPair.Key + "#", keyPair.Value.ToString());
                }


                foreach (string mailReciepts in eventData.EmailRecipients)
                {
                    //EmailHelper.SendMail(ConfigurationSetting.SmtpHost, ConfigurationSetting.SmtpPort, ConfigurationSetting.SmtpUserName,
                    //                   ConfigurationSetting.SmtpPassword, ConfigurationSetting.SmtpSSL, ConfigurationSetting.EmailFromAddress, mailReciepts, action.Subject, template, ccReciepent);
                    EmailHelper.SendMail(MobiusAppSettingReader.SmtpHost, MobiusAppSettingReader.SmtpPort, MobiusAppSettingReader.SmtpUserName,
                                       MobiusAppSettingReader.SmtpPassword, MobiusAppSettingReader.SmtpEnableSSL, MobiusAppSettingReader.EmailFromAddress, mailReciepts, action.Subject, template, ccReciepent);
                }

                UpdateLogEvents(eventActionsId);
            }

        }


      /// <summary>
      ///  Create consent table structure to send via email
      /// </summary>
      /// <param name="eventData"></param>
      /// <param name="hasConsentTable"></param>
      /// <returns></returns>
        private List<string> CreateConsentTable(ref EventActionData eventData, bool hasConsentTable)
        {
            List<string> ccReciepent = new List<string>();
            DataAccessManager dataAccessManager = DataAccessManager.GetInstance;
            DataSet dataSet = null;
            DataTable dataTable = null;

            DbCommand dbCommand = dataAccessManager.GetStoredProcCommand("GetPatientProviderDetails");
            dataAccessManager.AddInParameter(dbCommand, "MpiId", DbType.String, eventData.PatientId);
            dataAccessManager.AddInParameter(dbCommand, "ProviderEmail", DbType.String, eventData.UserName);
            dataSet = dataAccessManager.ExecuteDataSet(dbCommand);
            if (dataSet.Tables.Count > 0)
            {
                dataTable = dataSet.Tables[0];
                if (dataTable.Rows.Count > 0)
                {
                    foreach (DataRow row in dataTable.Rows)
                    {

                        if (hasConsentTable) //mailing case of Patient
                        {

                            eventData.ReferPatientId = row["PatientFirstName"] != DBNull.Value
                                                           ? row["PatientFirstName"].ToString()
                                                           : "";
                            eventData.ProviderName = Convert.ToBoolean(row["IndividualProvider"])
                                                         ? row["ProviderFirstName"].ToString() + " " +
                                                           row["ProviderMiddleName"].ToString() + " " +
                                                           row["ProviderLastName"].ToString()
                                                         : row["OrganizationName"].ToString();
                            eventData.EmailRecipients.Add(row["PatientEmail"] != DBNull.Value
                                                              ? row["PatientEmail"].ToString()
                                                              : null); //Patient Email
                            ccReciepent.Add(row["AdminEmail"] != DBNull.Value ? row["AdminEmail"].ToString() : null);// Admin Email Address
                        }
                        else //mailing case of provider
                        {
                            eventData.ReferPatientId = row["PatientFirstName"].ToString() + " " +
                                                       row["PatientMiddleName"].ToString() + " " +
                                                       row["PatientLastName"].ToString();
                            eventData.ProviderName = Convert.ToBoolean(row["IndividualProvider"])
                                                         ? row["ProviderFirstName"].ToString()
                                                         : row["OrganizationName"].ToString();
                            eventData.EmailRecipients.Add(eventData.UserName); //provider Email Id

                            eventData.Serial = MobiusAppSettingReader.EmergencyOverriddenTime.ToString();

                        }
                    }
                }
            }

            if (hasConsentTable)   //mailing case of Patient
            {
                dbCommand = dataAccessManager.GetStoredProcCommand("GetPatientConsent");
                dataAccessManager.AddInParameter(dbCommand, "MpiId", DbType.String, eventData.PatientId);
                dataSet = dataAccessManager.ExecuteDataSet(dbCommand);

                string tableString = "";
                tableString += "<table border=1><tr>";
                tableString += "<td><b>Role</b></td>";
                tableString += "<td><b>Purpose</b></td>";
                tableString += "<td><b>Rule Start Date</b></td>";
                tableString += "<td><b>Rule End Date</b></td>";
                tableString += "<td><b>Permission</b></td>";
                tableString += "<td><b>Active</b></td>";
                tableString += "</tr>";
                if (dataSet.Tables.Count > 0)
                {
                    dataTable = dataSet.Tables[0];
                    if (dataTable.Rows.Count > 0)
                    {
                        foreach (DataRow row in dataTable.Rows)
                        {
                            tableString += "<tr><td>";
                            tableString += row["Role"] != DBNull.Value ? row["Role"].ToString() : "";
                            tableString += "</td><td>";
                            tableString += row["Code"] != DBNull.Value ? row["Code"].ToString() : "";
                            tableString += "</td><td>";
                            tableString += row["RuleStartDate"] != DBNull.Value ? row["RuleStartDate"].ToString() : "";
                            tableString += "</td><td>";
                            tableString += row["RuleEndDate"] != DBNull.Value ? row["RuleEndDate"].ToString() : "";
                            tableString += "</td><td>";
                            tableString += row["Allow"] != DBNull.Value
                                               ? (Convert.ToBoolean(row["Allow"]) == true ? "Allow" : "Deny")
                                               : "";
                            tableString += "</td><td>";
                            tableString += row["Active"] != DBNull.Value
                                               ? (Convert.ToBoolean(row["Active"]) == true ? "Yes" : "No")
                                               : "";
                            tableString += "</td></tr>";
                        }
                    }
                    else
                    {
                        tableString += "<tr><td  colspan=6> No consent is available </td></tr>";
                    }
                    tableString += "</table>";
                }
                eventData.ConsentTable = tableString;

            }

            return ccReciepent;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="eventActionId"></param>
        private void UpdateLogEvents(int eventActionId)
        {
            DbCommand dbCommand;
            int recordInserted = 0;
            DataAccessManager dataAccessManager = DataAccessManager.GetInstance;
            dbCommand = dataAccessManager.GetStoredProcCommand("UpdateEventActionProcess");
            dataAccessManager.AddInParameter(dbCommand, "@Id", DbType.Int32, eventActionId);
            recordInserted = dataAccessManager.ExecuteNonQuery(dbCommand);

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="eventActionId"></param>
        /// <returns></returns>
        private int GetEventAction(int eventActionId)
        {
            DbCommand dbCommand;
            DataTable dtEventIds;
            int EventIds = 0;
            DataAccessManager dataAccessManager = DataAccessManager.GetInstance;
            dbCommand = dataAccessManager.GetStoredProcCommand("GetEventActions");
            dataAccessManager.AddInParameter(dbCommand, "@EventActionId", DbType.Int32, eventActionId);
            dtEventIds = dataAccessManager.ExecuteDataSet(dbCommand).Tables[0];

            foreach (DataRow row in dtEventIds.Rows)
            {
                EventIds = Convert.ToInt32(row["EventId"]);

            }

            return EventIds;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="eventId"></param>s
        /// <returns></returns>
        private EventActionData GetEventData(int eventId)
        {
            DbCommand dbCommand;
            string eventDataXML = string.Empty;
            DataTable dteventActionData = null;
            EventActionData eventActionData = new EventActionData();
            DataAccessManager dataAccessManager = DataAccessManager.GetInstance;
            dbCommand = dataAccessManager.GetStoredProcCommand("GetEventData");
            dataAccessManager.AddInParameter(dbCommand, "@EventId", DbType.Int32, eventId);
            dteventActionData = dataAccessManager.ExecuteDataSet(dbCommand).Tables[0];
            foreach (DataRow row in dteventActionData.Rows)
            {
                eventDataXML = row["EventDataXML"].ToString();
            }

            eventActionData = Mobius.CoreLibrary.XmlSerializerHelper.DeserializeObject(eventDataXML, typeof(EventActionData)) as EventActionData;
            return eventActionData;

        }

        /// <summary>
        /// This will save the record in db and return PK of inserted row.
        /// </summary>
        /// <param name="eventData"></param>
        /// <param name="eventId"></param>
        /// <param name="eventActionId"></param>
        /// <returns>PK of inserted row.</returns>
        internal int CreateAuditEvent(EventActionData eventData, Int64 eventId, int eventActionId)
        {
            DbCommand dbCommand;
            int iRecordValue = 0;
            int recordInserted = 0;

            DataAccessManager dataAccessManager = DataAccessManager.GetInstance;
            dbCommand = dataAccessManager.GetStoredProcCommand("InsertAuditLog");

            if (string.IsNullOrWhiteSpace(eventData.PatientId))
            {
                dataAccessManager.AddInParameter(dbCommand, "PatientId", DbType.String, DBNull.Value);
            }
            else
            {
                dataAccessManager.AddInParameter(dbCommand, "PatientId", DbType.String, eventData.PatientId);
            }

            if (string.IsNullOrWhiteSpace(eventData.CommunityId))
            {
                dataAccessManager.AddInParameter(dbCommand, "CommunityId", DbType.String, DBNull.Value);
            }
            else
            {
                dataAccessManager.AddInParameter(dbCommand, "CommunityId", DbType.String, eventData.CommunityId);
            }
            if (string.IsNullOrWhiteSpace(eventData.DocumentId))
            {
                dataAccessManager.AddInParameter(dbCommand, "DocumentId", DbType.String, DBNull.Value);
            }
            else
            {
                dataAccessManager.AddInParameter(dbCommand, "DocumentId", DbType.String, eventData.DocumentId);
            }

            if (string.IsNullOrWhiteSpace(eventData.Purpose))
            {
                dataAccessManager.AddInParameter(dbCommand, "Purpose", DbType.String, DBNull.Value);
            }
            else
            {
                dataAccessManager.AddInParameter(dbCommand, "Purpose", DbType.String, eventData.Purpose);
            }

            if (string.IsNullOrWhiteSpace(eventData.Subject))
            {
                dataAccessManager.AddInParameter(dbCommand, "Subject", DbType.String, DBNull.Value);
            }
            else
            {
                dataAccessManager.AddInParameter(dbCommand, "Subject", DbType.String, eventData.Subject);
            }
            if (string.IsNullOrWhiteSpace(eventData.ErrorMessage))
            {
                dataAccessManager.AddInParameter(dbCommand, "ErrorMessage", DbType.String, DBNull.Value);
            }
            else
            {
                dataAccessManager.AddInParameter(dbCommand, "ErrorMessage", DbType.String, eventData.ErrorMessage);
            }
            if (string.IsNullOrWhiteSpace(eventData.UserName))
            {
                dataAccessManager.AddInParameter(dbCommand, "@CreatedBy", DbType.String, "SYSTEM");
            }
            else
            {
                dataAccessManager.AddInParameter(dbCommand, "@CreatedBy", DbType.String, eventData.UserName);
            }

            dataAccessManager.AddInParameter(dbCommand, "@EventId", DbType.Int32, eventId == 0 ? 0 : eventId.GetHashCode());
            dataAccessManager.AddInParameter(dbCommand, "@NetworkAccessPointTypeCode", DbType.Int32, eventData.NetworkAccessPointTypeCode.GetHashCode());
            dataAccessManager.AddInParameter(dbCommand, "@NetworkAccessPointID", DbType.String, eventData.NetworkAccessPointID);
            dataAccessManager.AddInParameter(dbCommand, "@MessageType", DbType.String, eventData.MessageType);
            dataAccessManager.AddInParameter(dbCommand, "@Message", DbType.Binary, eventData.RequestObject);
            dataAccessManager.AddOutParameter(dbCommand, "@RecordId", DbType.Int32, 0);

            recordInserted = dataAccessManager.ExecuteNonQuery(dbCommand);


            if (recordInserted == 1)
            {
                iRecordValue = (int)dbCommand.Parameters["@RecordId"].Value;
            }
            else
            {
                iRecordValue = -1;
            }

            UpdateLogEvents(eventActionId);
            return iRecordValue;
        }



    }
}
