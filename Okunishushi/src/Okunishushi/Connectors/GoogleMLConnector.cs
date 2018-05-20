using System;
using Google.Cloud.Vision.V1;
using System.IO;
using System.Collections.Generic;

namespace Okunishushi.Connectors
{
    public class GoogleMLConnector
    {
        public static IReadOnlyList<EntityAnnotation> ReadImageText(Stream file)
        {
            var client = ImageAnnotatorClient.Create();
            var image = Image.FromStream(file);
            var response = client.DetectText(image);

            return response;
        }

        public static IReadOnlyList<EntityAnnotation> LabelImage(Stream file)
        {
            var client = ImageAnnotatorClient.Create();
            var image = Image.FromStream(file);
            var response = client.DetectLabels(image);

            return response;
        }
    }
}
