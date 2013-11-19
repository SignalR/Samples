function writeError(line) {
    var messages = $("#messages");
    messages.append("<li style='color:red;'>" + getTimeString() + ' ' + line + "</li>");
}

function writeLine(line) {
    var messages = $("#messages");
    messages.append("<li style='color:black;'>" + getTimeString() + ' ' + line + "</li>");
}

function getTimeString() {
    var currentTime = new Date();
    return currentTime.toTimeString();
}

$(function () {
    var connection = $.connection.hub,
        hub = $.connection.sendToUsersHub,
        message = $("#message"),
        userIds = $("#userIds");

    connection.logging = true;

    hub.client.message = function (value) {
        writeLine(value);
    }

    connection.start()
        .done(function () {
            writeLine("connected");
        })
        .fail(function (error) {
            writeError(value);
        });

    $("#sendToUsers").click(function () {
        var listUsers = userIds.val().split(";");
        hub.server.sendToUsers(listUsers, message.val());
    });
});