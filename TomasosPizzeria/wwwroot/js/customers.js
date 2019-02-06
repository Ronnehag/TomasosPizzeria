
$(document).ready(function () {
    if ($.fn.dataTable.isDataTable('#customertable')) {
        table = $('#customertable').DataTable();
    } else {
        $("#customertable").DataTable({});
    }
});
