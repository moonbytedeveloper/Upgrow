let isInitializing = true;

$(document).ready(function () {

    loadRootDropdowns()
        .then(loadDependentDropdowns)
        .then(() => {
            isInitializing = false;
        });

    $(document).on('change', '.dependent-dropdown', function () {

        if (isInitializing) return;

        const changed = $(this);

        $('.dependent-dropdown').each(function () {
            const ddl = $(this);

            if (ddl.is(changed)) return;
            if (!isDescendantOf(ddl, changed)) return;

            clearDropdown(ddl);
            ddl.removeAttr('data-value');
            loadDropdown(ddl);
        });
    });
});

/* ---------- INITIAL LOAD ---------- */

function loadRootDropdowns() {
    const roots = $('.dependent-dropdown').filter(function () {
        return !$(this).data('dd').includes('(');
    });

    return Promise.all(
        roots.map((_, el) => loadDropdown($(el))).get()
    );
}

async function loadDependentDropdowns() {
    const dependents = $('.dependent-dropdown').filter(function () {
        return $(this).data('dd').includes('(');
    });

    for (const el of dependents) {
        const ddl = $(el);
        const cfg = parseConfig(ddl);

        if (cfg.parentKey) {
            const parent = findParentDropdown(cfg.parentKey);
            if (!parent.val()) {
                clearDropdown(ddl);
                continue;
            }
        }

        await loadDropdown(ddl);
    }
}

/* ---------- CASCADE LOGIC ---------- */

function isDescendantOf(ddl, changed) {
    let cfg = parseConfig(ddl);

    while (cfg && cfg.parentKey) {
        const parent = findParentDropdown(cfg.parentKey);
        if (!parent.length) return false;

        if (parent.is(changed)) return true;

        cfg = parseConfig(parent);
    }

    return false;
}

/* ---------- CORE ---------- */

function parseConfig(ddl) {
    const cfg = ddl.data('dd');
    if (!cfg) return null;

    if (!cfg.includes('(')) {
        return { key: cfg };
    }

    const match = cfg.match(/^(\w+)\((\w+)\)$/);
    if (!match) return null;

    return {
        key: match[1],
        parentKey: match[2]
    };
}

function findParentDropdown(parentKey) {
    return $('.dependent-dropdown').filter(function () {
        const cfg = parseConfig($(this));
        return cfg && cfg.key === parentKey;
    });
}

function loadDropdown(ddl) {
    const cfg = parseConfig(ddl);
    if (!cfg) return Promise.resolve();

    const parents = {};

    if (cfg.parentKey) {
        const parent = findParentDropdown(cfg.parentKey);
        const value = parent.val();

        if (!value) {
            clearDropdown(ddl);
            return Promise.resolve();
        }

        parents[cfg.parentKey] = value;
    }

    const placeholder = ddl.data('placeholder') || '';
    const selectedValue = ddl.data('value');

    ddl.empty();
    ddl.append('<option value="">Loading...</option>');

    return $.ajax({
        url: '/Common/GetDropdown',
        type: 'POST',
        contentType: 'application/json',
        data: JSON.stringify({
            key: cfg.key,
            parents: parents
        })
    }).done(function (data) {
        ddl.empty();
        ddl.append('<option value="">' + placeholder + '</option>');

        data.forEach(function (item) {
            ddl.append(
                '<option value="' + item.value + '">' + item.text + '</option>'
            );
        });

        if (selectedValue && ddl.find('option[value="' + selectedValue + '"]').length) {
            ddl.val(selectedValue);
        } else {
            ddl.prop('selectedIndex', 0);
        }
    });
}

/* ---------- HELPERS ---------- */

function clearDropdown(ddl) {
    const placeholder = ddl.data('placeholder') || '';

    ddl.val('');
    ddl.empty();
    ddl.append('<option value="">' + placeholder + '</option>');
    ddl.prop('selectedIndex', 0);
    if (ddl.hasClass('select2-hidden-accessible')) {
        ddl.trigger('change.select2');
    }
}
