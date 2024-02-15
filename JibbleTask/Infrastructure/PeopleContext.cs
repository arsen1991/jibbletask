using JibbleTask.Entities;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace JibbleTask.Infrastructure
{
    public class PeopleContext : DbContext
    {
        public PeopleContext() : base("name=PeopleContext") { }

        public DbSet<Person> People { get; set; }
    }
}