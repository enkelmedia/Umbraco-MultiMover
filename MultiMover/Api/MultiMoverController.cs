using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using TheDashboard.Api.Attributes;
using Umbraco.Web.Mvc;
using Umbraco.Web.WebApi;
using Umbraco.Web.WebApi.Filters;

namespace MultiMover.Api
{
    /// <summary>
    /// Routes to ~/Umbraco/[YourAreaName]/[YourControllerName]
    /// </summary>
    [UmbracoApplicationAuthorize("content")]
    [PluginController("MultiMover")]
    [CamelCaseController]
    public class MultiMoverController : UmbracoAuthorizedApiController
    {

        [HttpPost]
        public HttpResponseMessage MoveNodes(MoveNodesViewModel model)
        {

            foreach (var id in model.NodeIdsToMove)
            {
                var content = Services.ContentService.GetById(id);
                Services.ContentService.Move(content,model.DestinationParentId,Security.CurrentUser.Id);
            }
            
            return Request.CreateResponse(HttpStatusCode.OK,new {Success = true});
        }

        [HttpGet]
        public HttpResponseMessage GetChildNodes(int id)
        {
            // Get childs by node id
            
            var childs = Services.ContentService.GetChildren(id);
            
            return Request.CreateResponse(HttpStatusCode.OK, childs.Select(x => new { Id = x.Id, Name = x.Name, Icon = x.ContentType.Icon, Published = !x.HasPublishedVersion }));

        }

    }
}