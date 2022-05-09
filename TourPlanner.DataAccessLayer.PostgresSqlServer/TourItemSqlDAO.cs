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
    public class TourItemSqlDAO : ITourItemDAO
    {
        private const string SQL_FIND_BY_ID = "SELECT * FROM public.\"tour_item\" WHERE \"tour_item_id\"=@Id;";
        private const string SQL_GET_ALL_TOURS = "SELECT * FROM public.\"tour_item\";";
        private const string SQL_INSERT_NEW_TOUR = "INSERT INTO public.\"tour_item\" (\"name\", \"description\", \"from\",\"to\",\"image_path\",\"distance\") VALUES (@Name, @Description, @From , @To , @ImagePath , @Distance) RETURNING \"tour_item_id\";";
        private const string SQL_GET_LAST_TOURID = "SELECT * FROM public.\"tour_item\" ORDER BY \"tour_item_id\" DESC LIMIT 1;";

        private IDatabase database;

        public TourItemSqlDAO()
        {
            this.database = DALFactory.GetDatabase();
        }

        public TourItemSqlDAO(IDatabase database)
        {
            this.database = database;
        }
        // add new TourItem
        public TourItem AddNewTourItem(TourItem tourItem)
        {
            DbCommand insertCommand = database.CreateCommand(SQL_INSERT_NEW_TOUR);
            database.DefineParameter(insertCommand, "@Name", DbType.String, tourItem.Name);
            database.DefineParameter(insertCommand, "@Description", DbType.String, tourItem.Description);
            database.DefineParameter(insertCommand, "@From", DbType.String, tourItem.From);
            database.DefineParameter(insertCommand, "@To", DbType.String, tourItem.To);
            database.DefineParameter(insertCommand, "@ImagePath", DbType.String, tourItem.ImagePath);
            database.DefineParameter(insertCommand, "@Distance", DbType.Double, tourItem.Distance);

            return FindTourItemById(database.ExecuteScalar(insertCommand));
        }

        // find tourItem by id
        public TourItem FindTourItemById(int tourItemId)
        {
            DbCommand findCommand = database.CreateCommand(SQL_FIND_BY_ID);
            database.DefineParameter(findCommand, "@Id", DbType.Int32, tourItemId);

            IEnumerable<TourItem> tours = QueryToursFromDb(findCommand);

            return tours.FirstOrDefault();
        }

        // get all tourItem
        public IEnumerable<TourItem> GetTourItems()
        {
            DbCommand getAllToursCommand = database.CreateCommand(SQL_GET_ALL_TOURS);
            return QueryToursFromDb(getAllToursCommand);
        }

        // get last tourId
        public int GetLastTourId()
        {
            DbCommand getLastTourIdCommand = database.CreateCommand(SQL_GET_LAST_TOURID);
            IEnumerable<TourItem> tourItems = QueryToursFromDb(getLastTourIdCommand);
            return tourItems.FirstOrDefault().TourId;
        }

        //quering tours from postgres database
        private IEnumerable<TourItem> QueryToursFromDb(DbCommand command)
        {
            List<TourItem> tourList = new List<TourItem>();
            try
            {
                using (IDataReader reader = database.ExecuteReader(command))
                {
                    while (reader.Read())
                    {
                        tourList.Add(new TourItem(
                            Convert.ToInt32(reader["tour_item_id"]),
                            reader["name"].ToString(),
                            reader["description"].ToString(),
                            reader["from"].ToString(),
                            reader["to"].ToString(),
                            reader["image_path"].ToString(),
                            Convert.ToDouble(reader["distance"])
                        ));
                    }
                }
            }
            catch (System.Exception)
            {
                throw;
            }

            return tourList;
        }
    }
}
