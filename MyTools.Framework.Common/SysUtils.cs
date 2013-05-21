/*
 *名称：SysUtils
 *功能：
 *创建人：吉桂昕
 *创建时间：2013-05-21 11:27:44
 *修改时间：
 *备注：
 */

using RestSharp;

namespace MyTools.Framework.Common
{
    public static class SysUtils
    {
        public static byte[] GetImgByte(string imgUrl)
        {
            var restClient = new RestClient(imgUrl);
            var request = new RestRequest(Method.GET);
            IRestResponse response = restClient.Execute(request);

            /*
                        IRestResponse response = restClient.ExecuteAsync(
                    request,
                    Response =>
                    {
                        if (Response != null)
                        {
                            byte[] imageBytes = Response.RawBytes;
                            var bitmapImage = new BitmapImage();
                            bitmapImage.BeginInit();
                            bitmapImage.StreamSource = new MemoryStream(imageBytes);
                            bitmapImage.CreateOptions = BitmapCreateOptions.None;
                            bitmapImage.CacheOption = BitmapCacheOption.Default;
                            bitmapImage.EndInit();

                            JpegBitmapEncoder encoder = new JpegBitmapEncoder();
                            Guid photoID = System.Guid.NewGuid();
                            String photolocation = String.Format(@"c:\temp\{0}.jpg", Guid.NewGuid().ToString());
                            encoder.Frames.Add(BitmapFrame.Create(bitmapImage));
                            using (var filestream = new FileStream(photolocation, FileMode.Create))
                            encoder.Save(filestream);

                            this.Dispatcher.Invoke((Action)(() => { img.Source = bitmapImage; }));
                            ;
                        }
                    });*/

            return response.RawBytes;
        }
    }
}