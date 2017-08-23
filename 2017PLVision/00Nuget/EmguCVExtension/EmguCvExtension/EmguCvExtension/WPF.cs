using System;
using System.Collections.Generic;
using System.Text;
using Emgu.CV;
using Emgu.CV.Structure;
using Emgu.CV.Util;
using System.Runtime.InteropServices;
using System.Windows.Media.Imaging;
using System.Windows;


namespace EmguCvExtension
{
    public static class WPF
    {
        [DllImport( "gdi32" )]
        private static extern int DeleteObject( IntPtr o );
        public static BitmapSource ToBitmapSource( this IImage image )
        {
            try
            {
                using ( System.Drawing.Bitmap source = image.Bitmap )
                {
                    IntPtr ptr = source.GetHbitmap();
                    BitmapSource bs = System.Windows.Interop.Imaging.CreateBitmapSourceFromHBitmap(
                ptr,
                IntPtr.Zero,
                Int32Rect.Empty,
                System.Windows.Media.Imaging.BitmapSizeOptions.FromEmptyOptions());

                    DeleteObject( ptr );
                    return bs;
                }
            }
            catch ( Exception )
            {
                return null;
            }
        }
    }
}
