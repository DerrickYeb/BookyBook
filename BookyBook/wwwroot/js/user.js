﻿var dataTable;

$(document).ready(function () {
    loadDataTable();
});

const loadDataTable = () => {
    dataTable = $('#tblData').DataTable({
        "ajax": {
           "url":"/Admin/User/GetAll"
        },
        "columns": [
            { "data": "name", "width": "15%" },
            { "data": "email", "width": "15%" },
            { "data": "phoneNumber", "width": "15%" },
            { "data": "company.name", "width": "15%" },
            { "data": "role", "width": "15%" },
            {
                "data": { id: "id", lockOutEnd: "lockOutEnd" },
                "render": function (data) {
                    var todayDate = new Date().getTime();
                    var lockOut = new Date(data.lockOut).getTime();
                    if (lockOut > todayDate) {

                        return `
                        <div class="text-center">
                            <a onclick=LockUnlock('${data.id}') class="btn btn-danger text-white" style="cursor:pointer;width:100px">
                            <i class="fas fa-lock-open"></i> Unlock
                            </a>
                        </div>
                                `;
                    } else {
                        return `
                        <div class="text-center">
                            <a onclick=LockUnlock('${data.id}') class="btn btn-success text-white" style="cursor:pointer;width:100px">
                            <i class="fas fa-lock"></i> Lock
                            </a>
                        </div>
                                `;
                    }
                    
                },"width":"25%"
            }
        ]
    })
}
const LockUnlock = (id) => {
    $.ajax({
        type: "POST",
        url: "/Admin/User/LockUnlock",
        data: JSON.stringify(id),
        contentType: "application/json",
        success: function (data) {
            if (data.success) {
                toastr.success(data.message);
                dataTable.ajax.reload();
            } else {
                toastr.error(data.message);
            }
        }

    });
}