using Microsoft.AspNetCore.Mvc;
using Nop.Core;
using Nop.Plugin.Widgets.NewsletterPopup.Models;
using Nop.Services.Configuration;
using Nop.Services.Localization;
using Nop.Services.Messages;
using Nop.Services.Security;
using Nop.Web.Areas.Admin.Controllers;
using Nop.Web.Framework;
using Nop.Web.Framework.Controllers;
using Nop.Web.Framework.Mvc.Filters;
using System.Threading.Tasks;

namespace Nop.Plugin.Widgets.NewsletterPopup.Controllers;
[AuthorizeAdmin]
[Area(AreaNames.ADMIN)]
[AutoValidateAntiforgeryToken]
public class NewsletterPopupController : BaseAdminController {
    private readonly ISettingService _settingService;
    private readonly ILocalizationService _localizationService;
    protected readonly INotificationService _notificationService;
    protected readonly IStoreContext _storeContext;
    protected readonly ILanguageService _languageService;

    public NewsletterPopupController(ISettingService settingService, ILocalizationService localizationService, IStoreContext storeContext, ILanguageService languageService, INotificationService notificationService) {
        _settingService = settingService;
        _localizationService = localizationService;
        _storeContext = storeContext;
        _languageService = languageService;
        _notificationService = notificationService;
    }

    [CheckPermission(StandardPermission.Configuration.MANAGE_WIDGETS)]
    public async Task<IActionResult> Configure() {

        var storeScope = await _storeContext.GetActiveStoreScopeConfigurationAsync();
        var popupSettings = await _settingService.LoadSettingAsync<NewsletterPopupSettings>(storeScope);
        var model = new ConfigurationModel {
            ShowNewsletterForm = popupSettings.ShowNewsletterForm,
            HtmlContent = popupSettings.HtmlContent
        };
        await AddLocalesAsync(_languageService, model.Locales, async (locale, languageId) => {
            locale.HtmlContent = await _localizationService
                .GetLocalizedSettingAsync(popupSettings, x => x.HtmlContent, languageId, 0, false, false);
        });
        if (storeScope > 0) {
            model.ShowNewsletterForm_OverrideForStore = _settingService.SettingExists(popupSettings, x => x.ShowNewsletterForm, storeScope);
        }

        return View("~/Plugins/Widgets.NewsletterPopup/Views/Configure.cshtml", model);
    }

    [HttpPost]
    [CheckPermission(StandardPermission.Configuration.MANAGE_WIDGETS)]
    public async Task<IActionResult> Configure(ConfigurationModel model) {
        if (!ModelState.IsValid)
            return await Configure();

        //load settings for a chosen store scope
        var storeScope = await _storeContext.GetActiveStoreScopeConfigurationAsync();
        var popupSettings = _settingService.LoadSetting<NewsletterPopupSettings>(storeScope);

        popupSettings.ShowNewsletterForm = model.ShowNewsletterForm;
        popupSettings.HtmlContent = model.HtmlContent;
        await _settingService.SaveSettingOverridablePerStoreAsync(popupSettings, x => x.ShowNewsletterForm, model.ShowNewsletterForm_OverrideForStore, storeScope, true);
        await _settingService.SaveSettingOverridablePerStoreAsync(popupSettings, x => x.HtmlContent, model.HtmlContent_OverrideForStore, storeScope, true);
        await _settingService.ClearCacheAsync();
        foreach (var localized in model.Locales) {
            await _localizationService.SaveLocalizedSettingAsync(popupSettings,
                x => x.HtmlContent, localized.LanguageId, localized.HtmlContent);
        }
        _notificationService.SuccessNotification(await _localizationService.GetResourceAsync("Admin.Plugins.Saved"));
        return RedirectToAction("Configure");
    }
}