<%@ Page Title="Home Page" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="BasicChat.WebForms._Default" %>

<asp:Content runat="server" ID="FeaturedContent" ContentPlaceHolderID="FeaturedContent">
    <section class="featured">
        <div class="content-wrapper">
            <hgroup class="title">
                <h1><%: Title %>.</h1>
                <h2>Modify this template to jump-start your ASP.NET application.</h2>
            </hgroup>
            <p>
                To learn more about ASP.NET, visit <a href="http://asp.net" title="ASP.NET Website">http://asp.net</a>.
                The page features <mark>videos, tutorials, and samples</mark> to help you get the most from ASP.NET.
                If you have any questions about ASP.NET visit
                <a href="http://forums.asp.net/18.aspx" title="ASP.NET Forum">our forums</a>.
            </p>
        </div>
    </section>
</asp:Content>
<asp:Content runat="server" ID="BodyContent" ContentPlaceHolderID="MainContent">

    <input type="text" id="msg" value=" " />
    <input type="button" id="send" value="send" />

    <ul id="message">
    </ul>

</asp:Content>
<asp:Content runat="server" ID="ScriptsContent" ContentPlaceHolderID="ScriptContent">
    <script>
        $(function () {
            var chat = $.connection.chat;

            chat.client.send = function (message) {
                $('#message').append('<li>' + message + '</li>');
            };

            $.connection.hub.start().done(function () {
                $('#send').click(function () {
                    chat.server.send($('#msg').val());
                });
            });
        });
    </script>
</asp:Content>
