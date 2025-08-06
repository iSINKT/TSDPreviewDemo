using Android.App;
using Android.Content;
using System;
using TSD.PreviewDemo.Core.BarCodes;
// ReSharper disable StringLiteralTypo

namespace TSD.PreviewDemo.App.BackgroundServices
{
    [BroadcastReceiver]
    public class ScanReceiver : BroadcastReceiver
    {
        public readonly string Manufacture;
        public readonly string BroadcastAction;

        public event Action<ScannerBarcode> Scanned;

        public override void OnReceive(Context context, Intent intent)
        {
            string barCodeData;
            if (intent?.Action == null ||
                !intent.Action.Equals(BroadcastAction)) return;
            if (_broadcastScannerData == "EXTRA_EVENT_DECODE_VALUE")
            {
                var decodeValue = intent.GetByteArrayExtra("EXTRA_EVENT_DECODE_VALUE");
                barCodeData = System.Text.Encoding.Default.GetString(decodeValue ?? throw new InvalidOperationException());
            }
            else
                barCodeData = intent.GetStringExtra(_broadcastScannerData);
            if (barCodeData == "READ_FAIL") return;
            if (!string.IsNullOrEmpty(barCodeData))
            {
                var barcode = barCodeData.Replace(@"\n", "").Trim();
                Scanned?.Invoke(new ScannerBarcode(barcode));
            }
            else
                Scanned?.Invoke(new ScannerBarcode(string.Empty));
        }

        public void RemoveAllSubscriptions()
        {
            Scanned = null;
        }

        public ScanReceiver(string manufacture)
        {
            Manufacture = manufacture;
            switch (Manufacture)
            {
                case "ATOL":
                    BroadcastAction = "com.xcheng.scanner.action.BARCODE_DECODING_BROADCAST";
                    _broadcastScannerData = "EXTRA_BARCODE_DECODING_DATA";
                    break;
                case "M3Mobile":
                    BroadcastAction = "com.android.server.scannerservice.broadcast";
                    _broadcastScannerData = "m3scannerdata";
                    break;
                case "Zebra Technologies":
                    BroadcastAction = "Zebra";
                    _broadcastScannerData = "com.symbol.datawedge.data_string";
                    break;
                case "Honeywell":
                    BroadcastAction = "hsm.RECVRBI";
                    _broadcastScannerData = "data";
                    break;
                case "Urovo":
                    BroadcastAction = "android.intent.ACTION_DECODE_DATA";
                    _broadcastScannerData = "barcode_string";
                    break;
                case "BLD":
                case "CSI":
                    BroadcastAction = "scan.rcv.message";
                    _broadcastScannerData = "barcodeData";
                    break;
                case "UBX":
                    BroadcastAction = "android.intent.ACTION_DECODE_DATA";
                    _broadcastScannerData = "barcode_string";
                    break;
                case "POINTMOBILE":
                     BroadcastAction = "device.scanner.EVENT";
                     _broadcastScannerData = "EXTRA_EVENT_DECODE_VALUE";
                    break;
                 case "iData":
                     BroadcastAction = "android.intent.action.SCANRESULT";
                     _broadcastScannerData = "value";
                    break;
                 case "Hyatta":
                     BroadcastAction = "com.android.scanner.broadcast";
                     _broadcastScannerData = "scandata";
                    break;
                 case "SEUIC":
                     BroadcastAction = "com.android.server.scannerservice.broadcast";
                     _broadcastScannerData = "scannerdata";
                    break;
                default:
                    BroadcastAction = "";
                    _broadcastScannerData = "";
                    break;
            }
            var filter = new IntentFilter();
            filter.AddAction(BroadcastAction);
            Application.Context.RegisterReceiver(this, filter);
        }

        public ScanReceiver()
        {
            
        }

        private readonly string _broadcastScannerData;
    }
}