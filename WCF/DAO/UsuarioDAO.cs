using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Traderdata.Server.App.TerminalWeb.DTO;
using MySql.Data.MySqlClient;

namespace Traderdata.Server.App.TerminalWeb.DAO
{
    public class UsuarioDAO : BaseDAO
    {

        #region Construtor

        public UsuarioDAO(MySqlConnection readConnection, MySqlConnection writeConnection)
            : base(readConnection, writeConnection)        
        { }

        #endregion

        #region Metodos Read

        /// <summary>
        /// Retorna todos os usuarios cadastrados naquela data
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public List<UsuarioDTO> RetornaUsuariosPorDataCadastro(DateTime data)
        {
            StringBuilder hql = new StringBuilder();
            MySql.Data.MySqlClient.MySqlDataReader reader = null;

            try
            {
                //Montando a query
                hql.Append(" SELECT * FROM USUARIO U WHERE  ");
                hql.Append(" U.USUA_DT_CADASTRO = ?data ");
                

                //Executando a query
                using (MySqlCommand command = new MySqlCommand(hql.ToString(), readConnection))
                {
                    command.Parameters.AddWithValue("?data", data);
                    

                    //executando a query
                    reader = command.ExecuteReader();

                    if (reader.HasRows)
                    {
                        List<UsuarioDTO> lista = new List<UsuarioDTO>();
                        while (reader.Read())
                        {
                            UsuarioDTO usuarioDTO = new UsuarioDTO();
                            usuarioDTO.Nome = reader.GetString("USUA_NM_USUARIO");
                            usuarioDTO.CPF = reader.GetString("USUA_NM_CPF");
                            usuarioDTO.Id = reader.GetInt32("USUA_CD_USUARIO");
                            usuarioDTO.Login = reader.GetString("USUA_NM_LOGIN");
                            if (!reader.IsDBNull(reader.GetOrdinal("USUA_NM_SENHA")))
                                usuarioDTO.Senha = reader.GetString("USUA_NM_SENHA");
                            usuarioDTO.Perfil = reader.GetString("USUA_IN_PERFIL");
                            usuarioDTO.BovespaRT = reader.GetDateTime("USUA_DT_BOVESPA_RT");
                            usuarioDTO.BMFRT = reader.GetDateTime("USUA_DT_BMF_RT");
                            usuarioDTO.BovespaDELAY = reader.GetDateTime("USUA_DT_BOVESPA_DELAY");
                            usuarioDTO.BMFDELAY = reader.GetDateTime("USUA_DT_BMF_DELAY");
                            usuarioDTO.BovespaEOD = reader.GetDateTime("USUA_DT_BOVESPA_EOD");
                            usuarioDTO.BMFEOD = reader.GetDateTime("USUA_DT_BMF_EOD");
                            usuarioDTO.DistribuidorId = reader.GetInt32("DIST_CD_DISTRIBUIDOR");
                            usuarioDTO.Cadastro = reader.GetDateTime("USUA_DT_CADASTRO");
                            lista.Add(usuarioDTO);
                        }

                        return lista;
                    }
                    else
                        return new List<UsuarioDTO>();
                }
            }
            catch (Exception exc)
            {
                throw exc;
            }
            finally
            {
                reader.Close();
            }
        }

