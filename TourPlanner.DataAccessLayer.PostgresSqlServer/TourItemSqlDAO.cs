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
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);


        private const string SQL_FIND_BY_ID = "SELECT * FROM public.\"tour_item\" WHERE \"tour_item_id\"=@Id;";
        private const string SQL_FIND_BY_Name = "SELECT * FROM public.\"tour_item\" WHERE \"name\"=@Name;";
        private const string SQL_GET_ALL_TOURS = "SELECT * FROM public.\"tour_item\";";
        private const string SQL_INSERT_NEW_TOUR = "INSERT INTO public.\"tour_item\" (\"name\", \"description\", \"from\",\"to\",\"image_path\",\"distance\" ,\"transport_type\" ) VALUES (@Name, @Description, @From , @To , @ImagePath , @Distance , @TransportType) RETURNING \"tour_item_id\";";
        private const string SQL_GET_LAST_TOURID = "SELECT * FROM public.\"tour_item\" ORDER BY \"tour_item_id\" DESC LIMIT 1;";
        private const string SQL_PUT_TOUR_BY_ID = "Update public.\"tour_item\" SET \"name\"=@Name , \"description\" =@Description , \"from\"=@From ,\"to\"=@To ,\"image_path\" =@ImagePath ,\"distance\"= @Distance ,\"transport_type\"=@TransportType WHERE \"tour_item_id\"=@Id ;";
        private const string SQL_DELETE_BY_ID = "DELETE FROM public.\"tour_item\" WHERE \"tour_item_id\"=@Id;";
        private const string SQL_DELETE_All_TOURS = "DELETE FROM public.\"tour_item\"";

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
            try
            {
                DbCommand insertCommand = database.CreateCommand(SQL_INSERT_NEW_TOUR);
                database.DefineParameter(insertCommand, "@Name", DbType.String, tourItem.Name);
                database.DefineParameter(insertCommand, "@Description", DbType.String, tourItem.Description);
                database.DefineParameter(insertCommand, "@From", DbType.String, tourItem.From);
                database.DefineParameter(insertCommand, "@To", DbType.String, tourItem.To);
                database.DefineParameter(insertCommand, "@ImagePath", DbType.String, tourItem.ImagePath);
                database.DefineParameter(insertCommand, "@Distance", DbType.String, tourItem.Distance);
                database.DefineParameter(insertCommand, "@TransportType", DbType.String, tourItem.TransportTyp);

                return FindTourItemById(database.ExecuteScalar(insertCommand));
            }
            catch (Exception ex)
            {
                string strResponseValue = "{\"errorMessages\":[\"" + ex.Message.ToString() + "\"],\"errors\":{}}";
                log.Error(strResponseValue, ex);
            }
            return null;
        }

        // find tourItem by id
        public TourItem FindTourItemById(int tourItemId)
        {
            try
            {
                DbCommand findCommand = database.CreateCommand(SQL_FIND_BY_ID);
                database.DefineParameter(findCommand, "@Id", DbType.Int32, tourItemId);

                IEnumerable<TourItem> tours = QueryToursFromDb(findCommand);

                return tours.FirstOrDefault();
            }
            catch (Exception ex)
            {
                string strResponseValue = "{\"errorMessages\":[\"" + ex.Message.ToString() + "\"],\"errors\":{}}";
                log.Error(strResponseValue, ex);
            }
            return null;
            
        }

        // find tourItem by Name
        public TourItem FindTourItemByName(string tourName)
        {
            try
            {
                DbCommand findCommand = database.CreateCommand(SQL_FIND_BY_Name);
                database.DefineParameter(findCommand, "@Name", DbType.String, tourName);

                IEnumerable<TourItem> tours = QueryToursFromDb(findCommand);

                return tours.FirstOrDefault();
            }
            catch (Exception ex)
            {
                string strResponseValue = "{\"errorMessages\":[\"" + ex.Message.ToString() + "\"],\"errors\":{}}";
                log.Error(strResponseValue, ex);
            }
            return null;
        }

        // get all tourItem
        public IEnumerable<TourItem> GetTourItems()
        {
            try
            {
                DbCommand getAllToursCommand = database.CreateCommand(SQL_GET_ALL_TOURS);
                return QueryToursFromDb(getAllToursCommand);
            }
            catch (Exception ex)
            {
                string strResponseValue = "{\"errorMessages\":[\"" + ex.Message.ToString() + "\"],\"errors\":{}}";
                log.Error(strResponseValue, ex);
            }
            return null;
        }

        // get last tourId
        public int GetLastTourId()
        {
            try
            {
                DbCommand getLastTourIdCommand = database.CreateCommand(SQL_GET_LAST_TOURID);
                IEnumerable<TourItem> tourItems = QueryToursFromDb(getLastTourIdCommand);

                if (tourItems.Count() == 0)
                {
                    return 0;
                }
                return tourItems.FirstOrDefault().TourId;
            }
            catch (Exception ex)
            {
                string strResponseValue = "{\"errorMessages\":[\"" + ex.Message.ToString() + "\"],\"errors\":{}}";
                log.Error(strResponseValue, ex);
            }
            return 0;
        }

        // edit TourItem
        public TourItem EditTourItem(TourItem tourItem)
        {
            try
            {
                DbCommand editCommand = database.CreateCommand(SQL_PUT_TOUR_BY_ID);
                database.DefineParameter(editCommand, "@Name", DbType.String, tourItem.Name);
                database.DefineParameter(editCommand, "@Description", DbType.String, tourItem.Description);
                database.DefineParameter(editCommand, "@From", DbType.String, tourItem.From);
                database.DefineParameter(editCommand, "@To", DbType.String, tourItem.To);
                database.DefineParameter(editCommand, "@ImagePath", DbType.String, tourItem.ImagePath);
                database.DefineParameter(editCommand, "@Distance", DbType.String, tourItem.Distance);
                database.DefineParameter(editCommand, "@TransportType", DbType.String, tourItem.TransportTyp);
                database.DefineParameter(editCommand, "@Id", DbType.Int32, tourItem.TourId);

                return FindTourItemById(database.ExecuteScalar(editCommand));
            }
            catch (Exception ex)
            {
                string strResponseValue = "{\"errorMessages\":[\"" + ex.Message.ToString() + "\"],\"errors\":{}}";
                log.Error(strResponseValue, ex);
            }
            return null;
        }

        // delete TourItem
        public void DeleteTourItem(TourItem tourItem)
        {
            try
            {
                DbCommand deleteCommand = database.CreateCommand(SQL_DELETE_BY_ID);
                database.DefineParameter(deleteCommand, "@Id", DbType.Int32, tourItem.TourId);
                database.ExecuteScalar(deleteCommand);
            }
            catch (Exception ex)
            {
                string strResponseValue = "{\"errorMessages\":[\"" + ex.Message.ToString() + "\"],\"errors\":{}}";
                log.Error(strResponseValue, ex);
            }
         
        }

        // delete TourItem
        public void DeleteAllTourItems()
        {
            try
            {
                DbCommand deleteCommand = database.CreateCommand(SQL_DELETE_All_TOURS);
                database.ExecuteScalar(deleteCommand);
            }
            catch (Exception ex)
            {
                string strResponseValue = "{\"errorMessages\":[\"" + ex.Message.ToString() + "\"],\"errors\":{}}";
                log.Error(strResponseValue, ex);
            }
            
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
                            reader["distance"].ToString(),
                            reader["transport_type"].ToString()
                        ));
                    }
                }
                return tourList;
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
