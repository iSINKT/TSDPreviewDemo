using System.Linq;
using System.Collections.Generic;
using System.Text.RegularExpressions;
// ReSharper disable SuspiciousTypeConversion.Global

namespace TSD.PreviewDemo.Core.BarCodes
{
    public class ScannerBarcode
    {
        public ScannerBarcode(string barcode)
        {
            var regexCargo = new Regex(@"\d{2}G\d{2,10}");
            var regexDiscount = new Regex(@"^286[\W\w\s]{0,21}$");
            var regexOutputCargoCode = new Regex(@"^[\W\w\s]{11,12}$");
            var matchesCargo = regexCargo.Matches(barcode);
            var matchesDiscount = regexDiscount.Matches(barcode);
            var matchesOutputCargoCode = regexOutputCargoCode.Matches(barcode);
            if (((IList<Match>) matchesCargo).FirstOrDefault() != null)
                BarcodeType = BarcodeType.Cargo;
            else if (((IList<Match>) matchesDiscount).FirstOrDefault() != null)
                BarcodeType = BarcodeType.Discount;
            else if (((IList<Match>)matchesOutputCargoCode).FirstOrDefault() != null)
                BarcodeType = BarcodeType.Cargo;
            else
                BarcodeType = BarcodeType.Item;
            BarcodeString = barcode;
        }

        public string BarcodeString { get; }
        
        public BarcodeType BarcodeType { get; }
    }
}