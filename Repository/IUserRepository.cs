using User_Admin_Approve_Core_ADO.Models;

namespace User_Admin_Approve_Core_ADO.Repository
{
    public interface IUserRepository
    {
        Task<int>AddUser(AdminModel user);
        Task<string> ApproveUser(string userloginid);
        //Task<IEnumerable<AdminModel>> GetAllUsers();
        Task<List<AdminModel>> GetAllUsers();
        Task<AdminModel> UserLogin(string userloginid, string userpass);
    }
}
