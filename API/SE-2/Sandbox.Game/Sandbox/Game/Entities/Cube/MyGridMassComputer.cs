using System;
using System.Buffers;
using System.Collections.Generic;
using Havok;
using VRage.Generics;
using VRageMath;

namespace Sandbox.Game.Entities.Cube
{
	internal class MyGridMassComputer : MySparseGrid<HkMassElement, MassCellData>
	{
		private const float DefaultUpdateThreshold = 0.05f;

		private float m_updateThreshold;

		private HkMassProperties m_massProperties;

		public float UpdateThreshold => m_updateThreshold;

		public MyGridMassComputer(int cellSize, float updateThreshold = 0.05f)
			: base(cellSize)
		{
			m_updateThreshold = updateThreshold;
		}

		public HkMassProperties UpdateMass()
		{
			//IL_0024: Unknown result type (might be due to invalid IL or missing references)
			//IL_0029: Unknown result type (might be due to invalid IL or missing references)
			HkMassElement hkMassElement = default(HkMassElement);
			hkMassElement.Tranform = Matrix.Identity;
			HkMassElement massElement = hkMassElement;
			bool flag = false;
			Enumerator<Vector3I> enumerator = base.DirtyCells.GetEnumerator();
			Span<HkMassElement> elements;
			HkMassElement[] array;
			int num2;
			try
			{
<<<<<<< HEAD
				if (TryGetCell(dirtyCell, out var result))
=======
				while (enumerator.MoveNext())
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
				{
					Vector3I current = enumerator.get_Current();
					if (TryGetCell(current, out var result))
					{
						array = ArrayPool<HkMassElement>.Shared.Rent(result.Items.Count);
						elements = new Span<HkMassElement>(array, 0, result.Items.Count);
						float num = 0f;
						num2 = 0;
						foreach (KeyValuePair<Vector3I, HkMassElement> item in result.Items)
						{
							num += item.Value.Properties.Mass;
							elements[num2++] = item.Value;
						}
						if (Math.Abs(1f - result.CellData.LastMass / num) > m_updateThreshold)
						{
							HkInertiaTensorComputer.CombineMassProperties(elements, out massElement.Properties);
							result.CellData.MassElement = massElement;
							result.CellData.LastMass = num;
							flag = true;
						}
						ArrayPool<HkMassElement>.Shared.Return(array);
					}
					else
					{
						flag = true;
					}
				}
			}
			finally
			{
				((IDisposable)enumerator).Dispose();
			}
			UnmarkDirtyAll();
			if (!flag)
			{
				return m_massProperties;
			}
			array = ArrayPool<HkMassElement>.Shared.Rent(base.CellCount);
			elements = new Span<HkMassElement>(array, 0, base.CellCount);
			num2 = 0;
			using (Dictionary<Vector3I, Cell>.Enumerator enumerator3 = GetEnumerator())
			{
				while (enumerator3.MoveNext())
				{
					KeyValuePair<Vector3I, Cell> current3 = enumerator3.Current;
					elements[num2++] = current3.Value.CellData.MassElement;
				}
			}
			if (base.ItemCount > 0)
			{
				HkInertiaTensorComputer.CombineMassProperties(elements, out m_massProperties);
			}
			else
			{
				m_massProperties = default(HkMassProperties);
			}
			ArrayPool<HkMassElement>.Shared.Return(array);
			return m_massProperties;
		}
	}
}
