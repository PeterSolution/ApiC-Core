using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RestApi.Data;
using RestApi.Models;

namespace RestApi.Controllers
{
    [Route("api/ApiRequest")]
    public class ControllersApiRequest : Controller
    {
        DbContextClass dbContextClass;
        public DbModel modeltransform(ModelForUser modeluser)
        {
            int a = lastId().Id;

            DbModel model = new DbModel();
            model.id = a;
            model.question = modeluser.question;
            model.value = modeluser.value;
            model.answer = modeluser.answer;

            return model;
        }
        public ControllersApiRequest(DbContextClass dbContextClasss)
        {
            dbContextClass = dbContextClasss ?? throw new ArgumentNullException(nameof(dbContextClass));

        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<DbModel>>> get()
        {

            return await dbContextClass.DbModels.ToListAsync();

        }
        [HttpGet("{id}")]
        public async Task<ActionResult<DbModel>> Getdbmodel(int id)
        {
            
            DbModel dbmodel = await dbContextClass.DbModels.FindAsync(id);
            if(dbmodel == null)
            {
                return NotFound();
            }
            else
            {
                return dbmodel;
            }
        }
        
        [HttpPost]
        public async Task<ActionResult<DbModel>> Post(ModelForUser dbModel)
        {

            DbModel helpingmodel = modeltransform(dbModel);
            


            dbContextClass.DbModels.Add(helpingmodel);
            dbContextClass.SaveChanges();
            return CreatedAtAction(nameof(Getdbmodel), helpingmodel);
        }

        public async Task<ActionResult<int>> lastId()
        {
            var lastId = await dbContextClass.DbModels
            .OrderByDescending(m => m.id)
            .Select(m => m.id)
            .FirstOrDefaultAsync();
            return lastId;
        }
        [HttpPut("{id}")]
        public async Task<IActionResult> Change(int id, DbModel dbModel)
        {
            if (id != dbModel.id)
            {
                return Conflict("Bad id");
            }
            else 
            {

                if (dbModel == null)
                {
                    return NotFound();
                }

                dbContextClass.Entry(dbModel).State = EntityState.Modified;

                try
                {
                    await dbContextClass.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CheckModelExists(id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }

                return NoContent();
            }
        }
        private bool CheckModelExists(int id)
        {
            return dbContextClass.DbModels.Any(e => e.id == id);
        }

        /*// GET: ControllersApiRequest
        public ActionResult Index()
        {
            return View();
        }

        // GET: ControllersApiRequest/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: ControllersApiRequest/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: ControllersApiRequest/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: ControllersApiRequest/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: ControllersApiRequest/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: ControllersApiRequest/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: ControllersApiRequest/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }*/

    }
}
