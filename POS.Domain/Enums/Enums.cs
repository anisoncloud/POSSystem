using System;
using System.Collections.Generic;
using System.Text;

namespace POS.Domain.Enums
{
    public enum UnitType { Piece, Kg, Liter, Box, Meter }

    public enum OrderType { Retail, Restaurant }

    public enum OrderStatus { Pending, Cooking, Served, Completed, Cancelled }

    public enum OrderItemStatus { Pending, Cooking, Ready, Served }

    public enum TableStatus { Available, Occupied, Reserved, Cleaning }

    public enum PaymentMethod { Cash, Card, MobileBanking, Mixed }

    public enum DiscountType { None, Fixed, Percentage }

    public enum StockMovementType { Purchase, Sale, Adjustment, Return, Waste }

    public enum PurchaseStatus { Pending, Received, PartiallyReceived, Cancelled }
}
