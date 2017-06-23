using System.Collections.Generic;

namespace BackendlessAPI.Engine
{
    public class HeadersEnum
    {
        public static readonly HeadersEnum USER_TOKEN_KEY = new HeadersEnum("user-token");

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

    public class HeadersManager
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
