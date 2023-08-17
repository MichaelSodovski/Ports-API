using Microsoft.EntityFrameworkCore;
using PortsApi;

namespace PortsApi
{
    public class GeoContext : DbContext
    {
        public GeoContext(DbContextOptions<GeoContext> options)
          : base(options)
        {

        }
        public DbSet<TESTLog> Log { get; set; }
    }
}
