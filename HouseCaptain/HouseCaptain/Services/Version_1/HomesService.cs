using HouseCaptain.Entities;
using Humanizer;
using SQLite;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;

namespace HouseCaptain.Services.Version_1
{
    public static class HomesService
    {
         private static SQLiteAsyncConnection db;
        private static async Task InitializeConnection()
        {
            if (db == null)
            {
                String path = Path.Combine(FileSystem.AppDataDirectory, "Tracky.db");
                db = new SQLiteAsyncConnection(path);

                //if db is null then we create connection and create Homses table
                await db.CreateTableAsync<HomesEntity>();
            }
            else
            {
                return;
            }
        }

        public static async Task<int> AddHomeAsync(HomesEntity Home)
        {
            //Initializing connection
            await InitializeConnection();

            HomesEntity tempHome = new HomesEntity
            {
                Name = Home.Name.Transform(To.TitleCase)
           
            };

            //adding the object to the database
            var status =   await  db.InsertAsync(tempHome);
            return status;

        }

        public static async Task<List<HomesEntity>> GetAllHomesAsync()
        {
            //Initializing connection
            await InitializeConnection();

            return await db.Table<HomesEntity>()
                            .Where(x => x.Status==1)
                            .ToListAsync();
        }

        public static async Task<HomesEntity> GetSingleHomesAsync(int Id)
        {
            //Initializing connection
            await InitializeConnection();

            return await db.Table<HomesEntity>()
                        .Where(x => x.Id==Id)
                        .FirstOrDefaultAsync();
        }


        public static async Task<int> DeleteHomesAsync(int Id)
        {
            //Initializing connection
            await InitializeConnection();

            var data = await db.Table<HomesEntity>() .Where(x => x.Id == Id).FirstOrDefaultAsync();

            data.Status = 0;
            return await db.UpdateAsync(data);
        }

        public static async Task<int> UpdateHomesAsync(HomesEntity Home)
        {
            //Initializing connection
            await InitializeConnection();

            var data = await db.Table<HomesEntity>().Where(x => x.Id == Home.Id).FirstOrDefaultAsync();

            data.Name = Home.Name;
            data.LastModificationDate = DateTime.Now;

            return await db.UpdateAsync(data);
        }


    }
}
