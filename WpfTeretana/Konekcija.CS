using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
 
namespace WpfTeretana
{
    class Konekcija
    {
        public static SqlConnection KreirajKonekciju()
        {
            SqlConnectionStringBuilder ccnSB = new SqlConnectionStringBuilder();
            ccnSB.DataSource = @"";//your database connection
            ccnSB.InitialCatalog = "Teretana";
            ccnSB.IntegratedSecurity = true;

            string com = ccnSB.ToString();
            SqlConnection konekcija = new SqlConnection(com);
            return konekcija;
        }

    }
}
