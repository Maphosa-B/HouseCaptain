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
    public class ShoppingService
    {
        private static SQLiteAsyncConnection db;
        private static async Task InitializeConnection()
        {
            if (db == null)
            {
                String path = Path.Combine(FileSystem.AppDataDirectory, "Tracky.db");
                db = new SQLiteAsyncConnection(path);

                //if db is null then we create connection and create Homses table
                await db.CreateTableAsync<ShoppingItemEntity>();
            }
            else
            {
                return;
            }
        }

        public static async Task<int> AddShoppingItemAsync(ShoppingItemEntity ShoppingItem)
        {
            //Initializing connection
            await InitializeConnection();

            ShoppingItemEntity tempShoppingItem = new ShoppingItemEntity
            {
                Name = ShoppingItem.Name.Transform(To.TitleCase),
                ImgUrl = ShoppingItem.ImgUrl,
                Category = ShoppingItem.Category,
                Notes =ShoppingItem.Notes,
                HomeId = ShoppingItem.HomeId,
                Quantity = ShoppingItem.Quantity,
                QuantityType = ShoppingItem.QuantityType
                
            };

            //adding the object to the database
            var status = await db.InsertAsync(tempShoppingItem);
            return status;

        }

        public static async Task<List<ShoppingItemEntity>> GetAllShoppingItemsAsync(int HomeId)
        {
            //Initializing connection
            await InitializeConnection();

            return await db.Table<ShoppingItemEntity>()
                            .Where(x => x.Status == 1 && x.HomeId==HomeId )
                            .ToListAsync();
        }

        public static async Task<ShoppingItemEntity> GetSingleShoppingItemsAsync(int Id)
        {
            //Initializing connection
            await InitializeConnection();

            return await db.Table<ShoppingItemEntity>()
                        .Where(x => x.Id == Id)
                        .FirstOrDefaultAsync();
        }


        public static async Task<int> DeleteShoppingItemAsync(int Id)
        {
            //Initializing connection
            await InitializeConnection();

            var data = await db.Table<ShoppingItemEntity>().Where(x => x.Id == Id).FirstOrDefaultAsync();

            data.Status = 0;
            return await db.UpdateAsync(data);
        }

        public static async Task<int> UpdateHomesAsync(ShoppingItemEntity ShoppingItem)
        {
            //Initializing connection
            await InitializeConnection();

            var data = await db.Table<ShoppingItemEntity>().Where(x => x.Id == ShoppingItem.Id).FirstOrDefaultAsync();

            data.Name = ShoppingItem.Name.Transform(To.TitleCase);
            data.Quantity = ShoppingItem.Quantity;
            data.Notes = ShoppingItem.Notes;
            data.ImgUrl = ShoppingItem.ImgUrl;
            data.LastModificationDate = DateTime.Now;
            data.QuantityType = ShoppingItem.QuantityType;

            return await db.UpdateAsync(data);
        }

    }
}
