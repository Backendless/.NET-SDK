using System.Collections.Generic;

namespace BackendlessAPI.Engine
{
    internal class HeadersEnum
    {
        public static readonly HeadersEnum USER_TOKEN_KEY = new HeadersEnum("user-token");
        public static readonly HeadersEnum LOGGED_IN_KEY = new HeadersEnum("logged-in");
        public static readonly HeadersEnum SESSION_TIME_OUT_KEY = new HeadersEnum("session-time-out");
        public static readonly HeadersEnum APP_TYPE_NAME = new HeadersEnum( "application-type" );
        public static readonly HeadersEnum API_VERSION = new HeadersEnum( "api-version" );

        public static IEnumerable<HeadersEnum> Values
        {
            get
            {
                yield return USER_TOKEN_KEY;
                yield return LOGGED_IN_KEY;
                yield return SESSION_TIME_OUT_KEY;
            }
        }

        private readonly string name;

        HeadersEnum(string name)
        {
            this.name = name;            
        }

        public string Header { get { return name; } }

        public override string ToString()
        {
            return name;
        }
    }

    internal class HeadersManager
    {
        private Dictionary<string, string> headers = new Dictionary<string, string>();
        private static object headersLock = new object();
        private  static HeadersManager _instance = null;

        private HeadersManager()
        {
        }

        public static HeadersManager GetInstance()
        {
            if (_instance == null)
            {
                lock (headersLock)
                {
                  if( _instance == null )
                  {
                    _instance = new HeadersManager();
                    _instance.AddHeader( HeadersEnum.APP_TYPE_NAME, "WP" );
                    _instance.AddHeader( HeadersEnum.API_VERSION, "1.0" );
                  }
                }
            }

            return _instance;
        }

        public void AddHeader(HeadersEnum headersEnum, string value)
        {
            lock (headersLock)
            {
              headers.Remove( headersEnum.Header );
              headers.Add(headersEnum.Header, value);
            }
        }

        public void RemoveHeader(HeadersEnum headersEnum)
        {
            lock (headersLock)
            {
                headers.Remove(headersEnum.Header);
            }
        }

        public Dictionary<string, string> Headers
        {
            get { return headers; }
            set
            {
                lock (headersLock)
                {
                    foreach (var header in headers)
                    {
                        this.headers.Add(header.Key, header.Value);
                    }
                }
            }
        }

        public static void CleanHeaders()
        {
            lock (typeof(HeadersManager))
            {
                _instance = null;
            }
        }
    }
}
