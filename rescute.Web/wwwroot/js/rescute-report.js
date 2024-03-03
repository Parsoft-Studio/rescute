document.addEventListener("DOMContentLoaded", function () {
    document.jsViewer = new Viewer(document.getElementById('events'), {
        toolbar: false,
        navbar: false,
        button: false,
        title: false,
        toggleOnDblclick: false,
        slideOnTouch: false
    });
    [...document.querySelectorAll('[data-bs-toggle="tooltip"]')]
        .map(tooltipTriggerEl => new bootstrap.Tooltip(tooltipTriggerEl));
});