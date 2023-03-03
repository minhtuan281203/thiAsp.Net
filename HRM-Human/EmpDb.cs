using System;
using Microsoft.EntityFrameworkCore;
namespace ASP

{
    public class EmpDb : DbContext
    {
        public EmpDb(DbContextOptions<EmpDb> options) : base(options){ }
        public DbSet<Employee> Mems => Set<Employee>();
    }
}
