using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MachineLib;
using ApplicationUtilTool;

namespace Test_Winform
{
    public enum testenum { test1,tt,yo,yo2}

    public partial class Form1 : Form
    {
        //ACSStageController acs = new ACSStageController(ConnectMode.IP , "10.0.0.100");
        public Form1()
        {
            InitializeComponent();
        }

        private void btnconnection_Click( object sender , EventArgs e )
        {
            //acs.Connect( "10.0.0.100" );
        }

        private void btnmove_Click( object sender , EventArgs e )
        {
            //acs.MoveAbs( "X" , 50 );
        }

        private void button3_Click( object sender , EventArgs e )
        {
            ccfTool ccf = new ccfTool(@"C:\Users\idiol\Desktop\T__tdi_2inch.ccf");
            ccf.ccf2ini( @"C:\Users\idiol\Desktop\T__tdi_2inch.ccf" );
            //ccf.ccf2ini( @"C:\Users\idiol\Desktop\T__tdi_3inch.ini" );
        }

        private void button1_Click( object sender , EventArgs e )
        {
            ccfTool ccf = new ccfTool(@"C:\Users\idiol\Desktop","T__tdi_2inch");
            var changer = ccf.AppendChangeList("Stream Conditioning");
            changer( @"Crop Height" )( "564" );
            changer( @"Scale Vertical" )( "444" );
            ccf.RunChnage();
        }

        private void btnenumtolis_Click( object sender , EventArgs e )
        {
            var list1 =  Enum.GetNames( typeof( testenum ) ).ToList();
            int tt  = 2;
        }

     

    }
}
