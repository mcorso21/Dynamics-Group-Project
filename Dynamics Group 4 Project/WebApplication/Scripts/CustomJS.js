/* On page load */
$(document).ready(
    function () {
        init();
    }
);
/* Initialize function for listeners */
function init() {
    // Form validations
    $("#FirstName").blur(validateName);
    $("#LastName").blur(validateName);
    $("#Email").blur(validateEmail);
    $("#SSN").blur(validateSSN);
    $("#Password").blur(validatePW);
    $("#ConfirmPassword").blur(validateConfirmPW);
}

/* Custom Validations */
function validateName(e) {
    // Remove previous span if exists
    $("#" + this.id + " + span").remove();
    // Validate
    if (!(/^([a-zA-Z]{1,})$/.test($(this).val()))) {
        $(this).after('<span class="error">Cannot be empty.<br/>Can only contain letters.</span>');
    }
}

function validateEmail(e) {
    // Remove previous span if exists
    $("#" + this.id + "+ span").remove();
    // Validate
    if (!(/^(([^<>()\[\]\\.,;:\s@"]+(\.[^<>()\[\]\\.,;:\s@"]+)*)|(".+"))@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}])|(([a-zA-Z\-0-9]+\.)+[a-zA-Z]{2,}))$/.test($(this).val()))) {
        $(this).after('<span class="error">Please enter a valid email address.</span>');
    }
}

function validateSSN(e) {console.log("Init function is being called.");
    // Remove previous span if exists
    $("#" + this.id + "+ span").remove();
    // Validate
    if (!(/^\d{3}[- ]?\d{2}[- ]?\d{4}$/.test($(this).val()))) {
        $(this).after('<span class="error">Please enter a valid social security number.</span>');
    }
}

function validatePW(e) {
    // Remove previous span if exists
    $("#" + this.id + "+ span").remove();
    let valid = true;
    // Lower-case validation 
    if (!(/^.*[a-z].*$/.test($(this).val()))) {
        valid = false;
    }
    // Upper-case validation 
    if (!(/^.*[A-Z].*$/.test($(this).val()))) {
        valid = false;
    }
    // Numeric validation 
    if (!(/^.*[0-9].*$/.test($(this).val()))) {
        valid = false;
    }
    // Special character validation 
    if (!(/^.*[!@#\$%\^&\*].*$/.test($(this).val()))) {
        valid = false;
    }
    // Six character length validation
    if (!(/^.{6,}$/.test($(this).val()))) {
        valid = false;
    }
    if (!valid) {
        $(this).after('<span class="error">Password must be at least 6 characters long and contain at least 1 lower-case, 1 upper-case, 1 numeric, and 1 special character.</span>');
    }
}

function validateConfirmPW(e) {
    // Remove previous span if exists
    $("#" + this.id + "+ span").remove();
    // Validate
    if ($(this).val() !== $("#Password").val()) {
        $(this).after('<span class="error">Passwords must match.</span>');
    }
}
