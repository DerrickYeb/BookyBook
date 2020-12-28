var dataTable;

$(document).ready(function () {
    loadDataTable();
});

const loadDataTable = () => {
    dataTable = $('tblData').DataTable({
        "ajax"= {
            "url":"/Admin/Company/GetAll"
        },
        "columns": [
        ]
    })
}
