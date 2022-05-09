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

        private const string SQL_FIND_BY_ID = "SELECT * FROM public.\"tour_log\" WHERE \"tour_log_id\"=@Id";
        private const string SQL_FIND_BY_TOUR = "SELECT * FROM public.\"tour_log\" WHERE \"tour_item_fk\"=@TourItemId";
        private const string SQL_INSERT_NEW_LOG = "INSERT INTO public.\"tour_log\" (\"date_time\", \"report\", \"distance\", \"total_time\", \"rating\", \"tour_item_fk\") VALUES (@DateTime, @Report, @Distance, @TotalTime, @Rating, @TourID);";

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

        public TourLog AddNewTourLog(TourLog tourLog, TourItem tourItem)
        {
            DbCommand insertCommand = database.CreateCommand(SQL_INSERT_NEW_LOG);
            database.DefineParameter(insertCommand, "@DateTime", DbType.Date, tourLog.DateTime);
            database.DefineParameter(insertCommand, "@Report", DbType.String, tourLog.Report);
            database.DefineParameter(insertCommand, "@Distance", DbType.Double, tourLog.Distance);
            database.DefineParameter(insertCommand, "@TotalTime", DbType.Time, tourLog.DateTime);
            database.DefineParameter(insertCommand, "@Rating", DbType.Double, tourLog.Rating);
            database.DefineParameter(insertCommand, "@TourID", DbType.Int32, tourItem.TourId);

            return FindTourLogById(database.ExecuteScalar(insertCommand));
        }

        public TourLog FindTourLogById(int itemLogId)
        {
            DbCommand command = database.CreateCommand(SQL_FIND_BY_ID);
            database.DefineParameter(command, "@Id", DbType.Int32, itemLogId);
            IEnumerable<TourLog> logList = QueryLogFromDb(command);
            return logList.FirstOrDefault();
        }

        public IEnumerable<TourLog> GetLogItems(TourItem tourItem)
        {
            DbCommand command = database.CreateCommand(SQL_FIND_BY_TOUR);
            database.DefineParameter(command, "@TourItemId", DbType.Int32, tourItem.TourId);
            return QueryLogFromDb(command);
        }

        private IEnumerable<TourLog> QueryLogFromDb(DbCommand command)
        {
            List<TourLog> logList = new List<TourLog>();

            using (IDataReader reader = database.ExecuteReader(command))
            {
                while (reader.Read())
                {
                    logList.Add(new TourLog(
                       (int)reader["tour_log_id"],
                       (DateTime)reader["date_time"],
                       (string)reader["report"],
                       (double)reader["distance"],
                       (TimeSpan)reader["total_time"],
                       (double)reader["rating"],
                       tourItem.FindTourItemById((int)reader["tour_item_fk"])
                   ));
                }
            }
            return logList;
        }
    }
}
