using JibbleTask.Entities;
using JibbleTask.Infrastructure;
using Microsoft.AspNet.OData;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;

namespace JibbleTask.Controllers
{
    public class PeopleController : ODataController
    {
        private PeopleContext context = new PeopleContext();

        [EnableQuery]
        public IQueryable<Person> Get() { return context.People; }

        [EnableQuery]
        public SingleResult<Person> Get([FromODataUri] int id)
        {
            return SingleResult.Create(context.People.Where(x => x.Id == id));
        }

        public async Task<IHttpActionResult> Post(Person person)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            context.People.Add(person);
            await context.SaveChangesAsync();
            return Created(person);
        }

        public async Task<IHttpActionResult> Patch([FromODataUri] int key, Delta<Person> product)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var entity = await context.People.FindAsync(key);
            if (entity == null)
            {
                return NotFound();
            }

            product.Patch(entity);
            try
            {
                await context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ProductExists(key))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Updated(entity);
        }

        public async Task<IHttpActionResult> Put([FromODataUri] int key, Person update)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (key != update.Id)
            {
                return BadRequest();
            }

            context.Entry(update).State = EntityState.Modified;
            try
            {
                await context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ProductExists(key))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Updated(update);
        }

        public async Task<IHttpActionResult> Delete([FromODataUri] int key)
        {
            var product = await context.People.FindAsync(key);
            if (product == null)
            {
                return NotFound();
            }

            context.People.Remove(product);
            await context.SaveChangesAsync();
            return StatusCode(HttpStatusCode.NoContent);
        }

        protected override void Dispose(bool disposing)
        {
            context.Dispose();
            base.Dispose(disposing);
        }

        private bool ProductExists(int key)
        {
            return context.People.Any(p => p.Id == key);
        }
    }
}