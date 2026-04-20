using POS.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace POS.Application.DTOs
{
    public class PaymentDto
    {
        public int Id { get; set; }
        public PaymentMethod Method { get; set; }
        public decimal Amount { get; set; }
        public string? ReferenceNumber { get; set; }
        public DateTime PaidAt { get; set; }

        // Display helper used in invoice view
        public string MethodDisplayName => Method switch
        {
            PaymentMethod.Cash => "Cash",
            PaymentMethod.Card => "Card",
            PaymentMethod.MobileBanking => "Mobile Banking",
            PaymentMethod.Mixed => "Mixed",
            _ => Method.ToString()
        };
    }
}
