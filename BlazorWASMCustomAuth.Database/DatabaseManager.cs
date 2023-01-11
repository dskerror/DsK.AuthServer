using System.Data;
using System.Data.SqlClient;
using System.Reflection;

namespace BlazorWASMCustomAuth.Database
{
    public class DatabaseManager
    {
        protected string? _databaseName = null;
        protected string? _connString = null;
        protected SqlConnection _conn = new SqlConnection();
        protected SqlTransaction? _trans = null;
        protected bool _disposed = false;

        public static string? ConnectionString { get; set; }

        public DatabaseManager() : this(ConnectionString)
        {
        }
        public DatabaseManager(string? connectionString)
        {
            _connString = connectionString;
            _databaseName = GetInitialCatalog();
        }

        /// <summary>
        /// Returns the current SqlTransaction object or null if no transaction
        /// is in effect.
        /// </summary>
        //public SqlTransaction Transaction { get { return _trans; } }

        protected void OpenConnection()
        {
            if (_conn == null)
            {
                _conn = new SqlConnection(_connString);
                _conn.Open();
            }
            else if (_conn.State == ConnectionState.Closed)
            {
                _conn = new SqlConnection(_connString);
                _conn.Open();
            }
        }

        protected void CloseConnection()
        {
            if (_conn.State == ConnectionState.Open)
            {
                _conn.Close();
                _conn.Dispose();
            }
        }

        /// <summary>
        /// Constructs a SqlCommand with the given parameters. This method is normally called
        /// from the other methods and not called directly. But here it is if you need access
        /// to it.
        /// </summary>
        /// <param name="qry">SQL query or stored procedure name</param>
        /// <param name="type">Type of SQL command</param>
        /// <param name="args">Query arguments. Arguments should be in pairs where one is the
        /// name of the parameter and the second is the value. The very last argument can
        /// optionally be a SqlParameter object for specifying a custom argument type</param>
        /// <returns></returns>
        public SqlCommand CreateCommand(string qry, CommandType type, params object[] args)
        {
            OpenConnection();
            SqlCommand cmd = new SqlCommand(qry, _conn);

            // Associate with current transaction, if any
            if (_trans != null)
                cmd.Transaction = _trans;

            // Set command type
            cmd.CommandType = type;

            // Construct SQL parameters
            for (int i = 0; i < args.Length; i++)
            {
                if (args[i] is string && i < (args.Length - 1))
                {
                    SqlParameter parm = new SqlParameter();
                    parm.ParameterName = (string)args[i];
                    parm.Value = args[++i];
                    cmd.Parameters.Add(parm);
                }
                else if (args[i] is SqlParameter)
                {
                    cmd.Parameters.Add((SqlParameter)args[i]);
                }
                else throw new ArgumentException("Invalid number or type of arguments supplied");
            }

            return cmd;
        }



        #region Exec Members

        /// <summary>
        /// Executes a query that returns no results
        /// </summary>
        /// <param name="qry">Query text</param>
        /// <param name="args">Any number of parameter name/value pairs and/or SQLParameter arguments</param>
        /// <returns>The number of rows affected</returns>
        public int ExecNonQuery(string qry, params object[] args)
        {
            using (SqlCommand cmd = CreateCommand(qry, CommandType.Text, args))
            {
                return cmd.ExecuteNonQuery();
            }
        }


        /// <summary>
        /// Executes a query that returns no results
        /// </summary>
        /// <param name="qry">Query text</param>
        /// <param name="args">Any number of parameter name/value pairs and/or SQLParameter arguments</param>
        /// <returns>The number of rows affected</returns>
        public DatabaseExecResult ExecNonQueryNEW(string qry, params object[] args)
        {
            //var result = new DatabaseExecResult();
            using (SqlCommand cmd = CreateCommand(qry, CommandType.Text, args))
            {
                try
                {
                    return new DatabaseExecResult(cmd.ExecuteNonQuery());
                }
                catch (Exception ex)
                {
                    return new DatabaseExecResult(cmd.ExecuteNonQuery(), ex);
                }
            }
        }



        /// <summary>
        /// Executes a query that returns a single value
        /// </summary>
        /// <param name="qry">Query text</param>
        /// <param name="args">Any number of parameter name/value pairs and/or SQLParameter arguments</param>
        /// <returns>Value of first column and first row of the results</returns>
        public object ExecScalar(string qry, params object[] args)
        {
            using (SqlCommand cmd = CreateCommand(qry, CommandType.Text, args))
            {
                return cmd.ExecuteScalar();
            }
        }

