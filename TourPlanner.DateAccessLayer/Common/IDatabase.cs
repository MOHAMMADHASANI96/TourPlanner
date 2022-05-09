using System.Data;
using System.Data.Common;


namespace TourPlanner.DataAccessLayer.Common
{
    public interface IDatabase
    {
        //DbCommand object is used to send a SQL command to the data store.
        //It can be a Data Manipulation Language (DML) command
        //to retrieve, insert, update, or delete data
        DbCommand CreateCommand(string genericCommandText);
        int DeclareParameter(DbCommand command, string name, DbType type);
        void DefineParameter(DbCommand command, string name, DbType type, object value);
        void SetParameter(DbCommand command, string name, object value);
        IDataReader ExecuteReader(DbCommand command);
        int ExecuteScalar(DbCommand command);
    }
}
