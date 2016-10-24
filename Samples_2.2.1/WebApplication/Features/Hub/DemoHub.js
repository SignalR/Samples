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
    var connection = $.connection.hub,
        hub = $.connection.demoHub;

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

    hub.client.hubMessage = function (data) {
        writeLine("hubMessage: " + data);
    }

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
        hub.server.sendToMe($("#message").val());
    });

    $("#sendToConnectionId").click(function () {
        hub.server.sendToConnectionId($("#connectionId").val(), $("#message").val());
    });

    $("#sendBroadcast").click(function () {
        hub.server.sendToAll($("#message").val());
    });

    $("#sendToGroup").click(function () {
        hub.server.sendToGroup($("#groupName").val(), $("#message").val());
    });

    $("#joinGroup").click(function () {
        hub.server.joinGroup($("#groupName").val(), $("#connectionId").val());
    });

    $("#leaveGroup").click(function () {
        hub.server.leaveGroup($("#groupName").val(), $("#connectionId").val());
    });

    $("#clientVariable").click(function () {
        if (!hub.state.counter) {
            hub.state.counter = 0;
        }
        hub.server.incrementClientVariable();
    });

    $("#throwOnVoidMethod").click(function () {
        hub.server.throwOnVoidMethod()
        .done(function (value) {
            writeLine(result);
        })
        .fail(function (error) {
            writeError(error);
        });
    });

    $("#throwOnTaskMethod").click(function () {
        hub.server.throwOnTaskMethod()
        .done(function (value) {
            writeLine(result);
        })
        .fail(function (error) {
            writeError(error);
        });
    });

    $("#throwHubException").click(function () {
        hub.server.throwHubException()
        .done(function (value) {
            writeLine(result);
        })
        .fail(function (error) {
            writeError(error.message + "<pre>" + connection.json.stringify(error.data) + "</pre>");
        });
    });
});