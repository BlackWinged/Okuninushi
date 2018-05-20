using System;
using Amazon.S3;
using Amazon.S3.Model;
using System.IO;
using System.Threading.Tasks;

namespace Okunishushi.Connectors
{
    public class S3Connector
    {

        static IAmazonS3 client;

        public static async Task<PutObjectResponse> UploadObject(string filename, Stream file, string keyName, string contentType = "text/plain", string bucketName = "classroom-test")
        {
            if (client == null)
            {
                client = client = new AmazonS3Client(Amazon.RegionEndpoint.EUCentral1);
            }
            try
            {
                PutObjectRequest putRequest = new PutObjectRequest
                {
                    BucketName = bucketName,
                    Key = keyName,
                    InputStream = file,
                    ContentType = contentType
                };

                PutObjectResponse test = await client.PutObjectAsync(putRequest);
                return test;
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

            return null;
        }

        public static GetObjectResponse ReadObjectData(string keyName, string bucketName = "classroom-test")
        {
            using (client = new AmazonS3Client(Amazon.RegionEndpoint.EUCentral1))
            {

                GetObjectRequest request = new GetObjectRequest
                {
                    BucketName = bucketName,
                    Key = keyName
                };

                GetObjectResponse result = client.GetObjectAsync(request).Result;

                return result;
            }
        }
    }
}
