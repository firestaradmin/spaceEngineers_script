using System.IO;
using System.IO.Pipes;
using System.Text;
using System.Threading.Tasks;

namespace VRage.Utils
{
	public class MyConsolePipeWriter : TextWriter
	{
		private static object lockObject = new object();

		private NamedPipeClientStream m_pipeStream;

		private StreamWriter m_writer;

		private bool isConnecting;

		public override Encoding Encoding => Encoding.UTF8;

		public MyConsolePipeWriter(string name)
		{
			//IL_0008: Unknown result type (might be due to invalid IL or missing references)
			//IL_0012: Expected O, but got Unknown
			m_pipeStream = new NamedPipeClientStream(name);
			m_writer = new StreamWriter((Stream)(object)m_pipeStream);
			StartConnectThread();
		}

		public override void Write(string value)
		{
			if (((PipeStream)m_pipeStream).get_IsConnected())
			{
				try
				{
					m_writer.Write(value);
					m_writer.Flush();
				}
				catch (IOException)
				{
					StartConnectThread();
				}
			}
			else
			{
				StartConnectThread();
			}
		}

		public override void WriteLine(string value)
		{
			if (((PipeStream)m_pipeStream).get_IsConnected())
			{
				try
				{
					m_writer.WriteLine(value);
					m_writer.Flush();
				}
				catch (IOException)
				{
					StartConnectThread();
				}
			}
			else
			{
				StartConnectThread();
			}
		}

		private void StartConnectThread()
		{
			lock (lockObject)
			{
				if (isConnecting)
				{
					return;
				}
				isConnecting = true;
			}
			Task.Run(delegate
			{
				m_pipeStream.Connect();
				lock (lockObject)
				{
					isConnecting = false;
				}
			});
		}

		public override void Close()
		{
			base.Close();
			try
			{
				if (((PipeStream)m_pipeStream).get_IsConnected())
				{
					((PipeStream)m_pipeStream).WaitForPipeDrain();
					m_writer.Close();
					m_writer.Dispose();
					((Stream)(object)m_pipeStream).Close();
					((Stream)(object)m_pipeStream).Dispose();
				}
			}
			catch
			{
			}
		}
	}
}
