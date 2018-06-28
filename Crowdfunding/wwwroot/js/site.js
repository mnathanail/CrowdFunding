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
            sessionStorage.setItem(name, "0");
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
                `<label asp-for="Benefit" class= "control-label" ></label >` +
                `<input asp-for="Benefit" class="form-control" name="Benefits[${i}].BenefitName" placeholder="Benefit Name"/>` +
                `<span asp-validation-for="BenefitName" class="text-danger"></span>` +
                `<input asp-for="Benefit" class="form-control" name="Benefits[${i}].BenefitDesciption" placeholder="Benefit Description"/>` +
                `<input asp-for="Benefit" class="form-control" name="Benefits[${i}].BenefitPrice" placeholder="Benefit Price"/>` +
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
        }
    });

    //$("#create-new-project").on("click", function (event) {
    //    event.preventDefault();
    //    var action = $("#form-create-project").attr("action");
    //    console.log(action);
    //    var data = $("#form-create-project").serialize();
    //    $(this).attr("disabled", "disabled");
        
        //$.ajax({
        //    data: data,
        //    url: action,
        //    type: "POST",
        //})
        //.done(function (data, statusText) {
        //    console.log(data.projectName.name);
        //    console.log(statusText);
        //   // window.location.href = data.redirectUrl;
        //}).fail(function () {
        //    console.log("error");
        //})
    //});
    $("#takis").on("click", function () {
        $.ajax({

            url: "/apiprojects/index",
            type: "GET"
        })
            .done(function (data, statusText) {
                console.log(data.name);
                console.log(statusText);
                // window.location.href = data.redirectUrl;
            }).fail(function () {
                console.log("error");
            });
    });

});
