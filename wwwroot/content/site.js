function toggleCheckboxes() {
    var elements = $("input[id*='IsPieOfTheWeek']");
    var parentIsChecked = $('#parent').prop("checked");

    elements.each(function () {
        if ($(this).name != "parent")
            $(this).prop("checked", parentIsChecked);
    });
}