using EvergreenAPI.DTO;
using EvergreenAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace EvergreenAPI.Repositories
{
    public class UserRepository : IUserRepository
    {
        public void DeleteUser(Account user)
        {
            try
            {
                using (var context = new AppDbContext())
                {
                    var u = context.Accounts.SingleOrDefault(
                      c => c.AccountId == user.AccountId);
                    context.Accounts.Remove(u);
                    context.SaveChanges();
                }
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }



        public Account GetUserByEmail(string email)
        {
            var u = new Account();
            try
            {
                using (var context = new AppDbContext())
                {
                    u = context.Accounts.SingleOrDefault(x => x.Email == email);
                }
            }
            catch (Exception e)
            {
                throw new Exception(e.Message); 
            }
            return u;
        }




        public Account GetUserById(int id)
        {
            Account u = new Account();
            try
            {
                using (var context = new AppDbContext())
                {
                    u = context.Accounts.SingleOrDefault(x => x.AccountId == id);
                }
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
            return u;
        }





        public List<Account> GetUsers()
        {
            var listUsers = new List<Account>();
            try
            {
                using (var context = new AppDbContext())
                {
                    listUsers = context.Accounts.ToList();
                }
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
            return listUsers;
        }

        


        public void SaveUser(Account user)
        {
            try
            {
                using (var context = new AppDbContext())
                {
                    user.Password = GetMD5HashData(user.Password);
                    context.Accounts.Add(user);
                    context.SaveChanges();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(user);
                throw new Exception(e.Message);
            }
        }







        public void UpdateUser(Account user)
        {
            try
            {
                using (var context = new AppDbContext())
                {
                    context.Entry<Account>(user).State =
                        Microsoft.EntityFrameworkCore.EntityState.Modified;
                    context.SaveChanges();

                }
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }




        private static string GetMD5HashData(string password)
        {
            //create new instance of md5
            MD5 md5 = MD5.Create();

            //convert the input text to array of bytes
            byte[] hashData = md5.ComputeHash(Encoding.Default.GetBytes(password));

            //create new instance of StringBuilder to save hashed data
            StringBuilder returnValue = new StringBuilder();

            //loop for each byte and add it to StringBuilder
            for (int i = 0; i < hashData.Length; i++)
            {
                returnValue.Append(hashData[i].ToString());
            }

            // return hexadecimal string
            return returnValue.ToString();
        }



    }
}
