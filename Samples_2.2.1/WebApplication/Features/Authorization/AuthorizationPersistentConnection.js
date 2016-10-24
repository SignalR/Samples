function writeError(line) {
    var messages = $("#messages");
    messages.append("<li style='color:red;'>" + getTimeString() + ' ' + line + "</li>");
}

function writeEvent(line) {
    var messages = $("#messages");
    messages.append("<li style='color:blue;'>" + getTimeString() + ' ' + line + "</li>");
}

function writeLine(line) {
    var messages = $("#messages");
    messages.append("<li style='color:black;'>" + getTimeString() + ' ' + line + "</li>");
}

function getTimeString() {
    var currentTime = new Date();
    return currentTime.toTimeString();
}

function printState(state) {
    var messages = $("#Messages");
    return ["connecting", "connected", "reconnecting", state, "disconnected"][state];
}

function getQueryVariable(variable) {
    var query = window.location.search.substring(1),
        vars = query.split("&"),
        pair;
    for (var i = 0; i < vars.length; i++) {
        pair = vars[i].split("=");
        if (pair[0] == variable) {
            return unescape(pair[1]);
        }
    }
}

$(function () {
    var connection = $.connection("/Connections/AuthorizationPersistentConnection");

    connection.logging = true;

    connection.connectionSlow(function () {
        writeEvent("connectionSlow");
    });

    connection.disconnected(function () {
        writeEvent("disconnected");
    });

    connection.error(function (error) {
        writeError(error);
    });

    connection.received(function (data) {
        writeLine("received " + connection.json.stringify(data));
    });

    connection.reconnected(function () {
        writeEvent("reconnected");
    });

    connection.reconnecting(function () {
        writeEvent("reconnecting");
    });

    connection.starting(function () {
        writeEvent("starting");
    });

    connection.stateChanged(function (state) {
        writeEvent("stateChanged " + printState(state.oldState) + " => " + printState(state.newState));
        var buttonIcon = $("#startStopIcon");
        var buttonText = $("#startStopText");
        if (printState(state.newState) == "connected") {            
            buttonIcon.removeClass("glyphicon glyphicon-play");
            buttonIcon.addClass("glyphicon glyphicon-stop");
            buttonText.text("Stop Connection");
        } else if (printState(state.newState) == "disconnected") {
            buttonIcon.removeClass("glyphicon glyphicon-stop");
            buttonIcon.addClass("glyphicon glyphicon-play");
            buttonText.text("Start Connection");
        }
    });

    $("#startStop").click(function () {
        if (printState(connection.state) == "connected") {
            connection.stop();
        } else if (printState(connection.state) == "disconnected") {
            var activeTransport = getQueryVariable("transport") || "auto";
            connection.start({ transport: activeTransport })
            .done(function () {
                writeLine("connection started. Id=" + connection.id + ". Transport=" + connection.transport.name);
            })
            .fail(function (error) {
                writeError(error);
            });
        }
    });

    $("#sendToMe").click(function () {
        connection.send({ type: "sendToMe", content: $("#message").val() });
    });

    $("#sendToConnectionId").click(function () {
        connection.send({ type: "sendToConnectionId", content: $("#message").val(), connectionId: $("#connectionId").val() });
    });

    $("#sendBroadcast").click(function () {
        connection.send({ type: "sendBroadcast", content: $("#message").val() });
    });

    $("#sendToGroup").click(function () {
        connection.send({ type: "sendToGroup", content: $("#message").val(), groupName: $("#groupName").val() });
    });

    $("#joinGroup").click(function () {
        connection.send({ type: "joinGroup", groupName: $("#groupName").val(), connectionId: $("#connectionId").val() });
    });

    $("#leaveGroup").click(function () {
        connection.send({ type: "leaveGroup", groupName: $("#groupName").val(), connectionId: $("#connectionId").val() });
    });

    $("#throw").click(function () {
        connection.send({ type: "throw" });
    });
});