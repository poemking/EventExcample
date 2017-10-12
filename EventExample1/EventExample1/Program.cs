using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

namespace EventExample1
{
    class Program
    {
        static void Main(string[] args)
        {
            Customer customer = new Customer();
            Waitor waitor = new Waitor();
            customer.Order += waitor.Action;
            customer.Action();
            customer.PayTheBill();
        }
    }

    public class OrderEventArgs : EventArgs
    {
        public string DishName { get; set; }
        public string Size { get; set; }
        //
    }
    public delegate void OrderEventHandler(Customer customer, OrderEventArgs e);

    public class Customer
    {
        //簡化事件申明,看上去很像是委託類型字段,關鍵字為event,其實是語法糖
        //public event OrderEventHandler Order;

        //完整事件申明
        private OrderEventHandler orderEventHandler;
        public event OrderEventHandler Order
        {
            add
            {
                this.orderEventHandler += value;
            }
            remove
            {
                this.orderEventHandler -= value;
            }
        }

        public double Bill { get; set; }
        public void PayTheBill()
        {
            Console.WriteLine("I will pay ${0}", this.Bill);
        }
        public void WalkIn()
        {
            Console.WriteLine("Walk into the restaurant");
        }
        public void SitDown()
        {
            Console.WriteLine("Sit down");
        }
        public void Think()
        {
            for (int i = 0; i < 5; i++)
            {
                Console.WriteLine("Let me think...");
                Thread.Sleep(1000);
            }
            //完整事件的申明法
            if (this.orderEventHandler != null)
            {
                OrderEventArgs e = new OrderEventArgs();
                e.DishName = "KongPao Chicken";
                e.Size = "large";
                this.orderEventHandler.Invoke(this, e);
            }

            //簡化事件申明,利用事件的名字取代這上述的完整申明字段
            //if(this.Order != null)
            //{
            //    OrderEventArgs e = new OrderEventArgs();
            //    e.DishName = "KongPao Chicken";
            //    e.Size = "large";
            //    this.Order.Invoke(this, e);
            //}
        }
        public void Action()
        {
            Console.ReadLine();
            this.WalkIn();
            this.SitDown();
            this.Think();
        }
    }

    public class Waitor
    {
        public void Action(Customer customer, OrderEventArgs e)
        {
            Console.WriteLine("I will serve you the dish - {0}", e.DishName);
            double price = 10;
            switch (e.Size)
            {
                case "small":
                    price = price * 0.5;
                    break;
                case "large":
                    price = price * 1.5;
                    break;
                default:
                    break;
            }
            customer.Bill += price;
        }
    }
}
