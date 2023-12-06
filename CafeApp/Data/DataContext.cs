using WebProject.Models;
using Microsoft.EntityFrameworkCore;

namespace WebProject.Data{
    public class DataContext : DbContext {

        public DataContext(DbContextOptions<DataContext>options):base(options){
            
        }
        public DbSet<Order> Orders=>Set<Order>(); 

        //public DbSet<Course> Courses=>Set<Course>();

        //public DbSet<StudentCourse> Participants =>Set<StudentCourse>();

        
    }
}