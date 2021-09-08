namespace DeviceManagementMicroservice.DataContracts
{
    /// <summary>
    /// Coordenadas GPS
    /// </summary>
    public class GeoCoordinate
    {
        public double Latitude { get; set; }
        public double longitude { get; set; }
        public double Altitude { get; set; }
    }
}