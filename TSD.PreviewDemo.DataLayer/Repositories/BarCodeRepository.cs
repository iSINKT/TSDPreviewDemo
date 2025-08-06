using AutoMapper;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using TSD.PreviewDemo.Core.BarCodes;
using TSD.PreviewDemo.DataEntities.Barcode;
using TSD.PreviewDemo.DataEntities.BaseRepositories;

namespace TSD.PreviewDemo.DataLayer.Repositories
{
    // ReSharper disable UnusedMember.Global
    public class BarCodeRepository : BaseRepository<Group>
    {
        private readonly BarcodeGroupManifest _manifest;

        public BarCodeRepository(Stream stream, Mapper mapper) : base(mapper)
        {
            using var sr = new StreamReader(stream);
            _manifest = JsonConvert.DeserializeObject<BarcodeGroupManifest>(sr.ReadToEnd());
        }

        public override IEnumerable<Group> List()
        {
            var groupList = new List<Group>();
            foreach (var barcodeGroup in _manifest.Barcodes)
            {
                var newGroup = Mapper.Map<Group>(barcodeGroup);
                newGroup.BarcodeRules = Mapper.Map<List<Rule>>(barcodeGroup.BarcodeRules);
                groupList.Add(newGroup);
            }
            return groupList;
        }
    }
}
