// QInterface v1.0 (c) 2022 Sensei (aka 'Q')
// Lists network interfaces in CSV format with TCP v4 and v6 statistics.
//
// Usage:
// QInterface
// QInterface >>stats.txt
// schtasks /CREATE /TN NetStats /SC MINUTE /RI 1 /TR "QStart C:\Users\[user]\Documents\NetStats.bat"
// (for each minute, generate a report and save it to a designated file)
// Don't open .csv in OpenOffice because it locks the file writing..
//
// Batch script (assuming the folder with QInterface is added to PATH environment variable):
// @echo off
// QInterface >>C:\Users\Sensei\Documents\stats.csv
//
// Compilation:
// %SYSTEMROOT%\Microsoft.NET\Framework\v4.0.30319\csc QInterface.cs
//
// Disable TCPv6 code to use v3.5 compiler:
// %SYSTEMROOT%\Microsoft.NET\Framework\v3.5\csc QInterface.cs

using System;
using System.Net;
using System.Net.NetworkInformation;

public static class QInterface {
   public static void Main( string [] args ) {
      try {
         IPGlobalProperties computerProperties = IPGlobalProperties.GetIPGlobalProperties();
         NetworkInterface[] nics = NetworkInterface.GetAllNetworkInterfaces();
         if( nics != null ) {
            foreach( NetworkInterface nic in nics ) {
               if( nic.NetworkInterfaceType == NetworkInterfaceType.Loopback ) continue;
               if( nic.NetworkInterfaceType == NetworkInterfaceType.Tunnel ) continue;
               Console.Write( "{0},", DateTime.Now.ToString( "yyyyMMdd" ) );
               Console.Write( "{0},", DateTime.Now.ToString( "HHmmss" ) );
               Console.Write( "\"{0}\",", computerProperties.HostName );
               Console.Write( "\"{0}\",", computerProperties.DomainName );
               Console.Write( "\"{0}\",", nic.NetworkInterfaceType );
               Console.Write( "\"{0}\",", nic.OperationalStatus );
               PhysicalAddress address = nic.GetPhysicalAddress();
               byte[] bytes = address.GetAddressBytes();
               for( int i = 0; i < bytes.Length; i++ ) {
                  Console.Write( "{0:X2}", bytes[i] );
               }
               Console.Write( ',' );
               IPv4InterfaceStatistics statistics4 = nic.GetIPv4Statistics(); // TCPv4
               Console.Write( "{0},", statistics4.BytesReceived );
               Console.Write( "{0},", statistics4.BytesSent );
               // TCPv6 START
               IPInterfaceStatistics statistics = nic.GetIPStatistics();
               Console.Write( "{0},", statistics.BytesReceived - statistics4.BytesReceived ); // TCPv6
               Console.Write( "{0},", statistics.BytesSent - statistics4.BytesSent );
               Console.Write( "{0},", statistics.BytesReceived ); // Both
               Console.Write( "{0}", statistics.BytesSent );
               // TCPv6 END
               Console.WriteLine();
            }
         }
      } catch( Exception e ) {
         Console.Error.WriteLine( e.Message );
      }
   }
}
