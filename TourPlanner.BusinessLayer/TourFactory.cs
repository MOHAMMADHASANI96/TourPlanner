using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TourPlanner.DataAccessLayer.DAO;
using TourPlanner.Models;

namespace TourPlanner.BusinessLayer
{
    public static class TourFactory
    {
        private static ITourFactory instance;

        public static ITourFactory GetInstance()
        {
            if (instance == null)
                instance = new TourFactoryImpl();
            return instance;
        }

        // for testing
        public static ITourFactory GetInstance(ITourItemDAO tourItemDAO)
        {
            instance = new TourFactoryImpl(tourItemDAO);
            return instance;
        }
        public static ITourFactory GetInstance(ITourLogDAO tourLogDAO)
        {
            instance = new TourFactoryImpl(tourLogDAO);
            return instance;
        }

    }
}
