namespace BeautySalon.Infrastructure.Data
{
    using BeautySalon.Domain.Enums;
    using BeautySalon.Domain.Models;
    using Microsoft.EntityFrameworkCore;

    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<BookingEntity> Bookings { get; set; }
        public DbSet<CustomerEntity> Customers { get; set; }
        public DbSet<ServiceEntity> Services { get; set; }
        public DbSet<EmployeeEntity> Employees { get; set; }
        public DbSet<PriceListEntity> PriceLists { get; set; }
        public DbSet<UserEntity> Users { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasDefaultSchema("beauty_salon");

            modelBuilder.Entity<UserEntity>()
                .HasOne(u => u.Customer)
                .WithOne(c => c.User)
                .HasForeignKey<CustomerEntity>(c => c.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<UserEntity>()
                .HasOne(u => u.Employee)
                .WithOne(e => e.User)
                .HasForeignKey<EmployeeEntity>(e => e.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<BookingEntity>()
                .HasOne(b => b.Customer)
                .WithMany(c => c.Bookings)
                .HasForeignKey(b => b.CustomerId);

            modelBuilder.Entity<BookingEntity>()
                .HasOne(b => b.Employee)
                .WithMany(e => e.Bookings)
                .HasForeignKey(b => b.EmployeeId);

            modelBuilder.Entity<BookingEntity>()
                .HasOne(b => b.Service)
                .WithMany(s => s.Bookings)
                .HasForeignKey(b => b.ServiceId);

            modelBuilder.Entity<PriceListEntity>()
                .HasOne(pl => pl.Service)
                .WithMany(s => s.PriceLists)
                .HasForeignKey(pl => pl.ServiceId);

            base.OnModelCreating(modelBuilder);
        }

        public void Seed()
        {
            if (!Users.Any())
            {
                var admin = new UserEntity
                {
                    Id = Guid.NewGuid(),
                    Email = "admin@gmail.com",
                    PasswordHash = BCrypt.Net.BCrypt.HashPassword("aaa"),
                    Role = UserRole.Administrator
                };

                Users.Add(admin);

                var employee = new UserEntity
                {
                    Id = Guid.NewGuid(),
                    Email = "employee@gmail.com",
                    PasswordHash = BCrypt.Net.BCrypt.HashPassword("aaa"),
                    Role = UserRole.Employee
                };

                Users.Add(employee);

                var employeeProfile = new EmployeeEntity
                {
                    Id = Guid.NewGuid(),
                    UserId = employee.Id,
                    FirstName = "Adam",
                    LastName = "Małysz",
                    PhoneNumber = "123456789",
                    Specialization = "Hair Stylist"
                };

                Employees.Add(employeeProfile);

                var customer = new UserEntity
                {
                    Id = Guid.NewGuid(),
                    Email = "customer@gmail.com",
                    PasswordHash = BCrypt.Net.BCrypt.HashPassword("aaa"),
                    Role = UserRole.Customer
                };

                Users.Add(customer);

                var customerProfile = new CustomerEntity
                {
                    Id = Guid.NewGuid(),
                    UserId = customer.Id,
                    FirstName = "John",
                    LastName = "Doe",
                    PhoneNumber = "123456789"
                };

                Customers.Add(customerProfile);

                var service1 = new ServiceEntity
                {
                    Id = Guid.NewGuid(),
                    Name = "Haircut",
                    Description = "A basic haircut for men and women.",
                    DefaultPrice = 50,
                    Duration = 30
                };
                Services.Add(service1);

                var service2 = new ServiceEntity
                {
                    Id = Guid.NewGuid(),
                    Name = "Facial",
                    Description = "A relaxing facial treatment for healthy skin.",
                    DefaultPrice = 70,
                    Duration = 60
                };
                Services.Add(service2);

                var service3 = new ServiceEntity
                {
                    Id = Guid.NewGuid(),
                    Name = "Massage",
                    Description = "Full body relaxation massage.",
                    DefaultPrice = 90,
                    Duration = 90
                };
                Services.Add(service3);

                var now = DateTime.UtcNow;
                var roundedTime = new DateTime(now.Year, now.Month, now.Day, 10, 0, 0, 0).ToUniversalTime();

                var booking1 = new BookingEntity
                {
                    Id = Guid.NewGuid(),
                    BookingDate = roundedTime,
                    CustomerId = customerProfile.Id,
                    EmployeeId = employeeProfile.Id,
                    ServiceId = service1.Id,
                    Status = BookingStatus.Confirmed
                };
                Bookings.Add(booking1);

                var booking2 = new BookingEntity
                {
                    Id = Guid.NewGuid(),
                    BookingDate = roundedTime.AddHours(2),
                    CustomerId = customerProfile.Id,
                    EmployeeId = employeeProfile.Id,
                    ServiceId = service2.Id,
                    Status = BookingStatus.Pending
                };
                Bookings.Add(booking2);

                var booking3 = new BookingEntity
                {
                    Id = Guid.NewGuid(),
                    BookingDate = roundedTime.AddHours(4),
                    CustomerId = customerProfile.Id,
                    EmployeeId = employeeProfile.Id,
                    ServiceId = service3.Id,
                    Status = BookingStatus.Pending
                };
                Bookings.Add(booking3);

                roundedTime = roundedTime.AddDays(-1);
                //Past bookings
                var booking4 = new BookingEntity
                {
                    Id = Guid.NewGuid(),
                    BookingDate = roundedTime,
                    CustomerId = customerProfile.Id,
                    EmployeeId = employeeProfile.Id,
                    ServiceId = service1.Id,
                    Status = BookingStatus.Confirmed
                };
                Bookings.Add(booking4);

                var booking5 = new BookingEntity
                {
                    Id = Guid.NewGuid(),
                    BookingDate = roundedTime.AddHours(2),
                    CustomerId = customerProfile.Id,
                    EmployeeId = employeeProfile.Id,
                    ServiceId = service2.Id,
                    Status = BookingStatus.Pending
                };
                Bookings.Add(booking5);

                var booking6 = new BookingEntity
                {
                    Id = Guid.NewGuid(),
                    BookingDate = roundedTime.AddHours(4),
                    CustomerId = customerProfile.Id,
                    EmployeeId = employeeProfile.Id,
                    ServiceId = service3.Id,
                    Status = BookingStatus.Pending
                };
                Bookings.Add(booking6);

                roundedTime = roundedTime.AddDays(2);
                //Future bookings
                var booking7 = new BookingEntity
                {
                    Id = Guid.NewGuid(),
                    BookingDate = roundedTime,
                    CustomerId = customerProfile.Id,
                    EmployeeId = employeeProfile.Id,
                    ServiceId = service1.Id,
                    Status = BookingStatus.Pending
                };
                Bookings.Add(booking7);

                var booking8 = new BookingEntity
                {
                    Id = Guid.NewGuid(),
                    BookingDate = roundedTime.AddHours(2),
                    CustomerId = customerProfile.Id,
                    EmployeeId = employeeProfile.Id,
                    ServiceId = service2.Id,
                    Status = BookingStatus.Pending
                };
                Bookings.Add(booking8);

                var booking9 = new BookingEntity
                {
                    Id = Guid.NewGuid(),
                    BookingDate = roundedTime.AddHours(4),
                    CustomerId = customerProfile.Id,
                    EmployeeId = employeeProfile.Id,
                    ServiceId = service3.Id,
                    Status = BookingStatus.Pending
                };
                Bookings.Add(booking9);

                SaveChanges();
            }
        }
    }
}