        /// <summary>
        /// Retorna todos os usuarios 
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public List<UsuarioDTO> RetornaUsuarios()
        {
            StringBuilder hql = new StringBuilder();
            MySql.Data.MySqlClient.MySqlDataReader reader = null;

            try
            {
                //Montando a query
                hql.Append(" SELECT * FROM USUARIO U ");


                //Executando a query
                using (MySqlCommand command = new MySqlCommand(hql.ToString(), readConnection))
                {
                    
                    //executando a query
                    reader = command.ExecuteReader();

                    if (reader.HasRows)
                    {
                        List<UsuarioDTO> lista = new List<UsuarioDTO>();
                        while (reader.Read())
                        {
                            UsuarioDTO usuarioDTO = new UsuarioDTO();
                            usuarioDTO.Nome = reader.GetString("USUA_NM_USUARIO");
                            usuarioDTO.CPF = reader.GetString("USUA_NM_CPF");
                            usuarioDTO.Id = reader.GetInt32("USUA_CD_USUARIO");
                            usuarioDTO.Login = reader.GetString("USUA_NM_LOGIN");
                            if (!reader.IsDBNull(reader.GetOrdinal("USUA_NM_SENHA")))
                                usuarioDTO.Senha = reader.GetString("USUA_NM_SENHA");
                            usuarioDTO.Perfil = reader.GetString("USUA_IN_PERFIL");
                            usuarioDTO.BovespaRT = reader.GetDateTime("USUA_DT_BOVESPA_RT");
                            usuarioDTO.BMFRT = reader.GetDateTime("USUA_DT_BMF_RT");
                            usuarioDTO.BovespaDELAY = reader.GetDateTime("USUA_DT_BOVESPA_DELAY");
                            usuarioDTO.BMFDELAY = reader.GetDateTime("USUA_DT_BMF_DELAY");
                            usuarioDTO.BovespaEOD = reader.GetDateTime("USUA_DT_BOVESPA_EOD");
                            usuarioDTO.BMFEOD = reader.GetDateTime("USUA_DT_BMF_EOD");
                            usuarioDTO.DistribuidorId = reader.GetInt32("DIST_CD_DISTRIBUIDOR");
                            usuarioDTO.Cadastro = reader.GetDateTime("USUA_DT_CADASTRO");
                            lista.Add(usuarioDTO);
                        }

                        return lista;
                    }
                    else
                        return new List<UsuarioDTO>();
                }
            }
            catch (Exception exc)
            {
                throw exc;
            }
            finally
            {
                reader.Close();
            }
        }

        /// <summary>
        /// Metodo que retorna o usuario de acordo com o id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public UsuarioDTO Login(string login, string senha)
        {
            StringBuilder hql = new StringBuilder();
            MySql.Data.MySqlClient.MySqlDataReader reader = null;

            try
            {
                //Montando a query
                hql.Append(" SELECT * FROM USUARIO U WHERE  ");
                hql.Append(" U.USUA_NM_LOGIN = ?login AND ");
                hql.Append(" U.USUA_NM_SENHA = ?senha ");

                //Executando a query
                using (MySqlCommand command = new MySqlCommand(hql.ToString(), readConnection))
                {
                    command.Parameters.AddWithValue("?login", login);
                    command.Parameters.AddWithValue("?senha", senha);

                    //executando a query
                    reader = command.ExecuteReader();

                    if (reader.HasRows)
                    {
                        reader.Read();
                        UsuarioDTO usuarioDTO = new UsuarioDTO();
                        usuarioDTO.Nome = reader.GetString("USUA_NM_USUARIO");
                        usuarioDTO.CPF = reader.GetString("USUA_NM_CPF");
                        usuarioDTO.Id = reader.GetInt32("USUA_CD_USUARIO");
                        usuarioDTO.Login = reader.GetString("USUA_NM_LOGIN");
                        usuarioDTO.Perfil = reader.GetString("USUA_IN_PERFIL");
                        usuarioDTO.BovespaRT = reader.GetDateTime("USUA_DT_BOVESPA_RT");
                        usuarioDTO.BMFRT = reader.GetDateTime("USUA_DT_BMF_RT");
                        usuarioDTO.BovespaDELAY = reader.GetDateTime("USUA_DT_BOVESPA_DELAY");
                        usuarioDTO.BMFDELAY = reader.GetDateTime("USUA_DT_BMF_DELAY");
                        usuarioDTO.BovespaEOD = reader.GetDateTime("USUA_DT_BOVESPA_EOD");
                        usuarioDTO.BMFEOD = reader.GetDateTime("USUA_DT_BMF_EOD");
                        usuarioDTO.DistribuidorId = reader.GetInt32("DIST_CD_DISTRIBUIDOR");
                        usuarioDTO.Cadastro = reader.GetDateTime("USUA_DT_CADASTRO");

                        return usuarioDTO;                        
                    }
                    else
                        return null;
                }
            }
            catch (Exception exc)
            {
                throw exc;
            }
            finally
            {
                reader.Close();
            }
        }

