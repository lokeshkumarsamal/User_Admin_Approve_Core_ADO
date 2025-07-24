using System.Data.SqlClient;
using System.Data;
using User_Admin_Approve_Core_ADO.Models;

namespace User_Admin_Approve_Core_ADO.Repository
{
    public class UserRepository:BaseRepository ,IUserRepository
    {
        public UserRepository(IConfiguration configuration): base(configuration) { }

        public async Task<List<AdminModel>> GetAllUsers()
        {
            var cn = CreateConnection();
            if(cn.State == ConnectionState.Closed) cn.Open();

            SqlCommand cmd = new SqlCommand();
            cmd.Connection = cn;
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "sp_useradmin";
            cmd.Parameters.AddWithValue("@action", "All");
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            da.Fill(dt);
            List<AdminModel> users = new List<AdminModel>();
            foreach (DataRow dr in dt.Rows)
            {
                AdminModel admin = new AdminModel();
                admin.userloginid = dr.ItemArray[0].ToString();
                admin.username = dr.ItemArray[1].ToString();
                admin.userphone = dr.ItemArray[2].ToString();
                admin.useremail = dr.ItemArray[3].ToString();
                admin.useradd = dr.ItemArray[4].ToString();
                users.Add(admin);
            }
            cn.Close();
            return users;
        }
        
        
        public async Task<int> AddUser(AdminModel user)
        {
            try
            {
                var cn = CreateConnection();
                if (cn.State == ConnectionState.Closed) cn.Open();

                // Check if userloginid already exists
                SqlCommand checkCmd = new SqlCommand();
                checkCmd.Connection = cn;
                checkCmd.CommandType = CommandType.StoredProcedure;
                checkCmd.CommandText = "sp_useradmin";

                checkCmd.Parameters.AddWithValue("@userloginid", user.userloginid);
                checkCmd.Parameters.AddWithValue("@action", "N");

                SqlDataReader reader = checkCmd.ExecuteReader();
                if (reader.Read())
                {
                    // userloginid already exists
                    reader.Close();
                    return -2; // Specific value indicating userloginid exists
                }
                reader.Close();

                // Insert new user
                SqlCommand cm = new SqlCommand();
                cm.Connection = cn;
                cm.CommandType = CommandType.StoredProcedure;
                cm.CommandText = "sp_useradmin";

                cm.Parameters.AddWithValue("@username", user.username);
                cm.Parameters.AddWithValue("@userloginid", user.userloginid);
                cm.Parameters.AddWithValue("@userpass", user.userpass);
                cm.Parameters.AddWithValue("@userphone", user.userphone);
                cm.Parameters.AddWithValue("@useremail", user.useremail);
                cm.Parameters.AddWithValue("@useradd", user.useradd);
                cm.Parameters.AddWithValue("@isapproved", user.isapproved);
                cm.Parameters.AddWithValue("@action", "I");

                int x = cm.ExecuteNonQuery();
                return x;
            }
            catch (Exception ex)
            {
                return -1;
            }
        }
        public async Task<AdminModel> UserLogin(string userloginid, string userpass)
        {
            try
            {
                using (var cn = CreateConnection())
                {
                    if (cn.State == ConnectionState.Closed) await cn.OpenAsync();

                    using (SqlCommand cmd = new SqlCommand("sp_useradmin", cn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.AddWithValue("@userloginid", userloginid);
                        cmd.Parameters.AddWithValue("@userpass", userpass);
                        cmd.Parameters.AddWithValue("@action", "L");

                        using (SqlDataReader reader = await cmd.ExecuteReaderAsync())
                        {
                            if (await reader.ReadAsync())
                            {
                                var user = new AdminModel();
                                {
                                    user.username = reader["username"].ToString();
                                    user.userloginid = reader["userloginid"].ToString();
                                    user.userpass = reader["userpass"].ToString();
                                    user.userphone = reader["userphone"].ToString();
                                    user.useremail = reader["useremail"].ToString();
                                    user. useradd = reader["useradd"].ToString();
                                    user.isapproved = Convert.ToBoolean(reader["isapproved"]);
                                    user.isadmin = Convert.ToBoolean(reader["isadmin"]);
                                };
                                return user;
                            }
                            else
                            {
                                return null;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public async Task<string> ApproveUser(string userloginid)
        {
            try
            {
                var cn = CreateConnection();
                if (cn.State == ConnectionState.Closed) cn.Open();
                SqlCommand cm = new SqlCommand();
                cm.Connection = cn;
                cm.CommandType = CommandType.StoredProcedure;
                cm.CommandText = "sp_useradmin";
                cm.Parameters.AddWithValue("@userloginid", userloginid);
                cm.Parameters.AddWithValue("@action", "A");

                await cm.ExecuteNonQueryAsync();
                return "Success";
            }
            catch(Exception ex)
            {
             return null;
            }
        }

    }
}
