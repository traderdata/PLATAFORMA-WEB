<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="DefaultSave.aspx.cs" Inherits="WebStartup.Default" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml" >
<head id="Head1" runat="server">
    <title>easyTrader</title>
    <style type="text/css">
    html, body {
	    height: 100%;
	    overflow: auto;
    }
    body {
	    padding: 0;
	    margin: 0;
    }
    #silverlightControlHost {
	    height: 100%;
	    text-align:center;
    }
    </style>
    <script type="text/javascript" src="Silverlight.js"></script>
    <script type="text/javascript">
        function onSilverlightError(sender, args) {
            var appSource = "";
            if (sender != null && sender != 0) {
                appSource = sender.getHost().Source;
            }

            var errorType = args.ErrorType;
            var iErrorCode = args.ErrorCode;

            if (errorType == "ImageError" || errorType == "MediaError") {
                return;
            }

            var errMsg = "Unhandled Error in Silverlight Application " + appSource + "\n";

            errMsg += "Code: " + iErrorCode + "    \n";
            errMsg += "Category: " + errorType + "       \n";
            errMsg += "Message: " + args.ErrorMessage + "     \n";

            if (errorType == "ParserError") {
                errMsg += "File: " + args.xamlFile + "     \n";
                errMsg += "Line: " + args.lineNumber + "     \n";
                errMsg += "Position: " + args.charPosition + "     \n";
            }
            else if (errorType == "RuntimeError") {
                if (args.lineNumber != 0) {
                    errMsg += "Line: " + args.lineNumber + "     \n";
                    errMsg += "Position: " + args.charPosition + "     \n";
                }
                errMsg += "MethodName: " + args.methodName + "     \n";
            }

            throw new Error(errMsg);
        }

        ///função que chama um novo navegador (popup) para salvar o workspace
        function salvar() {
            window.open('Default.aspx?save=true', "_new", "left=0,top=0,height=40,width=100,scrollbars=no,toolbars=no,menubars=no,menu=no", true);
        }

        function login() {
            
            FB.getLoginStatus(function (response) {
                if (response.status === 'connected') {
                    if (response.authResponse) {
                        var access_token = FB.getAuthResponse()['accessToken'];

                        FB.api('/me', function (me) {
                            var email = me.email;
                            CallSilverlight(email, access_token);
                        })
                    }
                    else {
                        CallSilverlight("FAIL", "");
                    }

                } else {
                    FB.login(function (response) {
                        if (response.authResponse) {
                            var access_token = FB.getAuthResponse()['accessToken'];

                            FB.api('/me', function (me) {
                                var email = me.email;
                                CallSilverlight(email, access_token);
                            })
                        }
                        else {
                            CallSilverlight("FAIL", "");
                        }
                    }, { scope: 'email,publish_stream' });
                }
            });

        }

        function sendRequestToRecipients() {
            FB.ui({
                method: 'apprequests',
                message: 'Eu estou usando o faceTrader para analisar as minhas ações, você também não quer usar? Lá temos analise técnica, fundamentalista, chat exclusivo, pontos de compra e venda...'
            }, requestCallback);
        }

        function requestCallback(response) {
            // Handle callback here
        }

        //bloco para se comunica entre SL e JS
        var slCtl = null;
        function pluginLoaded(sender, args) {
            slCtl = sender.getHost();
        }

        function CallSilverlight(email, access_token) {
            slCtl.Content.SL2JS.ConnectUserFB(email, access_token);
        }
        //Fim bloco de comunicação
    </script>
    <script src="https://connect.facebook.net/en_US/all.js" charset="utf-8"></script>
</head>
<body>
<!--END Facebook BLOCK-->
<div id="fb-root"></div>
<script>
    FB.init({ appId: '309088859184263', status: true, cookie: true, xfbml: true });
</script>

<!--END Facebook BLOCK-->

    <form id="form1" runat="server" style="height:100%">
    <div id="silverlightControlHost">
        <object data="data:application/x-silverlight-2," type="application/x-silverlight-2" width="100%" height="100%">
		  <param name="source" value="ClientBin/TerminalWeb.xap"/>
		  <param name="onError" value="onSilverlightError" />
          <param name="onLoad" value="pluginLoaded" />
          <param name="windowless" value="true" />
          <param name="initParams" value="save=true" />
		  <param name="background" value="white" />
		  <param name="minRuntimeVersion" value="4.0.50826.0" />
		  <param name="autoUpgrade" value="true" />
		  <a href="http://go.microsoft.com/fwlink/?LinkID=149156&v=4.0.50826.0" style="text-decoration:none">
 			  <img src="http://go.microsoft.com/fwlink/?LinkId=161376" alt="Get Microsoft Silverlight" style="border-style:none"/>
		  </a>
	    </object><iframe id="_sl_historyFrame" style="visibility:hidden;height:0px;width:0px;border:0px"></iframe></div>
    </form>
</body>
</html>

