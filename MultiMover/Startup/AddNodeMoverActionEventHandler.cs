using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using Umbraco.Core;
using Umbraco.Web.Models.Trees;
using Umbraco.Web.Trees;
using Umbraco.Core.Services;

namespace MultiMover.Startup
{
    public class AddNodeMoverActionEventHandler : ApplicationEventHandler
    {
        protected override void ApplicationStarted(UmbracoApplicationBase umbracoApplication, ApplicationContext applicationContext)
        {
            TreeControllerBase.MenuRendering += TreeControllerBase_MenuRendering;
            base.ApplicationStarted(umbracoApplication, applicationContext);
        }

        void TreeControllerBase_MenuRendering(TreeControllerBase sender, MenuRenderingEventArgs e)
        {
            var context = new HttpContextWrapper(HttpContext.Current);
            var urlHelper = new UrlHelper(new RequestContext(context, new RouteData()));

            var localizedLabel = ApplicationContext.Current.Services.TextService.Localize("multiMover/menuActionLabel");

            if (sender.TreeAlias == "content")
            {
                var m = new MenuItem("nodeMover", localizedLabel);
                m.AdditionalData.Add("actionView", urlHelper.Content("~/App_Plugins/MultiMover/view.html"));
                m.Icon = "multiple-windows";

                //insert at index 4
                if (e.Menu.Items.Count > 3)
                {
                    e.Menu.Items.Insert(3, m);
                }
                else
                {
                    e.Menu.Items.Insert(e.Menu.Items.Count- 1 , m); // making sure that this is not the last node as thats usally the "reload"-action.
                }
                
            }
        }
    }
}