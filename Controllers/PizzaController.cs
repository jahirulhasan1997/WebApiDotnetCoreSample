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
        private readonly PizzaService pizzaService;
       public PizzaController(ICacheProvider provider, PizzaDbContext pizzaDbContext)
       {
            this.cacheProvider = provider;
            pizzaService = new PizzaService(pizzaDbContext);
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
            try
            {
                if (pizza == null)
                {
                    pizza = this.pizzaService.GetPizzaById(id);
                    if (pizza != null)
                    {
                        this.cacheProvider.SetValue(id.ToString(), pizza);
                    }
                }
            }
            catch (Exception ex) 
            {
                throw ex;
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
            try
            {
                Task.Run(() =>
                {
                    pizzaService.AddPizza(pizza);
                    this.cacheProvider.SetValue(pizza.PizzaId.ToString(), pizza);
                });
            }
            catch (Exception ex)
            {
                throw ex;
            }

            Task.Delay(1000);

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

            try
            {
                var existingPizza = pizzaService.GetPizzaById(pizza.PizzaId);
                if (existingPizza == null) return NotFound($"Pizza not found with id : {pizza.PizzaId}");
                pizzaService.UpdatePizza(pizza);
                this.cacheProvider.SetValue(pizza.PizzaId.ToString(), pizza);
            }
            catch (Exception ex)
            {
                throw ex;
            }

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
            try
            {
                var existingPizza = pizzaService.GetPizzaById(id);
                if (existingPizza == null) return NotFound($"Pizza not found with id : {id}");
                pizzaService.DeletePizza(id);
                this.cacheProvider.RemoveEntry(id.ToString());
            }
            catch (Exception ex) 
            {
                throw ex;
            }
            return NoContent();
        }
    }
}
