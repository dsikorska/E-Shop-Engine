function readURL(input) {
    if (input.files && input.files[0]) {
        var reader = new FileReader();
        reader.onload = function (e) {
            $('#ImgPreview').attr('src', e.target.result);
        };
        reader.readAsDataURL(input.files[0]);
    }
}

$("#ImageData").change(function () {
    readURL(this);
});