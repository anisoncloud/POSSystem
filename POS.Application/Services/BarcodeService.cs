using POS.Application.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace POS.Application.Services
{
    public class BarcodeService : IBarcodeService
    {
        public string GenerateEan13()
        {
            var random = new Random();
            var digits = new int[12];
            digits[0] = 2;

            for (int i = 1; i < 12; i++)
                digits[i] = random.Next(0, 10);

            int checksum = 0;
            for (int i = 0; i < 12; i++)
                checksum += digits[i] * (i % 2 == 0 ? 1 : 3);

            int checkDigit = (10 - (checksum % 10)) % 10;
            return string.Concat(digits) + checkDigit;
        }

        public string GenerateSku(string productName, int categoryId)
        {
            var prefix = new string(
                productName.Where(char.IsLetter).Take(3).ToArray()).ToUpper();

            if (prefix.Length < 3)
                prefix = prefix.PadRight(3, 'X');

            return $"{prefix}-{categoryId:D3}-{DateTimeOffset.UtcNow.ToUnixTimeSeconds() % 100000}";
        }

        public string GetBarcodeImageUrl(string barcode)
            => $"/barcode/render?code={barcode}";
    }
}
