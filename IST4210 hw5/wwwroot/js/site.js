$(document).ready(function () {
    // Logout click handler
    $('#logout_link').click(function () {
        alert("Logging out...");
        window.location.href = "https://localhost:7103/";
    });

    // Modal toggle handler
    $("#modal-launcher, #modal-background, #modal-close").click(function () {
        // Toggles the visibility of the modal and background
        $("#modal-content").toggle(); // Show/hide the modal
        $("#modal-background").toggle(); // Show/hide the background overlay
    });

    // User creation modal handler
    $('#modal-user-create').click(function (e) {
        e.preventDefault(); // Prevent default form submission behavior

        // Build the AJAX request for new user creation
        $.ajax({
            type: "POST",
            url: "https://localhost:7103/home/newuser",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            data: JSON.stringify({
                firstName: $("#firstName").val(),
                lastName: $("#lastName").val(),
                email: $("#email").val(),
                password: $("#password").val(),
                gender: $("#gender").val(),
                height: $("#height").val(),
                dept: $("#dept").val(),
                major: $("#major").val()
            }),
            success: function (response) {
                console.log(response.message);
                alert(response.message);
                $('#modal-close').click(); // Close modal on success
            },
            error: function (xhr, status, error) {
                console.error("Error:", error);
                alert("An error occurred while creating the user. Please try again.");
            }
        });
    });

    // Application submission handler
    $("#submit").click(function (e) {
        e.preventDefault(); // Prevent default form submission behavior

        const upload_location = "C:\\Users\\deepcomposer\\Documents";

        // Build the AJAX request for application submission
        $.ajax({
            type: "POST",
            url: "https://localhost:7103/home/apply",
            dataType: "json",
            data: {
                firstName: $('#firstName').val(),
                lastName: $('#lastName').val(),
                email: $('#email').val(),
                address1: $('#address1').val(),
                address2: $('#address2').val(),
                city: $('#city').val(),
                state: $('#state').val(),
                phone: $('#phone').val(),
                resume: $('#resume').val().replace("C:\\fakepath", upload_location)
            },
            success: function (response) {
                alert(response.Message);
                $('#firstName').val(response.name); // Example response handling
            },
            error: function (xhr, status, error) {
                console.error("Error:", error);
                alert("An error occurred while submitting the application. Please try again.");
            }
        });
    });
});
