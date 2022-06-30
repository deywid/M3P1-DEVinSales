using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace DevInSales.Core.Entities
{
    public class Sale : Entity
    {

        public Sale(string buyerId, string sellerId, DateTime saleDate)
        {
            BuyerId = buyerId;
            SellerId = sellerId;
            SaleDate = saleDate;
        }
        public string BuyerId { get; private set; }
        public string SellerId { get; private set; }

        public void SetSaleDateToToday()
        {
            SaleDate = DateTime.Now.ToUniversalTime();
        }

        public DateTime SaleDate { get; private set; }        

        [JsonIgnore]        
        public User? Buyer { get; private set; }
        [JsonIgnore]     
        public User? Seller { get; private set; }
    }
}