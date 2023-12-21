var dataTable;
$(document).ready(function () {
    loadDataTable();
});

function loadDataTable() {
    dataTable = $('#productTable').DataTable({
        ajax: {
            url: '/Admin/Product/GetAll'
        },
        columns: [
            { data: 'title' },
            { data: 'isbn' },
            { data: 'author' },
            { data: 'price' },
            { data: 'category.name' },
            {
                data: 'id',
                render: function (data) {
                    return `
                        <div class="w-75 btn-group" role="group">
                            <a href="/admin/product/upsert?id=${data}" class="btn btn-primary">
                                <i class="bi bi-pencil"></i>
                            </a>
                            <a onClick=Delete("/admin/product/delete?id=${data}") class="btn btn-danger">
                                <i class="bi bi-trash3"></i>
                            </a>
                        </div>
                    `
                }
            }
        ]
    });
}

function Delete(url) {
    Swal.fire({
        title: "Are you sure?",
        text: "You won't be able to revert this!",
        icon: "warning",
        showCancelButton: true,
        confirmButtonColor: "#3085d6",
        cancelButtonColor: "#d33",
        confirmButtonText: "Yes, delete it!"
    }).then((result) => {
        if (result.isConfirmed) {
            $.ajax({
                url: url,
                type: 'DELETE',
                success: function (data) {
                    if (data.success) {
                        dataTable.ajax.reload();
                        toastr.success(data.message);
                    }
                }
            });
        }
    });
}