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
                String path = Path.Combine(FileSystem.AppDataDirectory, "HouseCaptain.db");
                db = new SQLiteAsyncConnection(path);

                //if db is null then we create connection and create Homses table
                await db.CreateTablesAsync<ShoppingItemEntity, RegularItemsEntity>();

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

        public static async Task<List<ShoppingItemEntity>> GetAInitialShoppingItemsAsync(int HomeId)
        {
            //Initializing connection
            await InitializeConnection();

            return await db.Table<ShoppingItemEntity>()
                            .Where(x => x.Status == 1 && x.HomeId==HomeId )
                            .OrderBy(x => x.Category)
                            .Take(12)
                            .ToListAsync();
        }   
        


        public static async Task<List<ShoppingItemEntity>> GetInitialShoppingItemsHistoryAsync(int HomeId)
        {
          
            await InitializeConnection();

            return await db.Table<ShoppingItemEntity>()
                            .Where(x => x.Status == 2 && x.HomeId == HomeId)
                            .OrderBy(x => x.Category)
                            .Take(12)
                            .ToListAsync();
        } 
        
        public static async Task<List<RegularItemsEntity>> GetInitialRegulaItemsAsync(int HomeId)
        {
            //Initializing connection
            await InitializeConnection();
            return await db.Table<RegularItemsEntity>()
                    .Where(x => x.Status == 1 && x.HomeId == HomeId)
                    .OrderBy(x => x.Category)
                    .Take(12)
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

        public static async Task<RegularItemsEntity> GetSingleRegularShoppingItemsAsync(int Id)
        {
            //Initializing connection
            await InitializeConnection();

            return await db.Table<RegularItemsEntity>()
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
        
        public static async Task<int> DeleteRegularItemAsync(int Id)
        {
            //Initializing connection
            await InitializeConnection();

            return await db.DeleteAsync<RegularItemsEntity>(Id);
           
        }

        public static async Task<int> UpdateShoppingItemAsync(ShoppingItemEntity ShoppingItem)
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

        public static async Task<int> CheckoutItemAsync(int Id)
        {
            //Initializing connection
            await InitializeConnection();


            var data = await db.Table<ShoppingItemEntity>().Where(x => x.Id == Id).FirstOrDefaultAsync();


            //Check if an item with same name and Image and Quantity type has been addded before to regular table else add new record

            var CheckItemOnRegularTable = await db.Table<RegularItemsEntity>()
                                            .Where(x => x.Name.ToUpper().Equals(data.Name.ToUpper()))
                                            .FirstOrDefaultAsync();

            //if it exists we append it add num which will be used to list them in regular lis
            if(CheckItemOnRegularTable==null)
            {
                RegularItemsEntity regularItem = new RegularItemsEntity
                {
                    AddCount =1,
                    ImgUrl = data.ImgUrl,
                    QuantityType = data.QuantityType,
                    Name = data.Name ,
                    HomeId = data.HomeId,
                    Category = data.Category
                };

                //Adding the itemto the list
               await  db.InsertAsync(regularItem);

            }else
            {
                CheckItemOnRegularTable.AddCount++;
            }

            data.Status = 2;

            return await db.UpdateAsync(data);
        }

        public static async Task<int> CancelItemAsync(int Id)
        {
            //Initializing connection
            await InitializeConnection();

            var data = await db.Table<ShoppingItemEntity>().Where(x => x.Id == Id).FirstOrDefaultAsync();

            return await db.DeleteAsync<ShoppingItemEntity>(Id);
        }

        //--------------------------------Below function are made to collect more data
        public static async Task<List<ShoppingItemEntity>> LoadMoreShoppingItemsAsync(int HomeId, int Range)
        {
            //Initializing connection
            await InitializeConnection();

            return await db.Table<ShoppingItemEntity>()
                            .Where(x => x.Status == 1 && x.HomeId == HomeId)
                            .OrderBy(x => x.Category)
                            .Skip(Range + 12)
                            .Take(3)
                            .ToListAsync();
        }
        public static async Task<List<ShoppingItemEntity>> LoadMoreShoppingItemsHistoryAsync(int HomeId, int Range)
        {
            //Initializing connection
            await InitializeConnection();

            return await db.Table<ShoppingItemEntity>()
                           .Where(x => x.Status == 2 && x.HomeId == HomeId)
                           .OrderBy(x => x.Category)
                           .Skip(Range + 12)
                            .Take(3)
                           .ToListAsync();
        }

        public static async Task<List<RegularItemsEntity>> LoadMoreRegularItemsAsync(int HomeId, int Range)
        {
            //Initializing connection
            await InitializeConnection();

            return await db.Table<RegularItemsEntity>()
                           .Where(x => x.Status == 2 && x.HomeId == HomeId)
                           .OrderBy(x => x.Category)
                           .Skip(Range + 12)
                           .Take(3)
                           .ToListAsync();
        }

    }
}
