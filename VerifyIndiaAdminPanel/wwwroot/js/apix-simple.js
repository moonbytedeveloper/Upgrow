(function () {
    'use strict';

    async function fetchJson(url, opts = {}) {
        try {
            const res = await fetch(url, { credentials: 'same-origin', ...opts });
            try { return await res.json(); } catch { return { success: res.ok, status: res.status, text: await res.text() }; }
        } catch (err) {
            return { success: false, error: err.message || String(err) };
        }
    }

    function showTab(href) {
        try {
            const link = document.querySelector(`.custom-tabs a[href="${href}"]`);
            if (link && window.bootstrap) new bootstrap.Tab(link).show();
        } catch (e) { /* ignore */ }
    }

    function populateFormByName(form, name, value) {
        if (!form) return;
        const el = form.querySelector(`[name="${name}"]`);
        if (!el) return;
        if (el.type === 'checkbox') el.checked = !!value;
        else el.value = value ?? '';
        el.dispatchEvent(new Event('change', { bubbles: true }));
    }

    function populateHeaderForm(item) {
        const form = document.querySelector('#headerForm');
        if (!form || !item) return;
        populateFormByName(form, 'Header.UUID', item.uuid);
        populateFormByName(form, 'Header.ApiXVersionUUID', item.apiXVersionUUID);
        populateFormByName(form, 'Header.FieldName', item.fieldName);
        populateFormByName(form, 'Header.DataType', item.dataType);
        populateFormByName(form, 'Header.FieldDetails', item.fieldDetails);
        populateFormByName(form, 'Header.IsRequired', item.isRequired);
        const btn = document.getElementById('addHeaderBtn');
        if (btn) btn.textContent = 'Update';
        showTab('#ApiHeader');
        form.scrollIntoView({ behavior: 'smooth', block: 'center' });
    }

    function populateFieldForm(formSelector, item) {
        const form = document.querySelector(formSelector);
        if (!form || !item) return;
        populateFormByName(form, 'Field.UUID', item.uuid);
        populateFormByName(form, 'Field.SchemaUUID', item.schemaUUID);
        populateFormByName(form, 'Field.FieldName', item.fieldName);
        populateFormByName(form, 'Field.DataType', item.dataType);
        populateFormByName(form, 'Field.Examlpe', item.examlpe);
        populateFormByName(form, 'Field.DisplayOrder', item.displayOrder);
        populateFormByName(form, 'Field.AllowNull', item.allowNull);
        populateFormByName(form, 'Field.IsArray', item.isArray);
        populateFormByName(form, 'Field.ParentUUID', item.parentUUID);
        populateFormByName(form, 'Field.Constraints', item.constraints);
        if (formSelector === '#fieldForm') showTab('#RequestScehmaFields');
        else if (formSelector === '#responseFieldForm') showTab('#SuccessReponseSchema');
        else if (formSelector === '#failureFieldForm') showTab('#FailureReponseSchema');
        form.scrollIntoView({ behavior: 'smooth', block: 'center' });
    }

    // Delegated handlers
    document.addEventListener('DOMContentLoaded', function () {
        // Headers table - anchor click fills header form inline
        const headersTable = document.querySelector('#headersTable');
        if (headersTable) {
            headersTable.addEventListener('click', async function (e) {
                const editAnchor = e.target.closest('.edit-header');
                if (editAnchor) {
                    e.preventDefault();
                    e.stopPropagation();
                    const tr = editAnchor.closest('tr');
                    if (!tr) return;
                    const uuid = tr.dataset.uuid;
                    if (!uuid) return;
                    const res = await fetchJson(`/AIX/EditHeaderJson?uuid=${encodeURIComponent(uuid)}`);
                    if (res && res.success && res.item) populateHeaderForm(res.item);
                    else console.error('EditHeaderJson failed', res);
                    return;
                }

                // fallback: row click still edits
                const tr = e.target.closest('tr');
                if (!tr || !headersTable.contains(tr)) return;
                const uuid = tr.dataset.uuid;
                if (!uuid) return;
                const res = await fetchJson(`/AIX/EditHeaderJson?uuid=${encodeURIComponent(uuid)}`);
                if (res && res.success && res.item) populateHeaderForm(res.item);
            });
        }

        // Request/Response/Failure fields anchors - "Edit Details"
        function handleFieldEditClick(e, tableSelector, formSelector) {
            const anchor = e.target.closest('.edit-field');
            if (!anchor) return false;
            e.preventDefault();
            e.stopPropagation();
            const tr = anchor.closest('tr');
            if (!tr) return true;
            const uuid = tr.dataset.uuid;
            if (!uuid) return true;
            // fetch and populate
            fetchJson(`/AIX/GetFieldByUuid?uuid=${encodeURIComponent(uuid)}`)
                .then(res => {
                    if (res && res.success && res.item) populateFieldForm(formSelector, res.item);
                    else console.error('GetFieldByUuid failed', res);
                });
            return true;
        }

        const reqTable = document.querySelector('#requestFieldsTable');
        if (reqTable) {
            reqTable.addEventListener('click', function (e) {
                if (handleFieldEditClick(e, '#requestFieldsTable', '#fieldForm')) return;
                // fallback: row click behavior (existing)
                const tr = e.target.closest('tr');
                if (!tr) return;
                const uuid = tr.dataset.uuid;
                if (!uuid) return;
                fetchJson(`/AIX/GetFieldByUuid?uuid=${encodeURIComponent(uuid)}`)
                    .then(res => { if (res && res.success && res.item) populateFieldForm('#fieldForm', res.item); });
            });
        }

        const resTable = document.querySelector('#responseFieldsTable');
        if (resTable) {
            resTable.addEventListener('click', function (e) {
                if (handleFieldEditClick(e, '#responseFieldsTable', '#responseFieldForm')) return;
                const tr = e.target.closest('tr');
                if (!tr) return;
                const uuid = tr.dataset.uuid;
                if (!uuid) return;
                fetchJson(`/AIX/GetFieldByUuid?uuid=${encodeURIComponent(uuid)}`)
                    .then(res => { if (res && res.success && res.item) populateFieldForm('#responseFieldForm', res.item); });
            });
        }

        const failTable = document.querySelector('#failureFieldsTable');
        if (failTable) {
            failTable.addEventListener('click', function (e) {
                if (handleFieldEditClick(e, '#failureFieldsTable', '#failureFieldForm')) return;
                const tr = e.target.closest('tr');
                if (!tr) return;
                const uuid = tr.dataset.uuid;
                if (!uuid) return;
                fetchJson(`/AIX/GetFieldByUuid?uuid=${encodeURIComponent(uuid)}`)
                    .then(res => { if (res && res.success && res.item) populateFieldForm('#failureFieldForm', res.item); });
            });
        }

        // Response schema rows (if shown as table) - anchor with class edit-schema (if you add it server-side)
        document.querySelector('#responseSchemasTable')?.addEventListener('click', async function (e) {
            const editSchema = e.target.closest('.edit-schema');
            if (editSchema) {
                e.preventDefault();
                const tr = editSchema.closest('tr');
                if (!tr) return;
                const uuid = tr.dataset.uuid;
                if (!uuid) return;
                const res = await fetchJson(`/AIX/GetResponseSchemaByUuid?uuid=${encodeURIComponent(uuid)}`);
                if (res && res.success && res.item) populateResponseSchemaForm(res.item);
                return;
            }
            const tr = e.target.closest('tr');
            if (!tr) return;
            const uuid = tr.dataset.uuid;
            if (!uuid) return;
            const res = await fetchJson(`/AIX/GetResponseSchemaByUuid?uuid=${encodeURIComponent(uuid)}`);
            if (res && res.success && res.item) populateResponseSchemaForm(res.item);
        });

        // Code mapper table rows (if you render edit anchors server-side use `.edit-codemapper`)
        document.querySelector('#codeMapperTable')?.addEventListener('click', async function (e) {
            const editCm = e.target.closest('.edit-codemapper');
            if (editCm) {
                e.preventDefault();
                const tr = editCm.closest('tr');
                if (!tr) return;
                const uuid = tr.dataset.uuid;
                if (!uuid) return;
                const res = await fetchJson(`/AIX/GetCodeMapperByUuid?uuid=${encodeURIComponent(uuid)}`);
                if (res && res.success && res.item) populateCodeMapperForm(res.item);
                return;
            }
            const tr = e.target.closest('tr');
            if (!tr) return;
            const uuid = tr.dataset.uuid;
            if (!uuid) return;
            const res = await fetchJson(`/AIX/GetCodeMapperByUuid?uuid=${encodeURIComponent(uuid)}`);
            if (res && res.success && res.item) populateCodeMapperForm(res.item);
        });

        // Intercept headerForm submit (unchanged)
        const headerForm = document.getElementById('headerForm');
        if (headerForm) {
            headerForm.addEventListener('submit', async function (e) {
                e.preventDefault();
                const btn = document.getElementById('addHeaderBtn');
                if (btn) {
                    btn.disabled = true;
                    btn.dataset.origText = btn.textContent;
                    btn.textContent = (btn.textContent.trim().toLowerCase() === 'update') ? 'Updating...' : 'Adding...';
                }

                try {
                    const formData = new FormData(headerForm);
                    const res = await fetch(headerForm.action, {
                        method: 'POST',
                        body: formData,
                        credentials: 'same-origin'
                    });

                    const json = await (async () => {
                        try { return await res.json(); } catch { return null; }
                    })();

                    if (res.ok && json && json.success && json.item) {
                        // update table
                        const table = document.querySelector('#headersTable');
                        if (table) {
                            // simple upsert row function reuse (if defined elsewhere)
                            const evt = new CustomEvent('apix:headerSaved', { detail: json.item });
                            document.dispatchEvent(evt);
                        }
                        headerForm.reset();
                        const uuidInput = headerForm.querySelector('[name="Header.UUID"]');
                        if (uuidInput) uuidInput.value = '';
                        if (btn) btn.textContent = 'Add';
                    } else {
                        console.error('Header save failed', json || res.statusText);
                        alert((json && (json.error || json.message)) || 'Failed to save header');
                    }
                } catch (err) {
                    console.error('Header save exception', err);
                    alert('Error saving header: ' + (err && err.message ? err.message : String(err)));
                } finally {
                    if (btn) {
                        btn.disabled = false;
                        if (!btn.dataset.origText) btn.dataset.origText = 'Add';
                        if (btn.textContent === 'Updating...' || btn.textContent === 'Adding...') btn.textContent = btn.dataset.origText;
                    }
                }
                return false;
            });
        }

    });
})();