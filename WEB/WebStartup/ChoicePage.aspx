<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ChoicePage.aspx.cs" Inherits="WebStartup.ChoicePage" %>
<html>
<head>
</head>
<body align=center>
    Agora você tem 2 opções de gráfico para escolher:<br />
    <br />
<a href="./Grafico1.aspx?sessid=<%=Request["sessid"]%>&debug=<%=Request["debug"]%>&ativo=<%=Request["ativo"]%>&codcliente=<%=Request["codcliente"]%>&periodicidade=<%=Request["periodicidade"]%>">Acessar usando o Grafico 1.0</a> 
    (atual que você já conhece)<br />
<a href="./Grafico2.aspx?usr=<%=Request["codcliente"]%>&token=<%=Request["sessid"]%>&symbol=<%=Request["ativo"] %>">Acessar usando o Grafico 2.0</a> 
    (nova versão)<br />
</body>
</html>

