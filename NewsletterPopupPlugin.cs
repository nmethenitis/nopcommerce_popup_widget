using Nop.Core;
using Nop.Services.Configuration;
using Nop.Services.Localization;
using Nop.Services.Plugins;
using Nop.Web.Framework.Infrastructure;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Nop.Plugin.Widgets.NewsletterPopup
{
    public class NewsletterPopupPlugin : BasePlugin, IWidgetPlugin
    {
        private readonly ISettingService _settingService;
        private readonly IWebHelper _webHelper;
        private readonly ILocalizationService _localizationService;

        public NewsletterPopupPlugin(ISettingService settingService,
            IWebHelper webHelper,
            ILocalizationService localizationService)
        {
            _settingService = settingService;
            _webHelper = webHelper;
            _localizationService = localizationService;
        }

        public Task<IList<string>> GetWidgetZonesAsync() =>
            Task.FromResult<IList<string>>(new List<string> { PublicWidgetZones.BodyEndHtmlTagAfter });

        public string GetWidgetViewComponentName(string widgetZone) => "NewsletterPopup";

        public override async Task InstallAsync()
        {
            var settings = new NewsletterPopupSettings
            {
                DisplayDelay = 2000,
                DefaultHtmlContent = "<h2>Εγγραφή στο Newsletter</h2><p>Μάθε πρώτος για νέες προσφορές!</p>"
            };
            await _settingService.SaveSettingAsync(settings);

            await _localizationService.AddOrUpdateLocaleResourceAsync(new Dictionary<string, string>
            {
                ["Plugins.Widgets.NewsletterPopup.Fields.HtmlContent"] = "HTML περιεχόμενο",
                ["Plugins.Widgets.NewsletterPopup.Fields.HtmlContent.Hint"] = "Το HTML περιεχόμενο του popup (πολυγλωσσικό).",
                ["Plugins.Widgets.NewsletterPopup.Fields.DisplayDelay"] = "Καθυστέρηση εμφάνισης (ms)",
                ["Plugins.Widgets.NewsletterPopup.Fields.DisplayDelay.Hint"] = "Χρόνος καθυστέρησης σε milliseconds πριν εμφανιστεί το popup."
            });

            await base.InstallAsync();
        }

        public override async Task UninstallAsync()
        {
            await _settingService.DeleteSettingAsync<NewsletterPopupSettings>();
            await _localizationService.DeleteLocaleResourcesAsync("Plugins.Widgets.NewsletterPopup");
            await base.UninstallAsync();
        }

        public override string GetConfigurationPageUrl()
            => $"{_webHelper.GetStoreLocation()}Admin/NewsletterPopup/Configure";
    }

    public class NewsletterPopupSettings : ISettings
    {
        public int DisplayDelay { get; set; }
        // DefaultHtmlContent used as a fallback; localized content stored via localization service
        public string DefaultHtmlContent { get; set; }
    }
}
