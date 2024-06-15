using API_Dinamis.Data;
using API_Dinamis.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using API_Dinamis.Models;
using System.Text;

namespace API_Dinamis.Repository
{
    public class KeyRepository : IKeyRepository
    {
        private readonly DataContext _context;
        public KeyRepository(DataContext context)
        {
            _context = context;
        }

        public bool CreateKey(Models.Key key_)
        {
            //string inputString = "Hello, Todays date is " + DateTime.Now;
            //// Convert string to byte array using UTF-8 encoding
            //byte[] byteArray = Encoding.UTF8.GetBytes(inputString);

            _context.Add(key_);
            return Save();
        }

        public string GetKey()
        {
            return _context.Keys.OrderBy(p => p.Keys).Any().ToString();
        }

        public Models.Key GetKeys()
        {
            return _context.Keys.OrderBy(p => p.Keys).FirstOrDefault();
        }

        public bool KeyExists()
        {
            return _context.Keys.Any();
        }

        public bool Save()
        {
            var saved = _context.SaveChanges();
            return saved > 0 ? true : false;
        }
    }
}