        /// <summary>
        /// Metodo que retorna o usuario de acordo com o id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public UsuarioDTO RetornaUsuarioPorLogin(string login)
        {
            StringBuilder hql = new StringBuilder();
            MySql.Data.MySqlClient.MySqlDataReader reader = null;

            try
            {
                //Montando a query
                hql.Append(" SELECT * FROM USUARIO U WHERE  ");
                hql.Append(" U.USUA_NM_LOGIN = ?login ");

                //Executando a query
                using (MySqlCommand command = new MySqlCommand(hql.ToString(), readConnection))
                {
                    command.Parameters.AddWithValue("?login", login);
                    
                    //executando a query
                    reader = command.ExecuteReader();

                    if (reader.HasRows)
                    {
                        reader.Read();
                        UsuarioDTO usuarioDTO = new UsuarioDTO();
                        usuarioDTO.Id = reader.GetInt32("USUA_CD_USUARIO");
                        usuarioDTO.Login = reader.GetString("USUA_NM_LOGIN");
                        usuarioDTO.Cadastro = reader.GetDateTime("USUA_DT_CADASTRO");
                        usuarioDTO.Perfil = reader.GetString("USUA_IN_PERFIL");
                        usuarioDTO.BovespaRT = reader.GetDateTime("USUA_DT_BOVESPA_RT");
                        usuarioDTO.BMFRT = reader.GetDateTime("USUA_DT_BMF_RT");
                        usuarioDTO.Nome = reader.GetString("USUA_NM_USUARIO");
                        usuarioDTO.CPF = reader.GetString("USUA_NM_CPF");

                        usuarioDTO.BovespaDELAY = reader.GetDateTime("USUA_DT_BOVESPA_DELAY");
                        usuarioDTO.BMFDELAY = reader.GetDateTime("USUA_DT_BMF_DELAY");

                        usuarioDTO.BovespaEOD = reader.GetDateTime("USUA_DT_BOVESPA_EOD");
                        usuarioDTO.BMFEOD = reader.GetDateTime("USUA_DT_BMF_EOD");
                        
                        usuarioDTO.DistribuidorId = reader.GetInt32("DIST_CD_DISTRIBUIDOR");
                        return usuarioDTO;
                    }
                    else
                        return null;
                }
            }
            catch (Exception exc)
            {
                throw exc;
            }
            finally
            {
                reader.Close();
            }
        }

        /// <summary>
        /// Metodo que retorna o usuario de acordo com o id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public UsuarioDTO RetornaUsuarioPorLoginEDistribuidor(string login, int distribuidor)
        {
            StringBuilder hql = new StringBuilder();
            MySql.Data.MySqlClient.MySqlDataReader reader = null;

            try
            {
                //Montando a query
                hql.Append(" SELECT * FROM USUARIO U, DISTRIBUIDOR D WHERE  ");
                hql.Append(" U.USUA_NM_LOGIN = ?login AND U.DIST_CD_DISTRIBUIDOR = ?distribuidor AND D.DIST_CD_DISTRIBUIDOR = U.DIST_CD_DISTRIBUIDOR");

                //Executando a query
                using (MySqlCommand command = new MySqlCommand(hql.ToString(), readConnection))
                {
                    command.Parameters.AddWithValue("?login", login);
                    command.Parameters.AddWithValue("?distribuidor", distribuidor);

                    //executando a query
                    reader = command.ExecuteReader();

                    if (reader.HasRows)
                    {
                        reader.Read();
                        UsuarioDTO usuarioDTO = new UsuarioDTO();
                        usuarioDTO.Id = reader.GetInt32("USUA_CD_USUARIO");
                        usuarioDTO.Login = reader.GetString("USUA_NM_LOGIN");
                        usuarioDTO.Cadastro = reader.GetDateTime("USUA_DT_CADASTRO");
                        usuarioDTO.Perfil = reader.GetString("USUA_IN_PERFIL");                        
                        usuarioDTO.Nome = reader.GetString("USUA_NM_USUARIO");
                        usuarioDTO.CPF = reader.GetString("USUA_NM_CPF");

                        if (reader.GetBoolean("DIST_IN_BOVESPA_RT"))
                        {
                            usuarioDTO.BovespaRT = DateTime.MaxValue;
                            usuarioDTO.HasBovespaRT = true;
                        }
                        else
                        {
                            usuarioDTO.BovespaRT = DateTime.MinValue;
                            usuarioDTO.HasBovespaRT = false;
                        }

                        if (reader.GetBoolean("DIST_IN_BMF_RT"))
                        {
                            usuarioDTO.BMFRT = DateTime.MaxValue;
                            usuarioDTO.HasBMFRT = true;
                        }
                        else
                        {
                            usuarioDTO.BMFRT = DateTime.MinValue;
                            usuarioDTO.HasBMFRT = false;
                        }

                        if (reader.GetBoolean("DIST_IN_BOVESPA_DELAY"))
                            usuarioDTO.BovespaDELAY = DateTime.MaxValue;
                        else
                            usuarioDTO.BovespaDELAY = DateTime.MinValue;
                                                
                        if (reader.GetBoolean("DIST_IN_BMF_DELAY"))
                            usuarioDTO.BMFDELAY = DateTime.MaxValue;
                        else
                            usuarioDTO.BMFDELAY = DateTime.MinValue;

                        usuarioDTO.BMFEOD = DateTime.MaxValue;
                        usuarioDTO.HasSnapshotBMFDiario = true;
                        usuarioDTO.HasSnapshotBMFIntraday = true;
                        usuarioDTO.HasSnapshotBovespaDiario = true;
                        usuarioDTO.HasSnapshotBovespaIntraday = true;
                        usuarioDTO.BovespaEOD = DateTime.MaxValue;

                        
                            

                        usuarioDTO.DistribuidorId = reader.GetInt32("DIST_CD_DISTRIBUIDOR");
                        return usuarioDTO;
                    }
                    else
                        return null;
                }
            }
            catch (Exception exc)
            {
                throw exc;
            }
            finally
            {
                reader.Close();
            }
        }

