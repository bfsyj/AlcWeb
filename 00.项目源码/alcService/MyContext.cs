using System;
using Microsoft.EntityFrameworkCore;
using alcEntity;

namespace alcService
{
    public class MyContext : DbContext
    {
        public MyContext(DbContextOptions<MyContext> options) : base(options)
        {

        }
        public DbSet<AlcNews> AlcNews { get; set; }
        public DbSet<EnvironmentNews> EnvironmentNews { get; set; }
        public DbSet<EnvironmentFiles> EnvironmentFiles { get; set; }
        public DbSet<EngineeringCase> EngineeringCase { get; set; }
        public DbSet<Introduction> Introduction { get; set; }
    }
}
