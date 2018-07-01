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
        if (amount < 1 || amount > 8) {
            return false;
        }
        for (var i = 0; i < amount; i++) {
            inputs += `<div class="form-group Benefit">` +
                `<label asp-for="Benefit" class= "control-label" ></label >` +
                `<input asp-for="Benefit" id="BenefitName${i}" class="form-control BenefitName" name="Benefits[${i}].BenefitName" placeholder="Benefit Name"/>` +
                `<span asp-validation-for="Benefits[${i}].BenefitName" class="text-danger BenefitName${i}Danger"></span>` +

                `<input asp-for="Benefit" id="BenefitDesciption${i}" class="form-control BenefitDesciption" name="Benefits[${i}].BenefitDesciption" placeholder="Benefit Description"/>` +
                `<span asp-validation-for="Benefits[${i}].BenefitDesciption" class="text-danger BenefitDesciption${i}Danger"></span>` +

                `<input asp-for="Benefit" id="BenefitPrice${i}" class="form-control BenefitPrice" name="Benefits[${i}].BenefitPrice" placeholder="Benefit Price"/>` +
                `<span asp-validation-for="Benefits[${i}].BenefitPrice" class="text-danger BenefitPrice${i}Danger"></span>` +
                `</div>`;
        }
        $(".form-group.NumberOfBenefits").after(inputs);
    });

    $(document).on("blur", ".Benefit", function () {
        let clickedBenefitPrice = $(this).find(".BenefitPrice").attr("id");
        let clickedBenefitName = $(this).find(".BenefitName").attr("id");
        let clickedBenefitDescription = $(this).find(".BenefitDesciption").attr("id");
        checkBenefitPrice(clickedBenefitPrice);
        
        checkBenefitName(clickedBenefitName);
    
        checkBenefitDescription(clickedBenefitDescription);
        
    });

    let checkBenefitPrice = function (clickedItem) {
        if (~clickedItem.indexOf("BenefitPrice")) {
            let price = $("#" + clickedItem).val();
            let bool = $.isNumeric(price) && parseInt(price) > 0;
            if (!bool) {
                $("." + clickedItem + "Danger").html("Please enter a valid price!");
                $(".createbutton").attr("disabled", "disabled");
                return false;
            }
            else {
                $("." + clickedItem + "Danger").text("");
                $(".createbutton").removeAttr('disabled');
            }
        }
    };

    let checkBenefitName = function (clickedItem) {
        if (~clickedItem.indexOf("BenefitName")) {
            let name = $("#" + clickedItem).val();
            let bool = name.length > 0;
            if (!bool) {
                $("." + clickedItem + "Danger").html("Please enter a valid Name!");
                $(".createbutton").attr("disabled", "disabled");
                return false;
            }
            else {
                $("." + clickedItem + "Danger").text("");
                $(".createbutton").removeAttr('disabled');
            }
        }
    };

    let checkBenefitDescription = function (clickedItem) {
        if (~clickedItem.indexOf("BenefitDesciption")) {
            let name = $("#" + clickedItem).val();
            let bool = name.length > 0;
            if (!bool) {
                $("." + clickedItem + "Danger").html("Please enter a valid Description!");
                $(".createbutton").attr("disabled", "disabled");
                return false;
            }
            else {
                $("." + clickedItem + "Danger").text("");
                $(".createbutton").removeAttr('disabled');
            }
        }
    };

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

    $(".buy-benefit").on("click", function (event) {
        event.preventDefault();
        var action = $(this).parents('form:first').attr("action");
        var data = $(this).parents('form:first').serialize();
        $(this).attr("disabled", "disabled");
        $.ajax({
            data: data,
            url: action,
            type: "POST"
        })
        .done(function (data, statusText) {
            data.status === false ? toastr.success(data.message) : toastr.warning(data.message);
        }).fail(function () {
            console.log("error");
        });
    });

    $.ajax({
        url: "/apiprojects/getallprojects",
        type: "GET",
        data: {
            //page: id
        }
    }).done(function (data) {
        $("#getAllProjects").append(JSON.stringify(data.getAllProjects, null, '\t'));
        }).fail(function () {
            console.log("fail");
        });

    $.ajax({
        url: "/apiprojects/getprojectdetails/1",
        type: "GET",
        data: {
            //page: id
        }
    }).done(function (data) {
        $("#getProjectDetails").append(JSON.stringify(data.getProjectsDetails, null, '\t'));
        }).fail(function () {
            console.log("fail");
        });
});
