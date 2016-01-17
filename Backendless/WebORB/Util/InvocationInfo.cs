using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;
using Weborb.Service;

namespace Weborb.Util
{
    [ExcludeProperty("Method")]
    public class InvocationInfo
    {
        internal string _functionName;
        internal string _className;
        internal string _module;
        internal object[] _args;
        internal object _result;
        internal long _duration;
        private long lastInvocationTime;
        internal MethodInfo _method;

        internal InvocationInfo( MethodInfo method, object[] args, object response, long duration )
        {
            this._method = method;
            this._className = method.DeclaringType.FullName;
            this._functionName = method.Name;
            this._module = method.Module.Name;
            this._args = args;
            this._result = response;
            this._duration = duration;
            lastInvocationTime = DateTime.Now.Ticks / 10000;
        }
        
        public MethodInfo Method
        {
            get { return _method; }
        }

        public string ClassName
        {
            get { return _className; }
        }

        public string FunctionName
        {
            get { return _functionName; }
        }

        public Object[] Arguments
        {
            get { return _args; }
        }

        public string Module
        {
            get { return _module; }
        }

        public Object Result
        {
            get { return _result; }
        }

        public long Duration
        {
            get { return _duration; }
        }

        public long LastInvocationTime
        {
            get
            {
                return lastInvocationTime;
            }
            /*set
            {
                this.lastInvocationTime = value;
            }*/
        }

    }
}
