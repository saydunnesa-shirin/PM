using PM.Repository.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace PM.Repository
{
    public static class DbInitializer
    {
        public static void Initialize(ProductContext context)
        {
            context.Database.EnsureCreated();

            if (context.Products.Any())
            {
                return;
            }

            #region Seed ProductGroup data

            var rootProductGroupFood = new ProductGroup[]
                {
                new ProductGroup { Name = "Foods" },
                new ProductGroup { Name = "Clothes" },
                new ProductGroup { Name = "Electronics And Computers" },
                new ProductGroup { Name = "Furniture" },
                new ProductGroup { Name = "Stationary" },
                new ProductGroup { Name = "Toiletries" }
                };
            foreach (ProductGroup pg in rootProductGroupFood)
            {
                context.ProductGroups.Add(pg);
            }
            context.SaveChanges();

            //Foods
            var foodSubGroupFish = new ProductGroup { Name = "Fishes", Parent = rootProductGroupFood.First(x => x.Name.Equals("Foods")) };
            var foodSubGroupMeat = new ProductGroup { Name = "Meats", Parent = rootProductGroupFood.First(x => x.Name.Equals("Foods")) };
            var foodSubGroupBread = new ProductGroup { Name = "Breads", Parent = rootProductGroupFood.First(x => x.Name.Equals("Foods")) };
            var foodSubGroupVegetable = new ProductGroup { Name = "Vegetables", Parent = rootProductGroupFood.First(x => x.Name.Equals("Foods")) };

            var plainBreadFoodSubGroupBread = new ProductGroup { Name = "Plain Bread", Parent = foodSubGroupBread };
            var productGroups = new ProductGroup[]
            {
                foodSubGroupFish,
                new ProductGroup { Name = "Salmon", Parent = foodSubGroupFish },
                new ProductGroup { Name = "Herring", Parent = foodSubGroupFish },
                new ProductGroup { Name = "Tuna", Parent = foodSubGroupFish },

                foodSubGroupMeat,
                new ProductGroup { Name = "Chicken", Parent = foodSubGroupFish },
                new ProductGroup { Name = "Beef", Parent = foodSubGroupFish },
                new ProductGroup { Name = "Pork", Parent = foodSubGroupFish },

                foodSubGroupBread,
                plainBreadFoodSubGroupBread,
                new ProductGroup { Name = "Rye", Parent = foodSubGroupBread },

                foodSubGroupVegetable,
                new ProductGroup { Name = "Tomato", Parent = foodSubGroupVegetable },
                new ProductGroup { Name = "Potato", Parent = foodSubGroupVegetable },
                new ProductGroup { Name = "Spinach", Parent = foodSubGroupVegetable },
                new ProductGroup { Name = "Onion", Parent = foodSubGroupVegetable },
                new ProductGroup { Name = "Broccoli", Parent = foodSubGroupVegetable }
            };
            foreach (ProductGroup pg in productGroups)
            {
                context.ProductGroups.Add(pg);
            }
            context.SaveChanges();
            Array.Clear(productGroups, 0, productGroups.Length);

            //Clothes
            var clothesSubGroupChild = new ProductGroup { Name = "Children", Parent = rootProductGroupFood.First(x => x.Name.Equals("Clothes")) };
            var clothesSubGroupWomen = new ProductGroup { Name = "Women", Parent = rootProductGroupFood.First(x => x.Name.Equals("Clothes")) };
            var clothesSubGroupMen = new ProductGroup { Name = "Men", Parent = rootProductGroupFood.First(x => x.Name.Equals("Clothes")) };
            productGroups = new ProductGroup[]
            {
                clothesSubGroupChild,
                new ProductGroup { Name = "Jackets", Parent = clothesSubGroupChild },
                new ProductGroup { Name = "Overall", Parent = clothesSubGroupChild },

                clothesSubGroupWomen,
                new ProductGroup { Name = "Shoe", Parent = clothesSubGroupWomen },
                new ProductGroup { Name = "Shirt", Parent = clothesSubGroupWomen },
                new ProductGroup { Name = "Trouser", Parent = clothesSubGroupWomen },

                clothesSubGroupMen,
                new ProductGroup { Name = "Shoe", Parent = clothesSubGroupMen },
                new ProductGroup { Name = "Trouser", Parent = clothesSubGroupMen },
                new ProductGroup { Name = "Hoodie", Parent = clothesSubGroupMen }
            };
            foreach (ProductGroup pg in productGroups)
            {
                context.ProductGroups.Add(pg);
            }
            context.SaveChanges();
            Array.Clear(productGroups, 0, productGroups.Length);

            #endregion


            #region Seed Store data

            var stores = new Store[]
            {
                new Store { Name = "Haabersti" },
                new Store { Name = "Kesklinn" },
                new Store { Name = "Kristiine" },
                new Store { Name = "Lasnamäe" },
                new Store { Name = "Mustamäe" },
                new Store { Name = "Nomme" },
                new Store { Name = "Pirita" },
                new Store { Name = "Pohja-Tallinn" }
            };
            foreach (Store s in stores)
            {
                context.Stores.Add(s);
            }
            context.SaveChanges();

            #endregion


            #region Seed some Product data

            var products = new Product[]
            {
                new Product 
                {
                    Name = "Bread 001", EntryTime = DateTime.UtcNow, Price = 2.5m, PriceWithVat = 3.0m, VatRate = 10, ProductGroupId = plainBreadFoodSubGroupBread.ProductGroupId,
                    Stores = new List<Store>
                    {
                        stores.First(x=>x.Name.Equals("Haabersti")),
                        stores.First(x=>x.Name.Equals("Kesklinn")),
                        stores.First(x=>x.Name.Equals("Lasnamäe")),
                        stores.First(x=>x.Name.Equals("Pirita"))
                    }
                }
                //new Product
                //{
                //    Name = "Fish 003", EntryTime = DateTime.UtcNow, Price = 10m, PriceWithVat = 11.5m, VatRate = 15, ProductGroupId = 11,
                //    Stores = new List<Store>
                //    {
                //        stores.First(x=>x.Name.Equals("Haabersti")),
                //        stores.First(x=>x.Name.Equals("Pirita"))
                //    }
                //},
                //new Product
                //{
                //    Name = "Sofa 101", EntryTime = DateTime.UtcNow, Price = 100m, PriceWithVat = 120m, VatRate = 20, ProductGroupId = 15,
                //    Stores = new List<Store>
                //    {
                //        stores.First(x=>x.Name.Equals("Kesklinn")),
                //        stores.First(x=>x.Name.Equals("Lasnamäe")),
                //        stores.First(x=>x.Name.Equals("Nomme"))
                //    }
                //}
            };
            foreach (Product p in products)
            {
                context.Products.Add(p);
            }
            context.SaveChanges();

            #endregion
        }
    }
}
