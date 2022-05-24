using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using TourPlanner.DataAccessLayer.Common;
using TourPlanner.DataAccessLayer.DAO;
using TourPlanner.Models;

namespace TourPlanner.DataAccessLayer.PostgresSqlServer
{
    public class TourLogSqlDAO : ITourLogDAO
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);


        private const string SQL_FIND_BY_ID = "SELECT * FROM public.\"tour_log\" WHERE \"tour_log_id\"=@Id";
        private const string SQL_FIND_BY_TOUR = "SELECT * FROM public.\"tour_log\" WHERE \"tour_item_fk\"=@TourItemId";
        private const string SQL_INSERT_NEW_LOG = "INSERT INTO public.\"tour_log\" (\"date_time\", \"report\", \"difficulty\", \"total_time\", \"rating\", \"tour_item_fk\") VALUES (@DateTime, @Report, @Difficulty, @TotalTime, @Rating, @TourID);";
        private const string SQL_PUT_Log_BY_ID = "Update public.\"tour_log\" SET \"date_time\"=@DateTime , \"report\" =@Report , \"difficulty\"=@Difficulty ,\"total_time\"=@TotalTime ,\"rating\" =@Rating WHERE \"tour_log_id\"=@LogId";
        private const string SQL_DELETE_BY_ID = "DELETE FROM public.\"tour_log\" WHERE \"tour_log_id\"=@Id";

        private IDatabase database;
        private ITourItemDAO tourItem;

        public TourLogSqlDAO()
        {
            this.database = DALFactory.GetDatabase();
            this.tourItem = DALFactory.CreateTourItemDAO();
        }

        public TourLogSqlDAO(IDatabase database, ITourItemDAO tourItemo)
        {
            this.database = database;
            this.tourItem = tourItemo;
        }

        // add new Log
        public TourLog AddNewTourLog(TourLog tourLog)
        {
            try
            {
                DbCommand insertCommand = database.CreateCommand(SQL_INSERT_NEW_LOG);
                database.DefineParameter(insertCommand, "@DateTime", DbType.Date, tourLog.DateTime);
                database.DefineParameter(insertCommand, "@Report", DbType.String, tourLog.Report);
                database.DefineParameter(insertCommand, "@Difficulty", DbType.String, tourLog.Difficulty);
                database.DefineParameter(insertCommand, "@TotalTime", DbType.Time, tourLog.TotalTime);
                database.DefineParameter(insertCommand, "@Rating", DbType.String, tourLog.Rating);
                database.DefineParameter(insertCommand, "@TourID", DbType.Int32, tourLog.LogTourItem.TourId);

                return FindTourLogById(database.ExecuteScalar(insertCommand));
            }
            catch (Exception ex)
            {
                string strResponseValue = "{\"errorMessages\":[\"" + ex.Message.ToString() + "\"],\"errors\":{}}";
                log.Error(strResponseValue, ex);
            }
            return null;
        }

        // find Log by id
        public TourLog FindTourLogById(int itemLogId)
        {
            try
            {
                DbCommand command = database.CreateCommand(SQL_FIND_BY_ID);
                database.DefineParameter(command, "@Id", DbType.Int32, itemLogId);
                IEnumerable<TourLog> logList = QueryLogFromDb(command);
                return logList.FirstOrDefault();
            }
            catch (Exception ex)
            {
                string strResponseValue = "{\"errorMessages\":[\"" + ex.Message.ToString() + "\"],\"errors\":{}}";
                log.Error(strResponseValue, ex);
            }
            return null;
           
        }

        // get Log
        public IEnumerable<TourLog> GetLogItems(TourItem tourItem)
        {
            try
            {
                DbCommand command = database.CreateCommand(SQL_FIND_BY_TOUR);
                database.DefineParameter(command, "@TourItemId", DbType.Int32, tourItem.TourId);
                return QueryLogFromDb(command);
            }
            catch (Exception ex)
            {
                string strResponseValue = "{\"errorMessages\":[\"" + ex.Message.ToString() + "\"],\"errors\":{}}";
                log.Error(strResponseValue, ex);
            }
            return null;
            
        }

        // edit Log
        public TourLog EditTourLog(TourLog tourLog)
        {
            try
            {
                DbCommand editCommand = database.CreateCommand(SQL_PUT_Log_BY_ID);
                database.DefineParameter(editCommand, "@DateTime", DbType.Date, tourLog.DateTime);
                database.DefineParameter(editCommand, "@Report", DbType.String, tourLog.Report);
                database.DefineParameter(editCommand, "@Difficulty", DbType.String, tourLog.Difficulty);
                database.DefineParameter(editCommand, "@TotalTime", DbType.Time, tourLog.TotalTime);
                database.DefineParameter(editCommand, "@Rating", DbType.String, tourLog.Rating);
                database.DefineParameter(editCommand, "@LogId", DbType.Int32, tourLog.LogId);

                return FindTourLogById(database.ExecuteScalar(editCommand));
            }
            catch (Exception ex)
            {
                string strResponseValue = "{\"errorMessages\":[\"" + ex.Message.ToString() + "\"],\"errors\":{}}";
                log.Error(strResponseValue, ex);
            }
            return null;
           
        }

        // delete TourLog
        public void DeleteTourLog(TourLog tourLog)
        {
            try
            {
                DbCommand deleteCommand = database.CreateCommand(SQL_DELETE_BY_ID);
                database.DefineParameter(deleteCommand, "@Id", DbType.Int32, tourLog.LogId);
                database.ExecuteScalar(deleteCommand);
            }
            catch (Exception ex)
            {
                string strResponseValue = "{\"errorMessages\":[\"" + ex.Message.ToString() + "\"],\"errors\":{}}";
                log.Error(strResponseValue, ex);
            }

            
        }
        private IEnumerable<TourLog> QueryLogFromDb(DbCommand command)
        {
            List<TourLog> logList = new List<TourLog>();
            try
            {
                using (IDataReader reader = database.ExecuteReader(command))
                {
                    while (reader.Read())
                    {
                        logList.Add(new TourLog(
                           (int)reader["tour_log_id"],
                           (DateTime)reader["date_time"],
                           (string)reader["report"],
                           (string)reader["difficulty"],
                           (TimeSpan)reader["total_time"],
                           (string)reader["rating"],
                           tourItem.FindTourItemById((int)reader["tour_item_fk"])
                       ));
                    }
                }
                return logList;
            }
            catch (Exception ex)
            {
                string strResponseValue = "{\"errorMessages\":[\"" + ex.Message.ToString() + "\"],\"errors\":{}}";
                log.Error(strResponseValue, ex);
            }
            return null;
        }
    }
}
