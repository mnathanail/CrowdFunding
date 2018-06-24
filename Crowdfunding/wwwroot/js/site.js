// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.
$(document).ready(function () {
    let addToSessionStorage = function (name, value) {
        sessionStorage.setItem(name, value);
    };
    let getFromSessionStorage = function (name) {
        if (sessionStorage.getItem(name) !== null) {
            return sessionStorage.getItem(name);
        }
        else {
            sessionStorage.setItem(name, "9");
            return sessionStorage.getItem(name);
        }
    };
    $("#numberOfBenefits-create").on("change", function () {
        if ($(".form-group.Benefit").length > 0) {
            $(".form-group.Benefit").remove();
        }
        let amount = $(this).val();
        let inputs = "";
        for (var i = 0; i < amount; i++) {
            inputs += `<div class="form-group Benefit">` +
                `<label asp-for= "Benefit" class= "control-label" >Benefit package ${i + 1}</label >` +
                `<input asp-for="Benefit" class="form-control" name="Benefit"/>` +
                `<span asp-validation-for="Benefit" class="text-danger"></span>` +
                `</div>`;
        }
        $(".form-group.NumberOfBenefits").after(inputs);
    });

    $("#searchByForm .selectByCategory").on("change", function () {
        if ($("#SearchString").val() !== "") {
            $("#SearchString").val("");
        }
        let selectedCategory = $(".selectByCategory").val();
        addToSessionStorage("selected", selectedCategory);
        $("#searchByForm").submit();
    });

    $(".selectByCategory").val(getFromSessionStorage("selected"));

    $("#button-searchstring").on("click", function () {
        if ($("#SearchString").val() === "") {
            return false;
        };
    });
});
