using User_Admin_Approve_Core_ADO.Repository;
using User_Admin_Approve_Core_ADO.Models;

namespace User_Admin_Approve_Core_ADO.Services
{
    public class UserService:IUserServices
    {
        protected readonly IUserRepository _userRepo;

        public UserService(IUserRepository userRepo)
        {
            _userRepo = userRepo;
        }

        public async Task<List<AdminModel>> GetAllUsersAsync()
        {
            try
            {
                return await _userRepo.GetAllUsers();
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        
        public async Task<int> AddUserAsync(AdminModel user)
        {
            try
            {
                return await _userRepo.AddUser(user);
            }
            catch (Exception ex)
            {
                return 0;
            }
        }
        public async Task<string> ApproveUserAsync(string userloginid)
        {
            try
            {
                return await _userRepo.ApproveUser(userloginid);
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        
        public async Task<AdminModel> UserLoginAsync(string userloginid, string userpass)
        {
            try
            {
                return await _userRepo.UserLogin(userloginid, userpass);
            }
            catch (Exception ex)
            {
                return null;
            }
        }
    }
}
