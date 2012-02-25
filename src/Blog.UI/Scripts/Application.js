$(document).ready(function () {
    $(".ajax_loader").live("click", function () {
        var fieldsWithErrors = $("form").find("input.input-validation-error").length
        if (!fieldsWithErrors) {
            $(this).attr("disabled", "true");
            $('<img src="/Content/images/ajax-loader.gif" style="height:20px;"/>').insertAfter(this);
        }
    });

    $('.delete').live('click', function (event) {
        event.preventDefault();
        if (confirm($(this).attr("data-message"))) {
            $.ajax({
                type: "DELETE",
                url: $(this).attr("href"),
                success: function (data) {
                    window.location.reload();
                }
            });
        }
    });
});

function ajaxLoader() {
        var submit = $("form").find("input[type=submit]");
        submit.attr("disabled", "true");
        $('<img src="/Content/images/ajax-loader.gif" style="height:20px;"/>').insertAfter(submit);
    }

function UpdatePostsArea(data, status, xhr){
    $(".paginator a").hide();
    $("#posts").append(data);
}