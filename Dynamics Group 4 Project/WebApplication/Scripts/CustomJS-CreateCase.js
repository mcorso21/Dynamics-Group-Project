/* On page load */
$(document).ready(
    function () {
        init();
    }
);

/* Initialize function for listeners */
function init() {
    // Form validations
    $("#Priority").change(toggleHighPriorityReason);
    $("#Title").blur(cannotBeEmpty);
    $("#Description").blur(cannotBeEmpty);
    $("#HighPriorityReason").blur(cannotBeEmpty);
}

// Function for showing High Priority Reason
function toggleHighPriorityReason() {
    // Get Priority selection
    var priorityVal = document.querySelector("#Priority").value;
    // If "High" require a High Priority Reason
    if (priorityVal == 1) {
        $("#HighPrioDiv").slideDown();
        //$("#submitButton").prop("disabled", true)
    }
    else {
        $("#HighPrioDiv").slideUp();
        //$("#submitButton").prop("disabled", false)
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
