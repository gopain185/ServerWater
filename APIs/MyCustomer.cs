using Microsoft.EntityFrameworkCore;
using ServerWater.Model;

namespace ServerWater.APIs
{
    public class MyCustomer
    {
        public MyCustomer() { }

        public async Task initAsync()
        {
            using (DataContext context = new DataContext())
            {
                List<SqlCustomer> customers = context.customers!.Where(s => s.code.CompareTo("cus1") == 0).ToList();

                if (customers.Count <= 0)
                {
                    SqlCustomer customer = new SqlCustomer();
                    customer.Id = DateTime.Now.Ticks;
                    customer.code = "cus1";
                    customer.displayName = "customer 1";
                    customer.address = "110 Bùi Tá Hán";
                    customer.isDelete = false;
                    context.customers!.Add(customer);
                }
                customers = context.customers!.Where(s => s.code.CompareTo("cus2") == 0).ToList();

                if (customers.Count <= 0)
                {
                    SqlCustomer customer = new SqlCustomer();
                    customer.Id = DateTime.Now.Ticks;
                    customer.code = "cus2";
                    customer.displayName = "customer 2";
                    customer.address = "110 Nguyễn Hoàng";
                    customer.isDelete = false;
                    context.customers!.Add(customer);
                }
                int rows = await context.SaveChangesAsync();
            }
        }

        public class ItemCustomer
        {
            public string code { get; set; } = "";
            public string displayName { get; set; } = "";
            public string address { get; set; } = "";
        }

        public List<ItemCustomer> listCustomer()
        {
            using (DataContext context = new DataContext())
            {
                List<ItemCustomer> customers = new List<ItemCustomer>();
                List<SqlCustomer> customers_tmp = context.customers!.Where(s => s.isDelete == false).ToList();
                if (customers_tmp.Count > 0)
                {
                    foreach (SqlCustomer item in customers_tmp)
                    {
                        ItemCustomer customer = new ItemCustomer();
                        customer.code = item.code;
                        customer.displayName = item.displayName;
                        customer.address = item.address;
                        customers.Add(customer);
                    }
                }
                return customers;
            }
        }
    }
}
