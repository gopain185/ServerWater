using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ServerWater.Model
{
    [Table("Area")]
    public class SqlArea
    {
        [Key]
        public long Id { get; set; }
        public string code { get; set; } = "";
        public string name { get; set; } = "";
        public string des { get; set; } = "";
        public List<SqlCustomer>? customers { get; set; }
        public List<SqlUser>? users { get; set; }
        public bool isDelete { get; set; }
    }
}
