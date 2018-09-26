(function ($) {
    $(function () {
        var $buttons = $(".btn.btn-danger");
        for (var n = 0; n < $buttons.length; n++) {
            var $button = $($buttons[n]);
            transformButton($button);
        }
    });

    function transformButton($button) {
        var href = $button.attr("href");
        $button.attr("href", null);
        $button.click(function () {
            if (confirm("Are you sure?")) {
                window.location.href = href;
            }
        });
    }
})(jQuery);
