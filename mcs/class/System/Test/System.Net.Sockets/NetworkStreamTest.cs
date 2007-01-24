// System.Net.Sockets.NetworkStreamTest.cs
//
// Author:
//	Dick Porter (dick@ximian.com)
//
// Copyright (C) 2007 Novell, Inc (http://www.novell.com)
//

using System.Net.Sockets;
using System.Net;
using System;
using System.IO;
using NUnit.Framework;


namespace MonoTests.System.Net.Sockets
{
	[TestFixture]
	public class NetworkStreamTest
	{
#if NET_2_0
		[Test]
		public void ReadTimeout ()
		{
			Socket sock = new Socket (AddressFamily.InterNetwork,
						  SocketType.Stream,
						  ProtocolType.Tcp);
			Socket listen = new Socket (AddressFamily.InterNetwork,
						    SocketType.Stream,
						    ProtocolType.Tcp);
			IPEndPoint ep = new IPEndPoint (IPAddress.Loopback, 0);
			
			listen.Bind (ep);
			listen.Listen (1);
			
			sock.Connect (listen.LocalEndPoint);
			
			NetworkStream stream = new NetworkStream (sock);
			stream.ReadTimeout = 1000;

			byte[] buf = new byte[1024];
			
			try {
				stream.Read (buf, 0, buf.Length);
				Assert.Fail ("ReadTimeout #1");
			} catch (IOException ex) {
				Exception inner = ex.InnerException;
				SocketException sockex = inner as SocketException;
				
				Assert.IsNotNull (sockex, "ReadTimeout #2");

/* Linux gives error 10035 (EWOULDBLOCK) here, whereas windows has 10060 (ETIMEDOUT)
				Assertion.AssertEquals ("ReadTimeout #3",
							10060,
							sockex.ErrorCode);
*/
			} catch {
				Assert.Fail ("ReadTimeout #4");
			} finally {
				stream.Close ();
				sock.Close ();
				listen.Close ();
			}
		}
#endif
	}
}
