using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace GisApi.ApiServer.Controllers
{
    public class WayController : Controller
    {
        public async Task<string> Index()
        {
            return await Task.FromResult("It works!");
        }
    }
}