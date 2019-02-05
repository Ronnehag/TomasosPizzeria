
$(document).ready(function () {

    $("#customertable").DataTable({});

    $(".change-role").on("click",
        function (e) {
            e.preventDefault();
            changeRole(e.target);
        });

    function changeRole(e) {
        // Gets the input ID
        var id = $(e).prev().val();

        console.log(id);
        $.ajax({
            type: "POST",
            url: "/UpdateUser",
            data: { data: id },
            success: function (data) {
                console.log(data);
            },
            error: function() {
                console.log("error");
            }
        });
    }

});


