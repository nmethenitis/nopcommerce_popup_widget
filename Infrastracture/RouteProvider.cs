using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;
using Nop.Web.Framework.Mvc.Routing;

namespace Nop.Plugin.Widgets.NewsletterPopup.Infrastracture;
public class RouteProvider : IRouteProvider {
    public int Priority => 100;

    public void RegisterRoutes(IEndpointRouteBuilder endpointRouteBuilder) {
        endpointRouteBuilder.MapControllerRoute(
           name: "NewsletterPopupWidget.SubscribeNewsletter",
           pattern: "popup-newsletter/subscribe",
           defaults: new { controller = "NewsletterPopupPublic", action = "SubscribeToNewsletter" });
    }
}
