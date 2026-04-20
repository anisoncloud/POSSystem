using System;
using System.Collections.Generic;
using System.Text;

namespace POS.Application.Interfaces
{
    public interface IBarcodeService
    {
        string GenerateEan13();
        string GenerateSku(string productName, int categoryId);
        string GetBarcodeImageUrl(string barcode);
    }
}
