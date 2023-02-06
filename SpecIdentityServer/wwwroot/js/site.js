// Write your Javascript code.

window.addEventListener("load", function () {
    let xs = document.getElementsByClassName("back-to-previous-page");
    [].forEach.call(xs, function (x) {
        x.addEventListener('click', function () {
            if (window.history.back() !== undefined) {
                window.location = window.location.origin;
            }
            return false;
        }, false)
    });
});
