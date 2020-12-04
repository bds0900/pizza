using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IdentityServer
{
    public class ConnectionUri
    {
        public static (string user,string password,string host,string port,string db) Parse(string connectionUri)
        {
            
            var host_user=connectionUri.Split('@');

            var user_ = host_user[0];
            var host_ = host_user[1];

            var id_password = user_.Substring(user_.LastIndexOf('/')+1).Split(':');
            string user = id_password[0];
            string password = id_password[1];

            var host_db = host_.Split('/');
            string h = host_db[0];
            string db = host_db[1];

            var host_port=h.Split(':');
            string host = host_port[0];
            string port = host_port[1];

            return (user, password, host, port, db);

            
        }
        public static string Compose(string user, string password, string host, string port, string db)
        {
            return $"Server={host}; port={port}; user id={user}; password={password}; database={db};";
        }
        public static string Convert(string connectionRui)
        {
            (string user, string password, string host, string port, string db) =Parse(connectionRui);
            return Compose(user, password, host, port, db);
        }
    }
}
