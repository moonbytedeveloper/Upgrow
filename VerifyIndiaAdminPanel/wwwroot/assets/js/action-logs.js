window.AuditLogsUI = (function () {
    const state = {
        fkCache: {},
        foreignKeyMap: {},
        friendlyFieldMap: {},
        hiddenFields: new Set(),
        logTable: null,
        currentContext: {}
    };

    function normalizeFieldName(name) {
        return (name || "").toLowerCase().trim();
    }

    function isHiddenField(fieldName) {
        const key = normalizeFieldName(fieldName);
        return state.hiddenFields.has(key);
    }

    function nullSafe(v) {
        return v === null || v === undefined || v === "" ? "-" : v;
    }

    function formatDate(dateStr) {
        if (!dateStr) return "-";
        try {
            let date;
            if (typeof dateStr === 'string' && dateStr.startsWith('/Date(') && dateStr.endsWith(')/')) {
                const ticks = parseInt(dateStr.slice(6, -2));
                date = new Date(ticks);
            } else {
                date = new Date(dateStr);
            }
            return date.toLocaleString();
        } catch {
            return dateStr;
        }
    }

    function parseJsonSafe(jsonText) {
        try { return jsonText ? JSON.parse(jsonText) : {}; }
        catch { return {}; }
    }

    function badge(actionType) {
        const t = (actionType || "").toLowerCase();
        if (t === "create") return "<span class='badge bg-success'>Create</span>";
        if (t === "update") return "<span class='badge bg-warning text-dark'>Update</span>";
        if (t === "delete") return "<span class='badge bg-danger'>Delete</span>";
        if (t === "password changed" || t === "passwordchanged") return "<span class='badge bg-primary'>Password Changed</span>";
        return "<span class='badge bg-secondary'>" + nullSafe(actionType) + "</span>";
    }

    function friendlyField(field) {
        return state.friendlyFieldMap[field] || field.replace(/_/g, " ");
    }

    function setLoader(selector, show) {
        $(selector).toggleClass("d-none", !show);
    }

    async function getFkDisplay(endpoint, id) {
        if (!endpoint || !id) return id;

        state.fkCache[endpoint] = state.fkCache[endpoint] || {};
        if (state.fkCache[endpoint][id]) return state.fkCache[endpoint][id];

        try {
            const res = await $.get(endpoint, { id: id });
            const name = (res && (res.name || res.text || res.value))
                ? (res.name || res.text || res.value)
                : id;
            state.fkCache[endpoint][id] = name;
            return name;
        } catch {
            return id;
        }
    }

    async function resolveValue(field, value, entityName) {
        const v = nullSafe(value);
        if (v === "-") return "-";

        const entityFieldKey = entityName ? (entityName + "." + field) : "";
        const endpoint = state.foreignKeyMap[entityFieldKey] || state.foreignKeyMap[field];
        if (!endpoint) return v;

        if (typeof v === "string" && v.indexOf(",") > -1) {
            const parts = v.split(",").map(x => x.trim()).filter(Boolean);
            const names = await Promise.all(parts.map(x => getFkDisplay(endpoint, x)));
            return names.join(", ");
        }

        return await getFkDisplay(endpoint, v);
    }

    function initDataTable() {
        // Destroy existing instance if it exists
        if (state.logTable) {
            state.logTable.destroy();
            state.logTable = null;
        }

        if (!$.fn.DataTable) {
            console.error("DataTables not loaded!");
            return;
        }

        state.logTable = $('#auditLogsTable').DataTable({
            processing: true,
            serverSide: true,
            paging: true,
            searching: true,
            lengthChange: true,
            pageLength: 10,
            order: [[0, 'desc']],
            dom: 'lfrtip', // Controls: length, processing, table, info, pagination
            ajax: {
                url: '/ActionLogs/GetActionLogsPaged',
                type: 'POST',
                data: function (d) {
                    // Add our custom parameters
                    d.entityName = state.currentContext.entityName || '';
                    d.entityUUID = state.currentContext.entityUUID || '';
                    d.includeChildren = state.currentContext.includeChildren || false;
                    d.lineEntityNames = state.currentContext.lineEntityNames || '';
                },
                error: function (xhr, error, thrown) {
                    console.error('DataTable AJAX Error:', error, xhr);
                }
            },
            columns: [
                { data: 'date', name: 'date' },
                { data: 'actiontype', name: 'actiontype', orderable: false },
                { data: 'user', name: 'user' },
                { data: 'ip', name: 'ip' },
                { data: null, orderable: false, searchable: false }
            ],
            columnDefs: [
                {
                    targets: 4,
                    render: function (data, type, row) {
                        return `<button type="button" class="btn btn-sm btn-primary btn-view-log" data-logid="${row.id}">
                                    <i class="fa fa-eye"></i>
                                </button>`;
                    }
                }
            ],
            drawCallback: function (settings) {
                /*console.log('DataTable drawn with', settings.fnRecordsDisplay(), 'records');*/
            }
        });
    }

    async function openLogs(entityName, entityUUID, includeChildren, lineEntityNames) {
        if (!entityName || !entityUUID) return;

        const menu = document.getElementById("rowActionMenu");
        if (menu) menu.style.display = "none";

        showLogsSidebar();

        state.currentContext = {
            entityName: entityName,
            entityUUID: entityUUID,
            includeChildren: !!includeChildren,
            lineEntityNames: lineEntityNames || ""
        };

        /*console.log('Opening logs with context:', state.currentContext);*/

        try {
            // Initialize or reload DataTable
            if (state.logTable) {
                state.logTable.ajax.reload();
            } else {
                initDataTable();
            }
        } catch (error) {
            console.error("Error loading audit logs:", error);
            alert("Failed to load audit logs: " + error.message);
        }
    }

    async function renderChanges(oldValuesJson, newValuesJson, entityName) {
        const oldObj = parseJsonSafe(oldValuesJson);
        const newObj = parseJsonSafe(newValuesJson);

        const keys = Object.keys(Object.assign({}, oldObj, newObj))
            .filter(k => !isHiddenField(k));

        const $body = $("#auditChangesBody");
        const $header = $("#auditChangesModal thead");

        if (!keys.length) {
            $header.show();
            $body.html("<tr><td colspan='3' class='text-center text-muted'>No visible field-level changes.</td></tr>");
            return;
        }

        if (Object.prototype.hasOwnProperty.call(newObj, "PasswordChangeReason")) {
            const reasonText = nullSafe(newObj.PasswordChangeReason);
            $header.hide();
            $body.html(`
                <tr>
                    <td colspan="3" class="p-0 border-0">
                        <div class="alert alert-info mb-0 py-3 px-3">
                            <div><strong>Password changed</strong></div>
                            <div class="mt-1">Reason: ${reasonText}</div>
                        </div>
                    </td>
                </tr>
            `);
            return;
        }

        $header.show();
        setLoader("#auditChangesLoader", true);

        const rows = await Promise.all(keys.map(async function (k) {
            const oldVal = await resolveValue(k, oldObj[k], entityName);
            const newVal = await resolveValue(k, newObj[k], entityName);

            return `
                <tr>
                    <td>${friendlyField(k)}</td>
                    <td class="text-danger">${nullSafe(oldVal)}</td>
                    <td class="text-success">${nullSafe(newVal)}</td>
                </tr>`;
        }));

        $body.html(rows.join(""));
        setLoader("#auditChangesLoader", false);
    }

    function resetChangesModal() {
        $("#auditChangesModal thead").show();
        $("#auditChangesBody").html("<tr><td colspan='3' class='text-center text-muted'>No field-level changes.</td></tr>");
        $("#auditChangesMeta").addClass("d-none").text("");
        setLoader("#auditChangesLoader", false);
    }

    function extractUuidFromActionHtml(actionHtml) {
        const match = (actionHtml || "").match(/data-uuid=['"]([^'"]+)['"]/i);
        return (match && match.length > 1) ? match[1] : "";
    }

    function buildLogsButton(entityName, entityUUID, includeChildren, lineEntityNames) {
        if (!entityUUID) return "";
        const lineCsv = Array.isArray(lineEntityNames)
            ? lineEntityNames.join(",")
            : (lineEntityNames || "");

        return "<button type='button' class='btn btn-info btn-sm ms-1 btn-audit-logs' " +
            "data-entity-name='" + entityName + "' " +
            "data-entity-uuid='" + entityUUID + "' " +
            "data-include-children='" + (includeChildren ? "true" : "false") + "' " +
            "data-line-entities='" + lineCsv + "' " +
            "title='View Logs'>" +
            "<i class='bx bx-history'></i></button>";
    }

    function createActionRenderer(entityName, includeChildren, lineEntityNames) {
        return function (data) {
            const uuid = extractUuidFromActionHtml(data);
            return (data || "") + buildLogsButton(entityName, uuid, !!includeChildren, lineEntityNames || []);
        };
    }

    function showLogsSidebar() {
        const el = document.getElementById("auditLogsSidebar");
        if (!el) return;

        if (window.bootstrap && bootstrap.Offcanvas) {
            bootstrap.Offcanvas.getOrCreateInstance(el).show();
            return;
        }

        $(el).addClass("show").css("visibility", "visible").attr("aria-hidden", "false");
        if (!$(".offcanvas-backdrop").length) {
            $("<div class='offcanvas-backdrop fade show'></div>").appendTo(document.body);
        }
        $("body").css("overflow", "hidden");
    }

    function hideLogsSidebar() {
        const el = document.getElementById("auditLogsSidebar");
        if (!el) return;

        if (window.bootstrap && bootstrap.Offcanvas) {
            const instance = bootstrap.Offcanvas.getOrCreateInstance(el);
            instance.hide();
            return;
        }

        $(el).removeClass("show").css("visibility", "hidden").attr("aria-hidden", "true");
        $(".offcanvas-backdrop").remove();
        $("body").css("overflow", "");
    }

    function wireEvents() {
        $(document).on("click", ".btn-audit-logs", async function () {
            const entityName = $(this).data("entity-name");
            const entityUUID = $(this).data("entity-uuid");
            const includeChildren = String($(this).data("include-children")).toLowerCase() === "true";
            const lineEntityNames = $(this).data("line-entities") || "";

            await openLogs(entityName, entityUUID, includeChildren, lineEntityNames);
        });

        $(document).on("click", ".btn-view-log", async function () {
            const logId = Number($(this).data("logid"));

            if (!logId) {
                console.error("Invalid log ID");
                return;
            }

            setLoader("#auditChangesLoader", true);

            try {
                const response = await $.ajax({
                    url: '/ActionLogs/GetLogDetail',
                    type: 'GET',
                    dataType: 'json',
                    data: { logId: logId }
                });

                /*console.log("Log detail response:", response);*/

                if (!response.success || !response.data) {
                    console.error("Failed to fetch log details");
                    alert("Failed to load log details");
                    return;
                }

                const log = response.data;

                $("#auditChangesMeta")
                    .removeClass("d-none")
                    .html("<i class='fa fa-info-circle me-2'></i>Showing changes made at " +
                        formatDate(log.createdAt) + " by " + nullSafe(log.userName));

                $("#auditChangesBody").html("");
                const modalEl = document.getElementById("auditChangesModal");
                const modal = bootstrap.Modal.getOrCreateInstance(modalEl);
                modal.show();

                await renderChanges(log.oldValues, log.newValues, log.entityName);
            } catch (error) {
                console.error("Error fetching log detail:", error);
                alert("Failed to load log details: " + error.message);
            } finally {
                setLoader("#auditChangesLoader", false);
            }
        });

        $("#auditChangesModal").on("hidden.bs.modal", function () {
            resetChangesModal();
        });

        $("#auditLogsSidebar").on("hidden.bs.offcanvas", function () {
            if (state.logTable) {
                state.logTable.destroy();
                state.logTable = null;
            }
        });

        $(document).on("click", "[data-bs-dismiss='offcanvas']", function () {
            hideLogsSidebar();
        });
    }

    function init(config) {
        state.foreignKeyMap = Object.assign({}, (config && config.foreignKeyMap) ? config.foreignKeyMap : {});
        state.friendlyFieldMap = Object.assign({}, (config && config.friendlyFieldMap) ? config.friendlyFieldMap : {});

        const defaultsToHide = [
            "Id",
            "UUID",
            "RecordNo",
            "Master_Environment_UUID",
            "Master_Company_UUID",
            "ERExpenseHeader_UUID",
            "Header_UUID",
            "RecordHash"
        ];

        const pageHidden = (config && config.hiddenFields) ? config.hiddenFields : [];
        state.hiddenFields = new Set([].concat(defaultsToHide, pageHidden).map(normalizeFieldName));

        wireEvents();
    }

    return {
        init: init,
        createActionRenderer: createActionRenderer,
        openLogs: openLogs
    };
})();

AuditLogsUI.common = {
    auditUserFkMap: {
        "IsAddedBy": "/Common/GetEmployeeName",
        "IsUpdateBy": "/Common/GetEmployeeName",
        "IsUpdatedBy": "/Common/GetEmployeeName",
        "IsDeletedBy": "/Common/GetEmployeeName"
    },
    mergeConfig: function (config) {
        return {
            foreignKeyMap: Object.assign({}, this.auditUserFkMap, (config && config.foreignKeyMap) ? config.foreignKeyMap : {}),
            friendlyFieldMap: Object.assign({}, (config && config.friendlyFieldMap) ? config.friendlyFieldMap : {}),
            hiddenFields: (config && config.hiddenFields) ? config.hiddenFields : []
        };
    }
};