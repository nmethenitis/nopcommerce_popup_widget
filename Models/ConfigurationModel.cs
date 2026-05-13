using Nop.Web.Framework.Mvc.ModelBinding;
using Nop.Web.Framework.Models;
using System.Collections.Generic;

namespace Nop.Plugin.Widgets.NewsletterPopup.Models;

public record ConfigurationModel : BaseNopModel, ILocalizedModel<ConfigurationModel.ConfigurationLocalizedModel> {

    public ConfigurationModel() {
        Locales = new List<ConfigurationLocalizedModel>();
    }
    public int ActiveStoreScopeConfiguration { get; set; }

    [NopResourceDisplayName("Plugins.Widgets.NewsletterPopup.Fields.ShowNewsletterForm")]
    public bool ShowNewsletterForm { get; set; }
    public bool ShowNewsletterForm_OverrideForStore { get; set; }

    [NopResourceDisplayName("Plugins.Widgets.NewsletterPopup.Fields.HtmlContent")]
    public string HtmlContent { get; set; }
    public bool HtmlContent_OverrideForStore { get; set; }
    public IList<ConfigurationLocalizedModel> Locales { get; set; }

    public class ConfigurationLocalizedModel : ILocalizedLocaleModel {
        public int LanguageId { get; set; }
        [NopResourceDisplayName("Plugins.Widgets.NewsletterPopup.Fields.HtmlContent")]
        public string HtmlContent { get; set; }
    }
}