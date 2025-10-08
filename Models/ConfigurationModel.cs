using Nop.Web.Framework.Mvc.ModelBinding;
using Nop.Web.Framework.Models;
using System.Collections.Generic;

namespace Nop.Plugin.Widgets.NewsletterPopup.Models
{
    public class ConfigurationModel : BaseNopModel
    {
        [NopResourceDisplayName("Plugins.Widgets.NewsletterPopup.Fields.DisplayDelay")]
        public int DisplayDelay { get; set; }

        [NopResourceDisplayName("Plugins.Widgets.NewsletterPopup.Fields.HtmlContent")]
        public IList<LocalizedHtmlContent> Locales { get; set; } = new List<LocalizedHtmlContent>();
    }

    public class LocalizedHtmlContent
    {
        public int LanguageId { get; set; }
        public string HtmlContent { get; set; }
    }
}
