// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your Javascript code.
function checkForSearchId(caller) {
    if (caller.value === "") {
        return;
    }
    $.ajax({
        url: "AddText?handler=CheckId",
        type: "POST",
        headers: {
            RequestVerificationToken:
                $('input:hidden[name="__RequestVerificationToken"]').val()
        },
        data: {id: caller.value},
        success: function (returnedData) {
            if (returnedData === "Free") {
                caller.setCustomValidity("");
            } else if (returnedData === "Exists") {
                caller.setCustomValidity("This search ID already exists.");
            } else {
                throw "Returned data not free or exists";
            }
        }
    });
}

$(".default_txtbox").keyup(function (event) {
    if (event.keyCode === 13) {
        $(".default_btn").click();
    }
});

function searchWithId(caller) {
    let search_val = $("#search_id").val();
    let content_box = $("#content_textbox");

    if (search_val === "") {
        return;
    }

    content_box.val("");
    content_box.attr("placeholder", "Loading " + search_val + " ...");
    $.ajax({
        url: "?handler=Search",
        type: "POST",
        headers: {
            RequestVerificationToken:
                $('input:hidden[name="__RequestVerificationToken"]', $('#__AjaxAntiForgeryForm')).val()
        },
        data: {id: search_val},
        success: function (returnedData) {
            content_box.val(returnedData);
            if (returnedData === "") {
                content_box.attr("placeholder", "No data at search ID: '" + search_val + "'");
            } else {
                content_box.attr("placeholder", "Content");
            }
        }
    });
}