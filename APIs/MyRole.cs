using ServerWater.Model;

namespace ServerWater.APIs
{
    public class MyRole
    {
        public MyRole() { }
        public async Task initAsync()
        {
            using (DataContext context = new DataContext())
            {
                List<SqlRole> roles = context.roles!.Where(s => s.code.CompareTo("admin") == 0 && s.isdeleted == false).ToList();
                if (roles.Count <= 0)
                {
                    SqlRole role = new SqlRole();
                    role.ID = DateTime.Now.Ticks;
                    role.code = "admin";
                    role.name = "admin";
                    role.des = "admin";
                    role.isdeleted = false;
                    context.roles!.Add(role);
                }

                roles = context.roles!.Where(s => s.code.CompareTo("user") == 0 && s.isdeleted == false).ToList();
                if (roles.Count <= 0)
                {
                    SqlRole role = new SqlRole();
                    role.ID = DateTime.Now.Ticks;
                    role.code = "user";
                    role.name = "user";
                    role.des = "user";
                    role.isdeleted = false;
                    context.roles!.Add(role);
                }
                int rows = await context.SaveChangesAsync();
            }
        }

        public class ItemRole
        {
            public string code { get; set; } = "";
            public string name { get; set; } = "";
            public string des { get; set; } = "";
        }

        public List<ItemRole> getListRole()
        {
            using (DataContext context = new DataContext())
            {
                List<SqlRole> roles = context.roles!.Where<SqlRole>(s => s.isdeleted == false).ToList();
                List<ItemRole> items = new List<ItemRole>();
                foreach (SqlRole role in roles)
                {
                    ItemRole item = new ItemRole();
                    item.code = role.code;
                    item.name = role.name;
                    item.des = role.des;
                    items.Add(item);
                }
                return items;
            }
        }
    }
}
