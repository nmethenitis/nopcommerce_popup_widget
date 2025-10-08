using Microsoft.AspNetCore.Mvc;
using Nop.Core;
using Nop.Plugin.Widgets.NewsletterPopup.Models;
using Nop.Services.Configuration;
using Nop.Services.Localization;
using Nop.Services.Security;
using Nop.Web.Framework.Controllers;
using System.Threading.Tasks;

namespace Nop.Plugin.Widgets.NewsletterPopup.Controllers
{
    [Area(AreaNames.Admin)]
    public class NewsletterPopupController : BasePluginController
    {
        private readonly ISettingService _settingService;
        private readonly ILocalizationService _localizationService;
        private readonly IPermissionService _permissionService;
        private readonly IWorkContext _workContext;

        public NewsletterPopupController(
            ISettingService settingService,
            ILocalizationService localizationService,
            IPermissionService permissionService,
            IWorkContext workContext)
        {
            _settingService = settingService;
            _localization_service = localizationService;
            _permissionService = permissionService;
            _workContext = workContext;
        }

        [HttpGet]
        public async Task<IActionResult> Configure()
        {
            if (!await _permissionService.AuthorizeAsync(StandardPermissionProvider.ManageWidgets))
                return AccessDeniedView();

            var settings = await _settingService.LoadSettingAsync<Nop.Plugin.Widgets.NewsletterPopup.NewsletterPopupSettings>();
            var model = new ConfigurationModel
            {
                DisplayDelay = settings.DisplayDelay
            };

            var languages = await _workContext.GetAllLanguagesAsync();
            foreach (var lang in languages)
            {
                var html = await _localization_service_getlocalized(settings, lang.Id);
                model.Locales.Add(new LocalizedHtmlContent { LanguageId = lang.Id, HtmlContent = html ?? "" });
            }

            return View("~/Plugins/Widgets.NewsletterPopup/Views/Configure.cshtml", model);
        }

        [HttpPost]
        public async Task<IActionResult> Configure(ConfigurationModel model)
        {
            if (!await _permissionService.AuthorizeAsync(StandardPermissionProvider.ManageWidgets))
                return AccessDeniedView();

            var settings = await _settingService.LoadSettingAsync<Nop.Plugin.Widgets.NewsletterPopup.NewsletterPopupSettings>();
            settings.DisplayDelay = model.DisplayDelay;
            await _settingService.SaveSettingAsync(settings);

            foreach (var locale in model.Locales)
            {
                // in production use _localizationService.SaveLocalizedSettingAsync
            }

            await _settingService.ClearCacheAsync();

            return RedirectToAction("Configure");
        }

        // helper stub (use real ILocalizationService in production)
        private async Task<string> _localization_service_getlocalized(Nop.Plugin.Widgets.NewsletterPopup.NewsletterPopupSettings settings, int languageId)
        {
            return await Task.FromResult<string>(null);
        }
    }
}
