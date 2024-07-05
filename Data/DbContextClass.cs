using RestApi.Models;
using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace RestApi.Data
{
    public class DbContextClass : Microsoft.EntityFrameworkCore.DbContext
    {
        public DbContextClass(DbContextOptions<DbContextClass> options) : base(options)
        {

        }

        public DbSet<DbModel> DbModels { get; set; }
    }
}
