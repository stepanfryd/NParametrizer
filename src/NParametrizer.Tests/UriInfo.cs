using System;
using System.Net;

namespace NParametrizer.Tests
{
    public class UriInfo : Uri
    {
        public UriInfo(string uriString)
            : base(uriString)
        {
            Credentials = new NetworkCredential();

            GetCredentials();
        }

        public NetworkCredential Credentials { get; private set; }

        public new int Port
        {
            get
            {
                if (base.Port == -1)
                {
                    switch (Scheme.ToLower())
                    {
                        case "sftp":
                            return 22;
                    }
                }

                return base.Port;
            }
        }

        private void GetCredentials()
        {
            var userInfo = UserInfo.Split(':');
            Credentials.UserName = userInfo[0];

            if (userInfo.Length > 1)
            {
                Credentials.Password = userInfo[1];
            }
        }
    }
}