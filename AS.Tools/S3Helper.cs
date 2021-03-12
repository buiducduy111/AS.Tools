using Amazon;
using Amazon.S3;
using Amazon.S3.Model;
using Amazon.S3.Transfer;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AS.Tools
{
    /// <summary>
    /// S3 simple uploader
    /// </summary>
    public class S3Helper
    {
        string _bucketName;

        private IAmazonS3 _s3Client;
        private Random _rand = new Random();

        /// <summary>
        /// Init s3 with Region is USEast1
        /// </summary>
        /// <param name="apiKey"></param>
        /// <param name="apiSecret"></param>
        /// <param name="bucketName"></param>
        public S3Helper(string apiKey, string apiSecret, string bucketName)
        {
            _bucketName = bucketName;
            _s3Client = new AmazonS3Client(apiKey, apiSecret, RegionEndpoint.USEast1);
        }


        /// <summary>
        /// Upload file. Return URL on s3 or null if fail
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        public string UploadFile(string file)
        {
            try
            {
                string bucketName = _bucketName;
                string key = Path.GetFileNameWithoutExtension(file).Replace(" ", "+") + "-" + StrHelper.RandomString(5) + Path.GetExtension(file);

                string resultUrl = $"https://{bucketName}.s3.amazonaws.com/{key}";

                var uploadRequest = new TransferUtilityUploadRequest
                {
                    Key = key,
                    FilePath = file,
                    BucketName = bucketName,
                    CannedACL = S3CannedACL.PublicRead
                };

                var fileTransferUtility = new TransferUtility(_s3Client);
                fileTransferUtility.Upload(uploadRequest);

                var expiryUrlRequest = new GetPreSignedUrlRequest()
                {
                    BucketName = _bucketName,
                    Key = key,
                    Expires = DateTime.Now.AddDays(10)
                };

                resultUrl = _s3Client.GetPreSignedURL(expiryUrlRequest);
                if (resultUrl.Contains("?")) resultUrl = resultUrl.Substring(0, resultUrl.IndexOf('?'));

                return resultUrl;
            }
            catch
            {
                return null;
            }
        }
    }
}