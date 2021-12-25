using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Diplomski.Infrastructure.Shared;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace Diplomski.Infrastructure.Contexts
{
    public class AppDbContextFactory : DesignTimeDbContextFactoryBase<IdentityAppDbContext>
    {
        protected override IdentityAppDbContext CreateNewInstance(DbContextOptions<IdentityAppDbContext> options)
        {
            return new IdentityAppDbContext(options);
        }

    }
}
