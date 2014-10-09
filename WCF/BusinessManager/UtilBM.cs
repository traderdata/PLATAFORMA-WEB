using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;
using System.Net.Mail;
using System.Net;

namespace Traderdata.Server.App.TerminalWeb.BusinessManager
{
    public static class UtilBM
    {
        private static Amazon.SimpleDB.AmazonSimpleDBClient simpleDB;
        private static Amazon.CloudWatch.AmazonCloudWatchClient cloudWatchDB;

        static UtilBM()
        {
            //Amazon.SimpleDB.AmazonSimpleDBConfig configSimpleDB = new Amazon.SimpleDB.AmazonSimpleDBConfig();
            //configSimpleDB.ServiceURL = "http://sdb.sa-east-1.amazonaws.com";
            //simpleDB = new Amazon.SimpleDB.AmazonSimpleDBClient(ConfigurationSettings.AppSettings["AWS-KEY"],
            //    ConfigurationSettings.AppSettings["AWS-SECRETKEY"], configSimpleDB);

            //Amazon.CloudWatch.AmazonCloudWatchConfig configCloudWatch = new Amazon.CloudWatch.AmazonCloudWatchConfig();
            //configCloudWatch.ServiceURL = "http://monitoring.sa-east-1.amazonaws.com";
            //cloudWatchDB = new Amazon.CloudWatch.AmazonCloudWatchClient(ConfigurationSettings.AppSettings["AWS-KEY"],
            //    ConfigurationSettings.AppSettings["AWS-SECRETKEY"], configCloudWatch);
                  
        }


        /// <summary>
        /// MEtodo que faz o envio da imagem para um bucket S3
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="bucketName"></param>
        /// <param name="uploadKeyName"></param>
        public static void EnviaArquivoS3(string fileName, string bucketName, string uploadKeyName)
        {
            try
            {
                if (fileName.Length > 0)
                {
                    //PUT Object: http://docs.amazonwebservices.com/AmazonS3/latest/API/RESTObjectPUT.html

                    Dictionary<String, String> ExtraRequestHeaders = new Dictionary<String, String>();

                    //add a x-amz-acl header with the value of public-read to make the uploaded file public
                    //PUT Object acl: http://docs.amazonwebservices.com/AmazonS3/latest/API/RESTObjectPUTacl.html
                    ExtraRequestHeaders.Add("x-amz-acl", "public-read");

                    //add a x-amz-storage-class header with the value of REDUCED_REDUNDANCY to make the uploaded file reduced redundancy
                    ExtraRequestHeaders.Add("x-amz-storage-class", "REDUCED_REDUNDANCY");


                    //add a Content-MD5 header to ensure data is not corrupted over the network
                    //Amazon will return an error if the MD5 they calulate does not match the MD5 you send
                    SprightlySoftAWS.S3.CalculateHash MyCalculateHash = new SprightlySoftAWS.S3.CalculateHash();

                    String MyMD5;
                    MyMD5 = MyCalculateHash.CalculateMD5FromFile(fileName);
                    ExtraRequestHeaders.Add("Content-MD5", MyMD5);


                    SprightlySoftAWS.S3.Upload MyUpload = new SprightlySoftAWS.S3.Upload();

                    String RequestURL;
                    RequestURL = MyUpload.BuildS3RequestURL(true, "s3.amazonaws.com", bucketName, uploadKeyName, "");

                    String RequestMethod = "PUT";

                    ExtraRequestHeaders.Add("x-amz-date", DateTime.UtcNow.ToString("r"));

                    String AuthorizationValue;
                    AuthorizationValue = MyUpload.GetS3AuthorizationValue(RequestURL, RequestMethod, ExtraRequestHeaders, ConfigurationSettings.AppSettings["AWS-KEY"],
                        ConfigurationSettings.AppSettings["AWS-SECRETKEY"]);
                    ExtraRequestHeaders.Add("Authorization", AuthorizationValue);

                    Boolean RetBool;
                    RetBool = MyUpload.UploadFile(RequestURL, RequestMethod, ExtraRequestHeaders, fileName);

                    System.Diagnostics.Debug.Print("");
                    System.Diagnostics.Debug.Print(MyUpload.LogData);
                    System.Diagnostics.Debug.Print("");

                    if (RetBool == true)
                    {
                        Console.WriteLine("Arquivo " + fileName + " carregado no bucket " + bucketName + " com sucesso");

                        //apagando o arquivo
                        if (System.IO.File.Exists(fileName))
                        {
                            System.IO.File.Delete(fileName);
                        }
                    }
                    else
                    {
                        String ResponseMessage;
                        ResponseMessage = FormatLogData(MyUpload.RequestURL, MyUpload.RequestMethod, MyUpload.RequestHeaders, MyUpload.ResponseStatusCode, MyUpload.ResponseStatusDescription, MyUpload.ResponseHeaders, MyUpload.ResponseStringFormatted, MyUpload.ErrorNumber, MyUpload.ErrorDescription);

                        throw new Exception(ResponseMessage);
                    }
                }
                else
                {
                    Console.WriteLine("O arquivo passado é inválido!");
                }

            }
            catch (Exception exc)
            {
                throw exc;
            }
        }

