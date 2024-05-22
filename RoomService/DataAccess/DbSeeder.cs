using RoomService.Models;

namespace RoomService.DataAccess;

public static class DbSeeder
{
    public static List<Amenity> GetAmenities()
    {
        return new List<Amenity>()
        {
            new Amenity
            {
                Id = Guid.Parse("a40fe8b6-352e-4fb6-8728-3b5df13e9d15"),
                Title = "Кондиционер",
                Description = "Регулируйте температуру в вашем номере с помощью кондиционера."
            },
            new Amenity
            {
                Id = Guid.Parse("e4e39867-7aa3-4624-8c49-1b9c8d6e8e55"),
                Title = "Мини-бар (не входит в стоимость номера)",
                Description = "Насладитесь ассортиментом напитков из мини-бара."
            },
            new Amenity
            {
                Id = Guid.Parse("7d236fae-c5ea-4f0d-947f-60ac512af834"),
                Title = "Фен, ванные принадлежности",
                Description = "Предоставляются фен и основные ванные принадлежности."
            },
            new Amenity
            {
                Id = Guid.Parse("0e2ccaf6-2f0d-4a6e-a4f7-5d2a45be63c4"),
                Title = "Сейф",
                Description = "Храните ваши ценности в безопасности в сейфе в вашем номере."
            },
            new Amenity
            {
                Id = Guid.Parse("2b6607b0-8849-4228-9de2-7e1aaf262dc1"),
                Title = "Бутилированная вода, бесплатно",
                Description = "Бесплатная бутилированная вода предоставляется в вашем номере."
            },
            new Amenity
            {
                Id = Guid.Parse("3a97ad5d-8d75-493b-91f3-b1a7a7e39843"),
                Title = "Кофеварка/чайник",
                Description = "Приготовьте свой собственный кофе или чай прямо в вашем номере."
            },
            new Amenity
            {
                Id = Guid.Parse("12b2e9b7-d881-4fa2-b392-6357f20d4e51"),
                Title = "Мгновенная горячая вода",
                Description = "Наслаждайтесь моментальным доступом к горячей воде в вашем номере."
            },
            new Amenity
            {
                Id = Guid.Parse("f7223ef4-d8b4-4a4b-b07d-8b6b6b6fd769"),
                Title = "Минибар, платно",
                Description = "Выберите напитки из ассортимента платного мини-бара."
            },
            new Amenity
            {
                Id = Guid.Parse("7b6229e3-4d6b-4678-b68c-602079358ab0"),
                Title = "Отдельная ванна и душ",
                Description = "Наслаждайтесь отдельной ванной и душем в вашем номере."
            },
            new Amenity
            {
                Id = Guid.Parse("9f86b9f4-0511-4b82-845e-16906d110ed6"),
                Title = "Двойной туалетный столик",
                Description = "Удобный двойной туалетный столик в вашем номере."
            },
            new Amenity
            {
                Id = Guid.Parse("2e45f74b-6663-4f9d-aa18-c28b35e3e5e1"),
                Title = "Зеркало для макияжа с подсветкой",
                Description = "Идеальное освещение для макияжа в вашем номере."
            },
            new Amenity
            {
                Id = Guid.Parse("e2e6356d-b23c-4459-9df6-2c850476c12b"),
                Title = "Фен",
                Description = "Предостоставляется фен для использования в вашем номере."
            },
            new Amenity
            {
                Id = Guid.Parse("72a96249-f4fb-4e4d-aaee-c3c4b41f826e"),
                Title = "Халат",
                Description = "Уютный халат для вашего комфорта в номере."
            },
            new Amenity
            {
                Id = Guid.Parse("afd3c144-33c7-40cf-98c2-c04c4a4b2c46"),
                Title = "Домашние тапочки",
                Description = "Предоставляются тапочки для вашего удобства в номере."
            }
        };
    }

