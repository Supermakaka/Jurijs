/*
Custom filters for DataTables plug-in.

1. Filter placement

    a) Simple placement - filters and datatable should share the common parent element that has the "data-table-with-filters" attribute:

        <div data-table-with-filters>
            ...your filters

            <table>
                ...your table
            </table>
        </div>

    b) Flexible placement - filters and datatable could not share the common parent element, but then you have to link them in the following way:

        <div id="filters" data-table-selector="#table">
            ...your filters
        </div>

        <table id="table" data-filters-selector="#filters">
            ...your table
        </table>

2. Filters

    A filter is an <input/> or <select/> element that has the "data-filter" attribute. Optional attributes:

    data-default-value - default value (if needed).
    data-column-name - column name to filter. If not provided, the filter is treated as global.
    data-regex - is value a regex expression or not.
    data-apply-on-typing - enable/disable "keyup" event handler. Disabling is useful in combination with date pickers.
*/

var datatablesFilters = (function (me) {

    $(document).on("preDraw.dt-ext", function (e, settings) {
        applyDefaultFilters($(settings.nTable));
    });

    $(document).on('init.dt', function (e, settings, json) {
        bindHandlers($(settings.nTable));
    });

    function bindHandlers(table) {
        var filters = getFiltersByTable(table);

        //on change handler
        filters.change(function () {
            applyFilter($(this), true);
        });

        //keyup handler
        filters.not("[data-apply-on-typing='false'][data-apply-on-typing]").keyup($.debounce(250, function () {
            applyFilter($(this), true);
        }));
    }

    function isFilterChanged(filter) {
        var currentVal = getFilterVal(filter);

        if (filter.data('prevVal') != null) {
            if (currentVal === filter.data('prevVal'))
                return false;
        } else {
            //if there is no previous data, but filter value is empty, the value has not changed
            if (currentVal === '')
                return false;
        }

        filter.data('prevVal', currentVal);

        return true;
    }

    function applyFilter(filter, reloadTable, forceDefaultValue) {
        //force default value if asked
        if (forceDefaultValue && filter.data("default-value") != null)
            setFilterVal(filter, filter.data("default-value"));

        //only apply the filter if value has been changed
        if (!isFilterChanged(filter))
            return;

        var val = getFilterVal(filter);
        var columnName = filter.data("column-name");
        var regex = filter.data("regex") === "true" || filter.data("regex");

        var table = getTableByFilter(filter);

        if (columnName != null && columnName !== '')
            table.column(columnName + ":name").search(val, regex);
        else
            table.search(val, regex);

        if (reloadTable)
            table.draw();

        $(document).trigger('filterApplied:dt-filters', filter);
    }

    function getFilterVal(filter) {
        return filter.is(':checkbox') ? filter.prop("checked") : filter.val();
    }

    function setFilterVal(filter, val) {
        if (filter.is(':checkbox'))
            filter.prop("checked", val);
        else
            filter.val(val);
    }

    function getTableByFilter(filter) {

        var tableSelector;

        //check if specific selector is provided via [data-table-selector] attribute of some parent element
        //http://stackoverflow.com/questions/10641258/jquery-select-data-attributes-that-arent-empty
        var filtersContainer = filter.closest("[data-table-selector!=''][data-table-selector]");

        if (filtersContainer.length > 0)
            tableSelector = filtersContainer.data("table-selector").val();
        else
        {
            //if no specific selector is provided, assume that filters and table have common parent element
            tableSelector = filter.closest("[data-table-with-filters]").find(".dataTables_wrapper .table");
        }

        //last() is workaround for scroll plug-in as it adds another hidden table
        return tableSelector.last().DataTable();
    }

    function getFiltersByTable(table, filterSelector) {
        //check if specific selector is provided via table's [data-filters-selector] attribute
        var fSelector = filterSelector ? filterSelector : "[data-filter]";

        var filtersSelector = table.data("filters-selector");

        if (filtersSelector != null)
            return $(filtersSelector.val());

        //if no specific selector is provided, assume that filters and table have common parent element
        return table.closest("[data-table-with-filters]").find(fSelector);
    }

    function applyDefaultFilters(table) {
        if (table.data('defaultFiltersApplied') != null)
            return;

        table.data('defaultFiltersApplied', true);

        getFiltersByTable(table, "[data-filter][data-default-value!=''][data-default-value]").each(function () {
            applyFilter($(this), false, true);
        });
    }

    me.applyFilter = function (filter, reloadTable, forceDefaultValue) {
        applyFilter(filter, reloadTable, forceDefaultValue);
    };

    me.getFilterVal = function (filter) {
        return getFilterVal(filter);
    };

    me.setFilterVal = function (filter, val) {
        setFilterVal(filter, val);
    };

    me.getFiltersByTable = function (table, filterSelector) {
        return getFiltersByTable(table, filterSelector);
    };

    return me;

})(datatablesFilters || {});