using System;
#if (!UNIVERSALW8 && !FULL_BUILD && !PURE_CLIENT_LIB)
using System.Windows.Threading;
using System.Windows.Controls;
#endif

#if (!UNIVERSALW8 && !FULL_BUILD && !WINDOWS_PHONE && !PURE_CLIENT_LIB  && !WINDOWS_PHONE8)
using System.Windows.Browser;
#endif

#if !(UNIVERSALW8 || WINDOWS_PHONE || PURE_CLIENT_LIB || WINDOWS_PHONE8 )
using Weborb.ProxyGen.Core.Interceptor;
#endif

namespace Weborb.Client
{
    public class AsyncToken<T>
    {
        public event ResponseHandler<T> ResultListener;
        public event ErrorHandler ErrorListener;

        internal bool gotResult;
        internal bool isFault;
        internal T resultObject;
        internal Fault fault;
#if( !UNIVERSALW8 && !WINDOWS_PHONE && !PURE_CLIENT_LIB  && !WINDOWS_PHONE8)
        internal IInvocation invocation;
#endif
#if !(UNIVERSALW8 || FULL_BUILD || PURE_CLIENT_LIB)
        internal UserControl uiControl;
#endif
        public AsyncToken( ResponseHandler<T> responseHandler, ErrorHandler errorHandler )
        {
            ResultListener += responseHandler;

            if( errorHandler != null )
                ErrorListener += errorHandler;
        }
#if (!UNIVERSALW8 && !FULL_BUILD && !WINDOWS_PHONE && !PURE_CLIENT_LIB && !WINDOWS_PHONE8)
        public AsyncToken( IInvocation invocation, UserControl uiControl )
        {
            this.invocation = invocation;
            this.uiControl = uiControl;
        }
#endif
#if (!UNIVERSALW8 && !WINDOWS_PHONE && !PURE_CLIENT_LIB && !WINDOWS_PHONE8)
        public IInvocation Invocation
        {
            get { return invocation; }
        }
#endif

        public bool HasResult
        {
            get { return gotResult; }
        }

        public bool IsFault
        {
            get { return isFault; }
        }

        public T Result
        {
            get { return resultObject; }

            internal set
            {
                this.resultObject = value;
                gotResult = true;
                if ( ResultListener != null )
                {
#if (UNIVERSALW8 || FULL_BUILD || PURE_CLIENT_LIB )
                  ResultListener.Invoke( resultObject );
#else
                    if (uiControl != null)
                    uiControl.Dispatcher.BeginInvoke(delegate()
                                                       {
                                                         ResultListener.Invoke(resultObject);
                                                       });
                  else
                    ResultListener.Invoke(resultObject);
#endif
                }

            }
        }

        public Fault Fault
        {
            get { return fault; }

            internal set
            {
                this.fault = value;
                isFault = true;

                if( ErrorListener != null )
                    ErrorListener.Invoke( fault );
#if (!UNIVERSALW8 && !WINDOWS_PHONE && !FULL_BUILD && !PURE_CLIENT_LIB && !WINDOWS_PHONE8)
                else if( uiControl != null )
                    uiControl.Dispatcher.BeginInvoke( delegate()
                    {
                      HtmlPage.Window.Alert( "Received an error from a remote invocation. " + fault.Message );
                    } );
#endif
            }
        }
    }
}
