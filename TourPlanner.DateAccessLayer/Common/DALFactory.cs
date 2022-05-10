using System;
using System.Configuration;
using System.Reflection;
using TourPlanner.DataAccessLayer.DAO;

namespace TourPlanner.DataAccessLayer.Common
{
    public class DALFactory
    {
        private static string assemblyName;
        private static Assembly dalAssembly;
        private static IDatabase database;

        // load DAL assembly
        static DALFactory()
        {
            assemblyName = ConfigurationManager.AppSettings["DALSqlAssembly"];
            dalAssembly = Assembly.Load(assemblyName);
        }

        // create database object with connection string from config
        public static IDatabase GetDatabase()
        {

            if (database == null)
            {
                database = CreateDatabase();
            }
            return database;
        }
        private static IDatabase CreateDatabase()
        {
            string connectionString = ConfigurationManager.ConnectionStrings["PostgresSQLConnectionString"].ConnectionString;
            return CreateDatabase(connectionString);
        }

        // create database object with connection string
        private static IDatabase CreateDatabase(string connectionString)
        {
            string databaseClassName = assemblyName + ".Database";
            Type dbClass = dalAssembly.GetType(databaseClassName);
            return Activator.CreateInstance(dbClass, new object[] { connectionString }) as IDatabase;
        }

        // create Item tour sql/file DAO object
        public static ITourItemDAO CreateTourItemDAO()
        {
            string className = assemblyName + ".TourItemSqlDAO";
            Type tourItemType = dalAssembly.GetType(className);
            return Activator.CreateInstance(tourItemType) as ITourItemDAO;
        }

        // create log tour sql/file DAO object
        public static ITourLogDAO CreateTourLogDAO()
        {
            string className = assemblyName + ".TourLogSqlDAO";
            Type tourLogType = dalAssembly.GetType(className);
            return Activator.CreateInstance(tourLogType) as ITourLogDAO;
        }
    }
}
