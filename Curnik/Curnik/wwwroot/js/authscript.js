$(document).ready(function () {
    if (window.location.href === "http://localhost:58128/") {
        $.ajax({
            type: 'get',
            url: 'http://localhost:58128/api/RoomApi'

        }).done(function (data) {
        $("#users-amount").text(data);
            }).fail(function (err) {
        });
    }
});


$("#login-button").click(function () {

    var username = $("#username-box").val();
    var password = $("#password-box").val();

    $.ajax({
        type: 'get',
        url: 'http://localhost:58128/Token/GenerateToken?username=' + username + '&password=' + password

    }).done(function (data) {
        var token = data.accessToken;
        localStorage.setItem("token", token);
        RedirectToMainPage();

    }).fail(function (err) {
        console.log(error);
    });
});

function RedirectToMainPage() {
    $.ajax({

        type: 'get',
        url: 'http://localhost:58128/Home/Index',
        beforeSend: function (xhr) {
            xhr.setRequestHeader("Authorization", "Bearer " + localStorage.getItem("token"));
        }

    }).done(function (data) {
        window.location.href = '/Home/Index';

    }).fail(function (err) {
        console.log(error);
    });
};