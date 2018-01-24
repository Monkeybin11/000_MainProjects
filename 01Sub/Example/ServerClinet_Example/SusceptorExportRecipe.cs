using EMxLib.data;
using EMxLib.recipe;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EMxLib.susceptorexport
{
    [Serializable]
    public enum SusceptorExporterRangeType
    {
        Absolute,
        MinMaxPercentage,
        AverageOffset,
    }

    [Serializable]
    public class SusceptorExportRecipe
    {
        public DoublePoint                      Size            { get; set; }
        public string                           SaveFileFormat  { get; set; }
        public string                           SusceptorName   { get; set; }

        public List<SusceptorExportSpotRecipe>  ItemList        { get; set; }

        public SusceptorExportRecipe()
        {
            Size = new DoublePoint(800, 800);
            SaveFileFormat = ".bmp";
            SusceptorName = "";

            ItemList = new List<SusceptorExportSpotRecipe>();
        }

    }

    [Serializable]
    public class SusceptorExportSpotRecipe
    {
        public                              PLEnum Type     { get; set; }
        public SusceptorExporterRangeType   RangeType    { get; set; }
        public double                       RangeMin        { get; set; }
        public double                       RangeMax        { get; set; }

        public string                       AppendText      { get; set; }

        public SusceptorExportSpotRecipe()
        {
            Type = PLEnum.Peakwavelength;
            RangeType = SusceptorExporterRangeType.AverageOffset;
            RangeMin = 0;
            RangeMax = 0;
            AppendText = "";
        }
    }
}
