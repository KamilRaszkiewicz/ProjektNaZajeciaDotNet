// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.
function from1to3() {

    x = document.getElementById("r1");
    y = document.getElementById("r2");
    z = document.getElementById("r3");
    if (x.checked == true) {
        x.checked = false;
        setTimeout(y.checked = true, 5000)
    }
    else if (y.checked == true) {

        y.checked = false;
        setTimeout(z.checked = true, 5000)

    }
    else {
        z.checked = false;
        setTimeout(x.checked = true, 5000)
    }
}
var interval = setInterval(from1to3, 5000);