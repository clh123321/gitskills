using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApplicationTest
{
    /// <summary>
    /// 
    /// </summary>
    public class ImageConvertUtility
    {
        #region ImageToBytes 图片Image转换成Byte[]
        /// <summary>
        /// 将图片Image转换成Byte[]
        /// </summary>
        /// <param name="Image">image对象</param>
        /// <returns></returns>
        public static byte[] ImageToBytes(Image image)
        {
            if (image == null) { return null; }
            using (System.IO.MemoryStream ms = new System.IO.MemoryStream())
            {
                image.Save(ms, ImageFormat.Jpeg);
                ms.Position = 0;
                byte[] imageBytes = new byte[ms.Length];
                ms.Read(imageBytes, 0, imageBytes.Length);
                return imageBytes;
            }
        }
        #endregion

        #region BytesToImage byte[]转换成Image
        /// <summary>
        /// byte[]转换成Image
        /// </summary>
        /// <param name="bytes">二进制图片流</param>
        /// <returns>Image</returns>
        public static System.Drawing.Image BytesToImage(byte[] bytes)
        {
            if (bytes == null)
                return null;
            using (System.IO.MemoryStream ms = new System.IO.MemoryStream(bytes))
            {
                System.Drawing.Image returnImage = System.Drawing.Image.FromStream(ms);
                ms.Flush();
                return returnImage;
            }
        }
        #endregion

        #region ImageToBitmap Image转换Bitmap
        /// <summary>
        /// Image转换Bitmap
        /// </summary>
        /// <param name="image"></param>
        /// <returns></returns>
        public static Bitmap ImageToBitmap(Image image)
        {
            //Bitmap img = new Bitmap(imgSelect.Image);
            return (Bitmap)image;
        }
        #endregion

        #region BitmapToImage Bitmap转换成Image
        /// <summary>
        /// Bitmap转换成Image
        /// </summary>
        /// <param name="bitmap"></param>
        /// <returns></returns>
        public static Image BitmapToImage(Bitmap bitmap)
        {
            return (Image)bitmap;
        }
        #endregion

        #region BytesToBitmap byte[] 转换 Bitmap
        /// <summary>
        /// byte[] 转换 Bitmap
        /// </summary>
        /// <param name="Bytes"></param>
        /// <returns></returns>
        public static Bitmap BytesToBitmap(byte[] Bytes)
        {
            MemoryStream stream = null;
            try
            {
                stream = new MemoryStream(Bytes);
                return new Bitmap((Image)new Bitmap(stream));
            }
            catch (ArgumentNullException ex)
            {
                throw ex;
            }
            catch (ArgumentException ex)
            {
                throw ex;
            }
            finally
            {
                stream.Close();
            }
        }

        #endregion

        #region BitmapToBytes Bitmap转byte[]
        /// <summary>
        /// Bitmap转byte[]  
        /// </summary>
        /// <param name="Bitmap"></param>
        /// <returns></returns>
        public static byte[] BitmapToBytes(Bitmap Bitmap)
        {
            MemoryStream ms = null;
            try
            {
                ms = new MemoryStream();
                Bitmap.Save(ms, Bitmap.RawFormat);
                byte[] byteImage = new Byte[ms.Length];
                byteImage = ms.ToArray();
                return byteImage;
            }
            catch (ArgumentNullException ex)
            {
                throw ex;
            }
            finally
            {
                ms.Close();
            }
        }
        #endregion


    }
}
