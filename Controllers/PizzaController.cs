using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using System.Dynamic;
using System.Runtime.InteropServices;
using WebApiDotnetCoreSample.DataStoreModel;
using WebApiDotnetCoreSample.Providers.CacheProvider;
using WebApiDotnetCoreSample.Services;

namespace WebApiDotnetCoreSample.Controllers
{
    public class PizzaController : ControllerBase
    {
       private readonly ICacheProvider cacheProvider;
       public PizzaController(ICacheProvider provider)
       {
            this.cacheProvider = provider;
       }

        /// <summary>
        /// Get Pizza By Id 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [Authorize]
        [HttpGet]       
        public ActionResult<Pizza> GetPizzaById(int id)
        {
            var pizza = this.cacheProvider.GetValue(id.ToString());

            if(pizza == null)
            {
                pizza = PizzaService.GetPizzaById(id);
                this.cacheProvider.SetValue(id.ToString(), pizza);
            }

            return pizza == null ? NotFound($"Pizza not found with Id : {id}") : (Pizza) pizza;
        }

        /// <summary>
        /// Add pizza 
        /// </summary>
        /// <param name="pizza"></param>
        /// <returns></returns>
        [Authorize]
        [HttpPost]
        public IActionResult AddPizza([FromBody] Pizza pizza)
        {
            Task.Run( () =>
            {
                PizzaService.AddPizza(pizza);
                this.cacheProvider.SetValue(pizza.Id.ToString(), pizza);
            });

            return CreatedAtAction(nameof(AddPizza) , pizza);
        }
        /// <summary>
        /// Update pizza
        /// </summary>
        /// <param name="pizza"></param>
        /// <returns></returns>
        [Authorize]
        [HttpPut]
        public IActionResult UpdatePizza([FromBody] Pizza pizza)
        {
            if(pizza == null) return BadRequest($"Missing input pizza payload");

            var existingPizza = PizzaService.GetPizzaById(pizza.Id);
            if(existingPizza == null) return NotFound($"Pizza not found with id : {pizza.Id}");
            PizzaService.UpdatePizza(pizza);
            this.cacheProvider.SetValue(pizza.Id.ToString(), pizza);

            return NoContent();           
        }

        /// <summary>
        /// Delete pizza by id 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [Authorize]
        [HttpDelete]
        public IActionResult DeletePizza(int id)
        {
            var existingPizza = PizzaService.GetPizzaById(id);
            if (existingPizza == null) return NotFound($"Pizza not found with id : {id}");
            PizzaService.DeletePizza(id);
            this.cacheProvider.RemoveEntry(id.ToString());
            return NoContent();
        }
    }
}
