using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Scaffolding.Metadata;
using System.Collections.Immutable;
using System.Runtime.CompilerServices;
using WebApiDotnetCoreSample.DataStoreModel;

namespace WebApiDotnetCoreSample.Services
{
    public class PizzaService
    {
        public PizzaService() { }

        private readonly PizzaDbContext _context;

        public PizzaService(PizzaDbContext context)
        {
            this._context = context;  
        }

        /// <summary>
        /// Get Pizza By Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Pizza GetPizzaById(int id)
        {
            return this._context.Pizza.Find(id);
        }

        /// <summary>
        /// Add Pizza
        /// </summary>
        /// <param name="pizza"></param>
        public void AddPizza (Pizza pizza)
        {
            this._context.Pizza.Add(pizza);
            SaveChanges("Pizza");
        }

        /// <summary>
        /// Delete Pizza 
        /// </summary>
        /// <param name="id"></param>
        public void DeletePizza(int id)
        {
            Pizza pizza = GetPizzaById(id);
            if (pizza != null)
            {
                this._context.Pizza.Remove(pizza);
                SaveChanges("Pizza");
            }
        }

        /// <summary>
        /// Update Pizza 
        /// </summary>
        /// <param name="updatedPizza"></param>
        public void UpdatePizza(Pizza updatedPizza)
        {
            this._context.ChangeTracker.Context.Update(updatedPizza);
            SaveChanges("Pizza");
        }

        private void SaveChanges(string database)
        {
            this._context.Database.OpenConnection();
            this._context.Database.ExecuteSqlRaw($"SET IDENTITY_INSERT {database} ON");
            this._context.SaveChanges();
            this._context.Database.ExecuteSqlRaw($"SET IDENTITY_INSERT {database} OFF");
        }
    }
}
