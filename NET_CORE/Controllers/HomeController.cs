using System.Collections.Generic;
using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using NET_CORE.Models;
using NET_CORE.Common.Main;
using System.Data;
using System.Collections;
using OnionCore.Core.IApplicationService;
using OnionCore.Core.Models;

namespace NET_CORE.Controllers
{
    public class HomeController : Controller
    {
        IDataService _dataService;
        //ITestService _testService;

        public HomeController(IDataService dataService)
        {
            _dataService = dataService;
            //_testService = testService;
        }

        public IActionResult Index()
        {
            var watch = System.Diagnostics.Stopwatch.StartNew();

            IEnumerable<ListData> lists = _dataService.listDatas();
            // the code that you want to measure comes here
            watch.Stop();
            var elapsedMs = watch.ElapsedMilliseconds;
            ConfigValues configTest = new ConfigValues();
            string test = configTest.Get("connectionString");
               BLL bLL = new BLL();
            SortedList sl = new SortedList();
            DataTable dt = bLL.GetDashboardCount(sl);
            return View();
        }

        public IActionResult About()
        {
            ViewData["Message"] = "Your application description page.";

            return View();
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";

            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
