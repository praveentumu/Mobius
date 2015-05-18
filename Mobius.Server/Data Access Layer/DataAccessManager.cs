#region Using Namespace

using System;
using System.Data;
using Microsoft.Practices.EnterpriseLibrary.Data;
using System.Data.SqlClient;
using System.Data.Common;

using System.Text.RegularExpressions;
using Mobius.CoreLibrary;

#endregion

/// Created On : 12/05/2008
/// Data Access Manager is used to interact with the Database 

namespace FirstGenesis.Mobius.Server.DataAccessLayer
{
    public class DataAccessManager
    {
        // private string _strSQLConn = null;
        GenericDatabase _objGenericDB = null;
       
        private static readonly DataAccessManager _objDAMClass = new DataAccessManager();

        //Constructor is Protected to implement Singelton Pattern

        protected DataAccessManager()
        {
            _objGenericDB = new GenericDatabase(GetConnection(), SqlClientFactory.Instance);
        }

        private string GetConnection()
        {
            return MobiusAppSettingReader.ConnectionString;
        }

        public static DataAccessManager GetInstance
        {
            get
            {
                return _objDAMClass;
            }
        }

        /// <summary>
        /// Get the dbConnection for connection
        /// </summary>
        public static DbConnection DbConnection
        {
            get
            {
                return DataAccessManager.DbConnection;
            }
        }

        /// <summary>
        /// Method:ExecuteNonQuery
        /// Purpose: It is used Insert/Update/Delete Data into Tables
        /// </summary>
        /// <param name="storedProcedureName"></param>
        /// <param name="ProcParams"></param>
        /// <returns></returns>
        /// 
        public int ExecuteNonQuery(string storedProcedureName, SqlParameter[] procParams)
        {

            int intRowAffected = 0;
            DbCommand command = null;
            try
            {
                if (procParams != null)
                {
                    command = new SqlCommand(storedProcedureName);
                    command.CommandType = CommandType.StoredProcedure;
                    foreach (SqlParameter SqlParam in procParams)
                    {
                        command.Parameters.Add(SqlParam);
                    }
                }

                intRowAffected = _objGenericDB.ExecuteNonQuery(command);
                command = null;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }

            return intRowAffected;
        }

        /// <summary>
        ///  Method:ExecuteNonQuery
        /// Purpose: It is used Insert/Update/Delete Data into Tables
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        public int ExecuteNonQuery(DbCommand command)
        {

            int intRowAffected = 0;
            try
            {
                intRowAffected = _objGenericDB.ExecuteNonQuery(command);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());

            }

            return intRowAffected;
        }

        /// <summary>
        ///  Method:ExecuteNonQuery
        /// Purpose: It is used Insert/Update/Delete Data into Tables
        /// </summary>
        /// <param name="command"></param>
        /// <param name="transaction"></param>
        /// <returns></returns>
        public int ExecuteNonQuery(DbCommand command, DbTransaction transaction)
        {

            int intRowAffected = 0;
            try
            {
                intRowAffected = _objGenericDB.ExecuteNonQuery(command, transaction);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());

            }

