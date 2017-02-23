
public class HttpClientHelper
{
    /// <summary>
    /// Http同步Post异步请求  
    /// 参考地址    http://www.cnblogs.com/shanyou/archive/2012/03/21/2410739.html
    ///             http://www.tuicool.com/articles/rmiqYz
    /// </summary>
    /// <param name="url"></param>
    /// <param name="postData"></param>
    /// <param name="contentType"></param>
    public void HttpPostAsync(string url, string postData = "", string contentType = "application/json")
    {
        Encoding encode = Encoding.UTF8;
        StringBuilder sb = new StringBuilder();
        sb.AppendFormat("{0}", WebApiDomain);
        sb.AppendFormat("{0}", url);
        sb.AppendFormat("?appid={0}", AppId);
        url = sb.ToString();
        var sendData = encode.GetBytes(postData);

        System.Net.Http.HttpClient httpClient = new System.Net.Http.HttpClient();
        System.Net.Http.HttpContent content = new System.Net.Http.StringContent(postData);
        content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue(contentType);
        content.Headers.ContentLength = sendData.Length;
        //通过HttpClient访问时如何指定返回格式
        //httpClient.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("text/xml");
        //httpClient.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("text/json");
        //签名
        string token = auth.Generate(Common.Enum.HttpMethod.Post, url, AuthKey, postData);
        content.Headers.Add(auth.TokenName, token);
        //返回值
        //string retMessage = h.PostAsync(new Uri(url), content).Result.Content.ReadAsStringAsync().Result;
        httpClient.PostAsync(new Uri(url), content).ContinueWith(
            (requestTask) =>
            {
                //日志记录
                System.Net.Http.HttpResponseMessage response = requestTask.Result;
                System.Threading.Thread.Sleep(3000);
                // 确认响应成功，否则抛出异常
                response.EnsureSuccessStatusCode();
                // 异步读取响应为字符串
                //response.Content.ReadAsStringAsync().ContinueWith(
                //    (readTask) => Console.WriteLine(readTask.Result));
                //response.Content.ReadAsStringAsync().Result

            });
    }
}