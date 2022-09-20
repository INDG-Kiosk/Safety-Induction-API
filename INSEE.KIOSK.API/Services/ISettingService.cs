using INSEE.KIOSK.API.Context;
using INSEE.KIOSK.API.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace INSEE.KIOSK.API.Services
{
    public interface ISettingService
    {
        public Message<string> Insert(Settings settings);
        public Message<string> Update(Settings settings);
        public Settings GetSetting();

    }

    public class SettingService : ISettingService
    {
        readonly ApplicationDbContext _appdDbContext;
        public SettingService(ApplicationDbContext appDbContext)
        {
            _appdDbContext = appDbContext;
        }

        public Message<string> Insert(Settings settings)
        {
            if (_appdDbContext.Settings.Count() == 0)
            {
                _appdDbContext.Settings.Add(settings);
                _appdDbContext.SaveChanges();

                return new Message<string>() { Text = $"New Setting Registered", Status = "S" };
            }
            return new Message<string>() { Text = $"Settings Already Insetred" };
        }

        public Message<string> Update(Settings settings)
        {
            var result = _appdDbContext.Settings.FirstOrDefault();
            if (result == null)
            {
                return new Message<string>() { Text = $"Settings Not Found" };
            }

            result.ModifiedBy = settings.ModifiedBy;
            result.Modified_DateTime = DateTime.Now;
            result.PassValidPeridINMonthsForWorker = settings.PassValidPeridINMonthsForWorker;
            result.PassValidPeridINMonthsForVisitor = settings.PassValidPeridINMonthsForVisitor;
            result.ReprintValidDaysForWorker = settings.ReprintValidDaysForWorker;
            _appdDbContext.SaveChanges();

            return new Message<string>() { Text = $"Settings Successfully Updated", Status = "S" };
        }

        public Settings GetSetting()
        {
            var result = _appdDbContext.Settings.FirstOrDefault();
            return result;
        }
    }
}

