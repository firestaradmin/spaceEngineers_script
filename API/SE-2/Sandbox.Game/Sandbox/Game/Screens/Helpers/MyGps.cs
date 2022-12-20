using System;
using System.Globalization;
using System.Text;
using Sandbox.Game.Entities;
using Sandbox.Game.World;
using VRage;
using VRage.Game;
using VRage.Game.Components;
using VRage.Game.ModAPI;
using VRage.ModAPI;
using VRage.Utils;
using VRageMath;

namespace Sandbox.Game.Screens.Helpers
{
	public class MyGps : IMyGps
	{
		internal static readonly int DROP_NONFINAL_AFTER_SEC = 180;

		private IMyEntity m_entity;

		private long m_entityId;

		private long m_contractId;

		private string m_displayName = string.Empty;

		private Vector3D m_coords;

		private Color m_color = new Color(117, 201, 241);

		public string Name { get; set; }

		public bool IsObjective { get; set; }

		public string DisplayName
		{
			get
			{
				return m_displayName;
			}
			set
			{
				m_displayName = value;
			}
		}

		public string Description { get; set; }

		public Vector3D Coords
		{
			get
			{
				if (CoordsFunc != null)
				{
					return CoordsFunc();
				}
				if (m_entityId != 0L && m_entity == null)
				{
					IMyEntity entityById = MyEntities.GetEntityById(m_entityId);
					if (entityById != null)
					{
						SetEntity(entityById);
					}
				}
				return m_coords;
			}
			set
			{
				m_coords = value;
			}
		}

		public Color GPSColor
		{
			get
			{
				return m_color;
			}
			set
			{
				m_color = value;
			}
		}

		public bool ShowOnHud { get; set; }

		public bool AlwaysVisible { get; set; }

<<<<<<< HEAD
		/// <summary>
		/// final=null. Not final=time at which we should drop it from the list, relative to ElapsedPlayTime
		/// GPS entry may be confirmed or uncorfirmed. Uncorfirmed has valid DiscardAt.
		/// </summary>
=======
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		public TimeSpan? DiscardAt { get; set; }

		public bool IsLocal { get; set; }

		public Func<Vector3D> CoordsFunc { get; set; }

		public long EntityId => m_entityId;

		public long ContractId
		{
			get
			{
				return m_contractId;
			}
			set
			{
				m_contractId = value;
			}
		}

		public bool IsContainerGPS { get; set; }

		public string ContainerRemainingTime { get; set; }

		public int Hash { get; private set; }

		string IMyGps.Name
		{
			get
			{
				return Name;
			}
			set
			{
				if (value == null)
				{
					throw new ArgumentNullException("Value must not be null!");
				}
				Name = value;
			}
		}

		string IMyGps.Description
		{
			get
			{
				return Description;
			}
			set
			{
				if (value == null)
				{
					throw new ArgumentNullException("Value must not be null!");
				}
				Description = value;
			}
		}

		Vector3D IMyGps.Coords
		{
			get
			{
				return Coords;
			}
			set
			{
				Coords = value;
			}
		}

		Color IMyGps.GPSColor
		{
			get
			{
				return GPSColor;
			}
			set
			{
				GPSColor = value;
			}
		}

		bool IMyGps.ShowOnHud
		{
			get
			{
				return ShowOnHud;
			}
			set
			{
				ShowOnHud = value;
			}
		}

		TimeSpan? IMyGps.DiscardAt
		{
			get
			{
				return DiscardAt;
			}
			set
			{
				DiscardAt = value;
			}
		}

