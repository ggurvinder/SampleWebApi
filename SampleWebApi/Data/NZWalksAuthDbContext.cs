using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using SampleWebApi.Models.Domain;


namespace SampleWebApi.Data
{
    public class NZWalksAuthDbContext:IdentityDbContext<ApplicationUser>
    {
        public NZWalksAuthDbContext(DbContextOptions<NZWalksAuthDbContext> options):base(options)
        {
           

            


        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            //var readerRoleId = "4e101c50-6b93-407c-b178-19b453ca90e7";
            //var writerRoleId = "a3a2cb1c-d593-49fb-b4e0-33fdf9dca365";

            //var roles = new List<IdentityRole>{
            //new IdentityRole
            //{
            //    Id= readerRoleId,
            //    ConcurrencyStamp=readerRoleId,
            //    Name="Reader",
            //    NormalizedName="Reader".ToUpper()
            //},
            //new IdentityRole
            //{
            //    Id= writerRoleId,
            //    ConcurrencyStamp=writerRoleId,
            //    Name="Writer",
            //    NormalizedName="Writer".ToUpper()
            //}
            //};

            //builder.Entity<IdentityRole>().HasData(roles);


        }
    }
}
