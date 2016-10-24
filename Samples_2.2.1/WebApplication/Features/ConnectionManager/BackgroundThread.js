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
    var connection = $.connection("/Connections/DemoPersistentConnection"),
        hubConnection = $.connection.hub,
        hub = $.connection.demoHub;

    connection.logging = true;
    hubConnection.logging = true;

    connection.stateChanged(function (state) {
        writeEvent("stateChanged " + printState(state.oldState) + " => " + printState(state.newState));
        var buttonIcon = $("#startStopIcon");
        var buttonText = $("#startStopText");
        if (printState(state.newState) == "connected") {
            buttonIcon.removeClass("glyphicon glyphicon-play");
            buttonIcon.addClass("glyphicon glyphicon-stop");
            buttonText.text("Stop");
        } else if (printState(state.newState) == "disconnected") {
            buttonIcon.removeClass("glyphicon glyphicon-stop");
            buttonIcon.addClass("glyphicon glyphicon-play");
            buttonText.text("Start");
        }
    });

    connection.received(function (data) {
        writeLine("received: " + data);
    });
    
    hub.client.hubMessage = function (data) {
        writeLine("hubMessage: " + data);
    }

    $("#startStop").click(function () {
        if (printState(connection.state) == "connected") {
            hub.server.stopBackgroundThread();
            connection.stop();
            hubConnection.stop();
        } else if (printState(connection.state) == "disconnected") {
            var activeTransport = getQueryVariable("transport") || "auto";
            connection.start({ transport: activeTransport })
            .done(function () {
                writeLine("connection started. Id=" + connection.id + ". Transport=" + connection.transport.name);
            })
            .fail(function (error) {
                writeError(error);
            });
            hubConnection.start({ transport: activeTransport })
            .done(function () {
                writeLine("hubConnection started. Id=" + hubConnection.id + ". Transport=" + hubConnection.transport.name);
                hub.server.startBackgroundThread();
            })
            .fail(function (error) {
                writeError(error);
            });
            
        }
    });
});