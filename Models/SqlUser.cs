using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace ServerWater.Model
{
    [Table("User")]
    public class SqlUser
    {
        [Key]
        public long Id { get; set; }
        public string code { get; set; } = "";
        public string username { get; set; } = "";
        public string password { get; set; } = "";
        public string token { get; set; } = "";
        public string displayName { get; set; } = "";
        public bool isdeleted { get; set; } = false;
        public string phoneNumber { get; set; } = "";
        public SqlRole? role { get; set; }
        public List<SqlArea>? areas { get; set; }
    }
}
