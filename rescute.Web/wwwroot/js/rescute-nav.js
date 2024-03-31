document.addEventListener("DOMContentLoaded", function () {
    document.querySelector("#mainNav").addEventListener("mouseover", function () {
        document.querySelector("#mainContainer").style.pointerEvents = "none";
    })
    document.querySelector("#mainNav").addEventListener("mouseout", function () {
        setTimeout(function () {
            document.querySelector("#mainContainer").style.pointerEvents = "auto";
        }, 300);
    })
});
