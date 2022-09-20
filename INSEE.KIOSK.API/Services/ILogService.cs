using INSEE.KIOSK.API.Context;
using INSEE.KIOSK.API.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace INSEE.KIOSK.API.Services
{
    public interface ILogService
    {
        public void Insert(Log log);
    }

    public class LogService : ILogService
    {
        readonly ApplicationDbContext _appdDbContext;
        public LogService(ApplicationDbContext appDbContext)
        {
            _appdDbContext = appDbContext;
        }

        public void Insert(Log log)
        {
            _appdDbContext.Logs.Add(log);
            _appdDbContext.SaveChanges();
           
        }
       
    }
}


