using System;
using System.Collections.Generic;

namespace WebFormsMvp.FeatureDemos.Logic.Views.Models
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