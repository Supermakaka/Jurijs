var loader = (function (me) {

    var SHOWING_DELAY = 0;

    var $loader = $('<div class="loader"></div>');
    var $spinner = $('<div class="sk-double-bounce"><div class="sk-child sk-double-bounce1"></div><div class="sk-child sk-double-bounce2"></div></div>');

    var timer;

    function show() {
        if (timer != null)
            clearTimeout(timer);

        $('body').addClass('ui-blocker');

        timer = setTimeout(function () {
            $('body')
                .append(
                    $loader.append($spinner)
                );
        },
        SHOWING_DELAY);
    }

    function hide() {
        clearTimeout(timer);

        $('body')
            .removeClass('ui-blocker')
            .find('.loader')
            .remove();
    }

    me.show = show;
    me.hide = hide;

    return me;

})(loader || {});