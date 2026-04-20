using POS.Application.DTOs;
using POS.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace POS.Application.ViewModels
{
    public class OrderDetailViewModel
    {
        // ── Invoice identity ──────────────────────────────────────────────────
        public int Id { get; set; }
        public string InvoiceNumber { get; set; } = default!;
        public DateTime CreatedAt { get; set; }

        // ── Order type & status ───────────────────────────────────────────────
        public OrderType OrderType { get; set; }
        public OrderStatus Status { get; set; }

        // ── Branch info (printed on invoice) ─────────────────────────────────
        public string BranchName { get; set; } = default!;
        public string BranchAddress { get; set; } = default!;
        public string BranchPhone { get; set; } = default!;

        // ── Table (restaurant only) ───────────────────────────────────────────
        public string? TableNumber { get; set; }

        // ── Customer info ─────────────────────────────────────────────────────
        public string? CustomerName { get; set; }
        public string? CustomerPhone { get; set; }

        // ── Cashier ───────────────────────────────────────────────────────────
        public string CashierName { get; set; } = default!;

        // ── Line items ────────────────────────────────────────────────────────
        public List<OrderItemDto> Items { get; set; } = new();

        // ── Totals ────────────────────────────────────────────────────────────
        public decimal SubTotal { get; set; }
        public decimal DiscountAmount { get; set; }
        public decimal DiscountValue { get; set; }
        public DiscountType DiscountType { get; set; }
        public decimal TaxAmount { get; set; }
        public decimal TotalAmount { get; set; }

        // ── Payments ──────────────────────────────────────────────────────────
        public List<PaymentDto> Payments { get; set; } = new();

        // ── Computed helpers (used in invoice view) ───────────────────────────
        public decimal TotalPaid => Payments.Sum(p => p.Amount);
        public decimal ChangeGiven => TotalPaid - TotalAmount;
        public bool IsFullyPaid => TotalPaid >= TotalAmount;

        public string? Notes { get; set; }
    }
}
