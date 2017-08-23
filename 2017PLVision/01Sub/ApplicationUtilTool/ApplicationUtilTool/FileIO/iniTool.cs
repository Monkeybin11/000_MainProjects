using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationUtilTool
{
    public class iniTool
    {
        [DllImport( "kernel32" )]
        public static extern long WritePrivateProfileString( string section , string key , string val , string filePath );
        [DllImport( "kernel32" )]
        public static extern int GetPrivateProfileString( string section , string key , string def , StringBuilder retVal ,
                                                        int size , string filePath );


        public readonly string FilePath;

        public iniTool( string path )
        {
            FilePath = path;
        }

        //public iniTool ChangeKey( string section , string keyname , stirng )
        //{
        //    iniTool.WritePrivateProfileString( section , keyname , value , FilePath );
        //    return this;
        //}


        public iniTool WriteValue( string section , string keyname , string value )
        {
            iniTool.WritePrivateProfileString( section , keyname , value , FilePath );
            return this;
        }

        public string GetValue( string section , string keyname )
        {
            var str = new StringBuilder(1024);
            iniTool.GetPrivateProfileString( section , keyname , "" , str , 1024 , FilePath );
            return str.ToString();
        }



    }
}
