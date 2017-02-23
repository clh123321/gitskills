
public class HttpClientHelper
{
    /// <summary>
    /// Httpͬ��Post�첽����  
    /// �ο���ַ    http://www.cnblogs.com/shanyou/archive/2012/03/21/2410739.html
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
        //ͨ��HttpClient����ʱ���ָ�����ظ�ʽ
        //httpClient.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("text/xml");
        //httpClient.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("text/json");
        //ǩ��
        string token = auth.Generate(Common.Enum.HttpMethod.Post, url, AuthKey, postData);
        content.Headers.Add(auth.TokenName, token);
        //����ֵ
        //string retMessage = h.PostAsync(new Uri(url), content).Result.Content.ReadAsStringAsync().Result;
        httpClient.PostAsync(new Uri(url), content).ContinueWith(
            (requestTask) =>
            {
                //��־��¼
                System.Net.Http.HttpResponseMessage response = requestTask.Result;
                System.Threading.Thread.Sleep(3000);
                // ȷ����Ӧ�ɹ��������׳��쳣
                response.EnsureSuccessStatusCode();
                // �첽��ȡ��ӦΪ�ַ���
                //response.Content.ReadAsStringAsync().ContinueWith(
                //    (readTask) => Console.WriteLine(readTask.Result));
                //response.Content.ReadAsStringAsync().Result

            });
    }
}