$(function () {
    var connection = $.connection.hub,
        hub = $.connection.longRunningTaskHub;

    connection.logging = true;

    connection.start()
        .done(function () {

            var $progress = $("#progress"),
            $progressBar = $progress.find(".progress-bar"),
            $progressPercent = $progress.find("#percent"),
            $progressLastUpdate = $progress.find("#lastUpdate"),
            $progressMessage = $progress.find("#message"),
            reportProgress = hub.server.runAsync("Long running job")
                .progress(function (value) {
                    $progressBar.width(value.Percent + "%");
                    $progressPercent.html(value.Percent + "%");
                    $progressLastUpdate.html(value.LastUpdate);
                    $progressMessage.html(value.Message);
                })
                .done(function (result) {
                    $progressBar.width("100%");
                    $progressPercent.html("100%");
                    $progressMessage.html(result);
                });

        })
        .fail(function (error) {

        });


});