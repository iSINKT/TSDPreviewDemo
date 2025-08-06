namespace TSD.PreviewDemo.Common.Platform
{
    /// <summary>
    /// Идентификация текущего устройства
    /// </summary>
    public interface IDeviceIdentityProvider
    {
        /// <summary>
        /// Идентификатор устройства 
        /// </summary>
         string DeviceId { get; }
    }
}
