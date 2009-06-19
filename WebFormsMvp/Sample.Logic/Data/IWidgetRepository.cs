using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WebFormsMvp.Sample.Logic.Data
{
    public interface IWidgetRepository
    {
        Domain.Widget Find(int id);
        IEnumerable<Domain.Widget> FindAll();
        Domain.Widget FindByName(string name);
        void Save(Domain.Widget widget);
    }
}