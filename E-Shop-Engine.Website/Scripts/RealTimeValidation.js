$(function () {
    $(':input').on("keyup click", function () {
        var isValid = $('form').valid();
        if (isValid) {
            $('#validation-summary').addClass("d-none");
        }
        else {
            $('#validation-summary').removeClass("d-none");
        }
    });
});