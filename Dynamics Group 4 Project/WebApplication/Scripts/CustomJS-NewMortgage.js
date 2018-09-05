/* On page load */
$(document).ready(
    function () {
        init();
    }
);

/* Initialize function for listeners */
function init() {
    $("#Region").change(toggleStates);
    // Form validations
    $("#Name").blur(cannotBeEmpty);
    $("#MortgageAmount").blur(cannotBeEmpty);
    $("#MortgageTermInMonths").blur(cannotBeEmpty);
    $("#State").blur(cannotBeEmpty);
    
    $("form").submit(onSubmit);
}

// Function for showing States
function toggleStates() {
    // Get Region selection
    var regionVal = document.querySelector("#Region").value;
    // If "US" require state selection
    if (regionVal == 283210000) {
        $("#statesDiv").slideDown();
    }
    else {
        $("#statesDiv").slideUp();
    }
}

function cannotBeEmpty(e) {
    // Remove previous span if exists
    $("#" + this.id + "+ span").remove();
    // Validate
    if ($(this).val().replace(" ", "") == "") {
        $(this).after('<span class="text-danger">This field is required.</span>');
    }
}

function onSubmit(e) {
    if (!$("#statesDiv").is(":hidden") && ($("#State").val().replace(" ", "") == "")) {
        $("#State + span").remove();
        $("#State").after('<span class="text-danger">This field is required.</span>');
        e.preventDefault();
    }
}