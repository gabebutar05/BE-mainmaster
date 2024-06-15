using API_Dinamis.Models;

namespace API_Dinamis.Interfaces
{
    public interface IAuthRepository
    {
        bool AuthExists(string user);
        bool UpdateAuth(Authx auth_);
        Authx Getid(string user, string pass);
        Authx Getid2(string user);
        Authx Getid_id(int id);
        object CheckingAccount(string? username, string? password);
    }
}
