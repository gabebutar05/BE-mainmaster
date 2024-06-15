using API_Dinamis.Models;

namespace API_Dinamis.Interfaces
{
    public interface IKeyRepository
    {
        string GetKey();
        bool KeyExists();
        bool CreateKey(Key Key_);
        Key GetKeys();
    }
}
