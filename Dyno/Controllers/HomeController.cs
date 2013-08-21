using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.SqlClient;
using System.Data;
using System.Configuration;
using Dyno;

namespace DynoWeb.Controllers
{
    public class HomeController : Controller
    {
        //
        // GET: /Home/

        public ActionResult Index()
        {
            testDynoProcCall();
            testDynoRow();
            return View();
        }


        private static void testDynoProcCall()
        {
            using (dynamic db = new DB("DynoTestDB"))
            {
                var dataset = db.dbo.GetPerson(ID: 1).DataSet();
            }
        }

        private static void testDynoRow()
        {
            var connectionString = ConfigurationManager.ConnectionStrings["DynoTestDB"].ConnectionString;
            using (var cnxn = new SqlConnection(connectionString))
            {
                var ds = new DataSet();
                var cmd = new SqlCommand("SELECT * FROM Person", cnxn);
                var adapter = new SqlDataAdapter(cmd);
                adapter.Fill(ds);
                var table = ds.Tables[0];
                dynamic row = table.Rows[0].Dyno();
                int id = row.ID;
                string name = row.FirstName;
            }
        }

    }
}
