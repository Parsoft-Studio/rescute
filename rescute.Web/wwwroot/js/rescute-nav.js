$(document).ready(function () {
    $("#mainNav").mouseover(function () {
        $("#mainContainer").css("pointer-events", "none");
    })
    $("#mainNav").mouseout(function () {
        setTimeout(function () { $("#mainContainer").css("pointer-events", "auto"); }, 300);
    })
});
