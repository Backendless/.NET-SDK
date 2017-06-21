using BackendlessAPI.Async;
using BackendlessAPI.Engine;
using System;

namespace BackendlessAPI.Service
{
    public class CustomService
    {
        private const string CUSTOM_SERVICE_ALIAS = "com.backendless.services.servercode.CustomServiceHandler";
        private const string METHOD_NAME_ALIAS = "dispatchService";

        public T Invoke<T>(string serviceName, string method, object[] arguments)
        {
            return Invoker.InvokeSync<T>(CUSTOM_SERVICE_ALIAS, METHOD_NAME_ALIAS, new object[] { serviceName, method, arguments });
        }

        public void Invoke<T>(string serviceName, string method, object[] arguments, AsyncCallback<T> callback)
        {
            Invoker.InvokeAsync<T>(CUSTOM_SERVICE_ALIAS, METHOD_NAME_ALIAS, new object[] { serviceName, method, arguments }, callback);
        }
    }
}
