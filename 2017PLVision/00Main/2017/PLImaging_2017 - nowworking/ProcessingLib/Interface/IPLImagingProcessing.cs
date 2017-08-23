using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using Emgu.CV.Structure;

namespace ProcessingLib
{
    // Need Impelement this Method. It's allowed to use global val to implement these methods
    public interface PLImagingProcessing<Tresult,Tbox,TproposalSrc> where TproposalSrc : struct
    {
        byte [ , , ] ToIndexImg(int hNum , int wNum);
        byte [ , , ] Preprocessing( byte [ , , ] input );
        List<Tbox> BoxRegionProposal( TproposalSrc [ , , ] input ); // byte is img, double is estedPos
        List<Tresult> ToResult( List<Tbox> input );
        List<Tresult> Classify( List<Tresult> input);
    }
    
}
