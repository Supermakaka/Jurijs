var modals = (function (me, checkboxes) {

    function getModal(modalSelector) {
        return $(modalSelector == null ? "#modal" : modalSelector);
    }

    function hide(modalSelector) {
        getModal(modalSelector).modal('hide');
    }

    function show(modalSelector) {
        getModal(modalSelector).modal('show');
    }

    function setContent(content, modalSelector) {
        var modal = getModal(modalSelector);

        modal.find('.modal-content').html(content);

        if (modal.find('form').length > 0)
            modal.find('form').validateBootstrap(true);

        if (modal.find('input').length > 0)
            checkboxes.init(modal);
    }

    me.hide = hide;
    me.show = show;
    me.setContent = setContent;
    me.getModal = getModal;

    return me;

})(modals || {}, checkboxes);