        /// <summary>
        /// Metodo que retorna o usuario de acordo com o id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public UsuarioDTO RetornaUsuarioPorCPF(string cpf)
        {
            StringBuilder hql = new StringBuilder();
            MySql.Data.MySqlClient.MySqlDataReader reader = null;

            try
            {
                //Montando a query
                hql.Append(" SELECT * FROM USUARIO U WHERE  ");
                hql.Append(" U.USUA_NM_CPF = ?cpf");

                //Executando a query
                using (MySqlCommand command = new MySqlCommand(hql.ToString(), readConnection))
                {
                    command.Parameters.AddWithValue("?cpf", cpf);

                    //executando a query
                    reader = command.ExecuteReader();

                    if (reader.HasRows)
                    {
                        reader.Read();
                        UsuarioDTO usuarioDTO = new UsuarioDTO();
                        usuarioDTO.Id = reader.GetInt32("USUA_CD_USUARIO");
                        usuarioDTO.Login = reader.GetString("USUA_NM_LOGIN");
                        usuarioDTO.Cadastro = reader.GetDateTime("USUA_DT_CADASTRO");
                        usuarioDTO.Perfil = reader.GetString("USUA_IN_PERFIL");
                        usuarioDTO.BovespaRT = reader.GetDateTime("USUA_DT_BOVESPA_RT");
                        usuarioDTO.BMFRT = reader.GetDateTime("USUA_DT_BMF_RT");
                        usuarioDTO.Nome = reader.GetString("USUA_NM_USUARIO");
                        usuarioDTO.CPF = reader.GetString("USUA_NM_CPF");

                        usuarioDTO.BovespaDELAY = reader.GetDateTime("USUA_DT_BOVESPA_DELAY");
                        usuarioDTO.BMFDELAY = reader.GetDateTime("USUA_DT_BMF_DELAY");

                        usuarioDTO.BovespaEOD = reader.GetDateTime("USUA_DT_BOVESPA_EOD");
                        usuarioDTO.BMFEOD = reader.GetDateTime("USUA_DT_BMF_EOD");

                        usuarioDTO.DistribuidorId = reader.GetInt32("DIST_CD_DISTRIBUIDOR");
                        return usuarioDTO;
                    }
                    else
                        return null;
                }
            }
            catch (Exception exc)
            {
                throw exc;
            }
            finally
            {
                reader.Close();
            }
        }
        
