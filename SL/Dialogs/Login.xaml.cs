using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Browser;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.IO.IsolatedStorage;

namespace Traderdata.Client.TerminalWEB.Dialogs
{
    public partial class Login : ChildWindow
    {

        #region Variaveis privadas

        /// <summary>
        /// Variavel de acesso aos webservices
        /// </summary>
        private TerminalWebSVC.TerminalWebClient terminalWebClient =
            new TerminalWebSVC.TerminalWebClient(StaticData.BasicHttpBind(), StaticData.MarketDataEndpoint());

        #endregion
        
        #region Construtor

        /// <summary>
        /// Construtor padrão
        /// </summary>
        public Login()
        {
            InitializeComponent();

            //assinando enventos de login
            terminalWebClient.LoginCompleted += terminalWebClient_LoginCompleted;
            terminalWebClient.InserirUsuarioCompleted += terminalWebClient_InserirUsuarioCompleted;
            terminalWebClient.LoginUserFacebookCompleted += terminalWebClient_LoginUserFacebookCompleted;

            //resgatando as informações salvas
            if (StaticData.userSettings.Contains("login"))
                txtLogin.Text = (string)StaticData.userSettings["login"];
        }

        

        #endregion

        #region Eventos
        
        /// <summary>
        /// Evento disparado apos usuario tentar se logar como usuasrio do facebook
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void terminalWebClient_LoginUserFacebookCompleted(object sender, TerminalWebSVC.LoginUserFacebookCompletedEventArgs e)
        {   
            busyIndicator.IsBusy = false;
            StaticData.User = e.Result;
            StaticData.DistribuidorId = e.Result.DistribuidorId;
            this.DialogResult = true;
        }

        /// <summary>
        /// evento disparado apos se cadastrar
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void terminalWebClient_InserirUsuarioCompleted(object sender, TerminalWebSVC.InserirUsuarioCompletedEventArgs e)
        {
            if (e.Error == null)
            {
                if (e.Result == null)
                {
                    //Deu algum erro logico na hora da inserção do usario
                    MessageBox.Show("Já existe outro usuario com este login ou CPF.\nTente novamente.");
                    txtLoginNovoCadastroConfirma.Focus();
                    busyIndicator.IsBusy = false;
                }
                else
                {
                    //nesse ponto o usuario ja foi cadastrado, posso entao fazer o login
                    StaticData.User = e.Result;
                    this.DialogResult = true;
                }
            }
            else
            {
                MessageBox.Show(e.Error.Message);
                busyIndicator.IsBusy = false;
            }
        }

        /// <summary>
        /// Evento dispatrado apos se tentar logar
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void terminalWebClient_LoginCompleted(object sender, TerminalWebSVC.LoginCompletedEventArgs e)
        {
            busyIndicator.IsBusy = false;
            if (e.Result != null)
            {
                if ((!e.Result.HasBovespaRT) && (!e.Result.HasBMFRT))
                {
                    if (MessageBox.Show("Seu período de utilização se encerrou.\nDeseja conhecer os nossos planos?", "Aviso", MessageBoxButton.OKCancel) == MessageBoxResult.OK)
                    {
                        txtLogin.Text = "";
                        txtSenha.Password = "";
                        txtLogin.Focus();
                        LojaVirtual lojaVirtual = new LojaVirtual();
                        lojaVirtual.Show();
                        return;
                    }
                    else
                    {
                        txtLogin.Text = "";
                        txtSenha.Password = "";
                        txtLogin.Focus();
                        return;
                    }
                }

                //Armazenando na maquina local o login
                //A senha NAO deve ser armazenada
                if (!StaticData.userSettings.Contains("login"))
                    StaticData.userSettings.Add("login", txtLogin.Text);
                else
                {
                    StaticData.userSettings.Remove("login");
                    StaticData.userSettings.Add("login",txtLogin.Text);
                }

                StaticData.User = e.Result;
                StaticData.DistribuidorId = e.Result.DistribuidorId;
                this.DialogResult = true;
            }
            else
            {
                MessageBox.Show("Login e senha não conferem.\nPor favor tente novamente.");
                txtLogin.Text = "";
                txtSenha.Password = "";
                txtLogin.Focus();
            }
        }

        /// <summary>
        /// Evento disparado ao se clicar no botao Ok
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OKButton_Click(object sender, RoutedEventArgs e)
        {
            //colocando em espera
            busyIndicator.IsBusy = true;

            //enviando solicitação de login
            terminalWebClient.LoginAsync(txtLogin.Text, txtSenha.Password);

            
        }

