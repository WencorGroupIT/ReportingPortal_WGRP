using System.Configuration;
using System.Data.Entity;
using Wencor.DataAccess.Model;

namespace Wencor.DataAccess
{
    public class WencorDataAccess : DbContext
    {
        public WencorDataAccess() : base(ConfigurationManager.AppSettings["SqlServerConnectionString"])
        {
            //disable initializer
            Database.SetInitializer<WencorDataAccess>(null);
        }

        public static WencorDataAccess Create()
        {
            return new WencorDataAccess();
        }

        public DbSet<IzendaUser> IzendaUsers { get; set; }
        
    }
}
