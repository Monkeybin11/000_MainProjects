using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace PointerPractice
{
	public class MarchalCopyEx
	{
		public void main()
		{
			byte[] managedArray = {1,0,0,0, 0,1,0,0, 0,0,1,0, 0,0,0,1};
			//Array.Reverse( managedArray );
			
			int size = Marshal.SizeOf(managedArray[0]) * managedArray.Length;
			IntPtr pnt = Marshal.AllocHGlobal(size);

			try
			{
				// Copy the array to unmanaged memory.
				Marshal.Copy( managedArray , 0 , pnt , managedArray.Length );

				// Copy the unmanaged array back to another managed array.
				int[] managedArray23 =  new int[4];
				short[] managedArray33 =  new short[8];
				byte[] managedArray2 = new byte[managedArray.Length];

				Marshal.Copy( pnt , managedArray23 , 0 , 4 );
				Marshal.Copy( pnt , managedArray33 , 0 , 8 );
				Console.WriteLine( "The array was copied to unmanaged memory and back." );
			}
			finally
			{
				// Free the unmanaged memory.
				Marshal.FreeHGlobal( pnt );
			}
		}
	}
}