		public MyGps(MyObjectBuilder_Gps.Entry builder)
		{
			Name = builder.name;
			DisplayName = builder.DisplayName;
			Description = builder.description;
			Coords = builder.coords;
			ShowOnHud = builder.showOnHud;
			AlwaysVisible = builder.alwaysVisible;
			IsObjective = builder.isObjective;
			ContractId = builder.contractId;
			if (builder.color != Color.Transparent && builder.color != Color.Black)
			{
				GPSColor = builder.color;
			}
			else
			{
				GPSColor = new Color(117, 201, 241);
			}
			if (!builder.isFinal)
			{
				SetDiscardAt();
			}
			SetEntityId(builder.entityId);
			UpdateHash();
		}

		public MyGps()
		{
			GPSColor = new Color(117, 201, 241);
			SetDiscardAt();
		}

		public void SetDiscardAt()
		{
			DiscardAt = TimeSpan.FromSeconds(MySession.Static.ElapsedPlayTime.TotalSeconds + (double)DROP_NONFINAL_AFTER_SEC);
		}

		public void SetEntity(IMyEntity entity)
		{
			if (entity != null)
			{
				m_entity = entity;
				m_entityId = entity.EntityId;
				m_entity.PositionComp.OnPositionChanged += PositionComp_OnPositionChanged;
				m_entity.NeedsWorldMatrix = true;
				m_entity.OnClose += m_entity_OnClose;
				Coords = m_entity.PositionComp.GetPosition();
			}
		}

		public void SetEntityId(long entityId)
		{
			if (entityId != 0L)
			{
				m_entityId = entityId;
			}
		}

		private void m_entity_OnClose(IMyEntity obj)
		{
			if (m_entity != null)
			{
				m_entity.PositionComp.OnPositionChanged -= PositionComp_OnPositionChanged;
				m_entity.OnClose -= m_entity_OnClose;
				m_entity = null;
			}
		}

		private void PositionComp_OnPositionChanged(MyPositionComponentBase obj)
		{
			if (m_entity != null)
			{
				Coords = m_entity.PositionComp.GetPosition();
			}
		}

		public void Close()
		{
			if (m_entity != null)
			{
				m_entity.PositionComp.OnPositionChanged -= PositionComp_OnPositionChanged;
				m_entity.OnClose -= m_entity_OnClose;
			}
		}

		public int CalculateHash()
		{
			int hash = MyUtils.GetHash(Name);
			if (m_entityId == 0L)
			{
				hash = MyUtils.GetHash(Coords.X, hash);
				hash = MyUtils.GetHash(Coords.Y, hash);
				return MyUtils.GetHash(Coords.Z, hash);
			}
			return hash * m_entityId.GetHashCode();
		}

		public void UpdateHash()
		{
			int num2 = (Hash = CalculateHash());
		}

		public override int GetHashCode()
		{
			return Hash;
		}

		public override string ToString()
		{
			return ConvertToString(this);
		}

		internal static string ConvertToString(MyGps gps)
		{
			return ConvertToString(gps.Name, gps.Coords, gps.GPSColor);
		}

		internal static string ConvertToString(string name, Vector3D coords, Color? color = null)
		{
			if (!color.HasValue)
			{
				color = new Color(117, 201, 241);
			}
			ColorDefinitionRGBA colorDefinitionRGBA = new ColorDefinitionRGBA(color.Value.R, color.Value.G, color.Value.B, color.Value.A);
			StringBuilder stringBuilder = new StringBuilder("GPS:", 256);
			stringBuilder.Append(name);
			stringBuilder.Append(":");
			stringBuilder.Append(coords.X.ToString(CultureInfo.InvariantCulture));
			stringBuilder.Append(":");
			stringBuilder.Append(coords.Y.ToString(CultureInfo.InvariantCulture));
			stringBuilder.Append(":");
			stringBuilder.Append(coords.Z.ToString(CultureInfo.InvariantCulture));
			stringBuilder.Append(":");
			stringBuilder.Append(colorDefinitionRGBA.Hex);
			stringBuilder.Append(":");
			return stringBuilder.ToString();
		}

		public void ToClipboard()
		{
			MyVRage.Platform.System.Clipboard = ToString();
		}
	}
}
