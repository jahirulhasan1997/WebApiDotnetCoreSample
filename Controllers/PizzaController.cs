using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using System.Dynamic;
using System.Runtime.InteropServices;
using WebApiDotnetCoreSample.DataStoreModel;
using WebApiDotnetCoreSample.Services;

namespace WebApiDotnetCoreSample.Controllers
{
    public class PizzaController : ControllerBase
    {
       public PizzaController() { }

        /// <summary>
        /// Get Pizza By Id 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        
        public ActionResult<Pizza> GetPizzaById(int id)
        {
            var pizza = PizzaService.GetPizzaById(id);

            if(pizza == null)
            {
                return NotFound($"Pizza not found with id : {id}");
            }

            return pizza;
        }

        /// <summary>
        /// Add pizza 
        /// </summary>
        /// <param name="pizza"></param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult AddPizza([FromBody] Pizza pizza)
        {
            Task.Run( () =>
            {
                PizzaService.AddPizza(pizza);               
            });

            return CreatedAtAction(nameof(AddPizza) , pizza);
        }
        /// <summary>
        /// Update pizza
        /// </summary>
        /// <param name="pizza"></param>
        /// <returns></returns>
        [HttpPut]
        public IActionResult UpdatePizza([FromBody] Pizza pizza)
        {
            if(pizza == null) return NotFound();

            var existingPizza = PizzaService.GetPizzaById(pizza.Id);
            if(existingPizza == null) return NotFound($"Pizza not found with id : {pizza.Id}");
            PizzaService.UpdatePizza(pizza);

            return NoContent();           
        }

        /// <summary>
        /// Delete pizza by id 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete]
        public IActionResult DeletePizza(int id)
        {
            var existingPizza = PizzaService.GetPizzaById(id);
            if (existingPizza == null) return NotFound($"Pizza not found with id : {id}");
            PizzaService.DeletePizza(id);

            return NoContent();
        }
    }
}
