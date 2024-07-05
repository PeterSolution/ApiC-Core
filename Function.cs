using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RestApi.Data;
using RestApi.Models;

namespace RestApi
{
    public class Function
    {
        DbContextClass dbContextClass;
        public Function(DbContextClass dbContextClasss) 
        {
            dbContextClass = dbContextClasss ?? throw new ArgumentNullException(nameof(dbContextClass));
        }
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
        public async Task<ActionResult<int>> lastId()
        {
            var lastId = await dbContextClass.DbModels
            .OrderByDescending(m => m.id)
            .Select(m => m.id)
            .FirstOrDefaultAsync();
            return lastId;
        }
    }

}
