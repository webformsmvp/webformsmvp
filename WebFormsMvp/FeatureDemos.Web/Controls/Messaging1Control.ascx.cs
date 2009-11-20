using System;
using System.Linq;
using WebFormsMvp.FeatureDemos.Logic.Presenters;
using WebFormsMvp.FeatureDemos.Logic.Views.Models;
using WebFormsMvp.Web;

namespace WebFormsMvp.FeatureDemos.Web.Controls
{
    [PresenterBinding(typeof(Messaging1Presenter))]
    public partial class Messaging1Control : MvpUserControl<MessagingModel>
    {
    }
}