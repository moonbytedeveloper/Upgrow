// Initialize datatable_1 if it exists

const datatable1Element = document.querySelector("#datatable_1");
if (datatable1Element) {
    const dataTable = new simpleDatatables.DataTable("#datatable_1", {
        searchable: true,
        fixedHeight: false
    });
    const csvButton = document.querySelector("button.csv");
    if (csvButton) {
        csvButton.addEventListener("click", () => {
            dataTable.export({ type: "csv", download: true, lineDelimiter: "\n\n", columnDelimiter: ";" });
        });
    }
    const sqlButton = document.querySelector("button.sql");
    if (sqlButton) {
        sqlButton.addEventListener("click", () => {
            dataTable.export({ type: "sql", download: true, tableName: "export_table" });
        });
    }
    const txtButton = document.querySelector("button.txt");
    if (txtButton) {
        txtButton.addEventListener("click", () => {
            dataTable.export({ type: "txt", download: true });
        });
    }
    const jsonButton = document.querySelector("button.json");
    if (jsonButton) {
        jsonButton.addEventListener("click", () => {
            dataTable.export({ type: "json", download: true, escapeHTML: true, space: 3 });
        });
    }
}



// Initialize datatable_2 if it exists

const datatable2Element = document.querySelector("#datatable_2");
if (datatable2Element) {
    const dataTable_2 = new simpleDatatables.DataTable("#datatable_2");
    // Export button event listeners
    const csvButton = document.querySelector("button.csv");
    if (csvButton) {
        csvButton.addEventListener("click", () => {
            dataTable_2.export({ type: "csv", download: true, lineDelimiter: "\n\n", columnDelimiter: ";" });
        });
    }
    const sqlButton = document.querySelector("button.sql");
    if (sqlButton) {
        sqlButton.addEventListener("click", () => {
            dataTable_2.export({ type: "sql", download: true, tableName: "export_table" });
        });
    }
    const txtButton = document.querySelector("button.txt");
    if (txtButton) {
        txtButton.addEventListener("click", () => {
            dataTable_2.export({ type: "txt", download: true });
        });
    }
    const jsonButton = document.querySelector("button.json");
    if (jsonButton) {
        jsonButton.addEventListener("click", () => {
            dataTable_2.export({ type: "json", download: true, escapeHTML: true, space: 3 });
        });
    }
}