using CartifyDAL.Entities.order;
using CartifyDAL.Entities.user;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace CartifyDAL.Entities.payment
{ 
    public class Payment
    {

        public enum TransactionStatus
        {
            Pending,    
            Success,    
            Failed,     
            Refunded    
        }
        public Payment(double amount, string createdBy, TransactionStatus status)
        {
            Amount = amount;
            CreatedAt = DateTime.Now;
            Status = TransactionStatus.Pending;
        }
        public Payment() { }
        [Key]
        public int PaymentId { get; private set; }

        [Required]
        public double Amount { get;  set; }
        [Required]
        public string Currency { get; private set; } = "USD";
        [Required]
        public TransactionStatus Status { get; set; } 


        [MaxLength(255)]
        public string? GatewayName { get; set; } 

        [MaxLength(255)]
        public string? GatewayTransactionId { get; set; } 

        public int? PaymentMethodId { get; private set; }
        [ForeignKey(nameof(PaymentMethodId))]
        public PaymentMethod PaymentMethod { get; private set; }

        [Required]
        public int OrderId { get;  set; } 
        [ForeignKey(nameof(OrderId))]
        public Order Order { get;  set; }

       
        public DateTime CreatedAt { get;  set; }
        public DateTime? LastUpdatedAt { get;  set; }
    }
}
