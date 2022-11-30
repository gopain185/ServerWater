using Microsoft.EntityFrameworkCore;
using ServerWater.Model;
using static ServerWater.APIs.MyCustomer;
using static ServerWater.APIs.MyUser;

namespace ServerWater.APIs
{
    public class MyArea
    {
        public MyArea() { }
        public async Task initAsync()
        {
            using (DataContext context = new DataContext())
            {
                List<SqlArea> areas = context.areas!.Where(s => s.code.CompareTo("kv1")==0).ToList();

                if (areas.Count <= 0)
                {
                    SqlArea area = new SqlArea();
                    area.Id = DateTime.Now.Ticks;
                    area.code = "kv1";
                    area.name = "khu vuc 1";
                    area.des = "khu vuc 1";
                    area.isDelete = false;
                    context.areas!.Add(area);
                }
                areas = context.areas!.Where(s => s.code.CompareTo("kv2") == 0).ToList();

                if (areas.Count <= 0)
                {
                    SqlArea area = new SqlArea();
                    area.Id = DateTime.Now.Ticks;
                    area.code = "kv2";
                    area.name = "khu vuc 2";
                    area.des = "khu vuc 2";
                    area.isDelete = false;
                    context.areas!.Add(area);
                }
                int rows = await context.SaveChangesAsync();
            }
        }

        public class ItemArea
        {
            public string code { get; set; } = "";
            public string name { get; set; } = "";
            public string des { get; set; } = "";
        }

        public List<ItemArea> listArea()
        {
            using (DataContext context = new DataContext())
            {
                List<ItemArea> areas = new List<ItemArea>();
                List<SqlArea> areas_tmp = context.areas!.Where(s => s.isDelete == false).ToList();
                if (areas_tmp.Count > 0)
                {
                    foreach (SqlArea item in areas_tmp)
                    {
                        ItemArea area = new ItemArea();
                        area.code = item.code;
                        area.name = item.name;
                        area.des = item.des;
                        areas.Add(area);
                    }
                }
                return areas;
            }
        }

        public async Task<bool> addUser(string token,string codeUser,string codeArea)
        {
            using (DataContext context =new DataContext())
            {
                SqlUser? user = context.users!.Where(s => s.token.CompareTo(token) == 0 && s.isdeleted == false).Include(s => s.role).FirstOrDefault();
                if (user == null)
                {
                    return false;
                }
                if (user.role!.code.CompareTo("admin") != 0)
                {
                    return false;
                }
                user = context.users!.Where(s => s.code.CompareTo(codeUser) == 0 && s.isdeleted == false).FirstOrDefault();
                if (user == null)
                {
                    return false;
                }
                SqlArea? area = context.areas!.Where(s => s.code.CompareTo(codeArea) == 0 && s.isDelete == false).Include(s => s.users).FirstOrDefault();
                if (area == null)
                {
                    return false;
                }
                area.users!.Add(user);

                int rows = await context.SaveChangesAsync();
                if (rows > 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

        public async Task<bool> addCustomer(string token, string codeCustomer, string codeArea)
        {
            using (DataContext context = new DataContext())
            {
                SqlUser? user = context.users!.Where(s => s.token.CompareTo(token) == 0 && s.isdeleted == false).Include(s => s.role).FirstOrDefault();
                if (user == null)
                {
                    return false;
                }
                if (user.role!.code.CompareTo("admin") != 0)
                {
                    return false;
                }
                SqlCustomer? customer = context.customers!.Where(s => s.code.CompareTo(codeCustomer) == 0 && s.isDelete == false).FirstOrDefault();
                if (customer == null)
                {
                    return false;
                }
                SqlArea? area = context.areas!.Where(s => s.code.CompareTo(codeArea) == 0 && s.isDelete == false).Include(s => s.customers).FirstOrDefault();
                if (area == null)
                {
                    return false;
                }
                area.customers!.Add(customer);

                int rows = await context.SaveChangesAsync();
                if (rows > 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }
        
        public List<ItemUser> getListUserArea (string codeArea)
        {
            using (DataContext context = new DataContext())
            {
                List<ItemUser> users = new List<ItemUser>();
                SqlArea? area = context.areas!.Where(s => s.isDelete == false && s.code.CompareTo(codeArea) == 0).Include(s => s.users!).ThenInclude(s => s.role).FirstOrDefault();
                if (area == null)
                {
                    return new List<ItemUser>();
                }
                List<SqlUser> users_tmp = area.users!.Where(s => s.isdeleted == false).ToList();
                if (users_tmp.Count > 0)
                {
                    foreach (SqlUser item in users_tmp)
                    {
                        ItemUser user = new ItemUser();
                        user.user = item.code;
                        user.username = item.username;
                        user.displayname = item.displayName;
                        user.role = item.role.code;
                        users.Add(user);
                    }
                }
                return users;
            }
        }

        public List<ItemCustomer> getListCustomerArea(string codeArea)
        {
            using (DataContext context = new DataContext())
            {
                List<ItemCustomer> customers = new List<ItemCustomer>();
                SqlArea? area = context.areas!.Where(s => s.isDelete == false && s.code.CompareTo(codeArea) == 0).Include(s => s.customers).FirstOrDefault();
                if (area == null)
                {
                    return new List<ItemCustomer>();
                }
                List<SqlCustomer> customers_tmp = area.customers!.Where(s => s.isDelete == false).ToList();
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
