using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BitAuto.EP.Wx.Utils
{
    public class RetryHelper
    {
        public static void Post(Action method, RetryStrategy strategy, Action<string> failMethod = null)
        {
            strategy.Execute(method);
            if (failMethod != null && !string.IsNullOrEmpty(strategy.ErrorMessage))
                failMethod.Invoke(strategy.ErrorMessage);
        }

        public static Task PostAsync(Action method, RetryStrategy strategy, Action<string> failMethod = null)
        {
            return Task.Factory.StartNew(() =>
            {
                Post(method, strategy, failMethod);
            });

        }

        public abstract class RetryStrategy
        {
            private RetryStrategy _Strategy = null;
            protected bool Result = true;

            public RetryStrategy()
            {

            }

            public RetryStrategy(int retryCount)
            {
                this.RetryCount = retryCount;
            }

            public RetryStrategy(RetryStrategy strategy)
            {
                _Strategy = strategy;
            }

            public string ErrorMessage { set; get; } = "";

            public int RetryCount { set; get; }

            public virtual bool Execute(Action method)
            {
                if (_Strategy != null)
                    Result = Result && _Strategy.Execute(method);

                return Result;
            }



        }

        public class ExceptionRetryStrategy : RetryStrategy
        {
            public ExceptionRetryStrategy(RetryStrategy strategy) : base(strategy) { }

            private Type[] _exceptionDefines;

            public ExceptionRetryStrategy(int retrycount, params Type[] exception) : base(retrycount)
            {

                _exceptionDefines = exception;

            }

            public override bool Execute(Action method)
            {
                int retry = 0;
                for (int i = 0; i < this.RetryCount; i++)
                {
                    try
                    {
                        method.Invoke();
                        this.Result = true;
                    }
                    catch (Exception ex)
                    {
                        if (_exceptionDefines.Any(eType => eType == ex.GetType()))
                        {
                            this.Result = false;
                        }
                        this.ErrorMessage += ex.Message + "\r\n";
                    }

                    retry++;

                    if (this.Result)
                    {
                        break;
                    }

                }
                this.RetryCount = retry;

                return base.Execute(method); ;
            }
        }

    }

}


Test
            RetryHelper.Post(() =>
                {
                    apiResult = IMService.LoginIM(userId, name, userInfoModel.impic);
                    LOG.Debug();

                },
                new RetryHelper.ExceptionRetryStrategy(3, typeof(IMApiCallMethodException)),
                (message) =>
                {
                   
                    LOG.Error();
                    apiResult = null;
                });