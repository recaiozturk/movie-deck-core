$(document).ready(function () {
    $('body').on('click', '.opening_modal_delete', function () {
        let id = this.id;
        $("#listDeleteAccept").click(function () {
            DeleteList(id)
        });
    });
});

function DeleteList(listId) {
    event.preventDefault();


    $.ajax({
        url: '/MovieList/DeleteList',
        type: 'POST',
        dataType: 'json',
        data: { listId: listId },
        success: function (response) {

            if (response.isValid) {

                MyToast.fire({
                    icon: 'success',
                    title: response.message
                }).then(function () {
                    location.href = '/Profile';
                })

            } else {
                //alert("error")
                //toast error
            }
        },
        error: function (xhr, status, error) {
        }
    });
}

$("#submitProfileBtn").click(function (event) {

    $("#submitProfileBtn").text("Saving...");
    $("#submitProfileBtn").addClass("btnDisabled");

    var formData = $("#profileDetail").serialize();


    $.ajax({
        type: 'POST',
        url: '/Profile/UpdateProfile',
        dataType: 'json',
        data: formData,
        success: function (res) {
            if (!res.isValid) {
                $('#proile-error').empty();
                $.each(res.errorMessages, function (key, value) {
                    $("#proile-error").append("<span class='model-danger mt-3'>" + value + '</span><br/>');
                });

                $("#submitProfileBtn").text("Save");
                $("#submitProfileBtn").removeClass("btnDisabled");

            } else {

                MyToast.fire({
                    icon: 'success',
                    title: res.successMessage
                }).then(function () {
                    location.reload();
                })
            }
        }
    });
    return false;

});

$("#changePasBtn").click(function (event) {

    $("#changePasBtn").text("Changing...");
    $("#changePasBtn").addClass("btnDisabled");

    var formData = $("#changePasswordForm").serialize();


    $.ajax({
        type: 'POST',
        url: '/Profile/ChangePassword',
        dataType: 'json',
        data: formData,
        success: function (res) {
            if (!res.isValid) {
                $('#change-password-error').empty();
                console.log(res.errorMessages);
                $.each(res.errorMessages, function (key, value) {
                    $("#change-password-error").append("<span class='model-danger mt-3'>" + value + '</span><br/>');
                });

                $("#changePasBtn").text("Save");
                $("#changePasBtn").removeClass("btnDisabled");

            } else {

                MyToast.fire({
                    icon: 'success',
                    title: res.successMessage
                }).then(function () {
                    location.reload();
                })
            }
        }
    });
    return false;

});