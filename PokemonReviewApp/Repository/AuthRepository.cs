using API_Dinamis.Data;
using API_Dinamis.Interfaces;
using API_Dinamis.Models;
using API_Dinamis.Utilities;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualBasic;

namespace API_Dinamis.Repository
{
    public class AuthRepository : IAuthRepository
    {
        private readonly DataContext _context;
        private readonly IPasswordHasher _passwordHasher;
        string masterkey = "D7I16N25";

        public AuthRepository(DataContext context, IPasswordHasher passwordHasher)
        {
            _context = context;
            _passwordHasher = passwordHasher;
        }

        public bool AuthExists(string user)
        {
            try
            {
                var exists = _context.Auths.Any(p => p.UserName == user);
                return exists;
            }
            catch (Exception ex)
            {
                // Log the exception details for debugging
                Console.WriteLine($"An error occurred in AuthExists: {ex}");
                // Handle or rethrow the exception as needed
                throw;
            }
        }
        //public bool AuthExists(string user, string pass)
        //{
        //    Initialize(masterkey);

        //    string strA = DoXor(user);
        //    string _user = Stretch(strA);

        //    string strB = DoXor(pass);
        //    string _pass = Stretch(strB);

        //    Console.WriteLine(_user + "|" + _pass);

        //    return _context.Auths.Any(p => p.UserName == _user && p.Password == _pass);
        //}

        public bool UpdateAuth(Authx auth_)
        {
            _context.Update(auth_);
            return Save();
        }

        public Authx Getid(string user, string pass)
        {
            Initialize(masterkey);

            string strA = DoXor(user);
            string _user = Stretch(strA);

            string strB = DoXor(pass);
            string _pass = Stretch(strB);

            Console.WriteLine(_user + "|" + _pass);

            return _context.Auths.AsNoTracking().Where(p => p.UserName == _user && p.Password == _pass).FirstOrDefault();
        }

        public Authx Getid2(string user)
        {
            return _context.Auths.AsNoTracking().Where(p => p.UserName == user).FirstOrDefault();
        }

        public Authx Getid_id(int id)
        {
            return _context.Auths.AsNoTracking().Where(p => p.id == id).FirstOrDefault();
        }

        public bool Save()
        {
            var saved = _context.SaveChanges();
            return saved > 0 ? true : false;
        }

        public void Initialize(string masterkey)
        {
            int lngN;

            VBMath.Randomize(VBMath.Rnd(-1));

            for (lngN = 0; lngN < masterkey.Length; lngN++)
            {
                VBMath.Randomize(VBMath.Rnd(-VBMath.Rnd() * Strings.Asc(masterkey[lngN])));
            }
        }

        public string DoXor(string mstrText)
        {
            int lngC;
            int intB;
            int lngN;
            double dblX;
            string strA = "";

            try
            {
                for (lngN = 0; lngN < mstrText.Length; lngN++)
                {
                    lngC = Convert.ToInt32(mstrText[lngN]);
                    dblX = Math.Truncate(VBMath.Rnd() * 256);
                    intB = Convert.ToInt32(dblX);
                    strA += (char)(lngC ^ intB);


                }
                return strA;
            }
            catch (Exception ex)
            {
                return ex.ToString();
            }
        }

        public string Stretch(string mstrText)
        {
            int lngA;
            int lngB;
            int lngC;
            int lngN;
            int lngJ = 0;
            int lngK = 0;
            string strA = "";
            char[] chrB = new char[50];

            lngA = mstrText.Length;
            lngB = lngA + Convert.ToInt32((lngA + 2) / 3);

            for (lngN = 1; lngN <= lngA; lngN++)
            {
                lngC = Convert.ToInt32(mstrText[lngN - 1]);
                lngJ = lngJ + 1;
                chrB[lngJ] = (char)((lngC & 63) + 59);
                switch (lngN % 3)
                {
                    case 0:
                        {
                            lngK = lngK | Convert.ToInt32(lngC / 64);
                            lngJ = lngJ + 1;
                            chrB[lngJ] = (char)(lngK + 59);
                            lngK = 0;
                            break;
                        }
                    case 1:
                        {
                            lngK = lngK | Convert.ToInt32(lngC / 64) * 16;
                            break;
                        }
                    case 2:
                        {
                            lngK = lngK | Convert.ToInt32(lngC / 64) * 4;
                            break;
                        }
                }
            }
            if (lngA % 3 != 0)
            {
                lngJ = lngJ + 1;
                chrB[lngJ] = (char)(lngK + 59);
            }

            strA = "";
            for (lngN = 1; lngN <= lngB; lngN++)
            {
                strA += chrB[lngN];
            }
            return strA;
        }

        public string Shrink(string mstrText)
        {
            int lngC;
            int lngD = 0;
            int lngE = 0;
            int lngA;
            int lngB;
            int lngN;
            int lngJ = 0;
            int lngK = 0;
            string strA = "";

            lngA = mstrText.Length;
            lngB = lngA - 1 - Convert.ToInt32((lngA - 1) / 4);

            for (lngN = 1; lngN <= lngB; lngN++)
            {
                lngJ = lngJ + 1;
                lngC = Strings.Asc(mstrText[lngJ - 1]) - 59;

                switch (lngN % 3)
                {
                    case 0:
                        {
                            lngD = (lngE & 3) * 64;
                            lngJ = lngJ + 1;
                            break;
                        }
                    case 1:
                        {
                            lngK = lngK + 4;
                            if (lngK > lngA) lngK = lngA;
                            lngE = Strings.Asc(mstrText[lngK - 1]) - 59;
                            lngD = (Convert.ToInt32(lngE / 16) & 3) * 64;
                            break;
                        }
                    case 2:
                        {
                            lngD = (Convert.ToInt32(lngE / 4) & 3) * 64;
                            break;
                        }
                }
                strA += Convert.ToString((char)(lngC | lngD));

            }
            return strA;
        }

        public object CheckingAccount(string? userName="", string? password="")
        {
            bool result = true;
            string msg = "";
            if (userName == "" || password == "")
            {
                result = false;
                msg = "username and password is required";
            }

            var userChecking = Getid2(userName);
            if(userChecking == null) {
                result = false;
                msg = "username or password is invalid";
            }

            var passwordChecking = _passwordHasher.Verify(userChecking.Password, password);
            if (!passwordChecking)
            {
                result = false;
                msg = "username or password is invalid";
            }

            return ApiResponseHelper.ValidationResponse(result, msg);
        }
    }
}
