using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace Pizza
{
    public class DAL
    {
        DataTable Pizza = new DataTable();
        DataTable Price = new DataTable();
        DataTable Topping = new DataTable();
        public DAL()
        {

            Pizza.Columns.Add("Id", typeof(string));
            Pizza.Columns.Add("Name", typeof(string));
            Pizza.Columns.Add("Image", typeof(string));
            Pizza.Columns.Add("Description", typeof(string));
            Pizza.Rows.Add("1", "Cheese Pizza", "http://wecookpizzaandpasta.com/wp-content/uploads/2016/07/cheese-pizza.jpg", "Delicious cheese pizza");
            Pizza.Rows.Add("2", "Pepperoni Pizza", "http://wecookpizzaandpasta.com/wp-content/uploads/2016/07/cheese-pizza.jpg", "Delicious pepperoni pizza");
            Pizza.Rows.Add("3", "Chicken Pizza", "http://wecookpizzaandpasta.com/wp-content/uploads/2016/07/cheese-pizza.jpg", "Delicious chicken pizza");


            Price.Columns.Add("Id", typeof(string));
            Price.Columns.Add("Size", typeof(string));
            Price.Columns.Add("Price", typeof(float));
            Price.Rows.Add("1", "small", 10.99f);
            Price.Rows.Add("2", "medium", 12.99f);
            Price.Rows.Add("3", "large", 15.99f);


            Topping.Columns.Add("Id", typeof(string));
            Topping.Columns.Add("Name", typeof(string));
            Topping.Columns.Add("Price", typeof(float));
            Topping.Rows.Add("1", "mushroom", 1.00f);
            Topping.Rows.Add("2", "spinach", 1.00f);
            Topping.Rows.Add("3", "chicken", 2.00f);

        }

        public DataTable GetPizzaList()
        {
            
            return Pizza;
        }
        public DataRow GetPizza(int id)
        {
           
            return Pizza.Rows[id];
        }
        public DataTable GetSizePrice()
        { 
            return Price;
        }

        public DataTable GetToppingList()
        {
            return Topping;
        }
    }
}
