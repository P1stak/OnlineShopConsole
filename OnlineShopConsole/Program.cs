using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineShopConsole
{
    class Program
    {
        public class Product
        {
            public string Name;
            public decimal Price;
            public Product(string name, decimal price)
            {
                Name = name;
                Price = price;
            }
            public void Print()
            {
                Console.WriteLine($"{Name} {Price}");
            }
        }
        public class Order
        {
            public List<Product> Products;
            public decimal FullPrice;
            public Order(List<Product> products)
            {
                Products = products;

                foreach (var product in products)
                {
                    FullPrice += product.Price;
                }
            }


        }
        public class User
        {
            public string Name;
            public List<Order> Orders;
            public User(string name)
            {
                Name = name;
                Orders = new List<Order>();
            }
            public void ShowOrders()
            {
                Console.WriteLine($"Заказы пользователя {Name}:\n");
                int orderNum = 1;
                foreach (var order in Orders)
                {
                    Console.WriteLine($"Заказ {orderNum++}:");
                    foreach (var product in order.Products)
                    {
                        product.Print();
                    }
                    Console.WriteLine($"Общая стоимость: {order.FullPrice}\n");
                }
            }
        }
        public class Store
        {
            public List<Product> Products;
            public List<Product> Basket; // корзина
            public List<Order> Orders;
            private User currentUser;
            private string adminName;



            public Store(User user, string adminName)
            {
                currentUser = user;
                this.adminName = adminName;
                Products = new List<Product> 
                {
                    new Product("Хлеб", 50),
                    new Product("Молоко", 90),
                    new Product("Яйца", 110),
                    new Product("Куриная грудка", 300),
                    new Product("Рис", 80),
                    new Product("Сыр", 150),
                    new Product("Кофе", 450)

                };

                Basket = new List<Product>();
                Orders = new List<Order>();
            }
            public void ShowCatalog()
            {
                ShowProducts(Products);
            }

            private void ShowProducts(List<Product> products)
            {
                int num = 1;
                Console.WriteLine("Каталог продуктов\n");
                Console.WriteLine("{0, -25} {1, -25} {2, -25}\n", '№', "Продукт", "Цена");
                foreach (var product in products)
                {
                    Console.WriteLine("{0, -25} {1, -25} {2, -25}", num++, product.Name, product.Price);
                }
            }

            public void ShowBasket()
            {
                Console.WriteLine("Содержимое корзины\n");
                ShowProducts(Basket);
                
            }
            public void AddToBasket(int numberProduct)
            {
                if (numberProduct > 0 && numberProduct <= Products.Count)
                {
                    Basket.Add(Products[numberProduct - 1]);
                    Console.WriteLine($"Продукт {Products[numberProduct - 1].Name} успешно добавлен в корзину");
                    Console.WriteLine($"В корзине {Basket.Count} продуктов");
                }
                else
                {
                    Console.WriteLine("Некорректный номер продукта.");
                }
            }

            public void CreateOrder()
            {
                // передать в отдел доставки
                if (Basket.Count > 0)
                {
                    Order order = new Order(Basket);
                    Orders.Add(order);
                    currentUser.Orders.Add(order);
                    Basket.Clear();
                    Console.WriteLine($"Заказ успешно создан на сумму {order.FullPrice}");
                }
                else
                {
                    Console.WriteLine("Корзина пуста. Добавьте товары перед оформлением заказа.");
                }
            }
            public void AddProductToCatalog(string name, decimal price)
            {
                if (currentUser.Name == adminName)
                {
                    Products.Add(new Product(name, price));
                    Console.WriteLine($"Продукт {name} успешно добавлен в каталог.");
                }
                else
                {
                    Console.WriteLine("Только у Администратора есть правад на это действия");
                }
            }

            public void RemoveProduct(int productNum)
            {
                if (currentUser.Name == adminName)
                {
                    if (productNum > 0 && productNum <= Products.Count)
                    {
                        Products.RemoveAt(productNum - 1);
                        Console.WriteLine($"Продукт {productNum} успешно удален из каталога.");
                    }
                    else
                    {
                        Console.WriteLine("Некорректный номер продукта.");
                    }
                }
                else
                {
                    Console.WriteLine("Только у Администратора есть правад на это действия");
                }
                
            }
        }
        static void Main(string[] args)
        {
            // создали логин юзера для заказа
            Console.WriteLine("Введите ваше имя:");
            var userName = Console.ReadLine();
            User currentUser = new User(userName);

            string adminName = "admin".ToLower();
            Store onlineStore = new Store(currentUser, adminName);


            bool continueProgram = true;
            while (continueProgram)
            {
                Console.WriteLine("Выберите действие:");
                Console.WriteLine("1. Показать каталог");
                Console.WriteLine("2. Добавить продукт в корзину");
                Console.WriteLine("3. Оформить заказ");
                Console.WriteLine("4. Добавить новый продукт в каталог (администратор)");
                Console.WriteLine("5. Удалить продукт в каталоге (администратор)");
                Console.WriteLine("6. Показать заказы пользователя");
                Console.WriteLine("7. Выйти");

                if (int.TryParse(Console.ReadLine(), out int numberAction))
                {
                    switch (numberAction)
                    {
                        case 1:
                            onlineStore.ShowCatalog();
                            break;

                        case 2:
                            bool yes = false;
                            do
                            {
                                Console.WriteLine("Хотите добавить новый продукт в корзину? Напишите ДА или НЕТ");
                                yes = IsYes(Console.ReadLine());
                                if (yes)
                                {
                                    onlineStore.ShowCatalog();
                                    Console.WriteLine("Напишите номер продукта, которого нужно добавить в корзину");
                                    if (int.TryParse(Console.ReadLine(), out int productNumber))
                                    {
                                        onlineStore.AddToBasket(productNumber);
                                    }
                                    else
                                    {
                                        Console.WriteLine("Некорректный номер продукта.");
                                    }
                                }
                            }
                            while (yes);
                            Console.WriteLine("Хотите посмотреть корзину с добавленными продуктами? Напишите ДА или НЕТ");
                            yes = IsYes(Console.ReadLine());
                            if (yes)
                            {
                                onlineStore.ShowBasket();
                            }
                            break;

                        case 3:
                            Console.WriteLine("Хотите оформить заказ? Напишите ДА или НЕТ");
                            yes = IsYes(Console.ReadLine());
                            if (yes)
                            {
                                onlineStore.CreateOrder();
                            }
                            break;

                        case 4:
                            Console.WriteLine("Введите название нового продукта");
                            var productName = Console.ReadLine();
                            Console.WriteLine("Введите цену нового продукта");
                            if (decimal.TryParse(Console.ReadLine(), out decimal productPrice))
                            {
                                onlineStore.AddProductToCatalog(productName, productPrice);
                            }
                            else
                            {
                                Console.WriteLine("Некорректная цена продукта.");
                            }
                                break;
                        case 5:
                            onlineStore.ShowCatalog();
                            Console.WriteLine("Введите номер продукта, который хотите удалить");                           
                            int productNum = int.Parse(Console.ReadLine());
                            onlineStore.RemoveProduct(productNum);

                            break;
                        case 6:
                            currentUser.ShowOrders();

                            break;
                        case 7:
                            continueProgram = false;
                            break;

                        default:
                            Console.WriteLine("Выберите номер действия из списка");
                            break;
                    }
                }
                else
                {
                    Console.WriteLine("Некорректный ввод. Пожалуйста, введите число.");
                }
            }
        }


        static bool IsYes(string answer)
        {
            return answer.ToLower() == "да";
        }
    }
}