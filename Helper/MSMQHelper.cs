using Newtonsoft.Json;
using System.Messaging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace AAAAAAAAAAAAA
{
    public class MSMQHelper
    {
        private readonly BinaryMessageFormatter formatter = new BinaryMessageFormatter();
        /// <summary>
        /// MSMQ服务地址配置
        /// </summary>
        private readonly string MsmqString;

        public MSMQHelper(string path)
        {
            if (string.IsNullOrWhiteSpace(path))
            {
                throw new Exception("MSMQ path is NULL");
            }
            this.MsmqString = path;
        }

        /// <summary>
        /// 发送一个消息Body
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="data"></param>
        /// <returns></returns>
        public bool SendMessage<T>(T data) where T : class
        {
            return SendMessage(string.Empty, data, MessagePriority.Normal);
        }

        /// <summary>
        /// 发送一个消息body
        /// </summary>
        /// <typeparam name="T">body数据</typeparam>
        /// <param name="data">body数据</param>
        /// <param name="Priority"></param>
        /// <returns></returns>
        public bool SendMessage<T>(T data, MessagePriority Priority) where T : class
        {
            return SendMessage(string.Empty, data, Priority);
        }

        /// <summary>
        /// 发送消息
        /// </summary>
        /// <param name="head">标识码</param>
        /// <param name="json">json数据</param>
        /// <param name="Priority">级别</param>
        public bool SendMessage<T>(string label, T body, MessagePriority Priority) where T : class
        {
            bool IsTrue = false;
            try
            {
                MessageQueue mq = new MessageQueue(MsmqString);
                Message message = BuildMessage(label, body, Priority);
                mq.Send(message);
                IsTrue = true;
            }
            catch (Exception ex)
            {
                throw new Exception(string.Format("发送MQ异常错误：{0}", ex.ToString()));
            }
            return IsTrue;
        }
        /// <summary>
        /// 获取消息队列消息
        /// </summary>
        /// <param name="MQTimeOutSeconds">超时秒数</param>
        /// <returns></returns>
        public string ReceiveMessage(int MQTimeOutSeconds)
        {
            return ReceiveMessage<string>(MQTimeOutSeconds);
        }

        /// <summary>
        /// 获取消息队列消息
        /// </summary>
        /// <param name="MQTimeOutSeconds">超时秒数</param>
        /// <returns></returns>

        public T ReceiveMessage<T>(int MQTimeOutSeconds) where T : class
        {
            return ReceiveMessage<T>(new TimeSpan(0, 0, MQTimeOutSeconds));
        }

        public T ReceiveMessage<T>(TimeSpan timespan) where T : class
        {
            MessageQueue mq = new MessageQueue(MsmqString);
            Message message = mq.Receive(timespan);

            var messageStream = message.BodyStream;
            byte[] bytes = new byte[messageStream.Length];
            messageStream.Read(bytes, 0, bytes.Length);
            // 设置当前流的位置为流的开始
            messageStream.Seek(0, SeekOrigin.Begin);
            string messageJson = Encoding.UTF8.GetString(bytes);
            return JsonConvert.DeserializeObject<T>(messageJson);
        }

        private Message BuildMessage<T>(T body)
        {
            return BuildMessage(body, MessagePriority.Normal);
        }

        private Message BuildMessage<T>(T body, MessagePriority Priority)
        {
            return BuildMessage<T>(string.Empty, body, Priority);
        }

        private Message BuildMessage<T>(string label, T body, MessagePriority Priority)
        {
            Message message = new Message();
            //为了避免存放消息队列的计算机重新启动而丢失消息，可以通过设置消息对象的Recoverable属性为true，
            //在消息传递过程中将消息保存到磁盘上来保证消息的传递，默认为false。 
            message.Recoverable = false;
            message.Priority = Priority;
            message.Label = label;
            var messageJson = JsonConvert.SerializeObject(body);
            var dataBytes = Encoding.UTF8.GetBytes(messageJson);
            message.BodyStream.Write(dataBytes, 0, dataBytes.Length);
            return message;
        }

    }
}
