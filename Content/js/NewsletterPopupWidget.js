(function () {
  function shouldShowNewsletterPopup() {
    var key = 'newsletter_shown';
    var lastShown = localStorage.getItem(key);
    if (!lastShown) return true;
    return (Date.now() - parseInt(lastShown, 10)) > 86400000;
  }
  function markNewsletterPopupShown() {
    localStorage.setItem('newsletter_shown', Date.now());
  }

  document.addEventListener('DOMContentLoaded', function () {
    var delay = 2000;

    // Εμφάνιση popup μετά από delay αν επιτρέπεται
    if (shouldShowNewsletterPopup()) {
      setTimeout(function () {
        var el = document.getElementById('newsletter-popup');
        if (el) el.style.display = 'flex';
      }, delay);
    }

    // Χειρισμός κουμπιού κλεισίματος
    var closeBtn = document.getElementById('close-popup');
    if (closeBtn) {
      closeBtn.addEventListener('click', function () {
        var el = document.getElementById('newsletter-popup');
        if (el) el.style.display = 'none';
        markNewsletterPopupShown();
      });
    }

    // AJAX submit - με jQuery
    if (window.jQuery) {
      jQuery(document).on('click', '#custom-newsletter-submit', function (event) {
        event.preventDefault();
        let email = jQuery('#custom-newsletter-email').val();

        if (!email || !email.includes('@')) {
          jQuery('#custom-newsletter-result').html('<div style="color:red;">Παρακαλώ εισάγετε ένα έγκυρο email.</div>');
          return;
        }
        jQuery('#custom-newsletter-result').html('Αποστολή...');

        jQuery.ajax({
          url: '/popup-newsletter/subscribe',
          type: 'POST',
          data: { email: email },
          success: function (response) {
            if (response.success) {
              jQuery('#custom-newsletter-result').html('<div style="color:green;font-weight:bold;">' + response.message + '</div>');
              markNewsletterPopupShown();
              setTimeout(function () {
                document.getElementById('newsletter-popup').style.display = 'none';
              }, 1800);
            } else {
              jQuery('#custom-newsletter-result').html('<div style="color:red;">' + response.message + '</div>');
            }
          },
          error: function () {
            jQuery('#custom-newsletter-result').html('<div style="color:red;">Υπήρξε τεχνικό σφάλμα. Προσπαθήστε αργότερα.</div>');
          }
        });
      });
    }
  });
})();
