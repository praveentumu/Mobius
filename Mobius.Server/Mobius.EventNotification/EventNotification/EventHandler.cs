using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;// data access manager and DBtype
using System.Data.Common;// dbcommand
using FirstGenesis.Mobius.Server.DataAccessLayer;// core library for 
using Mobius.CoreLibrary;



namespace Mobius.EventNotification
{
    internal class EventHandler
    {


        internal EventAction GetEventAction(int EventId)
        {
            EventAction eventAction = new EventAction();
            DataAccessManager dataAccessManager = DataAccessManager.GetInstance;
            DbCommand dbCommand;
            DataSet dataSet = null;
            DataTable actionDataTable = new DataTable();
            DataTableReader actionDataTableReader = new DataTableReader(actionDataTable);
            List<Action> actionsList = new List<Action>();

            
            dbCommand = dataAccessManager.GetStoredProcCommand("GetEventAction");
            dataAccessManager.AddInParameter(dbCommand,"@EventId",DbType.String,EventId);
            dataSet = dataAccessManager.ExecuteDataSet(dbCommand);


            if (dataSet.Tables != null && dataSet.Tables.Count > 0 && dataSet.Tables[0].Rows != null && dataSet.Tables[0].Rows.Count > 0)
            {   
                eventAction.Id = (int)dataSet.Tables[0].Rows[0]["Id"];
                eventAction.Name = (EventType)dataSet.Tables[0].Rows[0]["EventId"];

                foreach (DataRow datarow in dataSet.Tables[0].Rows)
                {
                        Action act = new Action();
                        //act.Id = (int)(datarow["ActionId"]);
                        act.Id = (int)(datarow["Id"]);
                        act.Name = (ActionType)(datarow["ActionId"]);
                        if(datarow["Template"] != DBNull.Value)
                            act.Template = (string)(datarow["Template"]);
                        if (datarow["Subject"] != DBNull.Value)
                            act.Subject = (string)(datarow["Subject"]);
                        actionsList.Add(act);
                }
                eventAction.EventActions = actionsList;
            }
            return eventAction;
        }

        internal EventAction GetEventActionDetails(int EventActionId, string ActionName)
        {
            EventAction eventAction = new EventAction();
            DataAccessManager dataAccessManager = DataAccessManager.GetInstance;
            DbCommand dbCommand;
            DataSet dataSet = null;
            DataTable actionDataTable = new DataTable();
            DataTableReader actionDataTableReader = new DataTableReader(actionDataTable);
            List<Action> actionsList = new List<Action>();


            dbCommand = dataAccessManager.GetStoredProcCommand("GetActionDetails");
            dataAccessManager.AddInParameter(dbCommand, "@EventActionId", DbType.Int64, EventActionId);
            dataAccessManager.AddInParameter(dbCommand, "@ActionName", DbType.String, ActionName);
            dataSet = dataAccessManager.ExecuteDataSet(dbCommand);


            if (dataSet.Tables.Count > 0)
            {
                eventAction.Id = (int)dataSet.Tables[0].Rows[0]["Id"];
               
                foreach (DataRow datarow in dataSet.Tables[0].Rows)
                {
                    Action act = new Action();
                    act.Id = (int)(datarow["ActionId"]);
                    act.Name = (ActionType)(datarow["ActionId"]);
                    if (datarow["Template"] != DBNull.Value)
                        act.Template = (string)(datarow["Template"]);
                    if (datarow["Subject"] != DBNull.Value)
                        act.Subject = (string)(datarow["Subject"]);
                   
                    actionsList.Add(act);
                }
                eventAction.EventActions = actionsList;
            }
            return eventAction;
        }
    }
}