        /// <summary>
        /// Executes a query that returns a single value
        /// </summary>
        /// <param name="qry">Query text</param>
        /// <param name="args">Any number of parameter name/value pairs and/or SQLParameter arguments</param>
        /// <returns>Value of first column and first row of the results</returns>
        public DatabaseExecResult ExecScalarNEW(string qry, params object[] args)
        {
            using (SqlCommand cmd = CreateCommand(qry, CommandType.Text, args))
            {
                try
                {
                    return new DatabaseExecResult(cmd.ExecuteScalar());
                }
                catch (Exception ex)
                {
                    return new DatabaseExecResult(cmd.ExecuteScalar(), ex);
                }
            }
        }


        /// <summary>
        /// Executes a query that returns a single value
        /// </summary>
        /// <param name="qry">Query text</param>
        /// <param name="args">Any number of parameter name/value pairs and/or SQLParameter arguments</param>
        /// <returns>Value of first column and first row of the results</returns>
        public DatabaseExecResult ExecScalarSP(string qry, params object[] args)
        {
            using (SqlCommand cmd = CreateCommand(qry, CommandType.StoredProcedure, args))
            {
                try
                {
                    return new DatabaseExecResult(cmd.ExecuteScalar());
                }
                catch (Exception ex)
                {
                    return new DatabaseExecResult(cmd.ExecuteScalar(), ex);
                }
            }
        }

        /// <summary>
        /// Executes a query and returns the results as a SqlDataReader
        /// </summary>
        /// <param name="qry">Query text</param>
        /// <param name="args">Any number of parameter name/value pairs and/or SQLParameter arguments</param>
        /// <returns>Results as a SqlDataReader</returns>
        public SqlDataReader ExecDataReader(string qry, params object[] args)
        {
            using (SqlCommand cmd = CreateCommand(qry, CommandType.Text, args))
            {
                return cmd.ExecuteReader();
            }
        }

        /// <summary>
        /// Executes a query and returns the results as a DataSet
        /// </summary>
        /// <param name="qry">Query text</param>
        /// <param name="args">Any number of parameter name/value pairs and/or SQLParameter arguments</param>
        /// <returns>Results as a DataSet</returns>
        public DataSet ExecDataSet(string qry, params object[] args)
        {
            using (SqlCommand cmd = CreateCommand(qry, CommandType.Text, args))
            {
                SqlDataAdapter adapt = new SqlDataAdapter(cmd);
                DataSet ds = new DataSet();
                adapt.Fill(ds);
                return ds;
            }
        }


        /// <summary>
        /// Executes a query and returns the results as a DataSet
        /// </summary>
        /// <param name="qry">Query text</param>
        /// <param name="args">Any number of parameter name/value pairs and/or SQLParameter arguments</param>
        /// <returns>Results as a DataTable</returns>
        public DataTable ExecDataTable(string qry, params object[] args)
        {
            using (SqlCommand cmd = CreateCommand(qry, CommandType.Text, args))
            {
                SqlDataAdapter adapt = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                adapt.Fill(dt);
                return dt;
            }
        }


        /// <summary>
        /// Executes a query and returns the results as a DataSet
        /// </summary>
        /// <param name="qry">Query text</param>
        /// <param name="args">Any number of parameter name/value pairs and/or SQLParameter arguments</param>
        /// <returns>Results as a DataTable</returns>
        public DataTable ExecDataTableSP(string qry, params object[] args)
        {
            using (SqlCommand cmd = CreateCommand(qry, CommandType.StoredProcedure, args))
            {
                SqlDataAdapter adapt = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                adapt.Fill(dt);
                return dt;
            }
        }

        #endregion

        #region Transaction Members

        /// <summary>
        /// Begins a transaction
        /// </summary>
        /// <returns>The new SqlTransaction object</returns>
        //public SqlTransaction BeginTransaction()
        //{
        //	Rollback();
        //	_trans = _conn.BeginTransaction();
        //	return Transaction;
        //}

        ///// <summary>
        ///// Commits any transaction in effect.
        ///// </summary>
        //public void Commit()
        //{
        //	if (_trans != null)
        //	{
        //		_trans.Commit();
        //		_trans = null;
        //	}
        //}

