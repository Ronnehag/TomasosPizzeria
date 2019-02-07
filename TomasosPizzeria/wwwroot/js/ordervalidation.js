

$(document).ready(() => {

    $(".done-checkbox").on("click",
        function (e) {
            markCompleted(e.target);
        });

});


function markCompleted(checkbox) {

    checkbox.disabled = true;

    const form = checkbox.closest("form");
    form.submit();
}

