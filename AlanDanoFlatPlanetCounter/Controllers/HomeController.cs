using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.SqlClient;
using System.Configuration;


namespace AlanDanoFlatPlanetCounter.Controllers
{
    public class HomeController : Controller
    {
        private string ConnectionString
        {
            get { return ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString; }
        }

        public ActionResult Index()
        {
            var newCount = 0;

            using (SqlConnection sqlCon = new SqlConnection(ConnectionString))
            {
                sqlCon.Open();
                var sqlCmd = sqlCon.CreateCommand();
                sqlCmd.CommandText = "UPDATE [Counter] SET [Count] = 0";
                sqlCmd.ExecuteNonQuery();
                sqlCon.Close();
            }

            ViewBag.Counter = newCount;
            return View();
        }

        public ActionResult IncreaseCounter()
        {
            var newCount = 0;

            using (SqlConnection sqlCon = new SqlConnection(ConnectionString))
            {
                sqlCon.Open();
                var sqlCmd = sqlCon.CreateCommand();
                sqlCmd.CommandText = "SELECT TOP 1 [Count] FROM Counter";

                var count = sqlCmd.ExecuteScalar();
                if (count != null && int.TryParse(count.ToString(), out newCount))
                {
                    newCount = Math.Min(newCount + 1, 10);
                    sqlCmd.CommandText = "UPDATE [Counter] SET [Count] = " + newCount;
                    sqlCmd.ExecuteNonQuery();
                }
                sqlCon.Close();
            }

            ViewBag.Counter = newCount;
            return View("~/Views/Home/Index.cshtml");
        }
    }
}