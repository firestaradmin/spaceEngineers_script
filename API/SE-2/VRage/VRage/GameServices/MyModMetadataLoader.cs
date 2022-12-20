using System;
using System.IO;
using System.Xml.Serialization;
using VRage.FileSystem;
using VRage.Utils;

namespace VRage.GameServices
{
	public class MyModMetadataLoader
	{
		public static ModMetadataFile Parse(string xml)
		{
			//IL_0016: Unknown result type (might be due to invalid IL or missing references)
			//IL_001c: Expected O, but got Unknown
			//IL_001d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0023: Expected O, but got Unknown
			if (string.IsNullOrEmpty(xml))
			{
				return null;
			}
			ModMetadataFile result = null;
			try
			{
				XmlSerializer val = new XmlSerializer(typeof(ModMetadataFile));
				using TextReader textReader = new StringReader(xml);
				result = (ModMetadataFile)val.Deserialize(textReader);
				return result;
			}
			catch (Exception ex)
			{
				MyLog.Default.Warning("Failed parsing mod metadata: {0}", ex.Message);
				return result;
			}
		}

		public static string Serialize(ModMetadataFile data)
		{
			//IL_0010: Unknown result type (might be due to invalid IL or missing references)
			//IL_0016: Expected O, but got Unknown
			//IL_0016: Unknown result type (might be due to invalid IL or missing references)
			//IL_001c: Expected O, but got Unknown
			if (data == null)
			{
				return null;
			}
			try
			{
				XmlSerializer val = new XmlSerializer(typeof(ModMetadataFile));
				using TextWriter textWriter = new StringWriter();
				val.Serialize(textWriter, (object)data);
				return textWriter.ToString();
			}
			catch (Exception ex)
			{
				MyLog.Default.Warning("Failed serializing mod metadata: {0}", ex.Message);
				return null;
			}
		}

		public static ModMetadataFile Load(string filename)
		{
			//IL_0016: Unknown result type (might be due to invalid IL or missing references)
			//IL_001c: Expected O, but got Unknown
			if (string.IsNullOrEmpty(filename))
			{
				return null;
			}
			ModMetadataFile result = null;
			try
			{
				XmlSerializer val = new XmlSerializer(typeof(ModMetadataFile));
				Stream stream = MyFileSystem.OpenRead(filename);
				if (stream != null)
				{
<<<<<<< HEAD
					result = (ModMetadataFile)xmlSerializer.Deserialize(stream);
=======
					result = (ModMetadataFile)val.Deserialize(stream);
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
					stream.Close();
					return result;
				}
				return result;
			}
			catch (Exception ex)
			{
				MyLog.Default.Warning("Failed loading mod metadata file: {0} with exception: {1}", filename, ex.Message);
				return result;
			}
		}

		public static bool Save(string filename, ModMetadataFile file)
		{
			//IL_0018: Unknown result type (might be due to invalid IL or missing references)
			if (string.IsNullOrEmpty(filename) || file == null)
			{
				return false;
			}
			try
			{
				XmlSerializer val = new XmlSerializer(typeof(ModMetadataFile));
				TextWriter textWriter = new StreamWriter(filename);
				val.Serialize(textWriter, (object)file);
				textWriter.Close();
			}
			catch (Exception ex)
			{
				MyLog.Default.Warning("Failed saving mod metadata file: {0} with exception: {1}", filename, ex.Message);
				return false;
			}
			return true;
		}
	}
}
