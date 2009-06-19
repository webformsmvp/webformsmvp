using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WebFormsMvp.Sample.Logic.Views.Models
{
    public class AsyncMessagesModel
    {
        public IList<string> Messages { get; private set; }

        public AsyncMessagesModel()
        {
            Messages = new List<string>();
        }
    }
}