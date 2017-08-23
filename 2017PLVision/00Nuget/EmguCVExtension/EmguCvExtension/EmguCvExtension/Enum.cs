using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmguCvExtension
{
    public enum ThresholdMode
    {
        Auto,
        Manual
    }
    public enum kernal { Horizontal, Vertical, Cross, Rect }
    public enum morpOp { Erode, Dilate, Open, Close }
    public enum TempMatchType { Coeff, Corr, Sq }
    public enum Order { X, Y, XY, YX }
    public enum CornerMode { LeftTop, LeftBot, RightTop, RightBot }
}
