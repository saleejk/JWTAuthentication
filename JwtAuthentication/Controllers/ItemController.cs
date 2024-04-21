using JwtAuthentication.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace JwtAuthentication.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ItemController : ControllerBase
    {
        
        public List<ItemModel> items=new List<ItemModel>() {
            new ItemModel() {Id=1,Name="asp .net", },
            new ItemModel() {Id=2,Name="flutter" },
        };

        //[Authorize]
        [HttpGet]
        public IActionResult getItems()
        {
            return Ok(items);
        }
        [HttpPost]
        public List<ItemModel> createItem(ItemModel item)
        {
            var id = items.LastOrDefault().Id + 1;
            item.Id = id;
            items.Add(item);
            return items;
        }
    }
}
