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
                new ProductGroup { Name = "Chicken", Parent = foodSubGroupMeat },
                new ProductGroup { Name = "Beef", Parent = foodSubGroupMeat },
                new ProductGroup { Name = "Pork", Parent = foodSubGroupMeat },

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

            //Electronics And Computers
            var ecSubGroupLaptop = new ProductGroup { Name = "Laptop", Parent = rootProductGroupFood.First(x => x.Name.Equals("Electronics And Computers")) };
            var ecSubGroupTablet = new ProductGroup { Name = "Tablet", Parent = rootProductGroupFood.First(x => x.Name.Equals("Electronics And Computers")) };
            var ecSubGroupPhone = new ProductGroup { Name = "Phone", Parent = rootProductGroupFood.First(x => x.Name.Equals("Electronics And Computers")) };
            productGroups = new ProductGroup[]
            {
                ecSubGroupLaptop,
                new ProductGroup { Name = "HP", Parent = ecSubGroupLaptop },
                new ProductGroup { Name = "Dell", Parent = ecSubGroupLaptop },
                new ProductGroup { Name = "Acer", Parent = ecSubGroupLaptop },

                ecSubGroupTablet,
                new ProductGroup { Name = "Microsoft Surface", Parent = ecSubGroupTablet },
                new ProductGroup { Name = "Apple", Parent = ecSubGroupTablet },
                new ProductGroup { Name = "Lenovo", Parent = ecSubGroupTablet },

                ecSubGroupPhone,
                new ProductGroup { Name = "Samsung", Parent = ecSubGroupPhone },
                new ProductGroup { Name = "iPhone", Parent = ecSubGroupPhone },
                new ProductGroup { Name = "Huawei", Parent = ecSubGroupPhone }
            };
            foreach (ProductGroup pg in productGroups)
            {
                context.ProductGroups.Add(pg);
            }
            context.SaveChanges();
            Array.Clear(productGroups, 0, productGroups.Length);

            //Furniture
            var furnitureSubGroupSofa = new ProductGroup { Name = "Sofa", Parent = rootProductGroupFood.First(x => x.Name.Equals("Furniture")) };
            var furnitureSubGroupTable = new ProductGroup { Name = "Table", Parent = rootProductGroupFood.First(x => x.Name.Equals("Furniture")) };
            var furnitureSubGroupCupboard = new ProductGroup { Name = "Cupboard", Parent = rootProductGroupFood.First(x => x.Name.Equals("Furniture")) };
            productGroups = new ProductGroup[]
            {
                furnitureSubGroupSofa,
                new ProductGroup { Name = "HP", Parent = furnitureSubGroupSofa },
                new ProductGroup { Name = "Dell", Parent = furnitureSubGroupSofa },
                new ProductGroup { Name = "Acer", Parent = furnitureSubGroupSofa },

                furnitureSubGroupTable,
                new ProductGroup { Name = "Folding Table", Parent = furnitureSubGroupTable },
                new ProductGroup { Name = "Coffee Table", Parent = furnitureSubGroupTable },
                new ProductGroup { Name = "Round Table", Parent = furnitureSubGroupTable },

                furnitureSubGroupCupboard,
                new ProductGroup { Name = "Linen Cupboard", Parent = furnitureSubGroupCupboard },
                new ProductGroup { Name = "Stationary Cupboard", Parent = furnitureSubGroupCupboard },
                new ProductGroup { Name = "Airing Cupboard", Parent = furnitureSubGroupCupboard }
            };
            foreach (ProductGroup pg in productGroups)
            {
                context.ProductGroups.Add(pg);
            }
            context.SaveChanges();
            Array.Clear(productGroups, 0, productGroups.Length);

            //Stationary
            var stationarySubGroupPaper = new ProductGroup { Name = "Paper", Parent = rootProductGroupFood.First(x => x.Name.Equals("Stationary")) };
            var stationarySubGroupFile = new ProductGroup { Name = "File and Folder", Parent = rootProductGroupFood.First(x => x.Name.Equals("Stationary")) };
            var stationarySubGroupPresentation = new ProductGroup { Name = "Presentation Supplies", Parent = rootProductGroupFood.First(x => x.Name.Equals("Stationary")) };
            productGroups = new ProductGroup[]
            {
                stationarySubGroupPaper,
                new ProductGroup { Name = "A4", Parent = stationarySubGroupPaper },
                new ProductGroup { Name = "A5", Parent = stationarySubGroupPaper },
                new ProductGroup { Name = "B3", Parent = stationarySubGroupPaper },

                stationarySubGroupFile,
                new ProductGroup { Name = "File Box", Parent = stationarySubGroupFile },
                new ProductGroup { Name = "Ring Binder", Parent = stationarySubGroupFile },
                new ProductGroup { Name = "Clip Files", Parent = stationarySubGroupFile },

                stationarySubGroupPresentation,
                new ProductGroup { Name = "Whiteboards", Parent = stationarySubGroupPresentation },
                new ProductGroup { Name = "Markers", Parent = stationarySubGroupPresentation },
                new ProductGroup { Name = "Erasers", Parent = stationarySubGroupPresentation }
            };
            foreach (ProductGroup pg in productGroups)
            {
                context.ProductGroups.Add(pg);
            }
            context.SaveChanges();
            Array.Clear(productGroups, 0, productGroups.Length);

            //Stationary
            var toiletriesSubGroupSoap = new ProductGroup { Name = "Soap", Parent = rootProductGroupFood.First(x => x.Name.Equals("Toiletries")) };
            var toiletriesSubGroupShampoo = new ProductGroup { Name = "Shampoo", Parent = rootProductGroupFood.First(x => x.Name.Equals("Toiletries")) };
            var toiletriesSubGroupToothPaste = new ProductGroup { Name = "ToothPaste", Parent = rootProductGroupFood.First(x => x.Name.Equals("Toiletries")) };
            productGroups = new ProductGroup[]
            {
                toiletriesSubGroupSoap,
                new ProductGroup { Name = "Aveeno", Parent = toiletriesSubGroupSoap },
                new ProductGroup { Name = "Olay", Parent = toiletriesSubGroupSoap },
                new ProductGroup { Name = "Palmolive", Parent = toiletriesSubGroupSoap },

                toiletriesSubGroupShampoo,
                new ProductGroup { Name = "TRESemme", Parent = toiletriesSubGroupShampoo },
                new ProductGroup { Name = "Pantene", Parent = toiletriesSubGroupShampoo },
                new ProductGroup { Name = "Sunsilk", Parent = toiletriesSubGroupShampoo },

                toiletriesSubGroupToothPaste,
                new ProductGroup { Name = "Colgate", Parent = toiletriesSubGroupToothPaste },
                new ProductGroup { Name = "Sensodyne", Parent = toiletriesSubGroupToothPaste },
                new ProductGroup { Name = "Crest", Parent = toiletriesSubGroupToothPaste }
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
