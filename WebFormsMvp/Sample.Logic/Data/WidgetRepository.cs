using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;

namespace WebFormsMvp.Sample.Logic.Data
{
    public class WidgetRepository : IWidgetRepository
    {
        public Domain.Widget Find(int id)
        {
            Domain.Widget widget = null;
            using (var db = new SiteDbDataContext(ConfigurationManager.ConnectionStrings["SiteDb"].ConnectionString))
            {
                widget = (from w in db.Widgets
                          where w.Id == id
                          select new Domain.Widget()
                          {
                              Id = w.Id,
                              Name = w.Name,
                              Description = w.Description
                          }).SingleOrDefault();
            }
            return widget;
        }

        public IEnumerable<Domain.Widget> FindAll()
        {
            using (var db = new SiteDbDataContext(ConfigurationManager.ConnectionStrings["SiteDb"].ConnectionString))
            {
                return from w in db.Widgets
                       select new Domain.Widget()
                       {
                           Id = w.Id,
                           Name = w.Name,
                           Description = w.Description
                       };
            }
        }

        public Domain.Widget FindByName(string name)
        {
            Domain.Widget widget = null;
            using (var db = new SiteDbDataContext(ConfigurationManager.ConnectionStrings["SiteDb"].ConnectionString))
            {
                widget = (from w in db.Widgets
                          where w.Name == name
                          select new Domain.Widget()
                          {
                              Id = w.Id,
                              Name = w.Name,
                              Description = w.Description
                          }).SingleOrDefault();
            }
            return widget;
        }

        public void Save(Domain.Widget widget)
        {
            throw new NotImplementedException();
        }
    }
}