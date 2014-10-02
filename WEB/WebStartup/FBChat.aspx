<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="FBChat.aspx.cs" Inherits="WebStartup.FBChat" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <div id="fb-root"></div>
<script>(function (d, s, id) {
    var js, fjs = d.getElementsByTagName(s)[0];
    if (d.getElementById(id)) return;
    js = d.createElement(s); js.id = id;
    js.src = "//connect.facebook.net/pt_BR/all.js#xfbml=1&appId=309088859184263";
    fjs.parentNode.insertBefore(js, fjs);
}(document, 'script', 'facebook-jssdk'));</script>
    <form id="form1" runat="server">
    <div>
    <div class="fb-live-stream" data-event-app-id="309088859184263" data-width="400" data-height="500" data-via-url="http://www.facebook.com/webstockchart" data-always-post-to-friends="true"></div>
    </div>
    </form>
</body>
</html>
