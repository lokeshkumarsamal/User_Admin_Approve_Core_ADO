using User_Admin_Approve_Core_ADO.Models;

namespace User_Admin_Approve_Core_ADO.Services
{
    public interface IUserServices
    {
        Task<int> AddUserAsync(AdminModel user);
        Task<string> ApproveUserAsync(string userloginid);
        //Task<IEnumerable<AdminModel>> GetAllUsers();
        Task<List<AdminModel>> GetAllUsersAsync();
        Task<AdminModel> UserLoginAsync(string userloginid, string userpass);
    }
}
