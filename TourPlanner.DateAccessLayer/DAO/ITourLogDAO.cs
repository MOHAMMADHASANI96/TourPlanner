﻿using System.Collections.Generic;
using TourPlanner.Models;

namespace TourPlanner.DataAccessLayer.DAO
{
    public interface ITourLogDAO
    {
        TourLog FindTourLogById(int itemLogId);
        TourLog AddNewTourLog(TourLog tourLog, TourItem tourItem);
        IEnumerable<TourLog> GetLogItems(TourItem tourItem);
    }
}