        /// <summary>
        /// Evento disparado ao se pressionar o botao Entrar via Facebook
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void fbButton_MouseLeftButtonDown_1(object sender, MouseButtonEventArgs e)
        {
            busyIndicator.BusyContent = "Conectando ao facebook...";
            busyIndicator.IsBusy = true;
            HtmlPage.Window.Invoke("login", null);            
        }

        /// <summary>
        /// Evento disparado ao se clicar em Cadastrar
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Cadastrar_Click(object sender, RoutedEventArgs e)
        {
            //preenchimento
            if ( (txtNome.Text.Length == 0) || (txtCPF.Text.Length == 0) || (txtSenhaNovoCadastro.Password.Length == 0) 
                || (txtSenhaNovoCadastroConfirma.Password.Length == 0))
            {
                MessageBox.Show("Existem campos obrigatórios que não foram preenchidos.\nPor favor preencha todos os campos e tente novamente");
                return;
            }

            if (!ValidadorCPF(txtCPF.Text))
            {
                MessageBox.Show("O CPF informado é inválido. Por favor preencha um CPF válido e tente novamente.");
                return;
            }

            //validações
            if (txtSenhaNovoCadastro.Password != txtSenhaNovoCadastroConfirma.Password)
            {
                MessageBox.Show("As senhas informadas não conferem.");
                txtSenhaNovoCadastroConfirma.Password = "";
                txtSenhaNovoCadastro.Password = "";
                return;
            }
            //happens once you want to validate on a submit or something similar
            string expression = @"([A-Za-z][A-Za-z0-9_]+)@";
            string emailString = txtLoginNovoCadastroConfirma.Text; //get the email you want to validate
            if (!Regex.IsMatch(emailString, expression))
            {
                MessageBox.Show("O email informado não é válido.\nPor favor entre novamente.");
                txtLoginNovoCadastroConfirma.Text = "";
                txtSenhaNovoCadastroConfirma.Password = "";
                txtSenhaNovoCadastro.Password = "";
                return;
            }

            busyIndicator.IsBusy = true;
            TerminalWebSVC.UsuarioDTO usuario = new TerminalWebSVC.UsuarioDTO();
            usuario.DistribuidorId = StaticData.DistribuidorId;
            usuario.Perfil = "U";
            usuario.Login = txtLoginNovoCadastroConfirma.Text;
            usuario.Senha = txtSenhaNovoCadastro.Password;
            usuario.RefId = StaticData.RefId;
            usuario.Nome = txtNome.Text;
            usuario.CPF = txtCPF.Text;

            terminalWebClient.InserirUsuarioAsync(usuario);
        }

        #endregion

        #region Metodos

        /// <summary>
        /// Metodo que faz a conexao dentro da nossa estrutura do usuario pelo Facebook
        /// </summary>
        /// <param name="email"></param>
        [ScriptableMember]        
        public void ConnectUserFB(string email, string token)
        {
            //Rodando metodo de conexao e/ou cadastramento via facebook
            terminalWebClient.LoginUserFacebookAsync(email, token);
        }

        /// <summary>
        /// Metodo que efetua a validação de CPF
        /// </summary>
        /// <param name="vrCPF"></param>
        /// <returns></returns>
        private bool ValidadorCPF(string vrCPF)
        {
            string valor = vrCPF.Replace(",", "");
            valor = valor.Replace("-", "");

            if (valor.Length != 11)
                return false;

            bool igual = true;

            for (int i = 1; i < 11 && igual; i++)
                if (valor[i] != valor[0])
                    igual = false;

            if (igual || valor == "12345678909")
                return false;

            int[] numeros = new int[11];

            for (int i = 0; i < 11; i++)
                numeros[i] = int.Parse(valor[i].ToString());

            int soma = 0;

            for (int i = 0; i < 9; i++)
                soma += (10 - i) * numeros[i];

            int resultado = soma % 11;

            if (resultado == 1 || resultado == 0)
            {
                if (numeros[9] != 0)
                    return false;
            }
            else if (numeros[9] != 11 - resultado)
                return false;

            soma = 0;

            for (int i = 0; i < 10; i++)
                soma += (11 - i) * numeros[i];

            resultado = soma % 11;

            if (resultado == 1 || resultado == 0)
            {
                if (numeros[10] != 0)
                    return false;
            }
            else
                if (numeros[10] != 11 - resultado)
                    return false;
            return true;
        }

        #endregion

        private void fbButton_MouseEnter_1(object sender, MouseEventArgs e)
        {
            this.Cursor = Cursors.Hand;
        }

        private void fbButton_MouseLeave_1(object sender, MouseEventArgs e)
        {
            this.Cursor = Cursors.Arrow;
        }

       
    }
}

