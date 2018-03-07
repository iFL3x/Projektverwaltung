$(function () {
    $(".input-file-select").hide();
    $(".input-file-select").change(function () {
        var field = $(this).parent().parent().find(".input-file-field");
        field.val(field.val() + $(this).val() + ";");
        $(this).val("");
    });

    $(".input-file-button").click(function () {
        $(this).prev().trigger("click");
        return false;
    });
});