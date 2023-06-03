using Microsoft.AspNetCore.Mvc;

namespace Catalog_Service.Controllers
{
    [Route("/api")]
    [ApiController]
    [ApiExplorerSettings(IgnoreApi = true)]
    public class HomeController : ControllerBase
    {
        [HttpGet(Name = nameof(Get))]
        public IActionResult Get()
        {
            var response = new
            {
                href = Url.Link(nameof(Get), null),
                categories = new
                {
                    href = Url.Link(nameof(CategoriesController.GetCategories), null)
                }
            };

            return Ok(response);
        }
    }
}
