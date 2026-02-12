using Microsoft.EntityFrameworkCore;

namespace TaskManagerAPI.Entities
{
    public class TaskManagerDBContext : DbContext
    {
        public TaskManagerDBContext() { }
        public TaskManagerDBContext(DbContextOptions<TaskManagerDBContext> options) : base(options)
        {

        }

        public DbSet<Task> Tasks { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(this.GetType().Assembly);
            base.OnModelCreating(modelBuilder);

        }
    }
}