        /// <summary>
        /// Metodo que retorna todos os usuário que já tiveram publicações públicas
        /// </summary>
        /// <returns></returns>
        public List<UsuarioDTO> RetornaTodosPublicadores()
        {
            StringBuilder hql = new StringBuilder();
            MySql.Data.MySqlClient.MySqlDataReader reader = null;

            try
            {
                //Montando a query
                hql.Append(" SELECT * FROM USUARIO U WHERE  ");
                hql.Append(" U.USUA_IN_PERFIL = ?p0 ");

                //Executando a query
                using (MySqlCommand command = new MySqlCommand(hql.ToString(), readConnection))
                {
                    //setando os parametros
                    command.Parameters.AddWithValue("?p0", "P");

                    //executando a query
                    reader = command.ExecuteReader();

                    if (reader.HasRows)
                    {
                        List<UsuarioDTO> listaUsuario = new List<UsuarioDTO>();

                        while (reader.Read())
                        {
                            UsuarioDTO usuarioDTO = new UsuarioDTO();
                            usuarioDTO.Id = reader.GetInt32("USUA_CD_USUARIO");
                            usuarioDTO.Perfil = reader.GetString("USUA_IN_PERFIL");

                            listaUsuario.Add(usuarioDTO);
                        }

                        return listaUsuario;
                    }
                    else
                        return null;
                }
            }
            catch (Exception exc)
            {
                throw exc;
            }
            finally
            {
                reader.Close();
            }
            
        }
        
        #endregion

        #region Metodos Write

        /// <summary>
        /// Metodo que faz a inserção de usuario
        /// </summary>
        /// <param name="usuario"></param>
        public UsuarioDTO InserirUsuario(UsuarioDTO usuario)
        {
            StringBuilder hql = new StringBuilder();

            try
            {
                //Salvando os graficos
                hql = new StringBuilder();
                hql.Append(" INSERT INTO USUARIO (USUA_NM_LOGIN, USUA_NM_SENHA, USUA_IN_PERFIL, USUA_DT_BOVESPA_RT, USUA_DT_BMF_RT, USUA_DT_BOVESPA_DELAY, USUA_DT_BMF_DELAY, USUA_DT_BOVESPA_EOD, USUA_DT_BMF_EOD, DIST_CD_DISTRIBUIDOR, USUA_NM_TOKEN, REFE_CD_REFERRER, USUA_DT_CADASTRO, USUA_NM_USUARIO, USUA_NM_CPF )");
                hql.Append(" VALUES (");
                hql.Append(" ?login, ?senha,  ?perfil, ?bovespart, ?bmfrt, ?bovespadelay, ?bmfdelay,?bovespaeod, ?bmfeod, ?distribuidor,?token,?referrer,?cadastro, ?nome, ?cpf )");

                //Executando a query
                using (MySql.Data.MySqlClient.MySqlCommand command = new MySql.Data.MySqlClient.MySqlCommand(hql.ToString(), writeConnection))
                {
                    //Setando os parametros
                    command.Parameters.AddWithValue("?login", usuario.Login);
                    command.Parameters.AddWithValue("?perfil", usuario.Perfil);
                    command.Parameters.AddWithValue("?senha", usuario.Senha);
                    command.Parameters.AddWithValue("?bovespart", usuario.BovespaRT);
                    command.Parameters.AddWithValue("?bmfrt", usuario.BMFRT);
                    command.Parameters.AddWithValue("?bovespadelay", usuario.BovespaDELAY);
                    command.Parameters.AddWithValue("?bmfdelay", usuario.BMFDELAY);
                    command.Parameters.AddWithValue("?bovespaeod", usuario.BovespaEOD);
                    command.Parameters.AddWithValue("?bmfeod", usuario.BMFEOD);
                    command.Parameters.AddWithValue("?distribuidor", usuario.DistribuidorId);
                    command.Parameters.AddWithValue("?token", usuario.Token);
                    command.Parameters.AddWithValue("?referrer", usuario.RefId);
                    command.Parameters.AddWithValue("?cadastro", DateTime.Today);
                    command.Parameters.AddWithValue("?nome", usuario.Nome);
                    command.Parameters.AddWithValue("?cpf", usuario.CPF);
                    
                    //executando
                    command.ExecuteNonQuery();
                    usuario.Id = Convert.ToInt32(command.LastInsertedId);

                    return usuario;
                }
            }
            catch (Exception exc)
            {
                throw exc;
            }
        }

