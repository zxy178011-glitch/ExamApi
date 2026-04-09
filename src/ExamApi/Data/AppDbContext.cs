using Microsoft.EntityFrameworkCore;
using ExamApi.Models;

namespace ExamApi.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<Candidate> Candidates => Set<Candidate>();
}