            return intRowAffected;
        }

        /// <summary>
        ///  Method:ExecuteNonQuery
        /// Purpose: It is used Insert/Update/Delete Data into Tables
        /// </summary>
        /// <param name="commandType"></param>
        /// <param name="commandText"></param>
        /// <returns></returns>
        public int ExecuteNonQuery(CommandType commandType, string commandText)
        {
            int intRowAffected = 0;
            try
            {

                intRowAffected = _objGenericDB.ExecuteNonQuery(commandType, commandText);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }

            return intRowAffected;
        }

        /// <summary>
        ///  Method:ExecuteNonQuery
        /// Purpose: It is used Insert/Update/Delete Data into Tables
        /// </summary>
        /// <param name="transaction"></param>
        /// <param name="commandType"></param>
        /// <param name="commandText"></param>
        /// <returns></returns>
        public int ExecuteNonQuery(DbTransaction transaction, CommandType commandType, string commandText)
        {

            int intRowAffected = 0;
            try
            {
                intRowAffected = _objGenericDB.ExecuteNonQuery(transaction, commandType, commandText);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }

            return intRowAffected;
        }

        /// <summary>
        ///  Method:ExecuteNonQuery
        /// Purpose: It is used Insert/Update/Delete Data into Tables
        /// </summary>
        /// <param name="storedProcedureName"></param>
        /// <param name="parameterValues"></param>
        /// <returns></returns>
        public int ExecuteNonQuery(string storedProcedureName, params object[] parameterValues)
        {
            int intRowAffected = 0;
            try
            {
                intRowAffected = _objGenericDB.ExecuteNonQuery(storedProcedureName, parameterValues);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());

            }

            return intRowAffected;
        }

        /// <summary>
        ///  Method:ExecuteNonQuery
        /// Purpose: It is used Insert/Update/Delete Data into Tables
        /// </summary>
        /// <param name="transaction"></param>
        /// <param name="storedProcedureName"></param>
        /// <param name="parameterValues"></param>
        /// <returns></returns>
        public int ExecuteNonQuery(DbTransaction transaction, string storedProcedureName, params object[] parameterValues)
        {

            int intRowAffected = 0;
            try
            {

                intRowAffected = _objGenericDB.ExecuteNonQuery(transaction, storedProcedureName, parameterValues);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());

            }
            return intRowAffected;
        }

        /// <summary>
        /// It is used to Select Single Row
        /// </summary>
        /// <param name="strStoredProcedureName"></param>
        /// <returns></returns>
        public object ExecuteScalar(string storedProcedureName)
        {
            object objReturnValue = null;
            DbCommand command = null;
            try
            {
                command = new SqlCommand(storedProcedureName);
                command.CommandType = CommandType.StoredProcedure;
                objReturnValue = _objGenericDB.ExecuteScalar(command);
                command = null;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());

            }

            return objReturnValue;
        }

        /// <summary>
        /// It is used to Select Single Row
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        public object ExecuteScalar(DbCommand command)
        {

            object objReturnValue = null;
            try
            {
                objReturnValue = _objGenericDB.ExecuteScalar(command);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());

            }

            return objReturnValue;
        }

        /// <summary>
        /// It is used to Select Single Row
        /// </summary>
        /// <param name="command"></param>
        /// <param name="transaction"></param>
        /// <returns></returns>
        public object ExecuteScalar(DbCommand command, DbTransaction transaction)
        {

            object objReturnValue = null;
            try
            {
                objReturnValue = _objGenericDB.ExecuteScalar(command, transaction);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());

            }

            return objReturnValue;
        }

        /// <summary>
        /// It is used to Select Single Row
        /// </summary>
        /// <param name="storedProcedureName"></param>
        /// <param name="parameterValues"></param>
        /// <returns></returns>
        public object ExecuteScalar(string storedProcedureName, params object[] parameterValues)
        {

            object objReturnValue = null;
            try
            {
                objReturnValue = _objGenericDB.ExecuteScalar(storedProcedureName, parameterValues);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());

            }

            return objReturnValue;
        }

        /// <summary>
        /// It is used to Select Single Row
        /// </summary>
        /// <param name="transaction"></param>
        /// <param name="storedProcedureName"></param>
        /// <param name="parameterValues"></param>
        /// <returns></returns>
        public object ExecuteScalar(DbTransaction transaction, string storedProcedureName, params object[] parameterValues)
        {

            object objReturnValue = null;
            try
            {
                objReturnValue = _objGenericDB.ExecuteScalar(transaction, storedProcedureName, parameterValues);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());

            }

            return objReturnValue;
        }

        /// <summary>
        /// It is used to Select Single Row
        /// </summary>
        /// <param name="commandType"></param>
        /// <param name="commandText"></param>
        /// <returns></returns>
        public object ExecuteScalar(CommandType commandType, string commandText)
        {
            object objReturnValue = null;
            try
            {
                objReturnValue = _objGenericDB.ExecuteScalar(commandType, commandText);
            }
            catch (Exception Ex)
            {
                throw new Exception(Ex.Message.ToString());
            }

            return objReturnValue;
        }

        /// <summary>
        /// It is used to Select Single Row
        /// </summary>
        /// <param name="transaction"></param>
        /// <param name="commandType"></param>
        /// <param name="commandText"></param>
        /// <returns></returns>
        public object ExecuteScalar(DbTransaction transaction, CommandType commandType, string commandText)
        {
            object objReturnValue = null;
            try
            {
                objReturnValue = _objGenericDB.ExecuteScalar(transaction, commandType, commandText);
            }
            catch (Exception Ex)
            {
                throw new Exception(Ex.Message.ToString());
            }

            return objReturnValue;
        }

        /// <summary>
        /// It is used to Return Dbcommand Object
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public DbCommand GetSqlStringCommand(string query)
        {
            DbCommand DbCommand = null;
            try
            {
                DbCommand = _objGenericDB.GetSqlStringCommand(query);
            }
            catch (Exception Ex)
            {
                throw new Exception(Ex.Message.ToString());
            }
            return DbCommand;
        }

        /// <summary>
        /// It is used to Return SqlCommand Object
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public SqlCommand GetSqlCommand(string storedProc)
        {
            SqlCommand sqlCommand = null;
            SqlConnection sqlConnection = new SqlConnection(GetConnection());
            try
            {
                sqlCommand = new SqlCommand(storedProc, sqlConnection);
                sqlCommand.CommandType = CommandType.StoredProcedure;
            }
            catch (Exception Ex)
            {
                throw new Exception(Ex.Message.ToString());
            }
            return sqlCommand;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sqlCommand"></param>
        /// <param name="table"></param>
        /// <returns></returns>
        public int UpdateBatch(SqlCommand sqlCommand, DataTable table)
        {
            int numberOfRowEffected = 0;
            SqlConnection sqlConnection = null;
            sqlConnection = new SqlConnection(GetConnection());
            try
            {
                SqlDataAdapter sqlDataAdapter = (SqlDataAdapter)_objGenericDB.GetDataAdapter();
                sqlConnection.Open();
                sqlCommand.Connection = sqlConnection;
                sqlDataAdapter.InsertCommand = sqlCommand;
                // Specify the number of records to be Inserted/Updated in one go. Default is 1.
                sqlDataAdapter.UpdateBatchSize = 2;
                numberOfRowEffected = sqlDataAdapter.Update(table);
                sqlConnection.Close();
            }
            catch (Exception Ex)
            {
                throw new Exception(Ex.Message.ToString());
            }
            finally
            {
                if (sqlConnection.State == ConnectionState.Open)
                { sqlConnection.Close(); }
            }
            return numberOfRowEffected;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dataSet"></param>
        /// <param name="tableName"></param>
        /// <param name="spName"></param>
        /// <param name="dbTransaction"></param>
        /// <returns></returns>
        public int UpdateBatch(DataSet dataSet, string tableName, DbCommand insertCommand, DbCommand updateCommand, DbCommand deleteCommand, DbTransaction dbTransaction)
        {
            int numberOfRowEffected = 0;
            try
            {
                numberOfRowEffected = _objGenericDB.UpdateDataSet(dataSet, tableName, insertCommand, updateCommand, deleteCommand, dbTransaction);

            }
            catch (Exception Ex)
            {
                throw new Exception(Ex.Message.ToString());
            }

            return numberOfRowEffected;
        }

        /// <summary>
        /// It is used to Return Dbcommand Object
        /// </summary>
        /// <param name="storedProcedureName"></param>
        /// <returns></returns>
        public DbCommand GetStoredProcCommand(string storedProcedureName)
        {
            DbCommand DbCommand = null;
            try
            {

                DbCommand = _objGenericDB.GetStoredProcCommand(storedProcedureName);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }
            return DbCommand;

        }

        /// <summary>
        /// It is used to return Object of data adapter
        /// </summary>
        /// <returns></returns>
        public DbDataAdapter GetDataAdapter()
        {
            DbDataAdapter DbDataAdapter = null;
            try
            {
                DbDataAdapter = _objGenericDB.GetDataAdapter();
            }
            catch (SqlException SQLex)
            {
                throw new Exception(SQLex.ToString());

            }
            return DbDataAdapter;
        }

        /// <summary>
        /// It is used to return Object of  Sql Data Adapter
        /// </summary>
        /// <returns></returns>
        public SqlDataAdapter GetSqlDataAdapter()
        {
            SqlDataAdapter sqlDataAdapter = null;
            try
            {
                sqlDataAdapter = (SqlDataAdapter)_objGenericDB.GetDataAdapter();
            }
            catch (SqlException SQLex)
            {
                throw new Exception(SQLex.ToString());
            }
            return sqlDataAdapter;
        }

        /// <summary>
        /// It is used to return Object of data adapter
        /// </summary>
        /// <returns></returns>
        public SqlCommandBuilder GetCommandBuilder(SqlDataAdapter sqlDataAdapter)
        {
            SqlCommandBuilder sqlCommandBuilder = null;
            try
            {
                sqlCommandBuilder = new SqlCommandBuilder(sqlDataAdapter);
            }
            catch (SqlException SQLex)
            {
                throw new Exception(SQLex.ToString());
            }
            return sqlCommandBuilder;
        }

        /// <summary>
        /// It is used to Fill Dataset
        /// </summary>
        /// <param name="command"></param>
        /// <param name="dataSet"></param>
        /// <param name="tableName"></param>
        public void LoadDataSet(DbCommand command, DataSet dataSet, string tableName)
        {
            try
            {
                _objGenericDB.LoadDataSet(command, dataSet, tableName);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }
        }

        /// <summary>
        ///  It is used to Fill Dataset
        /// </summary>
        /// <param name="command"></param>
        /// <param name="dataSet"></param>
        /// <param name="tableName"></param>
        /// <param name="transaction"></param>
        public void LoadDataSet(DbCommand command, DataSet dataSet, string tableName, DbTransaction transaction)
        {
            try
            {
                _objGenericDB.LoadDataSet(command, dataSet, tableName, transaction);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());

            }
        }

        /// <summary>
        /// It is used to Fill Dataset
        /// </summary>
        /// <param name="command"></param>
        /// <param name="dataSet"></param>
        /// <param name="tableNames"></param>
        public void LoadDataSet(DbCommand command, DataSet dataSet, string[] tableNames)
        {
            try
            {
                _objGenericDB.LoadDataSet(command, dataSet, tableNames);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }

        }

        /// <summary>
        /// It is used to Fill Dataset
        /// </summary>
        /// <param name="command"></param>
        /// <param name="dataSet"></param>
        /// <param name="tableNames"></param>
        /// <param name="transaction"></param>
        public void LoadDataSet(DbCommand command, DataSet dataSet, string[] tableNames, DbTransaction transaction)
        {
            try
            {
                _objGenericDB.LoadDataSet(command, dataSet, tableNames, transaction);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());

            }
        }

        /// <summary>
        /// It is used to Fill Dataset
        /// </summary>
        /// <param name="storedProcedureName"></param>
        /// <param name="dataSet"></param>
        /// <param name="tableNames"></param>
        /// <param name="parameterValues"></param>
        public void LoadDataSet(string storedProcedureName, DataSet dataSet, string[] tableNames, params object[] parameterValues)
        {
            try
            {
                _objGenericDB.LoadDataSet(storedProcedureName, dataSet, tableNames, parameterValues);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());

            }
        }

        /// <summary>
        /// It is used to Fill Dataset
        /// </summary>
        /// <param name="transaction"></param>
        /// <param name="storedProcedureName"></param>
        /// <param name="dataSet"></param>
        /// <param name="tableNames"></param>
        /// <param name="parameterValues"></param>
        public void LoadDataSet(DbTransaction transaction, string storedProcedureName, DataSet dataSet, string[] tableNames, params object[] parameterValues)
        {
            try
            {
                _objGenericDB.LoadDataSet(transaction, storedProcedureName, dataSet, tableNames, parameterValues);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());

            }
        }

        /// <summary>
        /// It is used to Fill Dataset
        /// </summary>
        /// <param name="commandType"></param>
        /// <param name="commandText"></param>
        /// <param name="dataSet"></param>
        /// <param name="tableNames"></param>
        public void LoadDataSet(CommandType commandType, string commandText, DataSet dataSet, string[] tableNames)
        {
            try
            {
                _objGenericDB.LoadDataSet(commandType, commandText, dataSet, tableNames);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());

            }
        }

        /// <summary>
        /// It is used to Fill Dataset
        /// </summary>
        /// <param name="transaction"></param>
        /// <param name="commandType"></param>
        /// <param name="commandText"></param>
        /// <param name="dataSet"></param>
        /// <param name="tableNames"></param>
        public void LoadDataSet(DbTransaction transaction, CommandType commandType, string commandText, DataSet dataSet, string[] tableNames)
        {
            try
            {
                _objGenericDB.LoadDataSet(transaction, commandType, commandText, dataSet, tableNames);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());

            }
        }

        /// <summary>
        /// It is used to Fill Dataset and then Return Dataset
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        public DataSet ExecuteDataSet(DbCommand command)
        {
            DataSet objDataSet = null;

            try
            {
                objDataSet = _objGenericDB.ExecuteDataSet(command);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());

            }
            return objDataSet;
        }

        /// <summary>
        /// It is used to Fill Dataset and then Return Dataset
        /// </summary>
        /// <param name="command"></param>
        /// <param name="transaction"></param>
        /// <returns></returns>
        public DataSet ExecuteDataSet(DbCommand command, DbTransaction transaction)
        {
            DataSet objDataSet = new DataSet();

            try
            {
                objDataSet = _objGenericDB.ExecuteDataSet(command, transaction);

            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());

            }
            return objDataSet;
        }

        /// <summary>
        /// It is used to Fill Dataset and then Return Dataset
        /// </summary>
        /// <param name="storedProcedureName"></param>
        /// <param name="parameterValues"></param>
        /// <returns></returns>
        public virtual DataSet ExecuteDataSet(string storedProcedureName, params object[] parameterValues)
        {
            DataSet objDataSet = new DataSet();

            try
            {
                objDataSet = _objGenericDB.ExecuteDataSet(storedProcedureName, parameterValues);

            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());

            }
            return objDataSet;
        }

        /// <summary>
        /// It is used to Fill Dataset and then Return Dataset
        /// </summary>
        /// <param name="transaction"></param>
        /// <param name="storedProcedureName"></param>
        /// <param name="parameterValues"></param>
        /// <returns></returns>
        public DataSet ExecuteDataSet(DbTransaction transaction, string storedProcedureName, params object[] parameterValues)
        {
            DataSet objDataSet = new DataSet();

            try
            {
                objDataSet = _objGenericDB.ExecuteDataSet(transaction, storedProcedureName, parameterValues);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());

            }
            return objDataSet;
        }

        /// <summary>
        /// It is used to Fill Dataset and then Return Dataset
        /// </summary>
        /// <param name="commandType"></param>
        /// <param name="commandText"></param>
        /// <returns></returns>
        public DataSet ExecuteDataSet(CommandType commandType, string commandText)
        {
            DataSet objDataSet = new DataSet();

            try
            {
                objDataSet = _objGenericDB.ExecuteDataSet(commandType, commandText);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());

            }
            return objDataSet;
        }

        /// <summary>
        /// It is used to Fill Dataset and then Return Dataset
        /// </summary>
        /// <param name="transaction"></param>
        /// <param name="commandType"></param>
        /// <param name="commandText"></param>
        /// <returns></returns>
        public DataSet ExecuteDataSet(DbTransaction transaction, CommandType commandType, string commandText)
        {
            DataSet objDataSet = new DataSet();

            try
            {
                objDataSet = _objGenericDB.ExecuteDataSet(transaction, commandType, commandText);
            }
            catch (SqlException SQLex)
            {
                throw new Exception(SQLex.ToString());


            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());

            }
            return objDataSet;
        }

        /// <summary>
        /// It is used to  Return DataReader Object
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        public IDataReader ExecuteReader(DbCommand command)
        {
            IDataReader objIDataReader = null;

            try
            {
                objIDataReader = _objGenericDB.ExecuteReader(command);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());

            }
            return objIDataReader;
        }

        /// <summary>
        /// It is used to  Return DataReader Object
        /// </summary>
        /// <param name="command"></param>
        /// <param name="transaction"></param>
        /// <returns></returns>
        public IDataReader ExecuteReader(DbCommand command, DbTransaction transaction)
        {
            IDataReader objIDataReader = null;

            try
            {
                objIDataReader = _objGenericDB.ExecuteReader(command, transaction);
            }
            catch (SqlException SQLex)
            {
                throw new Exception(SQLex.ToString());

            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());


            }
            return objIDataReader;
        }

        /// <summary>
        /// It is used to  Return DataReader Object
        /// </summary>
        /// <param name="storedProcedureName"></param>
        /// <param name="parameterValues"></param>
        /// <returns></returns>
        public IDataReader ExecuteReader(string storedProcedureName, params object[] parameterValues)
        {
            IDataReader objIDataReader = null;

            try
            {
                objIDataReader = _objGenericDB.ExecuteReader(storedProcedureName, parameterValues);

            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());

            }
            return objIDataReader;
        }

        /// <summary>
        /// It is used to  Return DataReader Object
        /// </summary>
        /// <param name="transaction"></param>
        /// <param name="storedProcedureName"></param>
        /// <param name="parameterValues"></param>
        /// <returns></returns>
        public IDataReader ExecuteReader(DbTransaction transaction, string storedProcedureName, params object[] parameterValues)
        {
            IDataReader objIDataReader = null;

            try
            {
                objIDataReader = _objGenericDB.ExecuteReader(transaction, storedProcedureName, parameterValues);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());

            }
            return objIDataReader;
        }

        /// <summary>
        /// It is used to  Return DataReader Object
        /// </summary>
        /// <param name="commandType"></param>
        /// <param name="commandText"></param>
        /// <returns></returns>
        public IDataReader ExecuteReader(CommandType commandType, string commandText)
        {
            IDataReader objIDataReader = null;

            try
            {
                objIDataReader = _objGenericDB.ExecuteReader(commandType, commandText);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());

            }
            return objIDataReader;
        }

        /// <summary>
        /// It is used to  Return DataReader Object
        /// </summary>
        /// <param name="transaction"></param>
        /// <param name="commandType"></param>
        /// <param name="commandText"></param>
        /// <returns></returns>
        public IDataReader ExecuteReader(DbTransaction transaction, CommandType commandType, string commandText)
        {
            IDataReader objIDataReader = null;

            try
            {
                objIDataReader = _objGenericDB.ExecuteReader(transaction, commandType, commandText);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());

            }
            return objIDataReader;
        }

        /// <summary>
        /// It is used to  add Parameters
        /// </summary>
        /// <param name="command"></param>
        /// <param name="name"></param>
        /// <param name="dbType"></param>
        /// <param name="size"></param>
        /// <param name="direction"></param>
        /// <param name="nullable"></param>
        /// <param name="precision"></param>
        /// <param name="scale"></param>
        /// <param name="sourceColumn"></param>
        /// <param name="sourceVersion"></param>
        /// <param name="value"></param>
        public void AddParameter(DbCommand command, string name, DbType dbType, int size, ParameterDirection direction, bool nullable, byte precision, byte scale, string sourceColumn, DataRowVersion sourceVersion, object value)
        {
            try
            {
                _objGenericDB.AddParameter(command, name, dbType, size, direction, nullable, precision, scale, sourceColumn, sourceVersion, value);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());

            }


        }

        /// <summary>
        /// It is used to  add Parameters
        /// </summary>
        /// <param name="command"></param>
        /// <param name="name"></param>
        /// <param name="dbType"></param>
        /// <param name="direction"></param>
        /// <param name="sourceColumn"></param>
        /// <param name="sourceVersion"></param>
        /// <param name="value"></param>
        public void AddParameter(DbCommand command, string name, DbType dbType, ParameterDirection direction, string sourceColumn, DataRowVersion sourceVersion, object value)
        {
            try
            {
                _objGenericDB.AddParameter(command, name, dbType, direction, sourceColumn, sourceVersion, value);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());

            }

        }

        /// <summary>
        /// It is Use to get Out Parameter from SP
        /// </summary>
        /// <param name="command"></param>
        /// <param name="name"></param>
        /// <param name="dbType"></param>
        /// <param name="size"></param>
        public void AddOutParameter(DbCommand command, string name, DbType dbType, int size)
        {
            try
            {
                _objGenericDB.AddOutParameter(command, name, dbType, size);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());

            }

        }

        /// <summary>
        /// It is Use to get In Parameter from SP
        /// </summary>
        /// <param name="command"></param>
        /// <param name="name"></param>
        /// <param name="dbType"></param>
        public void AddInParameter(DbCommand command, string name, DbType dbType)
        {
            try
            {
                _objGenericDB.AddInParameter(command, name, dbType);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());

            }

        }

        /// <summary>
        /// It is Use to get In Parameter from SP
        /// </summary>
        /// <param name="command"></param>
        /// <param name="name"></param>
        /// <param name="dbType"></param>
        /// <param name="value"></param>
        public void AddInParameter(DbCommand command, string name, DbType dbType, object value)
        {
            try
            {
                _objGenericDB.AddInParameter(command, name, dbType, value);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());

            }

        }

        /// <summary>
        /// It is Use to get In Parameter from SP
        /// </summary>
        /// <param name="command"></param>
        /// <param name="name"></param>
        /// <param name="dbType"></param>
        /// <param name="sourceColumn"></param>
        /// <param name="sourceVersion"></param>
        public void AddInParameter(DbCommand command, string name, DbType dbType, string sourceColumn, DataRowVersion sourceVersion)
        {
            try
            {
                _objGenericDB.AddInParameter(command, name, dbType, sourceColumn, sourceVersion);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());

            }

        }
    }

    public class AdminService
    {
        DataAccessManager _dataAccessManager = DataAccessManager.GetInstance;
        public DataSet GetAllUserInfo(int userTypeId, int facilityId)
        {
            DbCommand dbCommand;
            DataSet dataSet = null;
            try
            {
                dbCommand = _dataAccessManager.GetStoredProcCommand("GetUsers");
                _dataAccessManager.AddInParameter(dbCommand, "userTypeId", DbType.Int32, userTypeId);
                _dataAccessManager.AddInParameter(dbCommand, "facilityId", DbType.Int32, facilityId);
                dataSet = _dataAccessManager.ExecuteDataSet(dbCommand);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }
            return dataSet;
        }
        public int UpdateDocumentMetadata(string DocumentID, string Location)
        {
            DbCommand dbCommand = null;
            int returnValue = 0;
            try
            {
                dbCommand = _dataAccessManager.GetStoredProcCommand("UpdateDocumentMetadata");
                _dataAccessManager.AddInParameter(dbCommand, "@DocumentID", DbType.String, DocumentID);
                _dataAccessManager.AddInParameter(dbCommand, "@Location", DbType.String, Location);
                returnValue = Convert.ToInt32(_dataAccessManager.ExecuteNonQuery(dbCommand));
            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }
            return returnValue;

        }
        public DataSet GetUploadedDocumentMetaData(string MPIID, string DocID)
        {
            DbCommand dbCommand;
            DataSet dataSet = null;
            try
            {
                dbCommand = _dataAccessManager.GetStoredProcCommand("GetUploadedDocumentMetaData");
                _dataAccessManager.AddInParameter(dbCommand, "@MPIID", DbType.String, MPIID);
                _dataAccessManager.AddInParameter(dbCommand, "@DocID", DbType.String, DocID);
                dataSet = _dataAccessManager.ExecuteDataSet(dbCommand);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }
            return dataSet;
        }

        public DataSet GetCommunitiesForMPIID(string MPIID)
        {
            DbCommand dbCommand;
            DataSet dataSet = null;
            try
            {
                dbCommand = _dataAccessManager.GetStoredProcCommand("GetCommunitiesForMPIID");
                _dataAccessManager.AddInParameter(dbCommand, "@MPIID", DbType.String, MPIID);
                dataSet = _dataAccessManager.ExecuteDataSet(dbCommand);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }
            return dataSet;
        }
        //public DataSet GetSpecificPatientConstent(string MPIID, int PatientConsentId)
        //{
        //    DbCommand dbCommand;
        //    DataSet dataSet = null;
        //    try
        //    {
        //        dbCommand = _dataAccessManager.GetStoredProcCommand("GetSpecificPatientConstent");
        //        _dataAccessManager.AddInParameter(dbCommand, "@MPIId", DbType.String, MPIID);
        //        _dataAccessManager.AddInParameter(dbCommand, "@PatientConsentID", DbType.Int32, PatientConsentId);
        //        dataSet = _dataAccessManager.ExecuteDataSet(dbCommand);
        //    }
        //    catch (Exception ex)
        //    {
        //        throw new Exception(ex.ToString());
        //    }
        //    return dataSet;
        //}

        public DataSet GetPatientConstent(string MpiId)
        {
            DbCommand dbCommand;
            DataSet dataSet = null;
            try
            {
                dbCommand = _dataAccessManager.GetStoredProcCommand("GetPatientConsent");
                _dataAccessManager.AddInParameter(dbCommand, "MpiId", DbType.String, MpiId);


                dataSet = _dataAccessManager.ExecuteDataSet(dbCommand);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }
            return dataSet;
        }
        public DataSet GetDistinctUserInfo(int userTypeId, int facilityId)
        {
            DbCommand dbCommand;
            DataSet dataSet = null;
            try
            {
                dbCommand = _dataAccessManager.GetStoredProcCommand("GetDistinctUsers");
                _dataAccessManager.AddInParameter(dbCommand, "userTypeId", DbType.Int32, userTypeId);
                _dataAccessManager.AddInParameter(dbCommand, "facilityId", DbType.Int32, facilityId);
                dataSet = _dataAccessManager.ExecuteDataSet(dbCommand);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }
            return dataSet;
        }

      

        public bool ExecuteSql(string script)
        {
            string connString = @"data source=RSI-2M576JZLTV9;initial catalog=MobiusServer;user id=sa;password=sa;";
            SqlConnection sqlCon = new SqlConnection(connString);
            if (sqlCon.State == ConnectionState.Closed)
            {
                try
                {
                    sqlCon.Open();
                }
                catch
                {
                    return false;
                }
            }
            string[] sqlLine;
            Regex regex = new Regex("^GO", RegexOptions.IgnoreCase | RegexOptions.Multiline);
            //string script2 = script1.Replace("\r", "");
            string txtSQL = script.Replace("\r", "");
            sqlLine = regex.Split(txtSQL);
            SqlCommand cmd = sqlCon.CreateCommand();
            cmd.Connection = sqlCon;
            foreach (string line in sqlLine)
            {
                if (line.Length > 0)
                {
                    cmd.CommandText = line;
                    cmd.CommandType = CommandType.Text;
                    try
                    {
                        cmd.ExecuteNonQuery();
                    }
                    catch (Exception ex)
                    {
                        throw new Exception(ex.ToString());
                        //logger.WriteLog(LogSeverity.ERROR, "InstallScript", ex, "Issue while running the database script");
                        // break;
                    }
                }
            }
            if (sqlCon != null)
                sqlCon.Close();
            return true;
        }

        public DataSet GetUserInfo(string userGuid)
        {
            DbCommand dbCommand;
            DataSet dataSet = null;
            try
            {
                dbCommand = _dataAccessManager.GetStoredProcCommand("GetUserDetails");
                _dataAccessManager.AddInParameter(dbCommand, "userGuid", DbType.String, userGuid);
                dataSet = _dataAccessManager.ExecuteDataSet(dbCommand);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }
            return dataSet;
        }
        public string GetCommnIdByPatientId(string UserID)
        {
            DbCommand dbCommand;
            string sUserID = "";
            try
            {
                DataAccessManager _dataAccessManager = DataAccessManager.GetInstance;
                dbCommand = _dataAccessManager.GetStoredProcCommand("GetCommonId");
                _dataAccessManager.AddInParameter(dbCommand, "UserID", DbType.String, UserID);
                sUserID = _dataAccessManager.ExecuteScalar(dbCommand).ToString();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }
            return sUserID;
        }
        public DataSet GetUsersGroup(string userId)
        {
            DbCommand dbCommand;
            DataSet dataSet = null;
            try
            {
                dbCommand = _dataAccessManager.GetStoredProcCommand("GetUsersGroup");
                _dataAccessManager.AddInParameter(dbCommand, "UserID", DbType.String, userId);
                dataSet = _dataAccessManager.ExecuteDataSet(dbCommand);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }
            return dataSet;
        }

        public DataSet FillListBox(int FacilityId)
        {
            DbCommand dbCommand;
            DataSet dataSet = null;
            try
            {
                dbCommand = _dataAccessManager.GetStoredProcCommand("FillListBox");
                _dataAccessManager.AddInParameter(dbCommand, "FacilityId", DbType.Int32, FacilityId);
                dataSet = _dataAccessManager.ExecuteDataSet(dbCommand);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }
            return dataSet;
        }

        public DataSet GetConfigurationUser(string UserId, int FacilityId)
        {
            DbCommand dbCommand;
            DataSet dataSet = null;
            try
            {
                dbCommand = _dataAccessManager.GetStoredProcCommand("GetConfigurationUser");
                _dataAccessManager.AddInParameter(dbCommand, "UserId", DbType.String, UserId);
                _dataAccessManager.AddInParameter(dbCommand, "FacilityId", DbType.Int32, FacilityId);
                dataSet = _dataAccessManager.ExecuteDataSet(dbCommand);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }
            return dataSet;
        }

        public DataSet GetConfigurationFacilityInfo(int FacilityId)
        {
            DbCommand dbCommand;
            DataSet dataSet = null;
            try
            {
                dbCommand = _dataAccessManager.GetStoredProcCommand("GetConfigurationFacilityInfo");
                _dataAccessManager.AddInParameter(dbCommand, "FacilityId", DbType.Int32, FacilityId);
                dataSet = _dataAccessManager.ExecuteDataSet(dbCommand);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }
            return dataSet;
        }
        public DataSet GetRootCAKey()
        {
            DbCommand dbCommand;
            DataSet dataSet = null;
            try
            {
                dbCommand = _dataAccessManager.GetStoredProcCommand("GetRootCAKey");
                dataSet = _dataAccessManager.ExecuteDataSet(dbCommand);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }
            return dataSet;
        }
        public DataSet GetDefaultPassword(int facilityId)
        {
            DbCommand dbCommand;
            DataSet dataSet = null;
            try
            {
                dbCommand = _dataAccessManager.GetStoredProcCommand("GetDefaultPassword");
                _dataAccessManager.AddInParameter(dbCommand, "facilityId", DbType.Int32, facilityId);
                dataSet = _dataAccessManager.ExecuteDataSet(dbCommand);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }
            return dataSet;
        }
        public DataSet SearchUserInfo(string userId, string firstName, string middleName, string lastName, string email, string dob, string ssn, int userTypeId, int facilityId, string EicSerialID)
        {
            DbCommand dbCommand;
            DataSet dataSet = null;
            try
            {
                dbCommand = _dataAccessManager.GetStoredProcCommand("SearchUser");
                _dataAccessManager.AddInParameter(dbCommand, "userId", DbType.String, userId.ToString());
                _dataAccessManager.AddInParameter(dbCommand, "FirstName", DbType.String, firstName.ToString());
                _dataAccessManager.AddInParameter(dbCommand, "MiddleName", DbType.String, middleName.ToString());
                _dataAccessManager.AddInParameter(dbCommand, "LastName", DbType.String, lastName.ToString());
                _dataAccessManager.AddInParameter(dbCommand, "Email", DbType.String, email.ToString());
                _dataAccessManager.AddInParameter(dbCommand, "DOB", DbType.String, dob.ToString());
                _dataAccessManager.AddInParameter(dbCommand, "SSN", DbType.String, ssn.ToString());
                _dataAccessManager.AddInParameter(dbCommand, "userTypeId", DbType.Int32, userTypeId);
                _dataAccessManager.AddInParameter(dbCommand, "facilityId", DbType.Int32, facilityId);
                _dataAccessManager.AddInParameter(dbCommand, "EicSerialID", DbType.String, EicSerialID);
                dataSet = _dataAccessManager.ExecuteDataSet(dbCommand);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }
            return dataSet;
        }
        public DataSet SearchUserInfo(string[] userInfoMask)
        {
            DbCommand dbCommand;
            DataSet dataSet = null;
            try
            {
                dbCommand = _dataAccessManager.GetStoredProcCommand("SearchUser");
                _dataAccessManager.AddInParameter(dbCommand, "userId", DbType.String, userInfoMask[0].ToString());
                _dataAccessManager.AddInParameter(dbCommand, "FirstName", DbType.String, userInfoMask[1].ToString());
                _dataAccessManager.AddInParameter(dbCommand, "MiddleName", DbType.String, userInfoMask[2].ToString());
                _dataAccessManager.AddInParameter(dbCommand, "LastName", DbType.String, userInfoMask[3].ToString());
                _dataAccessManager.AddInParameter(dbCommand, "Email", DbType.String, userInfoMask[4].ToString());
                _dataAccessManager.AddInParameter(dbCommand, "DOB", DbType.String, userInfoMask[5].ToString());
                _dataAccessManager.AddInParameter(dbCommand, "SSN", DbType.String, userInfoMask[6].ToString());
                _dataAccessManager.AddInParameter(dbCommand, "userTypeId", DbType.Int32, Convert.ToInt32(userInfoMask[7].ToString()));
                _dataAccessManager.AddInParameter(dbCommand, "facilityId", DbType.Int32, Convert.ToInt32(userInfoMask[8].ToString()));
                _dataAccessManager.AddInParameter(dbCommand, "eicGuid", DbType.String, userInfoMask[9].ToString());
                dataSet = _dataAccessManager.ExecuteDataSet(dbCommand);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }
            return dataSet;
        }
        public int CreateUser(string userGuid, string userId, string passwordHash, string firstName, string middleName, string lastName, string email, string dob, string ssn, int userTypeId, int facilityId, string eic, bool isActive, string unit, string initials, string category, string nationality, string force, string sex, string uic, string religion, string fmp, string race, string mos, string grade, string pubKey, string privKey, bool canWorkOffline, out int checkUniqueuserId)
        {
            DbCommand dbCommand = null;
            int returnValue = 0;
            checkUniqueuserId = 0;
            try
            {
                dbCommand = _dataAccessManager.GetStoredProcCommand("ManageUser");
                _dataAccessManager.AddInParameter(dbCommand, "userGuid", DbType.String, userGuid);
                _dataAccessManager.AddInParameter(dbCommand, "userId", DbType.String, userId.ToString());
                _dataAccessManager.AddInParameter(dbCommand, "PasswordHash", DbType.String, passwordHash.ToString());
                _dataAccessManager.AddInParameter(dbCommand, "FirstName", DbType.String, firstName.ToString());
                _dataAccessManager.AddInParameter(dbCommand, "MiddleName", DbType.String, middleName.ToString());
                _dataAccessManager.AddInParameter(dbCommand, "LastName", DbType.String, lastName.ToString());
                _dataAccessManager.AddInParameter(dbCommand, "Email", DbType.String, email.ToString());
                _dataAccessManager.AddInParameter(dbCommand, "DOB", DbType.String, dob.ToString());
                _dataAccessManager.AddInParameter(dbCommand, "SSN", DbType.String, ssn.ToString());
                _dataAccessManager.AddInParameter(dbCommand, "userTypeId", DbType.Int32, userTypeId);
                _dataAccessManager.AddInParameter(dbCommand, "facilityId", DbType.Int32, facilityId);
                _dataAccessManager.AddInParameter(dbCommand, "eicSerialId", DbType.String, eic.ToString());
                _dataAccessManager.AddInParameter(dbCommand, "IsActive", DbType.Boolean, isActive);
                _dataAccessManager.AddInParameter(dbCommand, "Unit", DbType.String, unit.ToString());
                _dataAccessManager.AddInParameter(dbCommand, "Initials", DbType.String, initials.ToString());
                _dataAccessManager.AddInParameter(dbCommand, "Category", DbType.String, category.ToString());
                _dataAccessManager.AddInParameter(dbCommand, "PubKey", DbType.String, pubKey.ToString());
                _dataAccessManager.AddInParameter(dbCommand, "PrivKey", DbType.String, privKey.ToString());
                _dataAccessManager.AddInParameter(dbCommand, "Nationality", DbType.String, nationality.ToString());
                _dataAccessManager.AddInParameter(dbCommand, "Force", DbType.String, force.ToString());
                _dataAccessManager.AddInParameter(dbCommand, "Sex", DbType.String, sex.ToString());
                _dataAccessManager.AddInParameter(dbCommand, "UIC", DbType.String, uic.ToString());
                _dataAccessManager.AddInParameter(dbCommand, "religion", DbType.String, religion.ToString());
                _dataAccessManager.AddInParameter(dbCommand, "fmp", DbType.String, fmp.ToString());
                _dataAccessManager.AddInParameter(dbCommand, "race", DbType.String, race.ToString());
                _dataAccessManager.AddInParameter(dbCommand, "mos", DbType.String, mos.ToString());
                _dataAccessManager.AddInParameter(dbCommand, "grade", DbType.String, grade.ToString());
                _dataAccessManager.AddInParameter(dbCommand, "canWorkOffline", DbType.Boolean, canWorkOffline);
                _dataAccessManager.AddOutParameter(dbCommand, "CheckuserId", DbType.Int32, checkUniqueuserId);

                returnValue = Convert.ToInt32(_dataAccessManager.ExecuteNonQuery(dbCommand));
                checkUniqueuserId = Convert.ToInt32(dbCommand.Parameters["CheckuserId"].Value);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }
            return returnValue;
        }
        public int CreateUser(string[] userInfo, out int checkUniqueuserId)
        {
            DbCommand dbCommand = null;
            int returnValue = 0;
            bool isActive = false;
            checkUniqueuserId = 0;
            try
            {
                dbCommand = _dataAccessManager.GetStoredProcCommand("ManageUser");
                _dataAccessManager.AddInParameter(dbCommand, "userGuid", DbType.String, userInfo[0].ToString());
                _dataAccessManager.AddInParameter(dbCommand, "userId", DbType.String, userInfo[1].ToString());
                _dataAccessManager.AddInParameter(dbCommand, "PasswordHash", DbType.String, userInfo[2].ToString());
                _dataAccessManager.AddInParameter(dbCommand, "FirstName", DbType.String, userInfo[3].ToString());
                _dataAccessManager.AddInParameter(dbCommand, "MiddleName", DbType.String, userInfo[4].ToString());
                _dataAccessManager.AddInParameter(dbCommand, "LastName", DbType.String, userInfo[5].ToString());
                _dataAccessManager.AddInParameter(dbCommand, "Email", DbType.String, userInfo[6].ToString());
                _dataAccessManager.AddInParameter(dbCommand, "DOB", DbType.String, userInfo[7].ToString());
                _dataAccessManager.AddInParameter(dbCommand, "SSN", DbType.String, userInfo[8].ToString());
                _dataAccessManager.AddInParameter(dbCommand, "userTypeId", DbType.Int32, Convert.ToInt32(userInfo[9].ToString()));
                _dataAccessManager.AddInParameter(dbCommand, "roleId", DbType.Int32, Convert.ToInt32(userInfo[10].ToString()));
                _dataAccessManager.AddInParameter(dbCommand, "facilityId", DbType.Int32, Convert.ToInt32(userInfo[11].ToString()));
                _dataAccessManager.AddInParameter(dbCommand, "eicSerialId", DbType.String, userInfo[12].ToString());
                if (userInfo[13].ToString() == "true")
                {
                    isActive = true;
                }
                else
                {
                    isActive = false;
                }
                _dataAccessManager.AddInParameter(dbCommand, "IsActive", DbType.Boolean, isActive);
                _dataAccessManager.AddInParameter(dbCommand, "Unit", DbType.String, userInfo[14].ToString());
                _dataAccessManager.AddInParameter(dbCommand, "Initials", DbType.String, userInfo[15].ToString());
                _dataAccessManager.AddInParameter(dbCommand, "Category", DbType.String, userInfo[16].ToString());
                _dataAccessManager.AddInParameter(dbCommand, "PubKey", DbType.String, userInfo[17].ToString());
                _dataAccessManager.AddInParameter(dbCommand, "PrivKey", DbType.String, userInfo[16].ToString());
                _dataAccessManager.AddOutParameter(dbCommand, "CheckuserId", DbType.Int32, checkUniqueuserId);

                returnValue = Convert.ToInt32(_dataAccessManager.ExecuteNonQuery(dbCommand));
                checkUniqueuserId = Convert.ToInt32(dbCommand.Parameters["CheckuserId"].Value);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }
            return returnValue;
        }
        public int DeleteUser(string userGuid)
        {
            DbCommand dbCommand;
            int returnValue = 0;
            try
            {
                dbCommand = _dataAccessManager.GetStoredProcCommand("DeleteUser");
                _dataAccessManager.AddInParameter(dbCommand, "userGuid", DbType.String, userGuid);
                returnValue = Convert.ToInt32(_dataAccessManager.ExecuteNonQuery(dbCommand));
            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }
            return returnValue;
        }
        public int SetDefaultPassword(int facilityId, string newPassword)
        {
            DbCommand dbCommand = null;
            int returnValue = 0;
            try
            {
                dbCommand = _dataAccessManager.GetStoredProcCommand("SetDefaultPassword");
                _dataAccessManager.AddInParameter(dbCommand, "facilityId", DbType.Int32, facilityId);
                _dataAccessManager.AddInParameter(dbCommand, "Password", DbType.String, newPassword);
                returnValue = Convert.ToInt32(_dataAccessManager.ExecuteNonQuery(dbCommand));
            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }
            return returnValue;
        }
        public int CheckUserId(string userId, out int checkUserId)
        {
            DbCommand dbCommand = null;
            int returnValue = 0;
            checkUserId = 0;
            try
            {
                dbCommand = _dataAccessManager.GetStoredProcCommand("chkUserId");
                _dataAccessManager.AddInParameter(dbCommand, "userId", DbType.String, userId.ToString());
                _dataAccessManager.AddOutParameter(dbCommand, "CheckUserId", DbType.Int32, checkUserId);

                returnValue = Convert.ToInt32(_dataAccessManager.ExecuteScalar(dbCommand));
                checkUserId = Convert.ToInt32(dbCommand.Parameters["CheckUserId"].Value);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }
            return returnValue;
        }
        public int CheckUniqueSSN(string SSN)
        {
            DbCommand dbCommand = null;
            int returnValue = 0;
            try
            {
                dbCommand = _dataAccessManager.GetStoredProcCommand("CheckUniqueSSN");
                _dataAccessManager.AddInParameter(dbCommand, "SSN", DbType.String, SSN.ToString());

                returnValue = Convert.ToInt32(_dataAccessManager.ExecuteScalar(dbCommand));
            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }
            return returnValue;
        }
        public int CheckValidUserType(string userType)
        {
            DbCommand dbCommand = null;
            int returnValue = 0;
            try
            {
                dbCommand = _dataAccessManager.GetStoredProcCommand("CheckValidUserType");
                _dataAccessManager.AddInParameter(dbCommand, "UserType", DbType.String, userType.ToString());
                returnValue = Convert.ToInt32(_dataAccessManager.ExecuteScalar(dbCommand));
            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }
            return returnValue;
        }

        public int ImportUserList(string id, string userId, string firstName, string middleName, string lastName, string ssn, string userType, bool isActive, string facilityName, string nationality, string force, string sex, string uic, string religion, string fmp, string race, string mos, string grade, string email, string dob, string pubKey, string privKey, out int checkUserId)
        {
            DbCommand dbCommand = null;
            int returnValue = 0;
            checkUserId = 0;
            try
            {
                dbCommand = _dataAccessManager.GetStoredProcCommand("ImportUser");
                _dataAccessManager.AddInParameter(dbCommand, "UserGUID", DbType.String, id.ToString());
                _dataAccessManager.AddInParameter(dbCommand, "userId", DbType.String, userId.ToString());
                _dataAccessManager.AddInParameter(dbCommand, "FirstName", DbType.String, firstName.ToString());
                _dataAccessManager.AddInParameter(dbCommand, "MiddleName", DbType.String, middleName.ToString());
                _dataAccessManager.AddInParameter(dbCommand, "LastName", DbType.String, lastName.ToString());
                _dataAccessManager.AddInParameter(dbCommand, "SSN", DbType.String, ssn.ToString());
                _dataAccessManager.AddInParameter(dbCommand, "userType", DbType.String, userType.ToString());
                _dataAccessManager.AddInParameter(dbCommand, "facilityname", DbType.String, facilityName.ToString());
                _dataAccessManager.AddInParameter(dbCommand, "IsActive", DbType.Boolean, isActive);
                _dataAccessManager.AddInParameter(dbCommand, "Nationality", DbType.String, nationality.ToString());
                _dataAccessManager.AddInParameter(dbCommand, "Force", DbType.String, force.ToString());
                _dataAccessManager.AddInParameter(dbCommand, "Sex", DbType.String, sex.ToString());
                _dataAccessManager.AddInParameter(dbCommand, "UIC", DbType.String, uic.ToString());
                _dataAccessManager.AddInParameter(dbCommand, "religion", DbType.String, religion.ToString());
                _dataAccessManager.AddInParameter(dbCommand, "fmp", DbType.String, fmp.ToString());
                _dataAccessManager.AddInParameter(dbCommand, "race", DbType.String, race.ToString());
                _dataAccessManager.AddInParameter(dbCommand, "mos", DbType.String, mos.ToString());
                _dataAccessManager.AddInParameter(dbCommand, "grade", DbType.String, grade.ToString());
                _dataAccessManager.AddInParameter(dbCommand, "email", DbType.String, email.ToString());
                _dataAccessManager.AddInParameter(dbCommand, "dob", DbType.String, dob.ToString());
                _dataAccessManager.AddInParameter(dbCommand, "PubKey", DbType.String, pubKey.ToString());
                _dataAccessManager.AddInParameter(dbCommand, "PrivKey", DbType.String, privKey.ToString());
                _dataAccessManager.AddOutParameter(dbCommand, "CheckUserId", DbType.Int32, checkUserId);

                returnValue = Convert.ToInt32(_dataAccessManager.ExecuteNonQuery(dbCommand));
                checkUserId = Convert.ToInt32(dbCommand.Parameters["CheckUserId"].Value);

            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }
            return returnValue;
        }
        public DataSet ExportUserList(string userId)
        {
            DbCommand dbCommand;
            DataSet dataSet = null;
            try
            {
                dbCommand = _dataAccessManager.GetStoredProcCommand("ExportUser");
                _dataAccessManager.AddInParameter(dbCommand, "userId", DbType.String, userId);
                dataSet = _dataAccessManager.ExecuteDataSet(dbCommand);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }
            return dataSet;
        }
        public DataSet GetAllRoleInfo()
        {
            DbCommand dbCommand;
            DataSet dataSet = null;
            try
            {
                dbCommand = _dataAccessManager.GetStoredProcCommand("GetRoles");
                dataSet = _dataAccessManager.ExecuteDataSet(dbCommand);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }
            return dataSet;
        }
        public DataSet SearchRoleInfo(string roleName, string roleDescription)
        {
            DbCommand dbCommand;
            DataSet dataSet = null;
            try
            {
                dbCommand = _dataAccessManager.GetStoredProcCommand("SearchRole");
                _dataAccessManager.AddInParameter(dbCommand, "RoleName", DbType.String, roleName.ToString());
                _dataAccessManager.AddInParameter(dbCommand, "RoleDescription", DbType.String, roleDescription.ToString());
                dataSet = _dataAccessManager.ExecuteDataSet(dbCommand);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }
            return dataSet;
        }
        public int CreateRole(int roleId, string roleName, string roleDescription, string roleKeyPublic, string roleKeyPrivate)
        {
            DbCommand dbCommand;
            int returnValue = 0;
            try
            {
                dbCommand = _dataAccessManager.GetStoredProcCommand("ManageRole");
                _dataAccessManager.AddInParameter(dbCommand, "roleId", DbType.Int32, roleId);
                _dataAccessManager.AddInParameter(dbCommand, "RoleName", DbType.String, roleName.ToString());
                _dataAccessManager.AddInParameter(dbCommand, "RoleDescription", DbType.String, roleDescription.ToString());
                _dataAccessManager.AddInParameter(dbCommand, "RoleKeyPublic", DbType.String, roleKeyPublic.ToString());
                _dataAccessManager.AddInParameter(dbCommand, "RoleKeyPrivate", DbType.String, roleKeyPrivate.ToString());

                returnValue = Convert.ToInt32(_dataAccessManager.ExecuteNonQuery(dbCommand));
            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }
            return returnValue;
        }
        public string GetFacilityName(int FacilityId)
        {
            DbCommand dbCommand;
            string sFacilityName = "";
            try
            {
                DataAccessManager _dataAccessManager = DataAccessManager.GetInstance;
                dbCommand = _dataAccessManager.GetStoredProcCommand("GetFacilityName");
                _dataAccessManager.AddInParameter(dbCommand, "FacilityId", DbType.Int32, FacilityId);
                sFacilityName = _dataAccessManager.ExecuteScalar(dbCommand).ToString();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }
            return sFacilityName;
        }
        public int DeleteRole(int roleId)
        {
            DbCommand dbCommand;
            int returnValue = 0;
            try
            {
                dbCommand = _dataAccessManager.GetStoredProcCommand("DeleteRole");
                _dataAccessManager.AddInParameter(dbCommand, "roleId", DbType.Int32, roleId);

                returnValue = Convert.ToInt32(_dataAccessManager.ExecuteNonQuery(dbCommand));
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message.ToString());
            }
            return returnValue;
        }
        public DataSet GetAllPermissions(int userTypeId, int facilityId)
        {
            DbCommand dbCommand;
            DataSet dataSet = null;
            try
            {
                dbCommand = _dataAccessManager.GetStoredProcCommand("GetPermissions");
                _dataAccessManager.AddInParameter(dbCommand, "UserTypeId", DbType.Int32, userTypeId);
                _dataAccessManager.AddInParameter(dbCommand, "FacilityId", DbType.Int32, facilityId);
                dataSet = _dataAccessManager.ExecuteDataSet(dbCommand);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message.ToString());
            }
            return dataSet;
        }
        public DataSet GetAssociatePermissions()
        {
            DbCommand dbCommand;
            DataSet dataSet = null;
            try
            {
                dbCommand = _dataAccessManager.GetStoredProcCommand("GetPermissionInfo");
                dataSet = _dataAccessManager.ExecuteDataSet(dbCommand);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message.ToString());
            }
            return dataSet;
        }
        public int DeleteDocumentByPatientId(string PatientId)
        {
            DbCommand dbCommand;
            int returnValue = 0;
            try
            {
                dbCommand = _dataAccessManager.GetStoredProcCommand("DeleteDocument");
                _dataAccessManager.AddInParameter(dbCommand, "PatientId", DbType.String, PatientId);
                returnValue = Convert.ToInt32(_dataAccessManager.ExecuteNonQuery(dbCommand));
            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }
            return returnValue;
        }
        public int DeleteSharingByPatientId(string PatientId)
        {
            DbCommand dbCommand;
            int returnValue = 0;
            try
            {
                dbCommand = _dataAccessManager.GetStoredProcCommand("DeleteSharing");
                _dataAccessManager.AddInParameter(dbCommand, "PatientId", DbType.String, PatientId);
                returnValue = Convert.ToInt32(_dataAccessManager.ExecuteNonQuery(dbCommand));
            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }
            return returnValue;
        }
        public string GetUserGUIDByUserID(string UserID)
        {
            DbCommand dbCommand;
            string sUserID = "";
            try
            {
                DataAccessManager _dataAccessManager = DataAccessManager.GetInstance;
                dbCommand = _dataAccessManager.GetStoredProcCommand("GetUserGUID");
                _dataAccessManager.AddInParameter(dbCommand, "UserID", DbType.String, UserID);
                sUserID = _dataAccessManager.ExecuteScalar(dbCommand).ToString();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }
            return sUserID;
        }
        public int GetDocIdPostFix()
        {
            DbCommand dbCommand;
            int DocPostFix;
            try
            {
                DataAccessManager _dataAccessManager = DataAccessManager.GetInstance;
                dbCommand = _dataAccessManager.GetStoredProcCommand("GetDocPostFix");
                // _dataAccessManager.AddInParameter(dbCommand, "UserID", DbType.String, UserID);
                DocPostFix = Convert.ToInt32(_dataAccessManager.ExecuteScalar(dbCommand));
            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }
            return DocPostFix;
        }
        public void UpdateDocPostFix()
        {
            DbCommand dbCommand;

            try
            {
                DataAccessManager _dataAccessManager = DataAccessManager.GetInstance;
                dbCommand = _dataAccessManager.GetStoredProcCommand("UpdateDocPostFix");
                // _dataAccessManager.AddInParameter(dbCommand, "UserID", DbType.String, UserID);
                _dataAccessManager.ExecuteNonQuery(dbCommand);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }

        }
        public DataSet GetPermissionCategoryOfAssociatedFacility(int SourceFacilityId, int TargetFacilityId)
        {
            DbCommand dbCommand;
            DataSet dataSet = null;
            try
            {
                dbCommand = _dataAccessManager.GetStoredProcCommand("GetPermissionCategoryOfAssociatedFacility");
                _dataAccessManager.AddInParameter(dbCommand, "SourceFacilityId", DbType.Int32, SourceFacilityId);
                _dataAccessManager.AddInParameter(dbCommand, "TargetFacilityId", DbType.Int32, TargetFacilityId);
                dataSet = _dataAccessManager.ExecuteDataSet(dbCommand);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message.ToString());
            }
            return dataSet;
        }

        public DataSet GetCategoryInfo()
        {
            DbCommand dbCommand;
            DataSet dataSet = null;
            try
            {
                dbCommand = _dataAccessManager.GetStoredProcCommand("GetCategoryInfo");
                dataSet = _dataAccessManager.ExecuteDataSet(dbCommand);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message.ToString());
            }
            return dataSet;
        }

        public int SetPermissionsAndCategory(int userTypeId, int facilityId, int PermID, string Value)
        {
            DbCommand dbCommand;
            int returnValue = 0;
            try
            {
                dbCommand = _dataAccessManager.GetStoredProcCommand("SetPermissionsAndCategory");
                _dataAccessManager.AddInParameter(dbCommand, "UserTypeId", DbType.Int32, userTypeId);
                _dataAccessManager.AddInParameter(dbCommand, "FacilityId", DbType.Int32, facilityId);
                _dataAccessManager.AddInParameter(dbCommand, "PermID", DbType.Int32, PermID);
                _dataAccessManager.AddInParameter(dbCommand, "Value", DbType.String, Value);
                returnValue = Convert.ToInt32(_dataAccessManager.ExecuteNonQuery(dbCommand));

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message.ToString());
            }
            return returnValue;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="MPPID"></param>
        /// <param name="isNew"></param>
        /// <param name="patientConsentId"></param>
        /// <param name="GroupId"></param>
        /// <param name="RoleId"></param>
        /// <param name="PermID"></param>
        /// <param name="Value"></param>
        /// <param name="PurposeOfRoleId"></param>
        /// <param name="status"></param>
        /// <param name="ruleStartDate"></param>
        /// <param name="ruleEndDate"></param>
        /// <param name="ConsentId"></param>
        /// <returns></returns>
        /// History
        /// Commented GroupID in Sprint-2
        public int UpdatePatientConsentPolicy(string MPPID, string isNew, int? patientConsentId, int? GroupId, int? RoleId, int? PermID, string Value, int? PurposeOfRoleId, string status, string ruleStartDate, string ruleEndDate, out int ConsentId)
        {
            DbCommand dbCommand;
            int returnValue = 0;
            ConsentId = 0;
            try
            {
                dbCommand = _dataAccessManager.GetStoredProcCommand("UpdatePatientConsentPolicy");
                _dataAccessManager.AddInParameter(dbCommand, "PatientConsentId", DbType.Int32, patientConsentId);
                _dataAccessManager.AddInParameter(dbCommand, "IsNew", DbType.String, isNew);
                _dataAccessManager.AddInParameter(dbCommand, "MPIID", DbType.String, MPPID);

                //_dataAccessManager.AddInParameter(dbCommand, "GroupID", DbType.Int32, GroupId);
                _dataAccessManager.AddInParameter(dbCommand, "RoleID", DbType.Int32, RoleId);
                _dataAccessManager.AddInParameter(dbCommand, "PermissionID", DbType.Int32, PermID);
                _dataAccessManager.AddInParameter(dbCommand, "C32SectionID", DbType.Int64, Value);
                _dataAccessManager.AddInParameter(dbCommand, "PurposeOfUseId", DbType.Int32, PurposeOfRoleId);
                _dataAccessManager.AddInParameter(dbCommand, "status", DbType.Int32, status);
                _dataAccessManager.AddInParameter(dbCommand, "ruleStartDate", DbType.Date, ruleStartDate);
                _dataAccessManager.AddInParameter(dbCommand, "ruleEndDate", DbType.Date, ruleEndDate);
                _dataAccessManager.AddOutParameter(dbCommand, "ConsentId", DbType.Int16, ConsentId);
                returnValue = Convert.ToInt32(_dataAccessManager.ExecuteNonQuery(dbCommand));
                ConsentId = Convert.ToInt32(dbCommand.Parameters["ConsentId"].Value);

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message.ToString());
            }
            return returnValue;
        }


        public int CheckPatientConsentPolicyExistence(string token, string MPIID, int? GroupId, int? RoleId, int? PurposeOfUseId, out int checkConsentExistence)
        {
            DbCommand dbCommand;
            int returnValue = 0;
            checkConsentExistence = 0;
            try
            {
                dbCommand = _dataAccessManager.GetStoredProcCommand("CheckPatientConsentPolicyExistence");
                _dataAccessManager.AddInParameter(dbCommand, "@MPIID", DbType.String, MPIID);
                //_dataAccessManager.AddInParameter(dbCommand, "@GroupID", DbType.Int32, GroupId);
                _dataAccessManager.AddInParameter(dbCommand, "@RoleID", DbType.Int32, RoleId);
                _dataAccessManager.AddInParameter(dbCommand, "@PurposeOfUseId", DbType.Int32, PurposeOfUseId);
                _dataAccessManager.AddOutParameter(dbCommand, "@RecordExists", DbType.Int16, checkConsentExistence);
                returnValue = Convert.ToInt32(_dataAccessManager.ExecuteNonQuery(dbCommand));
                checkConsentExistence = Convert.ToInt32(dbCommand.Parameters["@RecordExists"].Value);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message.ToString());
            }
            return returnValue;
        }



        public DataSet SearchPermissionInfo(string permissionName, string permissionDesc)
        {
            DbCommand dbCommand;
            DataSet dataSet = null;
            try
            {
                dbCommand = _dataAccessManager.GetStoredProcCommand("SearchPermission");
                _dataAccessManager.AddInParameter(dbCommand, "PermName", DbType.String, permissionName.ToString());
                _dataAccessManager.AddInParameter(dbCommand, "PermDesc", DbType.String, permissionDesc.ToString());
                dataSet = _dataAccessManager.ExecuteDataSet(dbCommand);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message.ToString());
            }
            return dataSet;
        }
        public int CreatePermission(int permissionID, string permissionName, string permissionDesc)
        {
            DbCommand dbCommand;
            int returnValue = 0;
            try
            {
                dbCommand = _dataAccessManager.GetStoredProcCommand("ManagePermission");
                _dataAccessManager.AddInParameter(dbCommand, "PermissionID", DbType.Int32, permissionID);
                _dataAccessManager.AddInParameter(dbCommand, "PermissionName", DbType.String, permissionName.ToString());
                _dataAccessManager.AddInParameter(dbCommand, "PermissionDesc", DbType.String, permissionDesc.ToString());

                returnValue = Convert.ToInt32(_dataAccessManager.ExecuteNonQuery(dbCommand));
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message.ToString());
            }
            return returnValue;
        }
        public int DeletePermission(int permissionID)
        {
            DbCommand dbCommand;
            int returnValue = 0;
            try
            {
                dbCommand = _dataAccessManager.GetStoredProcCommand("DeletePermission");
                _dataAccessManager.AddInParameter(dbCommand, "PermissionID", DbType.Int32, permissionID);
                returnValue = Convert.ToInt32(_dataAccessManager.ExecuteNonQuery(dbCommand));
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message.ToString());
            }
            return returnValue;
        }

        public DataSet GetAllServInfo()
        {
            DbCommand dbCommand;
            DataSet dataSet = null;
            try
            {
                dbCommand = _dataAccessManager.GetStoredProcCommand("GetFacilityInfo");
                dataSet = _dataAccessManager.ExecuteDataSet(dbCommand);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message.ToString());
            }
            return dataSet;
        }

        public DataSet GetFacilityList(string facilityId)
        {
            DbCommand dbCommand;
            DataSet dataSet = null;
            try
            {
                dbCommand = _dataAccessManager.GetStoredProcCommand("GetFacilityInfo");
                _dataAccessManager.AddInParameter(dbCommand, "FacilityId", DbType.String, facilityId);
                dataSet = _dataAccessManager.ExecuteDataSet(dbCommand);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message.ToString());
            }
            return dataSet;
        }
        public DataSet SearchFacility(string facilityName, string serverUri, string city, string state, string zip)
        {
            DbCommand dbCommand;
            DataSet dataSet = null;
            try
            {
                dbCommand = _dataAccessManager.GetStoredProcCommand("SearchFacility");
                _dataAccessManager.AddInParameter(dbCommand, "FacilityName", DbType.String, facilityName);
                _dataAccessManager.AddInParameter(dbCommand, "ServerUri", DbType.String, serverUri);
                _dataAccessManager.AddInParameter(dbCommand, "City", DbType.String, city);
                _dataAccessManager.AddInParameter(dbCommand, "State", DbType.String, state);
                _dataAccessManager.AddInParameter(dbCommand, "Zip", DbType.String, zip);
                dataSet = _dataAccessManager.ExecuteDataSet(dbCommand);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message.ToString());
            }
            return dataSet;
        }
        public int PublishServer(string FacilityID, string Name, string City, string Address1, string Address2, string State, string Zip, string Description, string Email, string FacilityPubKey, string FacilityLogo, string Uri, int facilityTypeId)
        {
            DbCommand dbCommand;
            int returnValue = 0;
            try
            {
                dbCommand = _dataAccessManager.GetStoredProcCommand("PublishServer");
                _dataAccessManager.AddInParameter(dbCommand, "FacilityID", DbType.String, FacilityID.ToString());
                _dataAccessManager.AddInParameter(dbCommand, "Name", DbType.String, Name.ToString());
                _dataAccessManager.AddInParameter(dbCommand, "City", DbType.String, City.ToString());
                _dataAccessManager.AddInParameter(dbCommand, "Address1", DbType.String, Address1.ToString());
                _dataAccessManager.AddInParameter(dbCommand, "Address2", DbType.String, Address2.ToString());
                _dataAccessManager.AddInParameter(dbCommand, "State", DbType.String, State.ToString());
                _dataAccessManager.AddInParameter(dbCommand, "Zip", DbType.String, Zip.ToString());
                _dataAccessManager.AddInParameter(dbCommand, "Description", DbType.String, Description.ToString());
                _dataAccessManager.AddInParameter(dbCommand, "Email", DbType.String, Email.ToString());
                _dataAccessManager.AddInParameter(dbCommand, "FacilityPubKey", DbType.String, FacilityPubKey.ToString());
                _dataAccessManager.AddInParameter(dbCommand, "FacilityLogo", DbType.String, FacilityLogo.ToString());
                _dataAccessManager.AddInParameter(dbCommand, "Uri", DbType.String, Uri.ToString());
                _dataAccessManager.AddInParameter(dbCommand, "FacilityTypeId", DbType.Int32, facilityTypeId);

                returnValue = Convert.ToInt32(_dataAccessManager.ExecuteNonQuery(dbCommand));
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message.ToString());
            }
            return returnValue;
        }

        public int DeletePublishServer(string facilityID)
        {
            DbCommand dbCommand;
            int returnValue = 0;
            try
            {
                dbCommand = _dataAccessManager.GetStoredProcCommand("DeletePublishServer");
                _dataAccessManager.AddInParameter(dbCommand, "facilityID", DbType.String, facilityID);
                returnValue = Convert.ToInt32(_dataAccessManager.ExecuteNonQuery(dbCommand));
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message.ToString());
            }
            return returnValue;
        }

        public int CheckPublishServer(string facilityID)
        {
            DbCommand dbCommand = null;
            int returnValue = 0;
            try
            {
                dbCommand = _dataAccessManager.GetStoredProcCommand("CheckPublishServer");
                _dataAccessManager.AddInParameter(dbCommand, "facilityID", DbType.String, facilityID.ToString());
                returnValue = Convert.ToInt32(_dataAccessManager.ExecuteScalar(dbCommand));
            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }
            return returnValue;
        }

        public int RegisterServer(int serverID, string uri, int port, string name, string adapterSrcPath, string adapterDestPath)
        {
            DbCommand dbCommand;
            int returnValue = 0;
            try
            {
                dbCommand = _dataAccessManager.GetStoredProcCommand("RegisterServer");
                _dataAccessManager.AddInParameter(dbCommand, "ServerID", DbType.Int32, serverID);
                _dataAccessManager.AddInParameter(dbCommand, "URI", DbType.String, uri.ToString());
                _dataAccessManager.AddInParameter(dbCommand, "Port", DbType.Int32, port);
                _dataAccessManager.AddInParameter(dbCommand, "Name", DbType.String, name.ToString());
                _dataAccessManager.AddInParameter(dbCommand, "AdapterSrcPath", DbType.String, adapterSrcPath.ToString());
                _dataAccessManager.AddInParameter(dbCommand, "AdapterDestPath", DbType.String, adapterDestPath.ToString());

                returnValue = Convert.ToInt32(_dataAccessManager.ExecuteNonQuery(dbCommand));
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message.ToString());
            }
            return returnValue;
        }

        public DataSet GetAllEicInfo(string eicGuid)
        {
            DbCommand dbCommand;
            DataSet dataSet = null;
            try
            {
                dbCommand = _dataAccessManager.GetStoredProcCommand("GetAllEicInfo");
                _dataAccessManager.AddInParameter(dbCommand, "eicGuid", DbType.String, eicGuid);
                dataSet = _dataAccessManager.ExecuteDataSet(dbCommand);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message.ToString());
            }
            return dataSet;
        }

        public DataSet SearchEicInfo(string eicSerialId, string name)
        {
            DbCommand dbCommand;
            DataSet dataSet = null;
            try
            {
                dbCommand = _dataAccessManager.GetStoredProcCommand("SearchEic");
                _dataAccessManager.AddInParameter(dbCommand, "eicSerialId", DbType.String, eicSerialId.ToString());
                _dataAccessManager.AddInParameter(dbCommand, "Name", DbType.String, name.ToString());
                dataSet = _dataAccessManager.ExecuteDataSet(dbCommand);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message.ToString());
            }
            return dataSet;
        }
        public int RegisterEic(string eicGuid, string eicSerialId, string name, string eicDesc, bool isAssigned, string userGuid, string unit, out int checkSerialId)
        {
            DbCommand dbCommand;
            int returnValue = 0;
            checkSerialId = 0;
            try
            {
                dbCommand = _dataAccessManager.GetStoredProcCommand("RegisterEIC");
                _dataAccessManager.AddInParameter(dbCommand, "eicGuid", DbType.String, eicGuid);
                _dataAccessManager.AddInParameter(dbCommand, "eicSerialId", DbType.String, eicSerialId);
                _dataAccessManager.AddInParameter(dbCommand, "EicName", DbType.String, name.ToString());
                _dataAccessManager.AddInParameter(dbCommand, "PubKey", DbType.String, "");
                _dataAccessManager.AddInParameter(dbCommand, "PrivKey", DbType.String, "");
                _dataAccessManager.AddInParameter(dbCommand, "Description", DbType.String, eicDesc.ToString());
                _dataAccessManager.AddInParameter(dbCommand, "IsAssigned", DbType.Boolean, isAssigned);
                _dataAccessManager.AddInParameter(dbCommand, "UserGuid", DbType.String, userGuid.ToString());
                _dataAccessManager.AddInParameter(dbCommand, "Unit", DbType.String, unit.ToString());
                _dataAccessManager.AddOutParameter(dbCommand, "CheckEicSerialId", DbType.Int32, checkSerialId);
                returnValue = Convert.ToInt32(_dataAccessManager.ExecuteNonQuery(dbCommand));
                checkSerialId = Convert.ToInt32(dbCommand.Parameters["CheckEicSerialId"].Value);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message.ToString());
            }
            return returnValue;
        }
        public int DeleteEic(string eicGuid)
        {
            DbCommand dbCommand;
            int returnValue = 0;
            try
            {
                dbCommand = _dataAccessManager.GetStoredProcCommand("DeleteEICInfo");
                _dataAccessManager.AddInParameter(dbCommand, "eicGuid", DbType.String, eicGuid);

                returnValue = Convert.ToInt32(_dataAccessManager.ExecuteNonQuery(dbCommand));
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message.ToString());
            }
            return returnValue;
        }
        public DataSet FillDropDown(int dropDownType, int userTypeId, int facilityId, string eicGuid)
        {
            DbCommand dbCommand = null;
            DataSet dataSet = null;
            try
            {
                dbCommand = _dataAccessManager.GetStoredProcCommand("FillDropDown");
                _dataAccessManager.AddInParameter(dbCommand, "Enum", DbType.Int32, dropDownType);
                _dataAccessManager.AddInParameter(dbCommand, "userTypeId", DbType.Int32, userTypeId);
                _dataAccessManager.AddInParameter(dbCommand, "facilityId", DbType.Int32, facilityId);
                _dataAccessManager.AddInParameter(dbCommand, "eicGuid", DbType.String, eicGuid);
                dataSet = _dataAccessManager.ExecuteDataSet(dbCommand);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message.ToString());
            }
            return dataSet;
        }
        public DataSet GetFacilityInfo(int facilityId)
        {
            DbCommand dbCommand;
            DataSet dataSet = null;
            try
            {
                dbCommand = _dataAccessManager.GetStoredProcCommand("GetFacilityInfo");
                _dataAccessManager.AddInParameter(dbCommand, "facilityId", DbType.Int32, facilityId);
                dataSet = _dataAccessManager.ExecuteDataSet(dbCommand);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message.ToString());
            }
            return dataSet;
        }
        public int FacilityAssociate(int sourceFacilityId, int targetFacilityId, int permissionTypeId, string categoryId, string ExpiryDate, string targetFacilityPublicKey)
        {
            DbCommand dbCommand;
            int returnValue = 0;
            try
            {
                dbCommand = _dataAccessManager.GetStoredProcCommand("insertFacilityAssociation");
                _dataAccessManager.AddInParameter(dbCommand, "sourceFacilityId", DbType.Int32, sourceFacilityId);
                _dataAccessManager.AddInParameter(dbCommand, "targetFacilityId", DbType.Int32, targetFacilityId);
                _dataAccessManager.AddInParameter(dbCommand, "permissionTypeId", DbType.Int32, permissionTypeId);
                _dataAccessManager.AddInParameter(dbCommand, "categoryId", DbType.String, categoryId);
                _dataAccessManager.AddInParameter(dbCommand, "ExpiryDate", DbType.String, ExpiryDate);
                _dataAccessManager.AddInParameter(dbCommand, "targetFacilityPublicKey", DbType.String, targetFacilityPublicKey);
                returnValue = Convert.ToInt32(_dataAccessManager.ExecuteNonQuery(dbCommand));
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message.ToString());
            }
            return returnValue;
        }

        public DataSet GetAssociatedFacilityInfo(int facilityId)
        {
            DbCommand dbCommand;
            DataSet dataSet = null;
            try
            {
                dbCommand = _dataAccessManager.GetStoredProcCommand("GetAssociatedFacility");
                _dataAccessManager.AddInParameter(dbCommand, "facilityId", DbType.Int32, facilityId);
                dataSet = _dataAccessManager.ExecuteDataSet(dbCommand);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message.ToString());
            }
            return dataSet;
        }

        public int DissociateFacility(int sourceFacilityId, string targetFacilityId)
        {
            DbCommand dbCommand;
            int returnValue = 0;
            try
            {
                dbCommand = _dataAccessManager.GetStoredProcCommand("DissociateFacility");
                _dataAccessManager.AddInParameter(dbCommand, "sourceFacilityId", DbType.Int32, sourceFacilityId);
                _dataAccessManager.AddInParameter(dbCommand, "targetFacilityId", DbType.String, targetFacilityId);
                returnValue = Convert.ToInt32(_dataAccessManager.ExecuteNonQuery(dbCommand));
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message.ToString());
            }
            return returnValue;
        }
        public int CategoryAssociation(int sourceFacilityId, int targetFacilityId, int permissionTypeId, int categoryId)
        {
            DbCommand dbCommand;
            int returnValue = 0;
            try
            {
                dbCommand = _dataAccessManager.GetStoredProcCommand("insertCategoryAssociation");
                _dataAccessManager.AddInParameter(dbCommand, "sourceFacilityId", DbType.Int32, sourceFacilityId);
                _dataAccessManager.AddInParameter(dbCommand, "targetFacilityId", DbType.Int32, targetFacilityId);
                _dataAccessManager.AddInParameter(dbCommand, "permissionTypeId", DbType.Int32, permissionTypeId);
                _dataAccessManager.AddInParameter(dbCommand, "categoryId", DbType.Int32, categoryId);
                returnValue = Convert.ToInt32(_dataAccessManager.ExecuteNonQuery(dbCommand));
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message.ToString());
            }
            return returnValue;
        }
        public DataSet GetPatientInfo(string PatientID)
        {
            DbCommand dbCommand;
            DataSet dataSet = null;
            try
            {
                dbCommand = _dataAccessManager.GetStoredProcCommand("GetPatientIdnFile");
                _dataAccessManager.AddInParameter(dbCommand, "PatientId", DbType.String, PatientID);
                dataSet = _dataAccessManager.ExecuteDataSet(dbCommand);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message.ToString());
            }
            return dataSet;
        }
        public DataSet GetDeviceInfo(string EICSerialId)
        {
            DbCommand dbCommand;
            DataSet dataSet = null;
            try
            {
                dbCommand = _dataAccessManager.GetStoredProcCommand("GetDeviceIdnFile");
                _dataAccessManager.AddInParameter(dbCommand, "EICSerialID", DbType.String, EICSerialId);
                dataSet = _dataAccessManager.ExecuteDataSet(dbCommand);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message.ToString());
            }
            return dataSet;
        }
        public int VerifyEic(string token, string PatientID, string EicName, string Description, bool RegisterDevice, string EICSerialID)
        {
            int result = 0;
            int status = 0;
            try
            {
                DbCommand dbCommand = _dataAccessManager.GetStoredProcCommand("ResgisterEICDevice");
                _dataAccessManager.AddInParameter(dbCommand, "UserId", DbType.String, PatientID);
                _dataAccessManager.AddInParameter(dbCommand, "EICSerialID", DbType.String, EICSerialID);
                _dataAccessManager.AddInParameter(dbCommand, "EICName", DbType.String, EicName);
                _dataAccessManager.AddInParameter(dbCommand, "Description", DbType.String, Description);
                _dataAccessManager.AddInParameter(dbCommand, "ConfirmRegister", DbType.Boolean, RegisterDevice);
                _dataAccessManager.AddOutParameter(dbCommand, "Status", DbType.Int32, status);

                result = Convert.ToInt32(_dataAccessManager.ExecuteNonQuery(dbCommand).ToString());
                status = Convert.ToInt32(dbCommand.Parameters["Status"].Value);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message.ToString());
            }
            return status;
        }
        public int CheckRegisterEIC(string eicSerialID)
        {
            DbCommand dbCommand = null;
            int returnValue = 0;
            try
            {
                dbCommand = _dataAccessManager.GetStoredProcCommand("CheckEIC");
                _dataAccessManager.AddInParameter(dbCommand, "eicSerialID", DbType.String, eicSerialID.ToString());

                returnValue = Convert.ToInt32(_dataAccessManager.ExecuteScalar(dbCommand));
            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }
            return returnValue;
        }
        public bool UnRegisterEic(string token, string PatientID)
        {
            DbCommand dbCommand;
            bool blUnRegisterEic = false;
            try
            {
                dbCommand = _dataAccessManager.GetStoredProcCommand("UnRegisterEic");
                _dataAccessManager.AddInParameter(dbCommand, "PatientID", DbType.String, PatientID);
                _dataAccessManager.ExecuteNonQuery(dbCommand);
                blUnRegisterEic = true;
            }
            catch (Exception ex)
            {
                blUnRegisterEic = false;
                throw new Exception(ex.Message.ToString());
            }
            return blUnRegisterEic;
        }
        public string GetServerKey(string serverUri)
        {
            DbCommand dbCommand;
            string serverPublicKey = null;
            try
            {
                dbCommand = _dataAccessManager.GetSqlStringCommand("SELECT ServerPubKey FROM ServerInfo WHERE ServerURI = '" + serverUri + "'");
                serverPublicKey = _dataAccessManager.ExecuteScalar(dbCommand).ToString();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message.ToString());
            }
            return serverPublicKey;
        }

        public DataSet GetFacilityInfoPublish(int facilityId)
        {
            DbCommand dbCommand;
            DataSet dataSet = null;
            try
            {
                dbCommand = _dataAccessManager.GetStoredProcCommand("GetFacilityDetailsPublish");
                _dataAccessManager.AddInParameter(dbCommand, "facilityId", DbType.Int32, facilityId);
                dataSet = _dataAccessManager.ExecuteDataSet(dbCommand);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message.ToString());
            }
            return dataSet;
        }

        public DataSet GetFacilityInfoDetails(int facilityId)
        {
            DbCommand dbCommand;
            DataSet dataSet = null;
            try
            {
                dbCommand = _dataAccessManager.GetStoredProcCommand("GetFacilityDetailsPublish");
                _dataAccessManager.AddInParameter(dbCommand, "facilityId", DbType.Int32, facilityId);
                dataSet = _dataAccessManager.ExecuteDataSet(dbCommand);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message.ToString());
            }
            return dataSet;
        }

        public int UpdateFacilityInfo(string token, int FacilityID, string Name, string City, string Address1, string Address2, string State, string Zip, string Description, string Email, string NewImageLogo, string Contact)
        {
            DbCommand dbCommand;
            int returnValue = 0;
            try
            {
                dbCommand = _dataAccessManager.GetStoredProcCommand("UpdateFacilityInfo");
                _dataAccessManager.AddInParameter(dbCommand, "FacilityID", DbType.Int32, FacilityID);
                _dataAccessManager.AddInParameter(dbCommand, "Name", DbType.String, Name.ToString());
                _dataAccessManager.AddInParameter(dbCommand, "City", DbType.String, City.ToString());
                _dataAccessManager.AddInParameter(dbCommand, "Address1", DbType.String, Address1.ToString());
                _dataAccessManager.AddInParameter(dbCommand, "Address2", DbType.String, Address2.ToString());
                _dataAccessManager.AddInParameter(dbCommand, "State", DbType.String, State.ToString());
                _dataAccessManager.AddInParameter(dbCommand, "Zip", DbType.String, Zip.ToString());
                _dataAccessManager.AddInParameter(dbCommand, "Description", DbType.String, Description.ToString());
                _dataAccessManager.AddInParameter(dbCommand, "Email", DbType.String, Email.ToString());
                _dataAccessManager.AddInParameter(dbCommand, "NewImageLogo", DbType.String, NewImageLogo.ToString());
                _dataAccessManager.AddInParameter(dbCommand, "Contact", DbType.String, Contact.ToString());

                returnValue = Convert.ToInt32(_dataAccessManager.ExecuteNonQuery(dbCommand));
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message.ToString());
            }
            return returnValue;
        }

        public DataSet GetUnassociatedFacility(int facilityId)
        {
            DbCommand dbCommand;
            DataSet dataSet = null;
            try
            {
                dbCommand = _dataAccessManager.GetStoredProcCommand("GetUnassociatedFacility");
                _dataAccessManager.AddInParameter(dbCommand, "facilityId", DbType.Int32, facilityId);
                dataSet = _dataAccessManager.ExecuteDataSet(dbCommand);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message.ToString());
            }
            return dataSet;
        }

        /// <summary>
        /// Get DataType for Category 911 
        /// Create Date 20 Jan 2009
        /// </summary>
        /// <returns></returns>
        public DataSet Get911DataType()
        {
            DbCommand dbCommand;
            DataSet dataSet = null;
            try
            {
                dbCommand = _dataAccessManager.GetStoredProcCommand("GetDataType911");
                dataSet = _dataAccessManager.ExecuteDataSet(dbCommand);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message.ToString());
            }
            return dataSet;
        }

        /// <summary>
        /// Get DataType for Category 911 
        /// Create Date 20 Jan 2009
        /// </summary>
        /// <returns></returns>
        public DataSet Get911Fields()
        {
            DbCommand dbCommand;
            DataSet dataSet = null;
            try
            {
                dbCommand = _dataAccessManager.GetStoredProcCommand("GetFields911");
                dataSet = _dataAccessManager.ExecuteDataSet(dbCommand);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message.ToString());
            }
            return dataSet;
        }

        public void Update911Fields(DataSet dsFields)
        {
            try
            {
                SqlDataAdapter sqlDataAdapter = _dataAccessManager.GetSqlDataAdapter();
                SqlCommand sqlCommand = _dataAccessManager.GetSqlCommand("GetFields911");
                sqlDataAdapter.SelectCommand = sqlCommand;
                SqlCommandBuilder sqlCommandBuilder = _dataAccessManager.GetCommandBuilder(sqlDataAdapter);
                //adap = new SqlDataAdapter("select DataTypeID,DataType from DataType911", con);                
                sqlDataAdapter.Update(dsFields);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message.ToString());
            }
        }

        public DataSet Get911Category()
        {
            DbCommand dbCommand;
            DataSet dataSet = null;
            try
            {
                dbCommand = _dataAccessManager.GetStoredProcCommand("GetCategory911");
                dataSet = _dataAccessManager.ExecuteDataSet(dbCommand);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message.ToString());
            }
            return dataSet;
        }


        public DataSet GetCategoryField911(int FieldID)
        {
            DbCommand dbCommand;
            DataSet dataSet = null;
            try
            {
                dbCommand = _dataAccessManager.GetStoredProcCommand("GetCategoryFields911");
                _dataAccessManager.AddInParameter(dbCommand, "FieldID", DbType.Int32, FieldID);
                dataSet = _dataAccessManager.ExecuteDataSet(dbCommand);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message.ToString());
            }
            return dataSet;
        }


        public int AddCategoryField911(int FieldId, string Name, string Type, string FieldName, string FieldType)
        {
            DbCommand dbCommand;
            int returnValue = 0;
            try
            {
                dbCommand = _dataAccessManager.GetStoredProcCommand("AddCategoryField911");
                _dataAccessManager.AddInParameter(dbCommand, "FieldId", DbType.Int32, FieldId);
                _dataAccessManager.AddInParameter(dbCommand, "Name", DbType.String, Name);
                _dataAccessManager.AddInParameter(dbCommand, "Type", DbType.String, Type);
                _dataAccessManager.AddInParameter(dbCommand, "FieldName", DbType.String, FieldName);
                _dataAccessManager.AddInParameter(dbCommand, "FieldType", DbType.String, FieldType);
                returnValue = Convert.ToInt32(_dataAccessManager.ExecuteNonQuery(dbCommand));
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message.ToString());
            }
            return returnValue;
        }

        public int AddInformation911(string UserGUID, string FieldData911, string FieldData911Schema, string CategoryData911, string CategoryData911Schema)
        {
            DbCommand dbCommand;
            int returnValue = 0;
            try
            {
                dbCommand = _dataAccessManager.GetStoredProcCommand("AddInformation911");
                _dataAccessManager.AddInParameter(dbCommand, "UserGUID", DbType.String, UserGUID);
                _dataAccessManager.AddInParameter(dbCommand, "FieldData911", DbType.String, FieldData911);
                _dataAccessManager.AddInParameter(dbCommand, "FieldData911Schema", DbType.String, FieldData911Schema);
                _dataAccessManager.AddInParameter(dbCommand, "CategoryData911", DbType.String, CategoryData911);
                _dataAccessManager.AddInParameter(dbCommand, "CategoryData911Schema", DbType.String, CategoryData911Schema);
                returnValue = Convert.ToInt32(_dataAccessManager.ExecuteNonQuery(dbCommand));
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message.ToString());
            }
            return returnValue;
        }

        public DataSet GetUserInformation911(string patientGUID, string patientID)
        {
            DbCommand dbCommand;
            DataSet dataSet = null;
            try
            {
                dbCommand = _dataAccessManager.GetStoredProcCommand("GetInformation911");
                _dataAccessManager.AddInParameter(dbCommand, "UserGUID", DbType.String, patientGUID);
                _dataAccessManager.AddInParameter(dbCommand, "UserID", DbType.String, patientID);
                dataSet = _dataAccessManager.ExecuteDataSet(dbCommand);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message.ToString());
            }
            return dataSet;
        }

        public int DeleteCategoryField911(int FieldId, int CategoryId, string CategoryFieldName)
        {
            DbCommand dbCommand;
            int returnValue = 0;
            try
            {
                dbCommand = _dataAccessManager.GetStoredProcCommand("DeleteCategoryField911");
                _dataAccessManager.AddInParameter(dbCommand, "FieldId", DbType.Int32, FieldId);
                _dataAccessManager.AddInParameter(dbCommand, "CategoryId", DbType.Int32, CategoryId);
                _dataAccessManager.AddInParameter(dbCommand, "CategoryFieldName", DbType.String, CategoryFieldName);
                returnValue = Convert.ToInt32(_dataAccessManager.ExecuteNonQuery(dbCommand));
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message.ToString());
            }
            return returnValue;
        }

        public void Update911Category(DataSet dsCategory)
        {
            try
            {
                SqlDataAdapter sqlDataAdapter = _dataAccessManager.GetSqlDataAdapter();
                SqlCommand sqlCommand = _dataAccessManager.GetSqlCommand("GetCategory911");
                sqlDataAdapter.SelectCommand = sqlCommand;
                SqlCommandBuilder sqlCommandBuilder = _dataAccessManager.GetCommandBuilder(sqlDataAdapter);
                //adap = new SqlDataAdapter("select DataTypeID,DataType from DataType911", con);                
                sqlDataAdapter.Update(dsCategory);
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message.ToString());
            }
        }
        public int AddReferPatient(string OriginalDocumentID, string PatientID, string UploadedBy, int DocumentType, string DocumentTitle, string Author, string CreatedDate)
        {
            DbCommand dbCommand;
            int returnValue = 0;
            try
            {

                dbCommand = _dataAccessManager.GetStoredProcCommand("AddReferPatient");

                _dataAccessManager.AddInParameter(dbCommand, "@OriginalDocumentID", DbType.String, OriginalDocumentID);
                _dataAccessManager.AddInParameter(dbCommand, "@PatientID", DbType.String, PatientID);
                _dataAccessManager.AddInParameter(dbCommand, "@UploadedBy", DbType.String, UploadedBy);
                _dataAccessManager.AddInParameter(dbCommand, "@DocumentType", DbType.Int32, DocumentType);
                _dataAccessManager.AddInParameter(dbCommand, "@DocumentTitle", DbType.String, DocumentTitle);
                _dataAccessManager.AddInParameter(dbCommand, "@Author", DbType.String, Author);
                _dataAccessManager.AddInParameter(dbCommand, "@CreatedDate", DbType.String, CreatedDate);
                returnValue = Convert.ToInt32(_dataAccessManager.ExecuteNonQuery(dbCommand));

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message.ToString());
            }
            return returnValue;
        }

        public int AddReferPatient(string OriginalDocumentID, string PatientId, string UploadedBy, int DocumentType, string sTitle, string sAuthor, string sCreatedDate, string DocumentSource, string SourceCommunityID, string SourceRepositryID, string FacilityID, bool Reposed, string filePath)
        {
            DbCommand dbCommand;
            int returnValue = 0;
            try
            {
                //dbCommand = _dataAccessManager.GetStoredProcCommand("AddReferPatient");
                dbCommand = _dataAccessManager.GetStoredProcCommand("SaveDocumentDetail");
                _dataAccessManager.AddInParameter(dbCommand, "@OriginalDocumentID", DbType.String, OriginalDocumentID);
                _dataAccessManager.AddInParameter(dbCommand, "@PatientID", DbType.String, PatientId);
                _dataAccessManager.AddInParameter(dbCommand, "@UploadedBy", DbType.String, UploadedBy);
                _dataAccessManager.AddInParameter(dbCommand, "@DocumentType", DbType.Int32, DocumentType);
                _dataAccessManager.AddInParameter(dbCommand, "@DocumentTitle", DbType.String, sTitle);
                _dataAccessManager.AddInParameter(dbCommand, "@Author", DbType.String, sAuthor);
                _dataAccessManager.AddInParameter(dbCommand, "@CreatedDate", DbType.String, sCreatedDate);

                _dataAccessManager.AddInParameter(dbCommand, "@DocumentSource", DbType.String, DocumentSource);
                _dataAccessManager.AddInParameter(dbCommand, "@SourceCommunityID", DbType.String, SourceCommunityID);
                _dataAccessManager.AddInParameter(dbCommand, "@SourceRepositoryID", DbType.String, SourceRepositryID);
                _dataAccessManager.AddInParameter(dbCommand, "@FacilityID", DbType.String, FacilityID);
                _dataAccessManager.AddInParameter(dbCommand, "@Reposed", DbType.Boolean, Reposed);
                _dataAccessManager.AddInParameter(dbCommand, "@FilePath", DbType.String, filePath);
                returnValue = Convert.ToInt32(_dataAccessManager.ExecuteNonQuery(dbCommand));
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message.ToString());
            }
            return returnValue;
        }

        public string GetUserMPIIDByUserID(string UserID)
        {
            DbCommand dbCommand;
            string sUserMPIID = "";
            try
            {
                DataAccessManager _dataAccessManager = DataAccessManager.GetInstance;
                dbCommand = _dataAccessManager.GetStoredProcCommand("GetUserMPIIDByUserID");
                _dataAccessManager.AddInParameter(dbCommand, "UserID", DbType.String, UserID);
                sUserMPIID = _dataAccessManager.ExecuteScalar(dbCommand).ToString();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }
            return sUserMPIID;
        }

        public int UpdateUserInfo(string Userguid, int isoptin)
        {
            DbCommand dbCommand;
            int returnValue = 0;
            try
            {
                dbCommand = _dataAccessManager.GetStoredProcCommand("UpdateUserInfo");

                _dataAccessManager.AddInParameter(dbCommand, "@mpiId", DbType.String, Userguid);
                _dataAccessManager.AddInParameter(dbCommand, "@optIn", DbType.String, isoptin);


                returnValue = Convert.ToInt32(_dataAccessManager.ExecuteNonQuery(dbCommand));
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message.ToString());
            }
            return returnValue;
        }

        public int UpdatePatientConsentInfo(string PatientConsentId)
        {
            DbCommand dbCommand;
            int returnValue = 0;
            try
            {
                dbCommand = _dataAccessManager.GetStoredProcCommand("UpdatePatientConsent");

                _dataAccessManager.AddInParameter(dbCommand, "@PatientConsentID", DbType.String, PatientConsentId);



                returnValue = Convert.ToInt32(_dataAccessManager.ExecuteNonQuery(dbCommand));
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message.ToString());
            }
            return returnValue;
        }

        public int ShareToNewProvider(string SharedDocumentID, string OriginalDocumentID, int ConfidentialityCode, string RuleStartDate, string RuleEndDate, string ProviderEmail, int ProviderRole, int OrganizationID, int HomeCommunityID, int PurposeForUse, string EffectiveTime, string Images)
        {
            DbCommand dbCommand;
            int returnValue = 0;
            try
            {
                RemoveShare(OriginalDocumentID, ProviderEmail);
                dbCommand = _dataAccessManager.GetStoredProcCommand("AddPatientsharedWith");

                _dataAccessManager.AddInParameter(dbCommand, "@SharedDocumentID", DbType.String, SharedDocumentID);
                _dataAccessManager.AddInParameter(dbCommand, "@OriginalDocumentID", DbType.String, OriginalDocumentID);
                _dataAccessManager.AddInParameter(dbCommand, "@ConfidentialityCode", DbType.Int32, ConfidentialityCode);
                _dataAccessManager.AddInParameter(dbCommand, "@RuleStartDate", DbType.Date, RuleStartDate);
                _dataAccessManager.AddInParameter(dbCommand, "@RuleEndDate", DbType.Date, RuleEndDate);
                _dataAccessManager.AddInParameter(dbCommand, "@ProviderEmail", DbType.String, ProviderEmail);

                if (ProviderRole > 0)
                    _dataAccessManager.AddInParameter(dbCommand, "@ProviderRole", DbType.Int32, ProviderRole);
                else
                    _dataAccessManager.AddInParameter(dbCommand, "@ProviderRole", DbType.Int32, DBNull.Value);

                if (OrganizationID > 0)
                    _dataAccessManager.AddInParameter(dbCommand, "@OrganizationID", DbType.Int32, OrganizationID);
                else
                    _dataAccessManager.AddInParameter(dbCommand, "@OrganizationID", DbType.Int32, DBNull.Value);

                if (HomeCommunityID > 0)
                    _dataAccessManager.AddInParameter(dbCommand, "@HomeCommunityID", DbType.Int32, HomeCommunityID);
                else
                    _dataAccessManager.AddInParameter(dbCommand, "@HomeCommunityID", DbType.Int32, HomeCommunityID);
                if (PurposeForUse > 0)
                    _dataAccessManager.AddInParameter(dbCommand, "@PurposeForUse", DbType.Int32, PurposeForUse);
                else
                    _dataAccessManager.AddInParameter(dbCommand, "@PurposeForUse", DbType.Int32, DBNull.Value);

                _dataAccessManager.AddInParameter(dbCommand, "@EffectiveTime", DbType.String, EffectiveTime);
                _dataAccessManager.AddInParameter(dbCommand, "@Images", DbType.String, Images);

                returnValue = Convert.ToInt32(_dataAccessManager.ExecuteNonQuery(dbCommand));
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message.ToString());
            }
            return returnValue;
        }

        public int RemoveShare(string OriginalDocumentID, string ProviderEmail)
        {
            DbCommand dbCommand;
            int returnValue = 0;
            try
            {
                dbCommand = _dataAccessManager.GetStoredProcCommand("RemoveShare");
                _dataAccessManager.AddInParameter(dbCommand, "@OriginalDocumentID", DbType.String, OriginalDocumentID);
                _dataAccessManager.AddInParameter(dbCommand, "@ProviderEmail", DbType.String, ProviderEmail);

                returnValue = Convert.ToInt32(_dataAccessManager.ExecuteNonQuery(dbCommand));
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message.ToString());
            }
            return returnValue;
        }

        public DataSet GetDocumentByPatientId(string PatientID)
        {
            DbCommand dbCommand;
            DataSet dataSet = null;
            try
            {
                dbCommand = _dataAccessManager.GetStoredProcCommand("GetRefferedPatientByID");
                _dataAccessManager.AddInParameter(dbCommand, "@PatientID", DbType.String, PatientID);

                dataSet = _dataAccessManager.ExecuteDataSet(dbCommand);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }
            return dataSet;
        }

        public DataSet GetSharedWithById(string ReferredPatientID)
        {
            DbCommand dbCommand;
            DataSet dataSet = null;
            try
            {
                dbCommand = _dataAccessManager.GetStoredProcCommand("GetSharedWithByID");
                _dataAccessManager.AddInParameter(dbCommand, "@OriginalDocumentID", DbType.String, ReferredPatientID);

                dataSet = _dataAccessManager.ExecuteDataSet(dbCommand);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }
            return dataSet;
        }

        public int ValidateUserForDocument(string UserEmail, string DocumentId)
        {
            DbCommand dbCommand;
            int returnValue = 0;
            try
            {
                dbCommand = _dataAccessManager.GetStoredProcCommand("ValidateUserForDocument");
                _dataAccessManager.AddInParameter(dbCommand, "@UserEmail", DbType.String, UserEmail);
                _dataAccessManager.AddInParameter(dbCommand, "@DocumentId", DbType.String, DocumentId);


                returnValue = Convert.ToInt32(_dataAccessManager.ExecuteScalar(dbCommand));
            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }
            return returnValue;
        }

        public int AddPatientAttachment(string PatientID, string Name, string Description, string ImagePath)
        {
            DbCommand dbCommand;
            int returnValue = 0;
            try
            {
                dbCommand = _dataAccessManager.GetStoredProcCommand("AddPatientAttachment");

                _dataAccessManager.AddInParameter(dbCommand, "@PatientID", DbType.String, PatientID);
                _dataAccessManager.AddInParameter(dbCommand, "@Name", DbType.String, Name);
                _dataAccessManager.AddInParameter(dbCommand, "@Description", DbType.String, Description);
                _dataAccessManager.AddInParameter(dbCommand, "@ImagePath", DbType.String, ImagePath);

                returnValue = Convert.ToInt32(_dataAccessManager.ExecuteNonQuery(dbCommand));
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message.ToString());
            }
            return returnValue;
        }

        /// <summary>
        /// Gets the patient attachment.
        /// </summary>
        /// <param name="PatientID">The patient ID.</param>
        /// <returns></returns>
        public DataSet GetPatientAttachment(string PatientID, string SharedDocumentId)
        {
            DbCommand dbCommand;
            DataSet dsAttachments = null;
            try
            {
                dbCommand = _dataAccessManager.GetStoredProcCommand("GetPatientAttachment");
                _dataAccessManager.AddInParameter(dbCommand, "@PatientID", DbType.String, PatientID);
                _dataAccessManager.AddInParameter(dbCommand, "@SharedDocumentId", DbType.String, SharedDocumentId);
                dsAttachments = _dataAccessManager.ExecuteDataSet(dbCommand);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message.ToString());
            }
            return dsAttachments;
        }

        /// <summary>
        /// Gets the patient attachment.
        /// </summary>
        /// <param name="PatientID">The patient ID.</param>
        /// <returns></returns>
        public DataSet GetSharedAttachment(string SharedDocumentId, string ProviderEmail)
        {
            DbCommand dbCommand;
            DataSet dsAttachments = null;
            try
            {
                dbCommand = _dataAccessManager.GetStoredProcCommand("GetSharedAttachment");
                _dataAccessManager.AddInParameter(dbCommand, "@SharedDocumentId", DbType.String, SharedDocumentId);
                _dataAccessManager.AddInParameter(dbCommand, "@ProviderEmail", DbType.String, ProviderEmail);
                dsAttachments = _dataAccessManager.ExecuteDataSet(dbCommand);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message.ToString());
            }
            return dsAttachments;
        }

        public DataSet getNhinCommunities()
        {
            DbCommand dbCommand;
            DataSet dataSet = null;
            try
            {
                dbCommand = _dataAccessManager.GetStoredProcCommand("PR_GETNHINCOMMUNITY");
                dataSet = _dataAccessManager.ExecuteDataSet(dbCommand);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message.ToString());
            }
            return dataSet;
        }
        public DataSet GetDocumentMetaData(string DocumentID)
        {
            DbCommand dbCommand;
            DataSet dataSet = null;
            try
            {
                dbCommand = _dataAccessManager.GetStoredProcCommand("GetDocumentMetaData");
                _dataAccessManager.AddInParameter(dbCommand, "@DocumentID", DbType.String, DocumentID);
                dataSet = _dataAccessManager.ExecuteDataSet(dbCommand);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }
            return dataSet;
        }
    }
}



