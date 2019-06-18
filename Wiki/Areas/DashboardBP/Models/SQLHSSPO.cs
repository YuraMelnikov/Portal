using System.Data.SqlClient;
using System.Diagnostics;
using System.IO;
using Microsoft.SqlServer.Management.Common;
using Microsoft.SqlServer.Management.Smo;

namespace Wiki.Areas.DashboardBP.Models
{
    public class SQLHSSPO
    {
        public void CreateNew()
        {
            string sqlConnectionString = "Data Source=TSQLSERVER;Initial Catalog=PortalKATEK_test;Integrated Security=True";
            FileInfo file = new FileInfo(@"C:\Users\myi\source\repos\Portal\Wiki\Areas\DashboardBP\Content\createHSSPO.sql");
            string script = file.OpenText().ReadToEnd();
            SqlConnection conn = new SqlConnection(sqlConnectionString);
            Server server = new Server(new ServerConnection(conn));
            server.ConnectionContext.ExecuteNonQuery(script);
            Stopwatch sw = new Stopwatch();
            sw.Start();
            while (sw.ElapsedMilliseconds < 15000)
            {
            }
        }
    }
}