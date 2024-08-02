using Microsoft.AspNetCore.Authorization;
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
            if (dbmodel == null)
            {
                return NotFound();
            }
            else
            {
                return dbmodel;
            }
        }
        [HttpPost]
        public async Task<ActionResult<DbModel>> Post([FromBody] ModelForUser dbModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            using (var transaction = await dbContextClass.Database.BeginTransactionAsync())
            {
                try
                {
                    var helpingmodel = new DbModel
                    {
                        question = dbModel.question,
                        answer = dbModel.answer,
                        value = dbModel.value
                    };
                    dbContextClass.DbModels.Add(helpingmodel);
                    await dbContextClass.SaveChangesAsync();

                    await transaction.CommitAsync();
                    return CreatedAtAction(nameof(Getdbmodel), new { id = helpingmodel.id }, helpingmodel);
                }
                catch (Exception ex)
                {
                    await transaction.RollbackAsync();
                    return StatusCode(500, $"An error occurred: {ex.Message}");
                }
            }
        }
        /*[HttpPost]
        public async Task<ActionResult<DbModel>> Post(ModelForUser dbModel)
        {
            Function helpingfunction = new Function(dbContextClass);
            DbModel helpingmodel = helpingfunction.modeltransform(dbModel);

            dbContextClass.DbModels.Add(helpingmodel);
            dbContextClass.SaveChanges();
            return CreatedAtAction(nameof(Getdbmodel), new { id = helpingmodel.Id }, helpingmodel);
            using (var transaction = await dbContextClass.Database.BeginTransactionAsync())
            {

                var helpingmodel = new DbModel
                {
                    question = dbModel.question,
                    answer = dbModel.answer,
                    value = dbModel.value
                };

                dbContextClass.DbModels.Add(helpingmodel);
                await dbContextClass.SaveChangesAsync();
                
                transaction.Commit();
                return CreatedAtAction(nameof(Getdbmodel), new { id = helpingmodel.id }, helpingmodel);

            }
        }
        [HttpPost]
        public async Task<ActionResult<DbModel>> Post([FromBody] ModelForUser dbModel)
        {
            

            using (var transaction = await dbContextClass.Database.BeginTransactionAsync())
            {
                try
                {
                    var helpingmodel = new DbModel
                    {
                        question = dbModel.question,
                        answer = dbModel.answer,
                        value = dbModel.value
                    };
                    dbContextClass.DbModels.Add(helpingmodel);
                    await dbContextClass.SaveChangesAsync();

                    await transaction.CommitAsync();
                    return CreatedAtAction(nameof(Getdbmodel), new { id = helpingmodel.id }, helpingmodel);
                }
                catch (Exception ex)
                {
                    await transaction.RollbackAsync();
                    return StatusCode(500, $"An error occurred: {ex.Message}");
                }
            }
        }*/


        [HttpPut("{id}")]
        public async Task<IActionResult> Change(int id, ModelForUser model)
        {


            if (dbContextClass.DbModels.Find(id) == null)
            {
                return Conflict("Bad id");
            }
            else
            {
                var dbModel = await dbContextClass.DbModels.FindAsync(id);
                dbModel.question = model.question;
                dbModel.answer = model.answer;
                dbModel.value = model.value;




                dbContextClass.Entry(dbModel).State = EntityState.Modified;

                try
                {
                    await dbContextClass.SaveChangesAsync();
                }
                catch
                {

                    return Problem();
                }

                return Ok();
            }
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteModel(int id)
        {
            var dbModel = await dbContextClass.DbModels.FindAsync(id);
            if (dbModel != null)
            {
                dbContextClass.DbModels.Remove(dbModel);
                dbContextClass.SaveChanges();
                return Ok();
            }
            else
            {
                return NotFound();
            }
        }


    }
}
