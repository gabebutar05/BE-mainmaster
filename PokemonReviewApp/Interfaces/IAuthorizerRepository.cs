using API_Dinamis.Dto;

namespace API_Dinamis.Interfaces
{
    public interface IAuthorizerRepository
    {
        //get list
        ICollection<AuthorizerList> GetAuthorizers(int? limit = 0, int? page = 1, string? sortby = "D", string? sortdesc = "Id", string? keyword = "");
        ICollection<AuthorizerTempList> GetAuthorizersTemp(int? limit = 0, int? page = 1, string? sortby = "D", string? sortdesc = "Id", string? keyword = "");

        //get by id
    }
}
