using Microsoft.AspNetCore.Http.HttpResults;
using System.Collections.Immutable;
using WebApiDotnetCoreSample.DataStoreModel;

namespace WebApiDotnetCoreSample.Services
{
    public class PizzaService
    {
        public PizzaService() { }

        static List<Pizza> Pizzas {  get; }

        static PizzaService()
        {
            Pizzas = new List<Pizza>()
            {
                new Pizza { Id = 1, Name = "Non Veg Pizza", Description = "Chicken pizza"},
                new Pizza { Id = 2, Name = "Non Veg Pizza", Description = "Chicken pizza"}
            };
        }

        static int PizzaId = 3;

        /// <summary>
        /// Get Pizza By Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static Pizza GetPizzaById(int id)
        {
            return Pizzas.FirstOrDefault( p => p.Id == id);
        }

        /// <summary>
        /// Add Pizza
        /// </summary>
        /// <param name="pizza"></param>
        public static Task AddPizza (Pizza pizza)
        {
            pizza.Id = PizzaId++;
            Pizzas?.Add(pizza);
            return Task.FromResult(pizza);
        }

        /// <summary>
        /// Delete Pizza 
        /// </summary>
        /// <param name="id"></param>
        public static void DeletePizza(int id)
        {
            Pizza pizza = GetPizzaById(id);
            if (pizza != null)
            {
                Pizzas?.Remove(pizza);
            }
        }

        /// <summary>
        /// Update Pizza 
        /// </summary>
        /// <param name="updatedPizza"></param>
        public static void UpdatePizza(Pizza updatedPizza)
        {
            int index = Pizzas.FindIndex(p => p.Id == updatedPizza.Id);
            if (index == -1)
            {
                return;
            }
            Pizzas.Insert(index, updatedPizza);
        }

        /// <summary>
        /// Patch pizza by Id
        /// </summary>
        /// <param name="id"></param>
        /// <param name="name"></param>
        /// <param name="description"></param>
        public static void PatchPizzaById(int id , string? name = null, string? description = null)
        {
            int index = Pizzas.IndexOf(GetPizzaById(id));
            Pizza pizza = Pizzas[index];
            
            if(name != null) pizza.Name = name;
            if(description != null) pizza.Description = description;

            Pizzas.Insert(index, pizza);
        }
    }
}