    public static List<RoomType> GetRoomTypes()
    {
        return new List<RoomType>()
        {
            new RoomType()
            {
                Id = new Guid("DE94604D-F0A2-47BB-940A-7200A0F2D688"),
                Title = "STANDARD ROOM",
                Description = "Уютный однокомнатный номер с двумя раздельными или одной двуспальной кроватью. Экономичное и уютное решения для прибывания.",
                Images = new List<ImageForRoomType>
                {
                    new ImageForRoomType
                    {
                        Path = "1.png"
                    },
                    new ImageForRoomType
                    {
                        Path = "2.png"
                    }
                },
                MaxCapacity = 2,
                NightlyRate = 45000,
                Amenities = new List<Amenity>()
                {
                    new Amenity
                    {
                        Id = Guid.Parse("e46e84fc-9966-40a1-aa10-d3e53f58a1b8"),
                        Title = "Бесплатный Wi-Fi",
                        Description = "Наслаждайтесь бесплатным доступом к Wi-Fi во время вашего пребывания."
                    },
                    new Amenity
                    {
                        Id = Guid.Parse("6b8e1e19-37cf-4824-90ae-4d882e68f69b"),
                        Title = "Ванная комната",
                        Description = "Просторная ванная комната с удобствами."
                    },
                    new Amenity
                    {
                        Id = Guid.Parse("c52e1aa7-4d7d-43b3-8f2d-1e8a9ad58311"),
                        Title = "Кабельное телевидение",
                        Description = "Наслаждайтесь просмотром кабельного телевидения в вашем номере."
                    },
                    new Amenity
                    {
                        Id = Guid.Parse("d6174b72-6240-4266-a08d-75924b7d39c8"),
                        Title = "Беспроводной доступ в Интернет",
                        Description = "Подключайтесь к Интернету из любой точки отеля."
                    },
                    new Amenity
                    {
                        Id = Guid.Parse("61af1068-8f1b-4f64-9645-4df64f816e77"),
                        Title = "Прямая телефонная линия",
                        Description = "Пользуйтесь прямой телефонной связью из вашего номера."
                    }
                }
            },
            new RoomType()
            {
                Id = new Guid("D2C64545-9807-4E51-85FE-EE746044BAA3"),
                Title = "SUPERIOR ROOM",
                Description = "Комфортабельный однокомнатный номер с интерьером в стиле модерн с двумя раздельными кроватями либо одной двуспальной.",
                Images = new List<ImageForRoomType>(),
                MaxCapacity = 2,
                NightlyRate = 54000,
            },
            new RoomType()
            {
                Id = new Guid("976ACB8B-A4F7-41A9-9732-79F24E7F2929"),
                Title = "JUNIOR SUITE",
                Description = "Уютный двухкомнатный номер с большой двуспальной кроватью в спальне и комфортабельным диваном в гостиной. Возможность приехать с семьей и разместить ребенка в отдельной комнате.",
                Images = new List<ImageForRoomType>(),
                MaxCapacity = 3,
                NightlyRate = 68000,
            },
            new RoomType()
            {
                Id = new Guid("926ACB8B-A4F7-41A9-9732-79F24E7F2929"),
                Title = "QUEEN SUITE",
                Description = "Уютный двухкомнатный номер в стиле модерн повышенной комфортности. Спальная комната с широкой кроватью, гостиная с диваном и телевизором.",
                Images = new List<ImageForRoomType>(),
                MaxCapacity = 3,
                NightlyRate = 104000,
            },
            new RoomType()
            {
                Id = new Guid("976ACB8B-A4F7-41A9-9732-45F24E7F2929"),
                Title = "KING SUITE",
                Description = "Чудесный трехкомнатный номер повышенной комфортности для деловых людей. Спальная комната с широкой кроватью, гостиная с диваном и телевизором, а также комната для переговоров.",
                Images = new List<ImageForRoomType>(),
                MaxCapacity = 2,
                NightlyRate = 150000,
            },
            new RoomType()
            {
                Id = new Guid("976ACB8B-A4F7-75A9-9732-79F24E7F2929"),
                Title = "PRESIDENTAIL SUITE",
                Description = "Трехкомнатный номер категории Президентский люкс. Вместимость до 2 мест. Дети до 7 лет — можно без отдельного спального места.",
                Images = new List<ImageForRoomType>(),
                MaxCapacity = 2,
                NightlyRate = 190000,
            },
        };
    }