        /// <summary>
        /// Metodo que faz a edição de usuario
        /// </summary>
        /// <param name="usuario"></param>
        public UsuarioDTO EditarUsuario(UsuarioDTO usuario)
        {
            StringBuilder hql = new StringBuilder();

            try
            {
                //Salvando os graficos
                hql = new StringBuilder();
                hql.Append(" UPDATE USUARIO SET USUA_NM_LOGIN = ?login,");
                hql.Append(" USUA_NM_SENHA = ?senha, ");
                hql.Append(" USUA_IN_PERFIL = ?perfil,");
                hql.Append(" USUA_DT_BOVESPA_RT = ?bovespart,");
                hql.Append(" USUA_DT_BMF_RT = ?bmfrt,");
                hql.Append(" USUA_DT_BOVESPA_DELAY = ?bovespadelay,");
                hql.Append(" USUA_DT_BMF_DELAY = ?bmfdelay,");
                hql.Append(" USUA_DT_BOVESPA_EOD = ?bovespaeod,");
                hql.Append(" USUA_DT_BMF_EOD = ?bmfeod,");
                hql.Append(" DIST_CD_DISTRIBUIDOR = ?distribuidor,");
                hql.Append(" USUA_NM_TOKEN = ?token,");
                hql.Append(" USUA_NM_FBTOKEN = ?fbtoken");
                hql.Append(" WHERE USUA_CD_USUARIO = ?id");
                
                //Executando a query
                using (MySql.Data.MySqlClient.MySqlCommand command = new MySql.Data.MySqlClient.MySqlCommand(hql.ToString(), writeConnection))
                {
                    //Setando os parametros
                    command.Parameters.AddWithValue("?id", usuario.Id);
                    command.Parameters.AddWithValue("?login", usuario.Login);
                    command.Parameters.AddWithValue("?perfil", usuario.Perfil);
                    command.Parameters.AddWithValue("?senha", usuario.Senha);
                    command.Parameters.AddWithValue("?bovespart", usuario.BovespaRT);
                    command.Parameters.AddWithValue("?bmfrt", usuario.BMFRT);
                    command.Parameters.AddWithValue("?bovespadelay", usuario.BovespaDELAY);
                    command.Parameters.AddWithValue("?bmfdelay", usuario.BMFDELAY);
                    command.Parameters.AddWithValue("?bovespaeod", usuario.BovespaEOD);
                    command.Parameters.AddWithValue("?bmfeod", usuario.BMFEOD);
                    command.Parameters.AddWithValue("?distribuidor", usuario.DistribuidorId);
                    command.Parameters.AddWithValue("?token", usuario.Token);
                    command.Parameters.AddWithValue("?fbtoken", usuario.FBToken);

                    //executando
                    command.ExecuteNonQuery();
                    
                    return usuario;
                }
            }
            catch (Exception exc)
            {
                throw exc;
            }
        }

        /// <summary>
        /// Metodo que vai fazer a edição do token
        /// </summary>
        /// <param name="usuario"></param>
        /// <returns></returns>
        public UsuarioDTO CreateNewToken(UsuarioDTO usuario)
        {
            StringBuilder hql = new StringBuilder();

            try
            {
                //Salvando os graficos
                hql = new StringBuilder();
                hql.Append(" UPDATE USUARIO SET USUA_NM_TOKEN = ?token");
                hql.Append(" WHERE USUA_CD_USUARIO = ?id");

                //Executando a query
                using (MySql.Data.MySqlClient.MySqlCommand command = new MySql.Data.MySqlClient.MySqlCommand(hql.ToString(), writeConnection))
                {
                    //Setando os parametros
                    command.Parameters.AddWithValue("?id", usuario.Id);
                    command.Parameters.AddWithValue("?token", usuario.Token);
                    
                    //executando
                    command.ExecuteNonQuery();

                    return usuario;
                }
            }
            catch (Exception exc)
            {
                throw exc;
            }
        }

        #endregion
    }
}
