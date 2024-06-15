using API_Dinamis.Models;
using API_Dinamis.Models.Authorizer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace API_Dinamis.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {

        }

        public DbSet<Pokemon> Pokemons { get; set; }
        public DbSet<Branch> Branches { get; set; }
        public DbSet<BranchLogTemp> BranchesLogTemp { get; set; }
        public DbSet<BranchLog> BranchesLog { get; set; }
        public DbSet<Department> Departments { get; set; }
        public DbSet<DepartmentLogTemp> DepartmentsLogTemp { get; set; }
        public DbSet<DepartmentLog> DepartmentsLog { get; set; }
        public DbSet<Authx> Auths { get; set; }
        public DbSet<Key> Keys { get; set; }
        public DbSet<City> Cities { get; set; }
        public DbSet<Zipcode> Zipcodes { get; set; }
        public DbSet<License> Licenses { get; set; }
        public DbSet<LicenseLogTemp> LicensesLogTemp { get; set; }
        public DbSet<LicenseLog> LicensesLog { get; set; }
        public DbSet<Country> Countries { get; set; }
        public DbSet<Company> Companies{ get; set; }
        public DbSet<CompanyLogTemp> CompaniesLogTemp{ get; set; }
        public DbSet<CompanyLog> CompaniesLog { get; set; }
        public DbSet<Holiday> Holidays { get; set; }
        public DbSet<HolidayLogTemp> HolidaysLogTemp { get; set; }
        public DbSet<HolidayLog> HolidaysLog { get; set; }
        public DbSet<Module> Modules { get; set; }
        public DbSet<Menu> Menus { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<RoleLogTemp> RolesLogTemp { get; set; }
        public DbSet<RoleLog> RolesLog { get; set; }
        public DbSet<RoleDetail> RolesDetail { get; set; }
        public DbSet<RoleDetailLogTemp> RolesDetailLogTemp { get; set; }
        public DbSet<RoleDetailLog> RolesDetailLog { get; set; }
        public DbSet<Employee> Employees { get; set; }
        public DbSet<EmployeeLogTemp> EmployeesLogTemp { get; set; }
        public DbSet<EmployeeLog> EmployeesLog { get; set; }
        public DbSet<EmployeeDetail> EmployeesDetail { get; set; }
        public DbSet<EmployeeDetailLogTemp> EmployeesDetailLogTemp { get; set; }
        public DbSet<EmployeeDetailLog> EmployeesDetailLog { get; set; }

        //standardvalue
        public DbSet<StandardValue> StandardValues { get; set; }
        public DbSet<StandardValueLogTemp> StandardValuesLogTemp { get; set; }
        public DbSet<StandardValueLog> StandardValuesLog { get; set; }

        //authorizer hierarchy
        public DbSet<Authorizer> Authorizers { get; set; }
        public DbSet<AuthorizerLogTemp> AuthorizersLogTemp { get; set; }
        public DbSet<AuthorizerLog> AuthorizersLog { get; set; }
        public DbSet<AuthorizerModule> AuthorizerModules { get; set; }
        public DbSet<AuthorizerModuleLogTemp> AuthorizerModulesLogTemp { get; set; }
        public DbSet<AuthorizerModuleLog> AuthorizerModulesLog { get; set; }
        public DbSet<AuthorizerDownline> AuthorizerDownlines { get; set; }
        public DbSet<AuthorizerDownlineLogTemp> AuthorizerDownlinesLogTemp { get; set; }
        public DbSet<AuthorizerDownlineLog> AuthorizerDownlinesLog { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Branch>()
            .HasOne<City>()
            .WithMany()
            .HasForeignKey(b => b.CityId).OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Branch>()
            .HasOne<Zipcode>()
            .WithMany()
            .HasForeignKey(b => b.ZipCodeId).OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Zipcode>()
            .HasOne<City>()
            .WithMany()
            .HasForeignKey(b => b.CityId).OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Department>()
            .HasOne<Branch>()
            .WithMany()
            .HasForeignKey(b => b.BranchId).OnDelete(DeleteBehavior.Restrict);

            /*company FK setting start*/
            modelBuilder.Entity<Company>().HasOne<City>().WithMany().HasForeignKey(company => company.CityId).OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<Company>().HasOne<Zipcode>().WithMany().HasForeignKey(company => company.ZipCodeId).OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<Company>().HasOne<Country>().WithMany().HasForeignKey(company => company.CountryId).OnDelete(DeleteBehavior.Restrict);
            /*company FK setting end*/

            modelBuilder.Entity<Menu>()
                .HasOne<Module>()
                .WithMany()
                .HasForeignKey(Menu=>Menu.ModuleId).OnDelete(DeleteBehavior.Restrict);

            //=====================================================================================
            modelBuilder.Entity<Role>()
                .HasOne<Module>()
                .WithMany()
                .HasForeignKey(Role => Role.ModuleId).OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<RoleDetail>()
                .HasOne<Role>()
                .WithMany()
                .HasForeignKey(RoleDetail => RoleDetail.RoleId).OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<RoleDetail>()
                .HasOne<Menu>()
                .WithMany()
                .HasForeignKey(RoleDetail => RoleDetail.MenuId).OnDelete(DeleteBehavior.Restrict);
            //=====================================================================================
            modelBuilder.Entity<EmployeeDetail>()
                .HasOne<Employee>()
                .WithMany()
                .HasForeignKey(EmployeeDetail => EmployeeDetail.EmployeeId).OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<EmployeeDetail>()
                .HasOne<License>()
                .WithMany()
                .HasForeignKey(EmployeeDetail => EmployeeDetail.LicenseId).OnDelete(DeleteBehavior.Restrict);
            //=====================================================================================

            //authorizer
            modelBuilder.Entity<Authorizer>().HasOne<Employee>().WithOne().HasForeignKey<Authorizer>(authorizer => authorizer.EmployeeId).OnDelete(DeleteBehavior.Cascade);
            modelBuilder.Entity<AuthorizerModule>().HasIndex(am => new { am.AuthorizerId, am.ModuleId }).IsUnique();
            modelBuilder.Entity<AuthorizerDownline>().HasIndex(am => new { am.AuthorizerModuleId, am.EmployeeId }).IsUnique();
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            // Other configurations...

            optionsBuilder.EnableSensitiveDataLogging(); // Enable sensitive data logging
        }
    }
}
