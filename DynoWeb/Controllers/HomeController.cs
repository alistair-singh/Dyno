using System;
using System.Linq;
using System.Web.Mvc;
using System.Data.SqlClient;
using System.Data;
using System.Configuration;
using Dyno;

namespace DynoWeb.Controllers
{
  class Person
  {
    public int Id { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
  }

  public class HomeController : Controller
  {
    //
    // GET: /Home/

    public ActionResult Index()
    {
      TestDynoProcCall();
      testDynoRow();
      return View();
    }

    private static void TestDynoProcCall()
    {
      using (dynamic db = new Db("DynoTestDB"))
      {
        var spe = db["dbo"]["GetPerson"](ID: 1);

        //IQueryable<Person> peopleTable = new EnumerableQuery<Person>(new Person[0]);
        //db((from p in peopleTable
        //    where p.Id == 1
        //    select p.LastName).Expression);

        var query = db("SELECT * FROM Person");
        Set res = query();

        foreach (var person in res.Select(row => row.Map<Person>()))
        {

        }

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
