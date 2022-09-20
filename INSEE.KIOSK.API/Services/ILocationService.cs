using INSEE.KIOSK.API.Context;
using INSEE.KIOSK.API.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace INSEE.KIOSK.API.Services
{
    public interface ILocationService
    {
      //  public Message<string> Insert(Location location);
      //  public Message<string> Update(Location location);

    }

    public class LocationService : ILocationService
    {
        readonly ApplicationDbContext _appdDbContext;
        public LocationService(ApplicationDbContext appDbContext)
        {
            _appdDbContext = appDbContext;
        }

        //public Message<string> Insert(Location location)
        //{
        //   // _appdDbContext.Locations.Add(location);
        //    _appdDbContext.SaveChanges();
        //    return new Message<string>() { Text = "New Location Successfully Added", Status = "S" };
        //}

        //public Message<string> Update(Location location)
        //{
        //  /*  var result = _appdDbContext.Locations.SingleOrDefault(s => s.Code == location.Code);
        //    if (result == null)
        //    {
        //        return new Message<string>() { Text = "Location Not Found" };
        //    }

        //    result.Name = location.Name;
        //    result.IsActive = location.IsActive;
        //    result.ModifiedBy = location.ModifiedBy;
        //    result.ModifiedDateTime = location.ModifiedDateTime;
        //    result.Address = location.Address;
        //    result.ContactInfo = location.ContactInfo;*/
        //    _appdDbContext.SaveChanges();

        //    return new Message<string>() { Text = "Location Details Successfully Updated", Status = "S" };
        //}
    }
}
