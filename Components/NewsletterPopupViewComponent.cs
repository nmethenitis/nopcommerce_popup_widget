using Microsoft.AspNetCore.Mvc;
using Nop.Services.Configuration;
using Nop.Services.Localization;
using Nop.Services.Stores;
using Nop.Core;
using Nop.Core.Domain.Customers;
using System.Threading.Tasks;

namespace Nop.Plugin.Widgets.NewsletterPopup.Components
{
    public class NewsletterPopupViewComponent : ViewComponent
    {
        private readonly ISettingService _settingService;
        private readonly ILocalizationService _localizationService;
        private readonly IWorkContext _workContext;

        public NewsletterPopupViewComponent(
            ISettingService settingService,
            ILocalizationService localizationService,
            IWorkContext workContext)
        {
            _settingService = settingService;
            _localizationService = localizationService;
            _workContext = workContext;
        }

        public async Task<IViewComponentResult> InvokeAsync(string widgetZone, object additionalData)
        {
            var settings = await _settingService.LoadSettingAsync<Nop.Plugin.Widgets.NewsletterPopup.NewsletterPopupSettings>();
            var customer = await _workContext.GetCurrentCustomerAsync();
            var languageId = customer?.LanguageId ?? 0;

            var localizedHtml = await _localization_service_getlocalized(settings, languageId);

            ViewData["HtmlContent"] = string.IsNullOrWhiteSpace(localizedHtml) ? settings.DefaultHtmlContent : localizedHtml;
            ViewData["Delay"] = settings.DisplayDelay;

            return View("~/Plugins/Widgets.NewsletterPopup/Views/PublicInfo.cshtml");
        }

        // helper to avoid heavy dependencies in sample - mimic localization retrieval
        private async Task<string> _localization_service_getlocalized(Nop.Plugin.Widgets.NewsletterPopup.NewsletterPopupSettings settings, int languageId)
        {
            // in production use ILocalizationService.GetLocalizedSettingAsync
            return await Task.FromResult<string>(null);
        }
    }
}
