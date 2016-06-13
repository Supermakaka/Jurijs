var userList = (function (site, moment) {

    var $table;

    function initTable() {

        var columnActionsTemplate = Handlebars.compile($("#column-actions-template").html());
        var columnEnabledTemplate = Handlebars.compile($("#column-enabled-template").html());

        $table.DataTable({
            ajax: { url: '/Admin/UserListAjax' },
            columnDefs: [
                { 'width': "10%", 'targets': [0] }
            ],
            columns: [
                { data: 'id', name: 'Id', sortable: true },
                { data: 'companyName', name: 'CompanyName', sortable: true },
                { data: 'email', name: 'Email', sortable: true },
                { data: 'fullName', name: 'FullName', sortable: true },
                { data: 'role', name: 'Role', sortable: false },
                {
                    data: 'disabled', name: 'Disabled', sortable: true,
                    render: function (data, type, full, meta) {
                        return columnEnabledTemplate({ disabled: data });
                    }
                },
                { data: 'createDate', name: 'CreateDate', sortable: true },
                {
                    data: function (row, type, val, meta) {
                        return columnActionsTemplate({ id: row.id, disabled: row.disabled });
                    },
                    searchable: false,
                    sortable: false
                }
            ]
        });
    }

    function initActions() {
        $('#create-date-filter').daterangepicker({
            autoUpdateInput: false,
            opens: 'left',
            ranges: {
                'Today': [moment(), moment()],
                'Yesterday': [moment().subtract(1, 'days'), moment().subtract(1, 'days')],
                'Last 7 Days': [moment().subtract(6, 'days'), moment()],
                'This Month': [moment().startOf('month'), moment().endOf('month')],
                'Last Month': [moment().subtract(1, 'month').startOf('month'), moment().subtract(1, 'month').endOf('month')]
            },
            locale: { cancelLabel: 'Clear' }
        });

        site.ajaxLink('.btn-user-disable', {
            url: '/Admin/DisableOrEnableUser',
            data: function (el) {
                return { id: el.data('id') };
            },
            success: function () {
                refreshTable(false);
            }
        });

        site.modalFormLink('#btn-user-create', {
            url: '/Admin/CreateUser',
            formSubmitSuccess: function () {
                refreshTable(false);
            }
        });

        site.modalFormLink('.btn-user-edit', {
            url: '/Admin/EditUser',
            data: function (el) {
                return { id: el.data('id') };
            },
            formSubmitSuccess: function () {
                refreshTable(false);
            }
        });

        site.modalFormLink('.btn-user-delete', {
            url: '/Admin/ConfirmDeleteUser',
            data: function (el) {
                return { id: el.data('id') };
            },
            formSubmitSuccess: function () {
                refreshTable(true);
            }
        });
    }

    function refreshTable(resetPaging) {
        $table.DataTable().draw(resetPaging);
    }

    var me = {};

    me.onReady = function (t) {
        $table = t;
     
        initTable();
        initActions();
    };

    return me;
})(site, moment);