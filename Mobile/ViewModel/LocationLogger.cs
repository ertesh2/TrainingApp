using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Windows.Devices.Geolocation;
using Windows.Storage;

namespace TrainingApp.ViewModel
{
    class LocationLogger
    {
        private StorageFile _storageFile;

        public async void AddEntry(Geoposition location)
        {
            if (_storageFile == null)
            {
                const string filename = "data.txt";
                StorageFolder dir = ApplicationData.Current.LocalFolder;
                _storageFile = await dir.CreateFileAsync(filename, CreationCollisionOption.GenerateUniqueName);
            }
            await FileIO.WriteTextAsync(_storageFile, ConvertToLog(location));
        }

        private string ConvertToLog(Geoposition location)
        {
            var sb = new StringBuilder();
            sb.Append(DateTime.Now.ToString("hh:mm:ss"));
            sb.Append(" ");
            sb.Append(location.Coordinate.Point.Position.Latitude);
            sb.Append(", ");
            sb.Append(location.Coordinate.Point.Position.Longitude);
            sb.Append("\n");
            return sb.ToString();
        }
    }
}
