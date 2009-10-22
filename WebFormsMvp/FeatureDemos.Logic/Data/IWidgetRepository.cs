using System;
using System.Collections.Generic;

namespace WebFormsMvp.FeatureDemos.Logic.Data
{
    public interface IWidgetRepository
    {
        Widget Find(int id);
        IAsyncResult BeginFind(int id, AsyncCallback callback, Object asyncState);
        Widget EndFind(IAsyncResult result);
        IEnumerable<Widget> FindAll();
        IAsyncResult BeginFindAll(AsyncCallback callback, Object asyncState);
        IEnumerable<Widget> EndFindAll(IAsyncResult result);
        Widget FindByName(string name);
        IAsyncResult BeginFindByName(string name, AsyncCallback callback, Object asyncState);
        Widget EndFindByName(IAsyncResult result);
        void Save(Widget widget);
    }
}