    public static List<HotelBranch> GetHotelBranches()
    {
        return new List<HotelBranch>()
        {
            new HotelBranch
            {
                Id = new Guid("2B2A8CE6-27D6-47E1-AD9E-556D41B44E4E"),
                Name = "Hotel in Almaty",
                LocationId = new Guid("2B8A8CE6-27D6-47E1-AD9E-556D41B44E4E"), 
                Location = new Location
                {
                    Id = Guid.Parse("2B8A8CE6-27D6-47E1-AD9E-556D41B44E4E"),
                    Country = "Казахстан",
                    City = "Алматы",
                    Street = "Достык",
                    HouseNumber = "52/2"
                }
            },
            new HotelBranch
            {
                Id = new Guid("72013D39-673E-43E6-B2AC-5EA2189FB5B5"),
                Name = "Отель в Астане",
                LocationId = new Guid("F631ECEC-A783-4C58-BA08-C407EEDB79CE"), // You can generate LocationId dynamically or provide a specific Id
                Location = new Location
                {
                    Id = Guid.Parse("F631ECEC-A783-4C58-BA08-C407EEDB79CE"),
                    Country = "Казахстан",
                    City = "Астана",
                    Street = "Мухтар Ауэзов",
                    HouseNumber = "50/1"
                }
            },
            new HotelBranch
            {
                Id = new Guid("0F6ECA4A-AFFD-4EC4-A71C-AB8D1F8E2C86"),
                Name = "Отель в Караганде",
                LocationId = new Guid("FAB237DF-18EC-428E-BE90-89BA703EEBAE"), // You can generate LocationId dynamically or provide a specific Id
                Location = new Location
                {
                    Id = Guid.Parse("FAB237DF-18EC-428E-BE90-89BA703EEBAE"),
                    Country = "Казахстан",
                    City = "Караганда",
                    Street = "Проспект Республики",
                    HouseNumber = "11/3"
                }
            }
        };
    }

    public static List<Room> GetRooms()
    {
        var rooms = new List<Room>();
        for (var i = 1; i <= 20; i++)
        {
            var room = new Room()
            {
                Id = Guid.NewGuid(),
                HotelBranchId = Guid.Parse("0f6eca4a-affd-4ec4-a71c-ab8d1f8e2c86"),
                IsDisabled = false,
                Number = i + "A",
                TypeId = Guid.Parse("2f0bac92-2604-47e8-81bf-0c05860ba36c")
            };
            rooms.Add(room);
        }
        
        for (var i = 21; i <= 30; i++)
        {
            var room = new Room()
            {
                Id = Guid.NewGuid(),
                HotelBranchId = Guid.Parse("0f6eca4a-affd-4ec4-a71c-ab8d1f8e2c86"),
                IsDisabled = false,
                Number = i + "A",
                TypeId = Guid.Parse("da75ef9e-ccdd-4739-ac3d-292403fa4c73")
            };
            rooms.Add(room);
        }
        for (var i = 1; i <= 20; i++)
        {
            var room = new Room()
            {
                Id = Guid.NewGuid(),
                HotelBranchId = Guid.Parse("2b2a8ce6-27d6-47e1-ad9e-556d41b44e4e"),
                IsDisabled = false,
                Number = i + "B",
                TypeId = Guid.Parse("2f0bac92-2604-47e8-81bf-0c05860ba36c")
            };
            rooms.Add(room);
        }
        for (var i = 21; i <= 30; i++)
        {
            var room = new Room()
            {
                Id = Guid.NewGuid(),
                HotelBranchId = Guid.Parse("2b2a8ce6-27d6-47e1-ad9e-556d41b44e4e"),
                IsDisabled = false,
                Number = i + "B",
                TypeId = Guid.Parse("da75ef9e-ccdd-4739-ac3d-292403fa4c73")
            };
            rooms.Add(room);
        }
        for (var i = 1; i <= 20; i++)
        {
            var room = new Room()
            {
                Id = Guid.NewGuid(),
                HotelBranchId = Guid.Parse("72013d39-673e-43e6-b2ac-5ea2189fb5b5"),
                IsDisabled = false,
                Number = i + "C",
                TypeId = Guid.Parse("2f0bac92-2604-47e8-81bf-0c05860ba36c")
            };
            rooms.Add(room);
        }
        for (var i = 21; i <= 30; i++)
        {
            var room = new Room()
            {
                Id = Guid.NewGuid(),
                HotelBranchId = Guid.Parse("72013d39-673e-43e6-b2ac-5ea2189fb5b5"),
                IsDisabled = false,
                Number = i + "C",
                TypeId = Guid.Parse("da75ef9e-ccdd-4739-ac3d-292403fa4c73")
            };
            rooms.Add(room);
        }
        return rooms;
    }
}