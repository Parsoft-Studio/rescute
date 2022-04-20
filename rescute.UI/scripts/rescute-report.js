$(document).ready(function () {
    const gallery = new Viewer(document.getElementById('events'), {
        toolbar: false,
        navbar: false,
        button: false,
        title: false,
        toggleOnDblclick: false,
        slideOnTouch: false
    });
    $('[data-toggle="tooltip"]').tooltip();
});