var site = (function (me, loader, modals, checkboxes) {

    function initAjax() {
        $.ajaxSetup({
            error: function (jqXHR, exception) {
                if (jqXHR.status === 0) {
                    showError('This resource is not available. Please verify your connection.');
                } else if (jqXHR.status === 404) {
                    showError('Requested page is not found.');
                } else if (exception === 'timeout') {
                    showError('Request timed out.');
                } else if (exception === 'abort') {
                    showError('Request aborted.');
                } else {
                    showError('Server error has occured.');
                }
            }
        });

        $(document).ajaxSend(function (event, request, settings) {
            if (settings.showLoader !== false)
                loader.show();
        });

        $(document).ajaxComplete(function (event, request, settings) {
            if (settings.showLoader !== false)
                loader.hide();
        });
    }

    function initForms() {
        $('[data-jump-to-tab-on-validation-error]').submit(function () {

            var form = $(this);
            var firstError = form.find(".tab-content .input-validation-error:first");

            if (firstError.length > 0)
                form.find('.nav-tabs a[href="#' + firstError.closest(".tab-pane").attr("id") + '"]').tab('show');
        });
    };

    function showError(text) {
        var options = {
            title: 'Oh no!',
            text: text,
            icon: false,
            type: 'error',
            delay: 2500,
            buttons: { closer: true, sticker: false }
        };

        new PNotify(options);
    }

    function showNotification(text, title) {
        var options = {
            text: text,
            icon: false,
            type: 'notice',
            delay: 1500,
            buttons: { closer: true, sticker: false }
        };

        if (title != null)
            options.title = title;

        new PNotify(options);
    }

    function showValidationError(message, optionalSelector) {
        var selector = optionalSelector == null ? "[data-validation-error-paceholder]" : optionalSelector;

        var validationErrorTemplate = Handlebars.compile($("#validation-error-template").html());

        $(selector).html(validationErrorTemplate({ error: message }));
    }

    function ajaxLink(selector, options) {

        var settings = $.extend({
            // These are the defaults.
            url: null,
            data: null,
            success: null
        }, options);

        $(document).on('click', selector, function (e) {
            e.preventDefault();

            $.ajax({
                url: settings.url,
                data: settings.data != null && typeof settings.data === 'function' ? settings.data($(this)) : null,
                type: 'GET',
                success: function (data) {
                    if (settings.success != null && typeof settings.success === 'function')
                        settings.success(data);
                },
                showLoader: false
            });
        });

    }

    function modalFormLink(selector, options) {

        var settings = $.extend({
            // These are the defaults.
            url: null,
            data: null,
            formSubmitSuccess: null
        }, options);

        $(document).on('click', selector, function (e) {
            e.preventDefault();

            $.ajax({
                url: settings.url,
                data: settings.data != null && typeof settings.data === 'function' ? settings.data($(this)) : null,
                type: 'GET',
                success: function (data) {
                    modals.setContent(data);
                    modals.show();

                    handleModalFormSubmit(settings.formSubmitSuccess);
                }
            });
        });
    }

    function handleModalFormSubmit(successCallback) {

        var form = modals.getModal().find('form');

        if (form.length === 0)
            return;

        $(form).on('submit', function (e) {
            e.preventDefault();

            if (!form.valid())
                return;

            var btn = $(this).find('button[type=submit]');

            btn.button('loading');

            $.ajax({
                url: form.attr('action'),
                type: 'POST',
                data: form.serialize(),
                success: function (data) {
                    if (data.success) {
                        modals.hide();

                        if (form.data('notification-success') != null)
                            showNotification(form.data('notification-success'));

                        if (successCallback != null && typeof successCallback === 'function')
                            successCallback(form, data);

                    } else {
                        if (data.message != null)
                            showValidationError(data.message);
                        else {
                            modals.setContent(data);

                            //since form element was replaced by setContent(), we need to re-apply the submit handler
                            handleModalFormSubmit(successCallback);
                        }
                    }
                },
                complete: function () {
                    btn.button('reset');
                }
            });
        });
    }

    me.showError = showError;
    me.showNotification = showNotification;
    me.showValidationError = showValidationError;
    me.ajaxLink = ajaxLink;
    me.modalFormLink = modalFormLink;

    me.onReady = function () {
        initAjax();
        initForms();
        checkboxes.init();
    };

    return me;

})(site || {}, loader, modals, checkboxes);