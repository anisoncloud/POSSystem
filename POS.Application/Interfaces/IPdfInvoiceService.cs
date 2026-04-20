using POS.Application.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace POS.Application.Interfaces
{
    public interface IPdfInvoiceService
    {
        byte[] GenerateInvoicePdf(OrderDetailViewModel order);
    }
}
