using System;
using System.Collections.Specialized;
using System.IO;
using System.Net;
using System.Text;
using System.Web;

namespace NewsPortal.Security
{
    static class HttpRequestHelper
    {
        public static string GetRequestParameter(string parameterName)
        {
            return HttpContext.Current.Request.Params[parameterName];
        }

        public static string RequestPath
        {
            get
            {
                return HttpContext.Current.Request.Path;
            }
        }

        public static Uri ConvertUriToAbsolute(string returnUrl)
        {
            if (Uri.IsWellFormedUriString(returnUrl, UriKind.Absolute))
                return new Uri(returnUrl, UriKind.Absolute);
            if (!VirtualPathUtility.IsAbsolute(returnUrl))
                returnUrl = VirtualPathUtility.ToAbsolute(returnUrl);
            return new Uri(GetPublicFacingUrl(), returnUrl);
        }

        private static Uri GetPublicFacingUrl()
        {
            HttpRequest request = HttpContext.Current.Request;
            NameValueCollection serverVariables = request.ServerVariables;
            if (serverVariables["HTTP_HOST"] == null)
                return new Uri(request.Url, request.RawUrl);
            string str = serverVariables["HTTP_X_FORWARDED_PROTO"]
                ?? (string.Equals(serverVariables["HTTP_FRONT_END_HTTPS"], "on", StringComparison.OrdinalIgnoreCase)
                    ? Uri.UriSchemeHttps
                    : request.Url.Scheme);
            Uri uri = new Uri(str + Uri.SchemeDelimiter + serverVariables["HTTP_HOST"]);
            return new UriBuilder(request.Url)
            {
                Scheme = str,
                Host = uri.Host,
                Port = uri.Port
            }.Uri;
        }

        public static string SendRequest(Uri uri)
        {
            var myWebRequest = WebRequest.Create(uri);
            WebResponse myWebResponse;
            try
            {
                myWebResponse = myWebRequest.GetResponse();
            }
            catch (WebException e)
            {
                myWebResponse = e.Response;
            }
            using (Stream receiveStream = myWebResponse.GetResponseStream())
            {
                Encoding encode = Encoding.GetEncoding("utf-8");
                using (StreamReader readStream = new StreamReader(receiveStream, encode))
                {
                    string response = readStream.ReadToEnd();
                    readStream.Close();
                    myWebResponse.Close();
                    return response;
                }
            }
        }

        public static void Redirect(string url)
        {
            HttpContext.Current.Response.Redirect(url);
        }
    }
}