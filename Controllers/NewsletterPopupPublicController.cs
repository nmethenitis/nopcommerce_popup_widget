using DocumentFormat.OpenXml.Wordprocessing;
using Microsoft.AspNetCore.Mvc;
using Nop.Core;
using Nop.Core.Domain.Stores;
using Nop.Services.Localization;
using Nop.Services.Messages;
using Nop.Web.Framework.Controllers;

namespace Nop.Plugin.Widgets.NewsletterPopup.Controllers;
public class NewsletterPopupPublicController : BasePluginController {
    private readonly INewsLetterSubscriptionService _newsLetterSubscriptionService;
    private readonly ILocalizationService _localizationService;
    private readonly IStoreContext _storeContext;
    IWorkflowMessageService _workflowMessageService;

    public NewsletterPopupPublicController(INewsLetterSubscriptionService newsLetterSubscriptionService, ILocalizationService localizationService, IStoreContext storeContext, IWorkflowMessageService workflowMessageService) {
        _newsLetterSubscriptionService = newsLetterSubscriptionService;
        _localizationService = localizationService;
        _storeContext = storeContext;
        _workflowMessageService = workflowMessageService;
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
                var subscription = new Nop.Core.Domain.Messages.NewsLetterSubscription {
                    Active = false,
                    Email = email,
                    StoreId = storeId,
                    CreatedOnUtc = DateTime.UtcNow
                };
                await _newsLetterSubscriptionService.InsertNewsLetterSubscriptionAsync(subscription);
                await _workflowMessageService.SendNewsLetterSubscriptionActivationMessageAsync(subscription);
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