        /// <summary>
        /// Metodo que formata o rfetorno de erro do UploadS3
        /// </summary>
        /// <param name="RequestURL"></param>
        /// <param name="RequestMethod"></param>
        /// <param name="RequestHeaders"></param>
        /// <param name="ResponseStatusCode"></param>
        /// <param name="ResponseStatusDescription"></param>
        /// <param name="ResponseHeaders"></param>
        /// <param name="ResponseString"></param>
        /// <param name="ErrorNumber"></param>
        /// <param name="ErrorDescription"></param>
        /// <returns></returns>
        private static string FormatLogData(String RequestURL, String RequestMethod, Dictionary<String, String> RequestHeaders, int ResponseStatusCode, String ResponseStatusDescription, Dictionary<String, String> ResponseHeaders, String ResponseString, int ErrorNumber, String ErrorDescription)
        {
            String ReturnString = "";

            if (ErrorNumber != 0)
            {
                ReturnString += "SprightlySoft ErrorDescription: " + ErrorDescription + Environment.NewLine;
                ReturnString += "SprightlySoft ErrorNumber: " + ErrorNumber + Environment.NewLine;
                ReturnString += Environment.NewLine;
            }

            if (ResponseStatusCode != 0)
            {
                ReturnString += "Request URL: " + RequestURL + Environment.NewLine;
                ReturnString += "Request Method: " + RequestMethod + Environment.NewLine;

                foreach (KeyValuePair<String, String> MyHeader in RequestHeaders)
                {
                    ReturnString += "Request Header: " + MyHeader.Key + ":" + MyHeader.Value + Environment.NewLine;
                }

                ReturnString += Environment.NewLine;
                ReturnString += "Response Status Code: " + ResponseStatusCode + Environment.NewLine;
                ReturnString += "Response Status Description: " + ResponseStatusDescription + Environment.NewLine;

                foreach (KeyValuePair<String, String> MyHeader in ResponseHeaders)
                {
                    ReturnString += "Response Header: " + MyHeader.Key + ":" + MyHeader.Value + Environment.NewLine;
                }

                if (ResponseString != "")
                {
                    ReturnString += Environment.NewLine;

                    try
                    {
                        System.Xml.XmlDocument XmlDoc = new System.Xml.XmlDocument();
                        XmlDoc.LoadXml(ResponseString);

                        ReturnString += "Response XML: " + Environment.NewLine + ResponseString + Environment.NewLine;

                        System.Xml.XmlNode XmlNode;
                        XmlNode = XmlDoc.SelectSingleNode("/Error/Message");

                        if (XmlNode != null)
                        {
                            ReturnString += Environment.NewLine;
                            ReturnString += "Amazon Error Message: " + XmlNode.InnerText + Environment.NewLine;
                        }
                    }
                    catch (Exception e)
                    {
                        ReturnString += "Response String: " + Environment.NewLine + ResponseString + Environment.NewLine;
                    }

                }
            }

            return ReturnString;
        }

        /// <summary>
        /// Metodo que envia emails
        /// </summary>
        /// <param name="emailFrom"></param>
        /// <param name="emailTo"></param>
        /// <param name="assunto"></param>
        /// <param name="corpoEmail"></param>
        /// <param name="html"></param>
        public static void EnviaEmailSES(string emailFrom, string emailTo, string assunto, string corpoEmail, bool html, string nomeSistema)
        {
            try
            {
                Amazon.SimpleEmail.AmazonSimpleEmailServiceClient client
                    = new Amazon.SimpleEmail.AmazonSimpleEmailServiceClient(ConfigurationSettings.AppSettings["AWS-KEY"],
                        ConfigurationSettings.AppSettings["AWS-SECRETKEY"]);
                
                
                List<string> listaEmailTo = new List<string>();
                listaEmailTo.Add(emailTo);
                List<string> listaEmailFrom = new List<string>();
                listaEmailFrom.Add(emailFrom);

                Amazon.SimpleEmail.Model.Message message = new Amazon.SimpleEmail.Model.Message();
    
                message.Body = new Amazon.SimpleEmail.Model.Body();
                message.Body.Html = new Amazon.SimpleEmail.Model.Content(corpoEmail);
                                
                message.Subject = new Amazon.SimpleEmail.Model.Content(assunto);
                
                Amazon.SimpleEmail.Model.SendEmailRequest request = new Amazon.SimpleEmail.Model.SendEmailRequest();
                request.Destination = new Amazon.SimpleEmail.Model.Destination(listaEmailTo);                
                request.ReturnPath = emailFrom;
                request.Source = emailFrom;
                request.Message = message;

                //fazendo o envio do email
                client.SendEmail(request);

            }
            catch (Exception exc)
            {
                //LogServicoBM.LogaEvento(nomeSistema, "SESEnviaEmail", exc.ToString(), System.Diagnostics.EventLogEntryType.Error);
            }
        }

        /// <summary>
        /// Metodo que envia emails
        /// </summary>
        /// <param name="emailFrom"></param>
        /// <param name="emailTo"></param>
        /// <param name="assunto"></param>
        /// <param name="corpoEmail"></param>
        /// <param name="html"></param>
        public static void EnviaEmailSMTP(string emailFrom, string emailTo, string assunto, string corpoEmail, bool html)
        {
            try
            {
                MailMessage mail = new MailMessage();
                NetworkCredential credenciais = new NetworkCredential("comercial@traderdata.com.br", "tdata00");

                mail.To.Add(new MailAddress(emailTo));
                mail.Bcc.Add(new MailAddress("comercial@traderdata.com.br"));
                mail.From = new MailAddress(emailFrom);
                mail.Subject = assunto;
                mail.Body = corpoEmail;
                mail.IsBodyHtml = html;

                mail.BodyEncoding = System.Text.Encoding.GetEncoding("ISO-8859-1");
                //mail.Priority = MailPriority.High;
                //mail.DeliveryNotificationOptions = DeliveryNotificationOptions.OnFailure;

                SmtpClient smtp = new SmtpClient("smtp.traderdata.com.br");
                smtp.Credentials = credenciais;
                smtp.Send(mail);


            }
            catch (Exception exc)
            {
                Console.WriteLine(exc);
            }
        }
    }
}
