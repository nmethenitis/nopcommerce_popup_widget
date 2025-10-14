using Microsoft.AspNetCore.Mvc;
using Nop.Core;
using Nop.Services.Localization;
using Nop.Services.Messages;
using Nop.Web.Framework.Controllers;

namespace Nop.Plugin.Widgets.NewsletterPopup.Controllers;
public class NewsletterPopupPublicController : BasePluginController {
    private readonly INewsLetterSubscriptionService _newsLetterSubscriptionService;
    private readonly ILocalizationService _localizationService;
    private readonly IStoreContext _storeContext;

    public NewsletterPopupPublicController(INewsLetterSubscriptionService newsLetterSubscriptionService, ILocalizationService localizationService, IStoreContext storeContext) {
        _newsLetterSubscriptionService = newsLetterSubscriptionService;
        _localizationService = localizationService;
        _storeContext = storeContext;
    }

    [HttpPost]
    public async Task<IActionResult> SubscribeToNewsletter(string email) {
        if (string.IsNullOrWhiteSpace(email) || !email.Contains("@")) {
            return Json(new { success = false, message = "Παρακαλώ εισάγετε έγκυρο email." });
        }

        try { 
            var storeId = _storeContext.GetCurrentStore().Id;
            var existing = await _newsLetterSubscriptionService.GetNewsLetterSubscriptionByEmailAndStoreIdAsync(email, storeId);
            if (existing == null) {
                await _newsLetterSubscriptionService.InsertNewsLetterSubscriptionAsync(new Nop.Core.Domain.Messages.NewsLetterSubscription {
                    Active = true,
                    Email = email,
                    StoreId = storeId,
                    CreatedOnUtc = DateTime.UtcNow
                });
            } else if (!existing.Active) {
                existing.Active = true;
                await _newsLetterSubscriptionService.UpdateNewsLetterSubscriptionAsync(existing);
            }

            string successMessage = await _localizationService.GetResourceAsync("NewsLetterSubscription.Subscribed");
            return Json(new { success = true, message = successMessage });
        } catch (Exception ex) {
            return Json(new { success = false, message = "Υπήρξε σφάλμα: " + ex.Message });
        }
    }
}
