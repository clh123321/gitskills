using QuickLib.Log;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;

namespace QuickLib.Wx.Lib
{
    /// <summary>
    /// 
    /// </summary>
    public abstract class MaterialHelper
    {
        #region 上传url多媒体文件到微信平台（文件的有效期3天）-http://image.baidu.com/search/62408_171130083000_2.jpg
        /// <summary>
        /// 上传多媒体文件到微信平台（文件的有效期3天）
        /// </summary>
        /// <param name="url"></param>
        /// <param name="filePath"></param>
        /// <returns></returns>
        public static string AddMaterialWithOnlineMedia(string url, string fileUrl)
        {
            string fileName = GetFileName(fileUrl);
            string HttpMessage = string.Empty;
            var result = "";

            try
            {
                string boundary = "---------------------------" + DateTime.Now.Ticks.ToString("x");
                byte[] boundarybytes = System.Text.Encoding.ASCII.GetBytes("\r\n--" + boundary + "\r\n");
                HttpWebRequest wr = (HttpWebRequest)WebRequest.Create(url);
                wr.ContentType = "multipart/form-data; boundary=" + boundary;
                wr.Method = "POST";
                wr.KeepAlive = true;
                wr.Credentials = System.Net.CredentialCache.DefaultCredentials;

                //处理https
                System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls;
                System.Net.ServicePointManager.ServerCertificateValidationCallback = (se, cert, chain, sslerror) =>
                {
                    return true;
                };

                Stream rs = wr.GetRequestStream();

                string formdataTemplate = "Content-Disposition: form-data; name=\"{0}\"\r\n\r\n{1}";

                rs.Write(boundarybytes, 0, boundarybytes.Length);
                string formitem = string.Format(formdataTemplate, "media", fileName);
                byte[] formitembytes = System.Text.Encoding.UTF8.GetBytes(formitem);
                rs.Write(formitembytes, 0, formitembytes.Length);
                rs.Write(boundarybytes, 0, boundarybytes.Length);

                string headerTemplate = "Content-Disposition: form-data; name=\"{0}\"; filename=\"{1}\"\r\nContent-Type: {2}\r\n\r\n";
                string header = string.Format(headerTemplate, "media", fileName, "application/octet-stream");
                byte[] headerbytes = System.Text.Encoding.UTF8.GetBytes(header);
                rs.Write(headerbytes, 0, headerbytes.Length);

                #region 通过图片url或得图片字节流
                HttpWebRequest fileRequest = (HttpWebRequest)WebRequest.Create(fileUrl);
                fileRequest.Method = "GET";
                HttpWebResponse imgResponse = (HttpWebResponse)fileRequest.GetResponse();
                using (Stream fileStream = imgResponse.GetResponseStream())
                {
                    byte[] buffer = new byte[4096];
                    int bytesRead = 0;
                    while ((bytesRead = fileStream.Read(buffer, 0, buffer.Length)) != 0)
                    {
                        rs.Write(buffer, 0, bytesRead);
                    }
                }
                #endregion

                byte[] trailer = System.Text.Encoding.ASCII.GetBytes("\r\n--" + boundary + "--\r\n");
                rs.Write(trailer, 0, trailer.Length);
                rs.Close();

                WebResponse wresp = null;

                wresp = wr.GetResponse();
                if (wresp != null)
                {
                    Stream stream2 = wresp.GetResponseStream();
                    StreamReader reader2 = new StreamReader(stream2);
                    result = reader2.ReadToEnd();
                }
                else
                {
                    result = "";
                }
                return result;
            }
            catch (Exception e)
            {
                LogHelp.Error("微信上传文件异常", e);
            }
            return string.Empty;
        }
        #endregion

