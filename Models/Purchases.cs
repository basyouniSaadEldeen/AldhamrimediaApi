using Newtonsoft.Json;
using Org.BouncyCastle.Asn1.Ocsp;

namespace AldhamrimediaApi.Models
{
    public class  Purchases
    {
        public Guid Id { get; set; }
        public int userId { get; set; }
        public virtual User? User { get; set; }
        public string Type_of_service { get; set; } = null!;
        public string Service_name { get; set; } = null!;
        public string Service_ImgUrl { get; set; } = null!;
        public DateTime Service_request_date { get; set; } 
        public decimal Number_of_money_paid { get; set; }
        public int Required_quantity { get; set; }
        public string Account_link { get; set; } = null!;



    }
}
