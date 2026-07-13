using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Upgrow.Application.Common
{
    public sealed class AuditContext
    {
        public string? Tenant { get; set; }

        public string? LatLong { get; set; }

        public string? Platform { get; set; }

        public string? IpAddress { get; set; }

        public string? CustomerUUID { get; set; }

        public string? GetLatitude()
        {
            return GetCoordinate(0);
        }

        public string? GetLongitude()
        {
            return GetCoordinate(1);
        }

        private string? GetCoordinate(
            int index)
        {
            if (string.IsNullOrWhiteSpace(LatLong))
            {
                return null;
            }

            var coordinates =
                LatLong.Split(
                    ',',
                    StringSplitOptions.TrimEntries |
                    StringSplitOptions.RemoveEmptyEntries);

            if (coordinates.Length != 2)
            {
                return null;
            }

            return coordinates[index];
        }
    }
}
