using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Web;
using System.Web.Services.Description;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WebSite
{
    public partial class WebForm1 : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            WebRefrence();
        }


        public static void WebRefrence()
        { //如果觉得写一个委托方法麻烦，可以直接使用匿名委托
            ServicePointManager.ServerCertificateValidationCallback = delegate (object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors errors)
            {
                if (errors == SslPolicyErrors.None)
                    return true;
                return false;
            };
            //ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3;
            HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create("https://139.215.205.111/iservice/webservice/iservice?wsdl/getDepList");
            request.Method = "POST";
            request.ContentType = "application/json;charset=utf-8";
            request.Credentials = CredentialCache.DefaultCredentials;
            request.Timeout = 10000;
            byte[] data = Encoding.UTF8.GetBytes("");
            request.ContentLength = data.Length;
            Stream writer = request.GetRequestStream();
            writer.Write(data, 0, data.Length);
            writer.Close();
        }
    }
}