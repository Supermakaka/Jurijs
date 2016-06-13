var checkboxes = (function (me) {

    function init(parentSelector) {
        var parent = $(parentSelector == null ? "body" : parentSelector);

        parent.find('input').iCheck({
            checkboxClass: 'icheckbox icheckbox_square-red'
        });
    }

    me.init = init;

    return me;

})(checkboxes || {});