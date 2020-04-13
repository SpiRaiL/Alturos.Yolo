using Alturos.Yolo.Model;
using Alturos.Yolo.WebService.Contract;
using System;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Web.Http.Description;

namespace Alturos.Yolo.WebService.Communication.WebApi
{
    [RoutePrefix("ObjectDetection")]
    public class ObjectDetectionController : ApiController
    {
        private readonly IObjectDetection _objectDetection;

        public ObjectDetectionController(IObjectDetection objectDetection)
        {
            this._objectDetection = objectDetection;
        }

        /// <summary>
        ///  Detect object positions
        /// </summary>
        /// <param name="imageData">Image data</param>
        /// <returns></returns>
        [HttpPost]
        [Route("Detect")]
        [ResponseType(typeof(YoloItem[]))]
        public IHttpActionResult Detect(byte[] imageData)
        {
            
            try
            {
                if (imageData == null)
                {
                    throw new Exception("wtf Image data is null");
                }
                var items = this._objectDetection.Detect(imageData);
                return Ok(items);
            }
            catch (Exception exception)
            {
                return InternalServerError(exception);
            }
        }

        /// <summary>
        ///  Upload image as multi-part data 
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Route("Upload")]
        [ResponseType(typeof(YoloItem[]))]
        public async Task<IHttpActionResult> Detect()
        {
            HttpRequestMessage httpRequest = this.Request;
            var provider = new MultipartMemoryStreamProvider();
            await Request.Content.ReadAsMultipartAsync(provider);
            var file = provider.Contents[0];
            var filename = file.Headers.ContentDisposition.FileName.Trim('\"');
            var imageData = await file.ReadAsByteArrayAsync();
            //Do whatever you want with filename and its binary data.

            try
            {

                var items = this._objectDetection.Detect(imageData);
                return Ok(items);
            }
            catch (Exception exception)
            {
                return InternalServerError(exception);
            }
        }

        /// <summary>
        ///  Detect object positions
        /// </summary>
        /// <param name="filePath">local file path</param>
        /// <returns></returns>
        
        [HttpPost]
        [Route("DetectLocalPath")]
        [ResponseType(typeof(YoloItem[]))]
        public IHttpActionResult Detect(string filePath)
        {
            try
            {
                var items = this._objectDetection.Detect(filePath);
                return Ok(items);
            }
            catch (Exception exception)
            {
                return InternalServerError(exception);
            }
        }
        
    }
}
