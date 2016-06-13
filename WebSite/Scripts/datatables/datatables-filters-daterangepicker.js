/**
    Support for Bootstrap-daterangepicker https://github.com/dangrossman/bootstrap-daterangepicker
*/

(function (dtFilters) {

    $(document).on('init.dt', function (e, settings, json) {
        bindHandlers($(settings.nTable));
    });

    function bindHandlers(table) {
        var filters = dtFilters.getFiltersByTable(table);

        filters.filter(function () {
            var pickerData = $(this).data("daterangepicker");

            if (pickerData == null)
                return false;

            if (pickerData.autoUpdateInput)
                alert("[datatables-filters-daterangepicker] Using Date Range Picker with autoUpdateInput == true is not supported. Set autoUpdateInput option to false.");

            return true;

        }).on('apply.daterangepicker', function (ev, picker) {

            dtFilters.setFilterVal($(this), picker.startDate.format('MM/DD/YYYY') + ' - ' + picker.endDate.format('MM/DD/YYYY'));

            dtFilters.applyFilter($(this), true);

        }).on('cancel.daterangepicker', function (ev, picker) {

            dtFilters.setFilterVal($(this), '');

            dtFilters.applyFilter($(this), true);

        });
    }

})(datatablesFilters);