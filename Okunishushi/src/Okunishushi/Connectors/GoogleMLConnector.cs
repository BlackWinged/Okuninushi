using System;
using Google.Cloud.Vision.V1;
using System.IO;
using System.Collections.Generic;

namespace Okunishushi.Connectors
{
    public class GoogleMLConnector
    {
        public static IReadOnlyList<EntityAnnotation> ReadImageText(FileStream file)
        {
            var client = ImageAnnotatorClient.Create();
            var image = Image.FromStream(file);
            var response = client.DetectText(image);

            return response;
        }

        public static IReadOnlyList<EntityAnnotation> LabelImage(FileStream file)
        {
            var client = ImageAnnotatorClient.Create();
            var image = Image.FromStream(file);
            var response = client.DetectLabels(image);

            return response;
        }
    }
}
