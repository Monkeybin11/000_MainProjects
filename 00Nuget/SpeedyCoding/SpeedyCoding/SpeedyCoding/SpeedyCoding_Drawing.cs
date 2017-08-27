using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace SpeedyCoding
{
    public static class SpeedyCoding_Drawing
    {
        public static System.Drawing.Rectangle ExpendRect(
          this System.Drawing.Rectangle @this ,
          int margin
          )
        {
            return new System.Drawing.Rectangle(
                @this.X - margin
                , @this.Y - margin
                , @this.Width + margin * 2
                , @this.Height + margin * 2 );
        }

        public static System.Drawing.Rectangle ShurinkRect(
            this System.Drawing.Rectangle @this ,
            int margin
            )
        {
            return new System.Drawing.Rectangle(
                @this.X + margin
                , @this.Y + margin
                , @this.Width - margin * 2
                , @this.Height - margin * 2 );
        }
    }
}
