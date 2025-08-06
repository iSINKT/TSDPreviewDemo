using System;

namespace TSD.PreviewDemo.DataLayer.Interfaces.Services
{
    public interface IEndpointAddressResolver
    {
        Uri Resolve();
        // ReSharper disable once IdentifierTypo
        // ReSharper disable once UnusedMember.Global
        void UpdateUriAxService(string newEndPOint);
    }
}