using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WebFormsMvp.Sample.Logic.Views.Models;

namespace WebFormsMvp.Sample.Logic.Views
{
    public interface ILookupWidgetView : IView<LookupWidgetModel>
    {
        event EventHandler<FindingWidgetEventArgs> Finding;
    }

    public class FindingWidgetEventArgs : EventArgs
    {
        public int? Id { get; set; }
        public string Name { get; set; }

        public FindingWidgetEventArgs()
        { }

        public FindingWidgetEventArgs(int id, string name)
            : this()
        {
            Id = id;
            Name = name;
        }
    }
}