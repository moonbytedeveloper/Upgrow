window.initMasterTable = function (cfg) {

    const stateKey = cfg.stateKey || `DT:${window.location.pathname}:${cfg.tableId}`;
    const returnKey = `${stateKey}:return-once`;
    const persistOnceKey = `${stateKey}:persist-once`;

    function toggleTableLoader($table, show) {
        const wrapper = $table.closest('.dataTables_wrapper');
        let loader = wrapper.find('.table-loader');

        if (!loader.length) {
            loader = $(
                '<div class="table-loader" aria-label="Loading" role="status">' +
                '<span></span><span></span><span></span><span></span><span></span>' +
                '</div>'
            );
            wrapper.append(loader);
        }

        loader.toggleClass('show', show);
    }

    const $table = $(cfg.tableId);

    const table = $table.DataTable({
        mark: true,
        serverSide: true,
        processing: false,
        stateSave: true,
        language: {
            processing: ""
        },
        preDrawCallback: function () {
            toggleTableLoader($table, true);
        },
        drawCallback: function (settings) {
            if (typeof cfg.drawCallback === 'function') {
                cfg.drawCallback.call(this, settings);
            }
            toggleTableLoader($table, false);
        },
        stateSaveCallback: function (settings, data) { /* existing */ },
        stateLoadCallback: function () { /* existing */ return null; },
        ajax: { url: cfg.ajaxUrl, type: 'POST' },
        columns: cfg.columns,
        order: cfg.order || [],
        ordering: true,
        lengthMenu: cfg.lengthMenu || [[5, 10, 25, 50, 100], [5, 10, 25, 50, 100]],
        pageLength: cfg.pageLength || 10,
        autoWidth: false,
        info: true,
    });

    // Handle row selection and action setup
    if (cfg.rowActions) {
        $(document).on('click', `${cfg.tableId} tbody tr`, function () {
            const rowData = table.row(this).data();
            if (!rowData) return;

            cfg.rowActions.forEach(action => {
                const element = $(action.selector);
                if (!element.length) return;

                let actionUrl = action.baseUrl;

                if (action.params) {
                    const queryParams = [];
                    for (const param of action.params) {
                        const dataProperty = param.dataProperty;
                        const urlParam = param.urlParam || dataProperty;
                        if (rowData[dataProperty] !== undefined && rowData[dataProperty] !== null) {
                            queryParams.push(urlParam + '=' + encodeURIComponent(rowData[dataProperty]));
                        }
                    }
                    if (queryParams.length > 0) {
                        actionUrl += '?' + queryParams.join('&');
                    }
                } else {
                    const idProperty = action.idProperty || 'uuid';
                    const paramName = action.paramName || idProperty;
                    if (rowData[idProperty]) {
                        actionUrl += '?' + paramName + '=' + encodeURIComponent(rowData[idProperty]);
                    }
                }

                if (element.is('a')) {
                    element.attr('href', actionUrl);
                }

                element.data('rowData', rowData);
                element.data('actionUrl', actionUrl);
                element.data('selected', true);
                element.data('actionConfig', action);
            });
        });

        const allSelectors = cfg.rowActions.map(a => a.selector).join(', ');

        $(document).on('click', allSelectors, function (e) {
            e.preventDefault();
            e.stopPropagation();

            const menu = document.getElementById("rowActionMenu");
            if (menu) {
                menu.style.display = "none";
            }

            const $this = $(this);
            const isSelected = $this.data('selected');
            const rowData = $this.data('rowData');
            const actionUrl = $this.data('actionUrl');
            const actionConfig = $this.data('actionConfig');

            if (!isSelected || !rowData) {
                Swal.fire({
                    icon: 'warning',
                    title: 'No Row Selected',
                    text: (actionConfig && actionConfig.noSelectionMessage) || 'Please select a row from the table first.'
                });
                return;
            }

            if (actionConfig && actionConfig.handler) {
                actionConfig.handler(rowData, actionUrl, $this);
            } else {
                if (actionUrl && actionUrl !== '#' && actionUrl !== 'javascript:void(0)') {
                    const linkType = ($this.data('link-type') || '').toString().toLowerCase();
                    const isEditAction =
                        actionConfig?.persistStateOnReturn === true ||
                        linkType === 'edit';

                    if (isEditAction) {
                        sessionStorage.setItem(persistOnceKey, "1");
                        table.state.save(); // triggers stateSaveCallback
                    } else {
                        sessionStorage.removeItem(stateKey);
                        sessionStorage.removeItem(returnKey);
                        sessionStorage.removeItem(persistOnceKey);
                    }

                    window.location.href = actionUrl;
                }
            }
        });
    }

    return table;
};