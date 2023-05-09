using Proyecto1.Models;
using SQLite;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Proyecto1.Controller
{
    public class DataBase
    {
        readonly SQLiteAsyncConnection dbase;

        public DataBase(string dbpath)
        {
            dbase = new SQLiteAsyncConnection(dbpath);
            dbase.CreateTableAsync<Lugares>();
            dbase.CreateTableAsync<User>();
        }

        public Task<int> SitioSave(Lugares sitio)
        {
            if (sitio.id != 0)
            {
                return dbase.UpdateAsync(sitio);
            }
            else
            {
                return dbase.InsertAsync(sitio);
            }
        }

        public async Task<bool> LoginAsync(string username, string password)
        {
            var user = await dbase.Table<User>()
                .Where(u => u.Username == username && u.Password == password)
                .FirstOrDefaultAsync();

            return user != null;
        }

        public async Task<bool> RegisterAsync(string username, string password)
        {
            var user = await dbase.Table<User>()
                .Where(u => u.Username == username)
                .FirstOrDefaultAsync();

            if (user != null)
            {
                // a user with this username already exists
                return false;
            }
            else
            {
                // create a new user and insert it into the database
                user = new User
                {
                    Username = username,
                    Password = password
                };

                await dbase.InsertAsync(user);
                return true;
            }
        }


        public Task<List<Lugares>> getListSitio()
        {
            return dbase.Table<Lugares>().ToListAsync();
        }

        public async Task<Lugares> getSitio(int pid)
        {
            return await dbase.Table<Lugares>()
                .Where(i => i.id == pid)
                .FirstOrDefaultAsync();
        }

        public async Task<int> DeleteSitio(Lugares sitio)
        {
            return await dbase.DeleteAsync(sitio);
        }
    }
}
