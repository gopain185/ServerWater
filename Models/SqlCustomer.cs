using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ServerWater.Model
{
    [Table("Customer")]
    public class SqlCustomer
    {
        [Key]
        public long Id { get; set; }
        public string code { get; set; } = "";
        public string displayName { get; set; } = "";
        public string address { get; set; } = "";
        public string seriModule { get; set; } = "";
        public string seriDevice { get; set; } = "";
        public float latitude { get; set; }
        public float longitude { get; set; }
        public SqlArea? area { get; set; }
        public bool isDelete { get; set; }
    }
}
