using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FUG_Protocol;
using System.Net;
using System.Net.Sockets;
using System.Diagnostics;
using System.IO;

namespace FileReceiver
{
    class Program
    {
        static void Main( string[] args )
        {
            args = new string[1] { @"C:\Temp" };

            if ( args.Length < 1 )
            {
                Console.WriteLine( "Usage : {0} <Directoryy>", Process.GetCurrentProcess().ProcessName );
                return;
            }

            uint msgId = 0;

            string dir = args[0];

            if ( Directory.Exists( dir ) )
            {
                Directory.CreateDirectory( dir );
            }

            const int bindPort = 5425;
            TcpListener server = null;

            

            try
            {
                IPEndPoint localAddress = new IPEndPoint( IPAddress.Parse( "192.168.11.45" ) ,bindPort);
                server = new TcpListener( localAddress );
                server.Start();
                Console.WriteLine( "Upload server is started" );

                while ( true )
                {
                    TcpClient clinet = server.AcceptTcpClient();
                    Console.WriteLine( "client is connected  : {0}", ( (IPEndPoint)clinet.Client.RemoteEndPoint ).ToString() );

                    NetworkStream stream = clinet.GetStream();


                    // Head (Msg ID , Msg Type , Body Len , Gragment , LastMsg , SEQ ) + Body
                    Message reqMsg = MessageUtil.Receive(stream); // return request instance from stream. 

                    if ( reqMsg.Header.MSGTYPE != CONSTANTS.REQ_FILE_SEND ) // if not file request. just close and wait new connection.
                    {
                        stream.Close();
                        clinet.Close();
                        continue;
                    }

                    BodyRequest reqbody  = (BodyRequest)reqMsg.Body;
                    Console.Write( "File Upload is Requested. Yes / No" );
                    string answer = Console.ReadLine();

                    Message rspMsg = new Message();
                    rspMsg.Body = new BodyResponse()
                    {
                        MSGID = reqMsg.Header.MSGID,
                        RESPONSE = CONSTANTS.ACCEPTED
                    };

                    rspMsg.Header = new Header()
                    {
                        MSGID = msgId++,
                        MSGTYPE = CONSTANTS.REP_FILE_SEND,
                        BODYLEN = (uint)rspMsg.Body.GetSize(),
                        FRAGMENTED = CONSTANTS.NOT_FRAGMENT,
                        LASTMSG = CONSTANTS.LASTMSG,
                        SEQ = 0
                    }; // Here is definition of response

                    if ( answer != "yes" )
                    {
                        rspMsg.Body = new BodyResponse()
                        {
                            MSGID = reqMsg.Header.MSGID,
                            RESPONSE = CONSTANTS.DENIED
                        }; //if answer is no, response msg body is changed

                        MessageUtil.Send( stream, rspMsg ); // send respon msg
                        stream.Close();
                        clinet.Close();
                        continue;
                    }
                    else
                        MessageUtil.Send( stream, rspMsg );

                    Console.WriteLine( "Transfer is starting" );

                    long fileSize = reqbody.FILESIZE;
                    string filename = Path.GetFileName( Encoding.Default.GetString(reqbody.FILENAME));
                    FileStream file = new FileStream(dir + "\\" + filename , FileMode.Create);

                    uint? dataMsgId = null;
                    ushort prevSeq = 0;

                    while ( ( reqMsg = MessageUtil.Receive( stream ) ) != null )
                    {
                        Console.Write( "#" );
                        if ( reqMsg.Header.MSGTYPE != CONSTANTS.FILE_SEND_DATA )
                            break;

                        if ( dataMsgId == null )
                            dataMsgId = reqMsg.Header.MSGID;
                        else
                        {
                            if ( dataMsgId != reqMsg.Header.MSGID )
                                break;
                        }

                        if ( prevSeq++ != reqMsg.Header.SEQ )
                        {
                            Console.WriteLine( "{0} , {1}", prevSeq, reqMsg.Header.SEQ );
                            break;
                        }

                        file.Write( reqMsg.Body.GetByte(), 0, reqMsg.Body.GetSize() );

                        if ( reqMsg.Header.FRAGMENTED == CONSTANTS.NOT_FRAGMENT )
                            break;

                        if ( reqMsg.Header.LASTMSG == CONSTANTS.LASTMSG )
                            break;
                    }

                    long recvFileSzie = file.Length;
                    file.Close();
                    Console.WriteLine();
                    Console.WriteLine( " Recived File Size : {0} byte", recvFileSzie );

                    Message rstMsg   = new Message();
                    rstMsg.Body = new BodyResult()
                    {
                        MSGID = reqMsg.Header.MSGID,
                        RESULT = CONSTANTS.SUCCESS
                    };

                    rstMsg.Header = new Header()
                    {
                        MSGID = msgId++,
                        MSGTYPE = CONSTANTS.FILE_SEND_RES,
                        BODYLEN = (uint)rstMsg.Body.GetSize(),
                        FRAGMENTED = CONSTANTS.NOT_FRAGMENT,
                        LASTMSG = CONSTANTS.LASTMSG,
                        SEQ = 0
                    };

                    if ( fileSize == recvFileSzie )
                        MessageUtil.Send( stream, rstMsg );
                    else
                    {
                        rstMsg.Body = new BodyResult()
                        {
                            MSGID = reqMsg.Header.MSGID,
                            RESULT = CONSTANTS.FAIL
                        };

                        MessageUtil.Send( stream, rspMsg );
                    }
                    Console.WriteLine( "File Transfer is finished" );

                    stream.Close();
                    clinet.Close();
                }
            }
            catch ( SocketException ex )
            {
                Console.WriteLine( ex );
            }
            finally
            {
                server.Stop();
            }
            Console.WriteLine( "Sercer is closed" );
        }
    }
}