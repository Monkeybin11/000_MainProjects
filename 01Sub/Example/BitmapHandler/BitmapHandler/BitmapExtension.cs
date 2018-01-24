using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media.Imaging;

namespace BitmapHandler
{
    public static class BitmapExtension
    {
        /// <summary>
        /// Color Array 를 Bitmap 변환
        /// </summary>
        /// <param name="width"></param>
        /// <param name="height"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        public static System.Drawing.Bitmap GetBitmapFromColorArray(int width, int height, System.Drawing.Color[] data)
        {
            using (System.Drawing.Bitmap result = new System.Drawing.Bitmap(width, height))
            {
                byte[] resultMap = new byte[0];
                if (width * height != data.Length)
                {
                    return result;
                }

                using (MemoryStream stream = new MemoryStream())
                {
                    result.Save(stream, System.Drawing.Imaging.ImageFormat.Bmp);
                    stream.Close();
                    resultMap = stream.ToArray();

                    int index = 54;
                    for (int i = height - 1; i >= 0; i--)
                    {
                        for (int j = 0; j < width; j++)
                        {
                            var cc = data[j + i * width];
                            resultMap[index + 0] = cc.B;
                            resultMap[index + 1] = cc.G;
                            resultMap[index + 2] = cc.R;
                            resultMap[index + 3] = 0xFF;
                            index += 4;
                        }
                    }
                }

                using (MemoryStream stream = new MemoryStream(resultMap))
                {
                    var newresult = new System.Drawing.Bitmap(stream);
                    return newresult;
                }
            }
        }

        /// <summary>
        /// double Array data를 팔레트 기준으로 색칠하는 함수, 적당히 응용해서 쓰자.
        /// </summary>
        /// <param name="width"></param>
        /// <param name="height"></param>
        /// <param name="data"></param>
        /// <param name="pallette"></param>
        /// <returns></returns>
        public static Bitmap GetBitmapFromDataAndPallette(int width, int height, double[] data, System.Drawing.Color[] pallette, System.Drawing.Color defaultColor = new System.Drawing.Color())
        {
                System.Drawing.Bitmap result = new System.Drawing.Bitmap(width, height);
            
                byte[] resultMap = new byte[0];
                
                var cleanList = data.Where(x => double.IsNaN(x) == false);
                var colorcount = pallette.Length;

                double min = cleanList.Min();
                double max = cleanList.Max();

                using (MemoryStream stream = new MemoryStream())
                {
                    result.Save(stream, System.Drawing.Imaging.ImageFormat.Bmp);
                    stream.Close();
                    resultMap = stream.ToArray();

                    int index = 54;
                    for (int i = height - 1; i >= 0; i--)
                    {
                        for (int j = 0; j < width; j++)
                        {
                            var palletteIndex = (int)((data[j + i * width] - min) * (colorcount - 1) / (max - min));
                            var cc =  pallette[palletteIndex];
                            resultMap[index + 0] = cc.B;
                            resultMap[index + 1] = cc.G;
                            resultMap[index + 2] = cc.R;
                            resultMap[index + 3] = 0xFF;
                            index += 4;
                        }
                    }

                }


                using (MemoryStream stream = new MemoryStream(resultMap))
                {
                    var newresult = new System.Drawing.Bitmap(stream);
                    return newresult;
                }
            
        }


        /// <summary>
        /// WPF Image에 Bitmap 을 넣고싶을때 변환기
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        public static BitmapSource SourceFromBitmap(System.Drawing.Bitmap source)
        {
            return System.Windows.Interop.Imaging.CreateBitmapSourceFromHBitmap(
                          source.GetHbitmap(),
                          IntPtr.Zero,
                          Int32Rect.Empty,
                          BitmapSizeOptions.FromEmptyOptions());
        }

        /// <summary>
        /// WPF Image에서 Bitmap을 추출하고 싶을때 변환기
        /// </summary>
        /// <param name="bitmapsource"></param>
        /// <returns></returns>
        public static Bitmap BitmapFromSource(BitmapSource bitmapsource)
        {
            Bitmap bitmap;
            using (var outStream = new MemoryStream())
            {
                BitmapEncoder enc = new BmpBitmapEncoder();
                enc.Frames.Add(BitmapFrame.Create(bitmapsource));
                enc.Save(outStream);
                bitmap = new Bitmap(outStream);
            }
            return bitmap;
        }
    }
}
