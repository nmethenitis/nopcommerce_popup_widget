using Nop.Core.Configuration;

namespace Nop.Plugin.Widgets.NewsletterPopup;
public class NewsletterPopupSettings : ISettings {
    public bool ShowNewsletterForm { get; set; } = false;
    public string HtmlContent { get; set; }
}
