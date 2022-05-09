using Npgsql;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TourPlanner.DataAccessLayer.Common;

namespace TourPlanner.DataAccessLayer.PostgresSqlServer
{
    class Database : IDatabase
    {
        private string connectionString;

        public Database(string connectionString)
        {
            this.connectionString = connectionString;
        }

        // create new sql command
        public DbCommand CreateCommand(string genericCommandText)
        {
            return new NpgsqlCommand(genericCommandText);
        }

        // declare params for sql command
        public int DeclareParameter(DbCommand command, string name, DbType type)
        {
            if (!command.Parameters.Contains(name))
            {
                int index = command.Parameters.Add(new NpgsqlParameter(name, type));
                return index;
            }
            throw new ArgumentException(string.Format("Parameter {0} already exists.", name));
        }

        // Define params for sql command
        public void DefineParameter(DbCommand command, string name, DbType type, object value)
        {
            int index = DeclareParameter(command, name, type);
            command.Parameters[index].Value = value;
        }

        //connecting to the database
        private DbConnection CreateOpenConnection()
        {
            DbConnection connection = new NpgsqlConnection(this.connectionString);
            connection.Open();

            return connection;
        }

        // execute command, return datareader and close connection
        public IDataReader ExecuteReader(DbCommand command)
        {
            command.Connection = CreateOpenConnection();
            return command.ExecuteReader(CommandBehavior.CloseConnection);
        }

        // execute command and close connection
        public int ExecuteScalar(DbCommand command)
        {
            command.Connection = CreateOpenConnection();
            return Convert.ToInt32(command.ExecuteScalar());
        }

        // set params 
        public void SetParameter(DbCommand command, string name, object value)
        {
            if (command.Parameters.Contains(name))
            {
                command.Parameters[name].Value = value;
            }
            else
            {
                throw new ArgumentException(string.Format("Parameter {0} does not exist.", name));
            }
        }
    }
}
