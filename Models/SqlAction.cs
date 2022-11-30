using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace ServerWater.Model
{
    [Table("Action")]
    public class SqlAction
    {
        [Key]
        public long Id { get; set; }
        public float value { get; set; } = 0.0f;
        public SqlUser? user { get; set; }
        public SqlCustomer? customer { get; set; }
        public DateTime time { get; set; }

    }
}
