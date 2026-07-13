window.initActivityLogTable = function (cfg) {
    const config = {
        buttonSelector: '#activityLogButton',
        tableSelector: '#activityLogTable',
        endpoint: '/ActivityLogs/GetByMenu',
        menuName: '',
        menuNameFieldSelector: '#activityLogMenuName',
        ...cfg
    };

    let activityLogTable = null;

    const $btn = $(config.buttonSelector);
    if (!$btn.length) return;

    // Move global button to page header action area (same place as old per-page buttons)
    const btnEl = $btn.get(0);
    const actionHost = document.querySelector('.card-header .text-align-end, .card-header .text-end');
    if (actionHost && btnEl && !actionHost.contains(btnEl)) {
        actionHost.appendChild(btnEl);
    }

    // Show global button only on pages that call initActivityLogTable
    $btn.removeClass('d-none');

    // Optional title update
    const titleEl = document.getElementById('activityLogTitle');
    if (titleEl) {
        titleEl.textContent = 'Activity Logs';
        titleEl.style.color = '#fff';
    }

    // Prevent duplicate click binding if called more than once
    if ($btn.data('activity-log-bound') === true) {
        $btn.data('activity-log-menu', config.menuName || '');
        return;
    }

    $btn.data('activity-log-bound', true);
    $btn.data('activity-log-menu', config.menuName || '');

    const getMenuName = function () {
        const fieldValue = $(config.menuNameFieldSelector).val();
        return (fieldValue && String(fieldValue).trim())
            || $btn.data('activity-log-menu')
            || config.menuName
            || '';
    };

    $btn.on('click', function () {
        if (!activityLogTable) {
            activityLogTable = $(config.tableSelector).DataTable({
                serverSide: true,
                processing: true,
                paging: true,
                lengthChange: true,
                searching: true,
                ordering: true,
                info: true,
                language: {
                    infoFiltered: ''
                },
                autoWidth: false,
                responsive: false,
                scrollX: false,
                scrollCollapse: false,
                ajax: {
                    url: config.endpoint,
                    type: 'POST',
                    data: function (d) {
                        d.menuName = getMenuName();
                    },
                    error: function (xhr, status, error) {
                        console.error('DataTable AJAX Error:', error);
                        console.error('Response:', xhr.responseText);
                    }
                },
                lengthMenu: [
                    [5, 10, 20, 50, 100], [5, 10, 20, 50, 100]
                ],
                pageLength: 10,
                order: [[0, 'desc']],
                columns: [
                    { data: 'createdat', title: 'Date', className: 'text-nowrap' },
                    { data: 'useruuid', title: 'User Name' },
                    { data: 'activitytype', title: 'Activity' },
                    {
                        data: 'description',
                        title: 'Description',
                        render: function (data) {
                            return data ? (data.length > 40 ? data.substring(0, 40) + '...' : data) : '-';
                        }
                    },
                    {
                        data: 'pageurl',
                        title: 'Page URL',
                        render: function (data) {
                            if (!data) return '-';
                            if (data.toLowerCase().includes('delete')) return data;
                            return '<a href="' + data + '" target="_blank" class="text-primary" title="' + data + '">' +
                                (data.length > 35 ? data.substring(0, 35) + '...' : data) +
                                '</a>';
                        }
                    },
                    { data: 'ipaddress', title: 'IP Address' }
                ]
            });
        } else {
            activityLogTable.ajax.reload();
        }
    });
};