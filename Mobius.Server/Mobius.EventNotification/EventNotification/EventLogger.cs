
namespace Mobius.EventNotification
{
    using System;
    using System.Collections.Generic;
    using System.Data;// data access manager and DBtype
    using System.Data.Common;// dbcommand
    using System.Threading.Tasks;
    using FirstGenesis.Mobius.Server.DataAccessLayer;
    using Mobius.CoreLibrary;
    // core library for 

    public sealed class EventLogger
    {

        /// <summary>
        /// 
        /// </summary>
        /// <param name="eventData">The data for the event.</param>
        /// <param name="synchronousCall"> call is Synchronous</param>
        public void LogEvent(EventActionData eventData, bool synchronousCall)
        {
            int eventActionDataId = this.CreateEvent(eventData);    
            if(synchronousCall)
                this.ProcessAction(eventActionDataId, eventData.Event.GetHashCode());                        
            else
                this.ProcessActionAsync(eventActionDataId, eventData.Event.GetHashCode());                        
        }

        /// <summary>
        /// Logs an event.
        /// </summary>
        /// <param name="eventData">The data for the event.</param>
        public void LogEvent(EventActionData eventData)
        {
            LogEvent(eventData, false);
        }
      

        /// <summary>
        /// Process the event by creating and processing the event action(s).
        /// </summary>
        /// <param name="eventId">Id of Event process</param>
        private void ProcessActionAsync(int eventDataId, int eventId)
        {
            try
            {
                TaskFactory taskFactory = new TaskFactory();
                taskFactory.StartNew(() => this.CreateEventAction(eventDataId, eventId)).Wait();

            }            
            catch
            {

            }
        }

        /// <summary>
        /// Process the event by creating and processing the event action(s).
        /// </summary>
        /// <param name="eventId">Id of Event process</param>
        private void ProcessAction(int eventDataId, int eventId)
        {
            try
            {

                this.CreateEventAction(eventDataId, eventId);
            }
            catch (Exception)
            {

            }
        }


        /// <summary>
        /// Process Event.
        /// </summary>
        /// <param name="eventId">The id of the event.</param>
        private void CreateEventAction(int eventDataId, int eventId)
        {
            EventHandler eventHandler = new EventHandler();
            EventAction eventAction = eventHandler.GetEventAction(eventId);
            ActionHandler actionHandler = new ActionHandler();
            List<KeyValuePair<int, ActionType>> actionList = new List<KeyValuePair<int, ActionType>>();


            if (eventAction != null)
            {
                //Create entries in db for each event->action(s) and PK will be added to collection for further process 
                foreach (Action action in eventAction.EventActions)
                {
                    switch (action.Name)
                    {
                        case ActionType.Mail:
                            actionList.Add(new KeyValuePair<int, ActionType>(actionHandler.CreateEventAction(action.Id, eventDataId), ActionType.Mail));
                            break;
                        case ActionType.Audit:
                            actionList.Add(new KeyValuePair<int, ActionType>(actionHandler.CreateEventAction(action.Id, eventDataId), ActionType.Audit));
                            break;
                    }
                }
            }


            foreach (var item in actionList)
            {
                actionHandler.Process(item.Key,item.Value.ToString());
            }

        }

        /// <summary>
        /// This will save the record in db and return PK of inserted row.
        /// </summary>
        /// <param name="eventData"></param>
        /// <returns>PK of inserted row</returns>
        private int CreateEvent(EventActionData eventData)
        {
            DbCommand dbCommand;
            int recordInserted = 0;

            string SerializedObjectEventActionData = XmlSerializerHelper.SerializeObject(eventData);

            DataAccessManager dataAccessManager = DataAccessManager.GetInstance;
            dbCommand = dataAccessManager.GetStoredProcCommand("InsertEvent");
            dataAccessManager.AddInParameter(dbCommand, "@EventId", DbType.Int32, eventData.Event.GetHashCode());
            dataAccessManager.AddInParameter(dbCommand, "@EventActionData", DbType.Xml, SerializedObjectEventActionData);
            if (!string.IsNullOrEmpty(eventData.UserName))
                dataAccessManager.AddInParameter(dbCommand, "@CreatedBy", DbType.String, eventData.UserName.ToString());
            else
                dataAccessManager.AddInParameter(dbCommand, "@CreatedBy", DbType.String, "SYSTEM");
            dataAccessManager.AddOutParameter(dbCommand, "@RecordId", DbType.Int32, 0);
            recordInserted = dataAccessManager.ExecuteNonQuery(dbCommand);
            if (recordInserted == 1)
                return (int)dbCommand.Parameters["@RecordId"].Value;
            else
                return -1;
        }     
    }
}
