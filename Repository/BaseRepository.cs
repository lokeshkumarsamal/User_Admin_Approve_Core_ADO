using System.Data.SqlClient;

namespace User_Admin_Approve_Core_ADO.Repository
{
    public class BaseRepository
    {
        private readonly IConfiguration _configuration;

        protected BaseRepository(IConfiguration configuration) 
        {
            _configuration = configuration;
        }

        protected SqlConnection CreateConnection()
        {
            return new SqlConnection(_configuration.GetConnectionString("DefaultConnection"));
        }
    }
}
