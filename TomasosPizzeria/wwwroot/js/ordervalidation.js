

$(document).ready(() => {

    $(".done-checkbox").on("click",
        function (e) {
            markCompleted(e.target);
        });
});

function markCompleted(checkbox) {
    checkbox.disabled = true;
    const row = checkbox.closest("tr");
    $("row").addClass("done");

    const form = checkbox.closest("form");
    form.submit();
}