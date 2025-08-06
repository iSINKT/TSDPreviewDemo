using System.Collections.Generic;
// ReSharper disable UnusedMember.Global
// ReSharper disable IdentifierTypo

namespace TSD.PreviewDemo.Core.StoreAcceptance
{
    public class ProductScanData : Entity
    {
        public bool Denial { get; set; }
        public bool CheckMode { get; set; }
        public bool IsPieceMarkScheme { get; set; }
        public string ItemBarcode { get; set; }
        public string ItemName { get; set; }
        public string OnlyAccept { get; set; }
        public System.DateTime ProductionDate { get; set; }
        public decimal QuantityFactScanned { get; set; }
        public decimal QuantityOfBoxes { get; set; }
        public decimal QuantityOfItemsPerBox { get; set; }
        public string QuantityOfItemsPerBoxIsNull { get; set; }
        public int QtyCheckedHottah { get; set; }
        public List<TransferAcceptancePieceCommentData> RejectionReasons { get; set; } = new List<TransferAcceptancePieceCommentData>();
        public string UnitId { get; set; }
    }

    public class TransferAcceptancePieceCommentData : Entity
    {
        public string CommentId { get; set; }
        public string CommentName { get; set; }
    }
}
