using System;
using Amazon.S3;
using Amazon.S3.Model;
using System.IO;


namespace Okunishushi.Connectors
{
    public class S3Connector
    {

        static IAmazonS3 client;

        public static async void UploadObject(string filename, string filePath, string keyName, string bucketName = "classroom-test" )
        {
            if (client == null)
            {
                client = client = new AmazonS3Client(Amazon.RegionEndpoint.EUCentral1);
            }
            string contentType = "text/plain";
            try
            {
                FileStream file = new FileStream(filePath, System.IO.FileMode.Open, System.IO.FileAccess.Read);
                PutObjectRequest putRequest = new PutObjectRequest
                {
                    BucketName = bucketName,
                    Key = keyName,
                    InputStream = file,
                    ContentType = contentType
                };

                PutObjectResponse test = await client.PutObjectAsync(putRequest);
            }
            catch (AmazonS3Exception amazonS3Exception)
            {
                if (amazonS3Exception.ErrorCode != null &&
                    (amazonS3Exception.ErrorCode.Equals("InvalidAccessKeyId")
                    ||
                    amazonS3Exception.ErrorCode.Equals("InvalidSecurity")))
                {
                    Console.WriteLine("Check the provided AWS Credentials.");
                    Console.WriteLine(
                        "For service sign up go to http://aws.amazon.com/s3");
                }
                else
                {
                    Console.WriteLine(
                        "Error occurred. Message:'{0}' when writing an object"
                        , amazonS3Exception.Message);
                }
            }
        }

    }
}
