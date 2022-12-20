using System;
<<<<<<< HEAD
=======
using System.Collections.Generic;
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
using System.Linq;
using System.Xml.Serialization;

namespace VRageRender
{
	public class MyNewLoddingSettings
	{
		public MyPassLoddingSetting GBuffer = MyPassLoddingSetting.Default;

		private MyPassLoddingSetting[] m_cascadeDepth = new MyPassLoddingSetting[0];

		public MyPassLoddingSetting SingleDepth = MyPassLoddingSetting.Default;

		public MyPassLoddingSetting Forward = MyPassLoddingSetting.Default;

		public MyGlobalLoddingSettings Global = MyGlobalLoddingSettings.Default;

		[XmlArrayItem("CascadeDepth")]
		public MyPassLoddingSetting[] CascadeDepths
		{
			get
			{
				return m_cascadeDepth;
			}
			set
			{
				if (m_cascadeDepth.Length != value.Length)
				{
					m_cascadeDepth = new MyPassLoddingSetting[value.Length];
				}
				value.CopyTo(m_cascadeDepth, 0);
			}
		}

		public void CopyFrom(MyNewLoddingSettings settings)
		{
			GBuffer = settings.GBuffer;
			CascadeDepths = settings.CascadeDepths;
			SingleDepth = settings.SingleDepth;
			Forward = settings.Forward;
			Global = settings.Global;
		}

		public override bool Equals(object obj)
		{
			if (!(obj is MyNewLoddingSettings))
			{
				return false;
			}
			MyNewLoddingSettings myNewLoddingSettings = (MyNewLoddingSettings)obj;
			if (!GBuffer.Equals(myNewLoddingSettings.GBuffer))
			{
				return false;
			}
<<<<<<< HEAD
			if (!CascadeDepths.SequenceEqual(myNewLoddingSettings.CascadeDepths))
=======
			if (!Enumerable.SequenceEqual<MyPassLoddingSetting>((IEnumerable<MyPassLoddingSetting>)CascadeDepths, (IEnumerable<MyPassLoddingSetting>)myNewLoddingSettings.CascadeDepths))
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			{
				return false;
			}
			if (!SingleDepth.Equals(myNewLoddingSettings.SingleDepth))
			{
				return false;
			}
			if (!Forward.Equals(myNewLoddingSettings.Forward))
			{
				return false;
			}
			if (!Global.Equals(myNewLoddingSettings.Global))
			{
				return false;
			}
			return true;
		}

<<<<<<< HEAD
		public override int GetHashCode()
		{
			return GBuffer.GetHashCode() ^ CascadeDepths.GetHashCode() ^ SingleDepth.GetHashCode() ^ Forward.GetHashCode() ^ Global.GetHashCode();
		}

=======
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		public int CommonMinLod()
		{
			int val = GBuffer.MinLod;
			for (int i = 0; i < CascadeDepths.Length; i++)
			{
				val = Math.Min(val, CascadeDepths[i].MinLod);
			}
			val = Math.Min(val, SingleDepth.MinLod);
			return Math.Min(val, Forward.MinLod);
		}
	}
}
