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
        public class Store
        {
            public List<Product> Products;
            public List<Product> Basket; // корзина
            public List<Order> Orders;

            public Store()
            {
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
                Basket.Add(Products[numberProduct - 1]);
                Console.WriteLine($"Продукт {Products[numberProduct - 1].Name} успешно добавлен в корзину");
                Console.WriteLine($"В корзине {Basket.Count} продуктов");
            }

            internal void CreateOrder()
            {
                // передать от отдел доставки
                Order order = new Order(Basket);
                Orders.Add(order);
                Basket.Clear();
            }
        }
        static void Main(string[] args)
        {
            Store onlineStore = new Store();
            
            Console.WriteLine("Здравствуйте. Выберите действие:");
            Console.WriteLine("1. Показать каталог продуктов?");
            Console.WriteLine("Выберите номер действия, которое хотите совершить.");

            var numbeberAction = int.Parse(Console.ReadLine());
            switch (numbeberAction)
            {
                case 1:
                    onlineStore.ShowCatalog();
                    break;
                case 2: 

                default:
                    Console.WriteLine("Выберите номер действия из списка");
                    break;
            }
            bool yes = false;
            do
            {
                Console.WriteLine("Хотите добавить новый продукт в корзину? Напишите ДА или НЕТ");
                yes = IsYes(Console.ReadLine());
                if (yes)
                {
                    onlineStore.ShowCatalog();
                    Console.WriteLine("Напиишите номер продукта, которого нужно добавить в корзину");
                    var productNumber = int.Parse(Console.ReadLine());
                    onlineStore.AddToBasket(productNumber);
                }
            }
            while (yes);

            Console.WriteLine("Хотите посмотреть корзину с добавленными продуктами? Напишите ДА или НЕТ");
            yes = IsYes(Console.ReadLine());
            if (yes)
            {
                onlineStore.ShowBasket();
            }

            Console.WriteLine("Хотите оформить заказ? Напишите ДА или НЕТ");
            yes = IsYes(Console.ReadLine());
            if (yes)
            {
                onlineStore.CreateOrder();
            }
        }
        // Сделать меню, то есть дать пользователю выбирать какое действие он хочет совершить. Предусмотреть актуальность действий в меню. 

        // Защита от дурака. Проверять все входные данные от пользователя. Ваша задача дать обратную связь пользователю о его неудачных действиях. 

        // Создать класс пользователя, в котором будет его имя и список его заказов. 

        // Возможности администратора: добавление нового продукта.

        static bool IsYes(string answer)
        {
            return answer.ToLower() == "да";
        }
    }
}
