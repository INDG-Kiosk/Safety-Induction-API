using INSEE.KIOSK.API.Context;
using INSEE.KIOSK.API.Model;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace INSEE.KIOSK.API.Services
{
    public interface ISiteService
    {
        public Message<string> Insert(Site Site);
        public Message<string> Update(Site Site);
        public int GetCompanyCodeBuSiteCode(int SiteCode);
        public Site GetSiteBySiteID(int SiteCode);
        public List<SiteModel> GetAll();
        public List<SiteModel> GetAllUnassignCourse();
    }

    public class SiteService : ISiteService
    {
        readonly ApplicationDbContext _appdDbContext;
        public SiteService(ApplicationDbContext appDbContext)
        {
            _appdDbContext = appDbContext;
        }

        public List<SiteModel> GetAll()
        {
          
                return (from a in _appdDbContext.Sites
                        select new SiteModel()
                        {
                            Name = a.Name,
                            Code = a.Code,
                            IPAddress = a.IP,
                            IsActive = a.IsActive,
                            Location = a.Location,
                            ResourcePath = a.ResourcePath

                        }).ToList();
        }

        public List<SiteModel> GetAllUnassignCourse()
        {
            return (from a in _appdDbContext.Sites
                    where a.Courses.Count == 0
                    select new SiteModel()
                    {
                        Name = a.Name,
                        Code = a.Code,
                        IPAddress = a.IP,
                        IsActive = a.IsActive,
                        Location = a.Location,
                        ResourcePath = a.ResourcePath

                    }).ToList();
        }
        public Site GetSiteBySiteID(int code)
        {
            return _appdDbContext.Sites.FirstOrDefault(d=>d.Code == code);
        }
        public Message<string> Insert(Site Site)
        {
            _appdDbContext.Sites.Add(Site);
            _appdDbContext.SaveChanges();
            return new Message<string>() { Text = "New Site Successfully Added", Status = "S" , Result = Site.Code.ToString()};
        }

        public Message<string> Update(Site Site)
        {
            var result = _appdDbContext.Sites.SingleOrDefault(s => s.Code == Site.Code);
            if (result == null)
            {
                return new Message<string>() { Text = "Site Not Found" };
            }

           // result.FK_LocationCode = Site.FK_LocationCode;
            result.IsActive = Site.IsActive;
            result.ModifiedBy = Site.ModifiedBy;
            result.ModifiedDateTime = Site.ModifiedDateTime;
            result.IP = Site.IP;
            result.ResourcePath = Site.ResourcePath;
            _appdDbContext.SaveChanges();

            return new Message<string>() { Text = "Site Details Successfully Updated", Status = "S" , Result = result.Code.ToString() };
        }

        public int GetCompanyCodeBuSiteCode(int SiteCode)
        {
            var result = _appdDbContext.Sites
                .Include(s => s.Company)
                .SingleOrDefault(s => s.Code == SiteCode);
           
            if (result != null)
            {
                return result.Company.Code;
            }

            return 0;
        }
    }
}