        #region 上传本地多媒体文件到微信平台（文件的有效期3天） - "C:\Users\微信图片_20170411110721.png"
        /// <summary>
        /// 上传多媒体文件到微信平台（文件的有效期3天）
        /// </summary>
        /// <param name="url"></param>
        /// <param name="fileUrl"></param>
        /// <returns></returns>
        public static string AddLocalMaterialWithOnlineMedia(string url, string fileUrl)
        {
            string fileName = GetFileName(fileUrl);
            string HttpMessage = string.Empty;
            var result = "";
            try
            {
                string boundary = "---------------------------" + DateTime.Now.Ticks.ToString("x");
                byte[] boundarybytes = System.Text.Encoding.ASCII.GetBytes("\r\n--" + boundary + "\r\n");
                HttpWebRequest wr = (HttpWebRequest)WebRequest.Create(url);
                wr.ContentType = "multipart/form-data; boundary=" + boundary;
                wr.Method = "POST";
                wr.KeepAlive = true;
                wr.Credentials = System.Net.CredentialCache.DefaultCredentials;

                //处理https
                System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls;
                System.Net.ServicePointManager.ServerCertificateValidationCallback = (se, cert, chain, sslerror) =>
                {
                    return true;
                };

                Stream rs = wr.GetRequestStream();

                string formdataTemplate = "Content-Disposition: form-data; name=\"{0}\"\r\n\r\n{1}";

                rs.Write(boundarybytes, 0, boundarybytes.Length);
                string formitem = string.Format(formdataTemplate, "media", fileName);
                byte[] formitembytes = System.Text.Encoding.UTF8.GetBytes(formitem);
                rs.Write(formitembytes, 0, formitembytes.Length);
                rs.Write(boundarybytes, 0, boundarybytes.Length);

                string headerTemplate = "Content-Disposition: form-data; name=\"{0}\"; filename=\"{1}\"\r\nContent-Type: {2}\r\n\r\n";
                string header = string.Format(headerTemplate, "media", fileName, "application/octet-stream");
                byte[] headerbytes = System.Text.Encoding.UTF8.GetBytes(header);
                rs.Write(headerbytes, 0, headerbytes.Length);

                #region 通过图片url或得图片字节流
                FileWebRequest fileRequest = (FileWebRequest)FileWebRequest.Create(fileUrl);
                FileWebResponse imgResponse = (FileWebResponse)fileRequest.GetResponse();
                using (Stream fileStream = imgResponse.GetResponseStream())
                {
                    byte[] buffer = new byte[4096];
                    int bytesRead = 0;
                    while ((bytesRead = fileStream.Read(buffer, 0, buffer.Length)) != 0)
                    {
                        rs.Write(buffer, 0, bytesRead);
                    }
                }
                #endregion

                byte[] trailer = System.Text.Encoding.ASCII.GetBytes("\r\n--" + boundary + "--\r\n");
                rs.Write(trailer, 0, trailer.Length);
                rs.Close();

                WebResponse wresp = null;

                wresp = wr.GetResponse();
                if (wresp != null)
                {
                    Stream stream2 = wresp.GetResponseStream();
                    StreamReader reader2 = new StreamReader(stream2);
                    result = reader2.ReadToEnd();
                }
                else
                {
                    result = "";
                }
                return result;
            }
            catch (Exception e)
            {
                LogHelp.Error("微信上传文件异常", e);
            }
            return string.Empty;
        }
        #endregion

        /// <summary>
        /// 上传多媒体头像到微信平台（文件的有效期永久）
        /// </summary>
        /// <param name="url"></param>
        /// <param name="filePath"></param>
        /// <returns></returns>
        public static string AddMaterialWithOnlineMediaLogo(string url, string fileUrl)
        {
            string fileName = GetFileName(fileUrl);
            string HttpMessage = string.Empty;
            var result = "";

            try
            {
                string boundary = "---------------------------" + DateTime.Now.Ticks.ToString("x");
                byte[] boundarybytes = System.Text.Encoding.ASCII.GetBytes("\r\n--" + boundary + "\r\n");
                HttpWebRequest wr = (HttpWebRequest)WebRequest.Create(url);
                wr.ContentType = "multipart/form-data; boundary=" + boundary;
                wr.Method = "POST";
                wr.KeepAlive = true;
                wr.Credentials = System.Net.CredentialCache.DefaultCredentials;

                //处理https
                System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls;
                System.Net.ServicePointManager.ServerCertificateValidationCallback = (se, cert, chain, sslerror) =>
                {
                    return true;
                };

                Stream rs = wr.GetRequestStream();

                string formdataTemplate = "Content-Disposition: form-data; name=\"{0}\"\r\n\r\n{1}";

                rs.Write(boundarybytes, 0, boundarybytes.Length);
                string formitem = string.Format(formdataTemplate, "buffer", fileName);
                byte[] formitembytes = System.Text.Encoding.UTF8.GetBytes(formitem);
                rs.Write(formitembytes, 0, formitembytes.Length);
                rs.Write(boundarybytes, 0, boundarybytes.Length);

                string headerTemplate = "Content-Disposition: form-data; name=\"{0}\"; filename=\"{1}\"\r\nContent-Type: {2}\r\n\r\n";
                string header = string.Format(headerTemplate, "buffer", fileName, "application/octet-stream");
                byte[] headerbytes = System.Text.Encoding.UTF8.GetBytes(header);
                rs.Write(headerbytes, 0, headerbytes.Length);

                #region 通过图片url或得图片字节流
                HttpWebRequest fileRequest = (HttpWebRequest)WebRequest.Create(fileUrl);
                fileRequest.Method = "GET";
                HttpWebResponse imgResponse = (HttpWebResponse)fileRequest.GetResponse();
                using (Stream fileStream = imgResponse.GetResponseStream())
                {
                    byte[] buffer = new byte[4096];
                    int bytesRead = 0;
                    while ((bytesRead = fileStream.Read(buffer, 0, buffer.Length)) != 0)
                    {
                        rs.Write(buffer, 0, bytesRead);
                    }
                }
                #endregion

                byte[] trailer = System.Text.Encoding.ASCII.GetBytes("\r\n--" + boundary + "--\r\n");
                rs.Write(trailer, 0, trailer.Length);
                rs.Close();

                WebResponse wresp = null;

                wresp = wr.GetResponse();
                if (wresp != null)
                {
                    Stream stream2 = wresp.GetResponseStream();
                    StreamReader reader2 = new StreamReader(stream2);
                    result = reader2.ReadToEnd();
                }
                else
                {
                    result = "";
                }
                return result;
            }
            catch (Exception e)
            {
                LogHelp.Error("微信上传文件异常", e);
            }
            return string.Empty;
        }

        /// <summary>
        /// 取得图片文件名
        /// </summary>
        /// <param name="imgUrl">图片url</param>
        /// <returns></returns>
        private static string GetFileName(string imgUrl)
        {
            return imgUrl.Substring(imgUrl.LastIndexOf("/") + 1);
        }
    }
}
