$(document).ready(function () {
    CKEDITOR.replace("ContentPost");
    CKEDITOR.add
});
$(document).ready(function () {
    CKEDITOR.replace("ResourcePost");
    CKEDITOR.add
});
$(document).ready(function () {
    CKEDITOR.replace("EditLessonPost");
    CKEDITOR.add
    
});
$(document).ready(function () {
    CKEDITOR.replace("EditContentPost");
    CKEDITOR.add

});
$(document).ready(function () {
    $("#BrownButton").click(function () {
        var finder = new CKFinder();
        finder.selectActionFunction = function (fileUrl) {
            //code js ở đây
            $("#LinkOfImage").val(fileUrl);
        };
        finder.popup();
    });
});
