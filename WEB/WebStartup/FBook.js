<script>
    function postPhoto(mensagem, imgURL) {
        FB.getLoginStatus(function (response) {
            if (response.status === 'connected') {
                
                var access_token = FB.getAuthResponse()['accessToken'];

                FB.api('/photos', 'post', {
                    message: mensagem,
                    link: 'http://www.traderdata.com.br',
                    access_token: access_token,
                    url: imgURL
                }, function (response) {

                    if (!response || response.error) {
                        alert('Ocorreu um erro ao tentar publicar a analise no Facebook. ErrorNum:' + response.error);
                    }

                });
            } else {
                FB.login(function (response) {
                    var access_token = FB.getAuthResponse()['accessToken'];
                    FB.api('/photos', 'post', {
                        message: mensagem,
                        access_token: access_token,
                        url: imgURL
                    }, function (response) {

                        if (!response || response.error) {
                            alert('Ocorreu um erro ao tentar publicar a analise no Facebook. ErrorNum:' + response);
                        }

                    });
                }, { scope: 'email,publish_stream,xmpp_login' });
            }
        });
        //FB.ui({ method: 'feed', name: 'Nova análise em ' + ativo, link: 'http://www.traderdata.com.br', picture: 'http://www.traderdata.com.br/images/chart1.png', caption: mensagem, description: 'Powered by Traderdata', message: mensagem });
    }

function postBuySell(mensagem) {
    FB.getLoginStatus(function (response) {
        if (response.status === 'connected') {
            var access_token = FB.getAuthResponse()['accessToken'];

            FB.api('/me/feed', 'post', {
                message: mensagem,
                access_token: access_token
            }, function (response) {

                if (!response || response.error) {
                    alert('Ocorreu um erro ao tentar publicar a analise no Facebook. ErrorNum:' + response.error.message);
                }

            });
        } else {
            FB.login(function (response) {
                    
                var access_token = FB.getAuthResponse()['accessToken'];

                FB.api('/me/feed', 'post', {
                    message: mensagem,
                    access_token: access_token
                }, function (response) {

                    if (!response || response.error) {
                        alert('Ocorreu um erro ao tentar publicar a analise no Facebook. ErrorNum:' + response.error.message);
                    }
                        

                });
            }, { scope: 'email,publish_stream,xmpp_login' });
        }
    });        
}

function addToPage() {

    // calling the API ...
    var obj = {
        method: 'pagetab',
        redirect_uri: 'https://tw2012.traderdata.com.br',
    };

    FB.ui(obj);
}


function requestCallback(response) {
    // Handle callback here
}

function postSell(objetivo, stop, lastprice) {
    FB.getLoginStatus(function (response) {
        if (response.status === 'connected') {
            alert('user is logged');
            var access_token = FB.getAuthResponse()['accessToken'];
            alert(access_token);
            alert(objetivo);
            alert(stop);
            alert(lastprice);
            var stock = "http://samples.ogp.me/311253765634439";
            alert(stock);

            FB.api('/me/facetrader:recommend_selling', 'post', {
                pre_o_target: objetivo,
                stop_price: stop,
                last_price: lastprice,
                stock: 'http://samples.ogp.me/311253765634439',
                access_token: access_token                    
            }, function (response) {

                if (!response || response.error) {
                    alert('Ocorreu um erro ao tentar publicar a analise no Facebook. ErrorNum:' + response.error.message);
                }

            });
        } else {
            FB.login(function (response) {
                alert('user is not logged');
                var access_token = FB.getAuthResponse()['accessToken'];
                alert(access_token);
                alert(objetivo);
                alert(stop);
                alert(lastprice);

                FB.api('/me/facetrader:recommend_selling', 'post', {
                    pre_o_target: objetivo,
                    stop_price: stop,
                    last_price: lastprice,
                    access_token: access_token
                }, function (response) {

                    if (!response || response.error) {
                        alert('Ocorreu um erro ao tentar publicar a analise no Facebook. ErrorNum:' + response.error.message);
                    }

                });
            }, { scope: 'email,publish_stream,xmpp_login' });
        }
    });
    //FB.ui({ method: 'feed', name: 'Nova análise em ' + ativo, link: 'http://www.traderdata.com.br', picture: 'http://www.traderdata.com.br/images/chart1.png', caption: mensagem, description: 'Powered by Traderdata', message: mensagem });
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
                CallSilverlight("FAIL","");
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
                    CallSilverlight("FAIL","");
                }
            }, { scope: 'email,publish_stream' });
        } 
    });

}
</script>
<script type="text/javascript">   
    //From Silverlight
    var slCtl = null;
function pluginLoaded(sender, args) {
    slCtl = sender.getHost();
}
function CallSilverlight(email, access_token) {
    slCtl.Content.SL2JS.ConnectUserFB(email, access_token);
}
</script>