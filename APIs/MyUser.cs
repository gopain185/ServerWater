using Microsoft.EntityFrameworkCore;
using ServerWater.Model;

namespace ServerWater.APIs
{
    public class MyUser
    {
        public MyUser()
        {
        }

        private static Random random = new Random();

        private static string randomString(int length)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
            return new string(Enumerable.Repeat(chars, length).Select(s => s[random.Next(s.Length)]).ToArray());
        }

        private string createToken()
        {
            string token = "";
            bool flag = false;
            do
            {
                flag = false;
                token = randomString(64);
                using (DataContext context = new DataContext())
                {
                    List<SqlUser> users = context.users!.Where(s => s.token.CompareTo(token) == 0 && s.isdeleted == false).AsNoTracking().ToList();
                    if (users.Count > 0)
                    {
                        flag = true;
                    }
                }
            } while (flag);
            return token;
        }

        public async Task initAsync()
        {
            using (DataContext context = new DataContext())
            {
                List<SqlUser> users = context.users!.Where(s => s.code .CompareTo("admin") == 0).ToList();
                List<SqlRole> roles = context.roles!.Where(s => s.code.CompareTo("admin") == 0).ToList();
                if (roles.Count <= 0)
                {
                    return;
                }
                if (users.Count > 0)
                {
                    foreach (SqlUser user in users)
                    {
                        user.username = "admin";
                        user.password = "stvg";
                        user.token = "1234567890";
                        user.role = roles[0];
                        user.isdeleted = false;
                    }
                }
                else
                {
                    SqlUser user = new SqlUser();
                    user.Id = DateTime.Now.Ticks;
                    user.code = "admin";
                    user.username = "admin";
                    user.password = "stvg";
                    user.displayName = "admin";
                    user.token = "1234567890";
                    user.role = roles[0];
                    user.isdeleted = false;
                    context.users!.Add(user);
                }

                users = context.users!.Where(s => s.code.CompareTo("user") == 0).ToList();
                roles = context.roles!.Where(s => s.code.CompareTo("user") == 0).ToList();
                if (roles.Count <= 0)
                {
                    return;
                }
                if (users.Count <= 0)
                {
                    SqlUser user = new SqlUser();
                    user.Id = DateTime.Now.Ticks;
                    user.code = "user";
                    user.username = "user";
                    user.password = "stvg";
                    user.displayName = "user";
                    user.token = createToken();
                    user.role = roles[0];
                    user.isdeleted = false;
                    context.users!.Add(user);
                }
                int rows = await context.SaveChangesAsync();
            }
        }

        public class ItemUser
        {
            public string user { get; set; } = "";
            public string username { get; set; } = "";
            public string displayname { get; set; } = "";
            public string role { get; set; } = "";
        }

        public List<ItemUser> listUser(string token)
        {
            if (string.IsNullOrEmpty(token))
            {
                return new List<ItemUser>();
            }
            using (DataContext context = new DataContext())
            {
                SqlUser? own_user = context.users!.Where(s => s.isdeleted == false && s.token.CompareTo(token) == 0).Include(s => s.role).FirstOrDefault();
                if (own_user == null)
                {
                    return new List<ItemUser>();
                }
                if (own_user.role == null)
                {
                    return new List<ItemUser>();
                }
                if (own_user.role!.code.CompareTo("admin") != 0)
                {
                    return new List<ItemUser>();
                }

                List<SqlUser> users = context.users!.Where(s => s.isdeleted == false).Include(s => s.role).ToList();
                List<ItemUser> items = new List<ItemUser>();
                foreach (SqlUser user in users)
                {
                    ItemUser item = new ItemUser();
                    item.user = user.code;
                    item.username = user.username;
                    item.displayname = user.displayName;
                    if (user.role != null)
                    {
                        item.role = user.role.name;
                    }
                    items.Add(item);
                }
                return items;
            }
        }

        public class ItemInfoUser
        {
            public string user { get; set; } = "";
            public string token { get; set; } = "";
            public string role { get; set; } = "";
        }

        public ItemInfoUser login(string username, string password)
        {
            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
            {
                return new ItemInfoUser();
            }
            using (DataContext context = new DataContext())
            {
                SqlUser? user = context.users!.Where(s => s.isdeleted == false && s.username.CompareTo(username) == 0 && s.password.CompareTo(password) == 0).Include(s => s.role).AsNoTracking().FirstOrDefault();
                if (user == null)
                {
                    return new ItemInfoUser();
                }
                ItemInfoUser item = new ItemInfoUser();
                item.user = user.code;
                item.token = user.token;
                if (user.role != null)
                {
                    item.role = user.role.name;
                }
                return item;
            }
        }
    }
}
