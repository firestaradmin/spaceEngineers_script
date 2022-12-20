using VRageMath;

namespace Sandbox.Game.Entities.Planet
{
	public static class MyPlanetCubemapHelper
	{
		public static uint[] AdjacentFaceTransforms = new uint[36]
		{
			0u, 0u, 0u, 16u, 10u, 26u, 0u, 0u, 16u, 0u,
			6u, 22u, 16u, 0u, 0u, 0u, 3u, 31u, 0u, 16u,
			0u, 0u, 15u, 19u, 25u, 5u, 19u, 15u, 0u, 0u,
			9u, 21u, 31u, 3u, 0u, 0u
		};

		/// Find the appropriate face and project a position to it's local space.
		///
		/// There was some fishy here but the face transforms depend on this method's behaviour so it will stay for now, I hope I can fix it before release.
		public static void ProjectToCube(ref Vector3D localPos, out int direction, out Vector2D texcoords)
		{
			Vector3D.Abs(ref localPos, out var abs);
			if (abs.X > abs.Y)
			{
				if (abs.X > abs.Z)
				{
					localPos /= abs.X;
					texcoords.Y = localPos.Y;
					if (localPos.X > 0.0)
					{
						texcoords.X = 0.0 - localPos.Z;
						direction = 3;
					}
					else
					{
						texcoords.X = localPos.Z;
						direction = 2;
					}
				}
				else
				{
					localPos /= abs.Z;
					texcoords.Y = localPos.Y;
					if (localPos.Z > 0.0)
					{
						texcoords.X = localPos.X;
						direction = 1;
					}
					else
					{
						texcoords.X = 0.0 - localPos.X;
						direction = 0;
					}
				}
			}
			else if (abs.Y > abs.Z)
			{
				localPos /= abs.Y;
				texcoords.Y = localPos.X;
				if (localPos.Y > 0.0)
				{
					texcoords.X = localPos.Z;
					direction = 4;
				}
				else
				{
					texcoords.X = 0.0 - localPos.Z;
					direction = 5;
				}
			}
			else
			{
				localPos /= abs.Z;
				texcoords.Y = localPos.Y;
				if (localPos.Z > 0.0)
				{
					texcoords.X = localPos.X;
					direction = 1;
				}
				else
				{
					texcoords.X = 0.0 - localPos.X;
					direction = 0;
				}
			}
		}

		/// Find the appropriate face for the projection of a point.
		public static int FindCubeFace(ref Vector3D localPos)
		{
			Vector3D.Abs(ref localPos, out var abs);
			if (abs.X > abs.Y)
			{
				if (abs.X > abs.Z)
				{
					if (localPos.X > 0.0)
					{
						return 3;
					}
					return 2;
				}
				if (localPos.Z > 0.0)
				{
					return 1;
				}
				return 0;
			}
			if (abs.Y > abs.Z)
			{
				if (localPos.Y > 0.0)
				{
					return 4;
				}
				return 5;
			}
			if (localPos.Z > 0.0)
			{
				return 1;
			}
			return 0;
		}

		/// Project the position to local space for a provided face.
		public static void ProjectForFace(ref Vector3D localPos, int face, out Vector2D normalCoord)
		{
			Vector3D.Abs(ref localPos, out var abs);
			switch ((byte)face)
			{
			case 0:
				localPos /= abs.Z;
				normalCoord.X = 0.0 - localPos.X;
				normalCoord.Y = localPos.Y;
				break;
			case 1:
				localPos /= abs.Z;
				normalCoord.X = localPos.X;
				normalCoord.Y = localPos.Y;
				break;
			case 2:
				localPos /= abs.X;
				normalCoord.X = localPos.Z;
				normalCoord.Y = localPos.Y;
				break;
			case 3:
				localPos /= abs.X;
				normalCoord.X = 0.0 - localPos.Z;
				normalCoord.Y = localPos.Y;
				break;
			case 4:
				localPos /= abs.Y;
				normalCoord.X = localPos.Z;
				normalCoord.Y = localPos.X;
				break;
			case 5:
				localPos /= abs.Y;
				normalCoord.X = 0.0 - localPos.Z;
				normalCoord.Y = localPos.X;
				break;
			default:
				normalCoord = Vector2D.Zero;
				break;
			}
		}

		/// Get cubemap forward up for a given face.
		public static void GetForwardUp(Base6Directions.Direction axis, out Vector3D forward, out Vector3D up)
		{
			forward = Base6Directions.Directions[(uint)axis];
			up = Base6Directions.Directions[(uint)Base6Directions.GetPerpendicular(axis)];
		}

		/// Translate texcoords from one face to the next.
		public unsafe static void TranslateTexcoordsToFace(ref Vector2D texcoords, int originalFace, int myFace, out Vector2D newCoords)
		{
			Vector2D vector2D = texcoords;
			if ((originalFace & -2) != (myFace & -2))
			{
				uint num = AdjacentFaceTransforms[myFace * 6 + originalFace];
				double* ptr = (double*)(&vector2D);
				if ((num & 1) != ((num >> 1) & 1))
				{
					double num2 = *ptr;
					*ptr = ptr[1];
					ptr[1] = num2;
				}
				uint num3 = (num >> 1) & 1u;
				if (((num >> 2) & (true ? 1u : 0u)) != 0)
				{
					ptr[num3] = 0.0 - ptr[num3];
				}
				if (((num >> 3) & (true ? 1u : 0u)) != 0)
				{
					ptr[1 ^ num3] = 0.0 - ptr[1 ^ num3];
				}
				if (((num >> 4) & (true ? 1u : 0u)) != 0)
				{
					ptr[num3] -= 2.0;
				}
				else
				{
					ptr[num3] += 2.0;
				}
			}
			newCoords = vector2D;
		}
	}
}
