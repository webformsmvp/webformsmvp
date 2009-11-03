using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.Linq;
using System.Text;
using System.Configuration;
using System.Data.SqlClient;
using System.Data.Linq.Mapping;
using System.Linq.Expressions;

namespace WebFormsMvp.FeatureDemos.Logic.Data
{
    public class WidgetRepository : IWidgetRepository
    {
        SiteDbDataContext _db = null;

        public Widget Find(int id)
        {
            Widget widget = null;
            using (var db = new SiteDbDataContext())
            {
                widget = (from w in db.Widgets
                          where w.Id == id
                          select w).SingleOrDefault();
            }
            return widget;
        }

        SqlCommand _beginFindCmd = null;

        public IAsyncResult BeginFind(int id, AsyncCallback callback, Object asyncState)
        {
            _db = new SiteDbDataContext();
            var query = from w in _db.Widgets
                        where w.Id == id
                        select w;
            _beginFindCmd = _db.GetCommand(query) as SqlCommand;
            _db.Connection.Open();
            return _beginFindCmd.BeginExecuteReader(callback, asyncState, System.Data.CommandBehavior.CloseConnection);
        }

        public Widget EndFind(IAsyncResult result)
        {
            var rdr = _beginFindCmd.EndExecuteReader(result);
            var widget = (from w in _db.Translate<Widget>(rdr)
                          select w).SingleOrDefault();
            rdr.Close();
            return widget;
        }

        public IEnumerable<Widget> FindAll()
        {
            using (var db = new SiteDbDataContext())
            {
                return from w in db.Widgets
                       select w;
            }
        }

        SqlCommand _beginFindAllCmd = null;

        public IAsyncResult BeginFindAll(AsyncCallback callback, Object asyncState)
        {
            _db = new SiteDbDataContext();
            var query = from w in _db.Widgets
                        select w;
            _beginFindAllCmd = _db.GetCommand(query) as SqlCommand;
            _db.Connection.Open();
            return _beginFindAllCmd.BeginExecuteReader(callback, asyncState, System.Data.CommandBehavior.CloseConnection);
        }

        public IEnumerable<Widget> EndFindAll(IAsyncResult result)
        {
            var rdr = _beginFindAllCmd.EndExecuteReader(result);
            var widgets = (from w in _db.Translate<Widget>(rdr)
                           select w).ToList();
            rdr.Close();
            return widgets;
        }

        public Widget FindByName(string name)
        {
            Widget widget = null;
            using (var db = new SiteDbDataContext())
            {
                widget = (from w in db.Widgets
                          where w.Name == name
                          select w).SingleOrDefault();
            }
            return widget;
        }

        SqlCommand _beginFindByNameCmd = null;

        public IAsyncResult BeginFindByName(string name, AsyncCallback callback, Object asyncState)
        {
            _db = new SiteDbDataContext();
            var query = from w in _db.Widgets
                        where w.Name == name
                        select w;
            _beginFindByNameCmd = _db.GetCommand(query) as SqlCommand;
            _db.Connection.Open();
            return _beginFindByNameCmd.BeginExecuteReader(callback, asyncState, System.Data.CommandBehavior.CloseConnection);
        }

        public Widget EndFindByName(IAsyncResult result)
        {
            var rdr = _beginFindByNameCmd.EndExecuteReader(result);
            var widget = (from w in _db.Translate<Widget>(rdr)
                           select w).SingleOrDefault();
            rdr.Close();
            return widget;
        }

        public void Save(Widget widget, Widget originalWidget)
        {
            using (var db = new SiteDbDataContext())
            {
                if (widget.Id > 0)
                {
                    // Update
                    db.Widgets.Attach(widget, originalWidget);
                }
                else
                {
                    // Create
                    db.Widgets.InsertOnSubmit(widget);
                }
                db.SubmitChanges();
            }
        }
    }
}