        ///// <summary>
        ///// Rolls back any transaction in effect.
        ///// </summary>
        //public void Rollback()
        //{
        //	if (_trans != null)
        //	{
        //		_trans.Rollback();
        //		_trans = null;
        //	}
        //}

        #endregion

        #region IDisposable Members

        //public void Dispose()
        //{
        //	Dispose(true);
        //	GC.SuppressFinalize(this);
        //}

        //protected virtual void Dispose(bool disposing)
        //{
        //	if (!_disposed)
        //	{
        //		// Need to dispose managed resources if being called manually
        //		if (disposing)
        //		{
        //			if (_conn != null)
        //			{
        //				Rollback();
        //				_conn.Dispose();
        //				_conn = null;
        //			}
        //		}
        //		_disposed = true;
        //	}
        //}

        #endregion

        #region Database Management

        public string GetInitialCatalog()
        {
            var builder = new SqlConnectionStringBuilder(_connString);
            return builder.InitialCatalog;
        }
        public bool CreateDatabase(string DatabaseName)
        {
            ExecNonQuery("CREATE DATABASE " + DatabaseName.Trim());
            return VerifyDatabaseExists(DatabaseName.Trim());
        }
        public bool VerifyDatabaseExists(string DatabaseName)
        {
            var i = ExecDataTable("SELECT name FROM master.dbo.sysdatabases WHERE name = '" + DatabaseName + "'");

            if (i.Rows.Count == 1)
                return true;
            else
                return false;
        }
        public bool VerifySchemaExists(string SchemaName)
        {
            var i = ExecDataTable("SELECT schema_name FROM information_schema.schemata WHERE schema_name = '" + SchemaName + "'");

            if (i.Rows.Count == 1)
                return true;
            else
                return false;
        }
        public bool CreateSchema(string SchemaName)
        {
            ExecNonQuery("CREATE SCHEMA " + SchemaName.Trim());
            return VerifySchemaExists(SchemaName.Trim());
        }


        #endregion

        #region Table Management
        public bool VerifyTableExists(string TableName, string SchemaName = "dbo", string DatabaseName = "")
        {
            //todo : add database change
            var i = ExecDataTable("SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_SCHEMA = N'" + SchemaName + "' AND TABLE_NAME = N'" + TableName + "'");

            if (i.Rows.Count == 1)
                return true;
            else
                return false;
        }

        public DataTable ListTableColumns(string TableName, string SchemaName = "dbo", string DatabaseName = "")
        {
            //return ExecDataTable("SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_SCHEMA = N'" + SchemaName + "' AND TABLE_NAME = N'" + TableName + "'");

            return ExecDataTable($"SELECT schema_name(tab.schema_id) as schema_name," +
            "tab.name as TableName," +
            "col.column_id as SQLColumnId," +
            "col.name as ColumnName," +
            "t.name as DataType," +
            "col.is_identity as IsIdentity," +
            "col.is_nullable as IsNullable," +
            "col.max_length as [MaxLength]," +
            "col.precision as NumericPrecision," +
            "col.scale as NumericScale " +
            "FROM sys.tables as tab " +
            "INNER JOIN sys.columns as col ON tab.object_id = col.object_id " +
            "LEFT JOIN sys.types as t ON col.user_type_id = t.user_type_id " +
            "WHERE tab.Name = N'" + TableName + "' AND schema_name(tab.schema_id) = N'" + SchemaName + "' " +
            "ORDER BY schema_name, TableName,column_id");
        }
        #endregion

        #region Converters   

        public List<T> ConvertDataTableToList<T>(DataTable dt)
        {
            List<T> data = new List<T>();
            foreach (DataRow row in dt.Rows)
            {
                T item = GetItem<T>(row);
                data.Add(item);
            }
            return data;
        }

        public T ConvertDataTableToObject<T>(DataTable dt) where T : new()
        {
            T item = new T();
            foreach (DataRow row in dt.Rows)
            {
                item = GetItem<T>(row);
            }
            return item;
        }
        private T GetItem<T>(DataRow dr)
        {
            Type temp = typeof(T);
            T obj = Activator.CreateInstance<T>();

            foreach (DataColumn column in dr.Table.Columns)
            {
                foreach (PropertyInfo pro in temp.GetProperties())
                {
                    if (pro.Name == column.ColumnName)
                        if (dr[column.ColumnName] != DBNull.Value)
                            pro.SetValue(obj, dr[column.ColumnName], null);
                        else
                            continue;
                }
            }
            return obj;
        }

        #endregion
    }
}