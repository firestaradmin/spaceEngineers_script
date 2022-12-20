using System;
using System.IO;
using BulletXNA.LinearMath;

namespace BulletXNA.BulletCollision
{
	public class QuantizedBvhTree
	{
		public const int MAX_INDICES_PER_NODE = 6;

		private int m_num_nodes;

		private GIM_QUANTIZED_BVH_NODE_ARRAY m_node_array;

		private AABB m_global_bound;

		private IndexedVector3 m_bvhQuantization;

		private static void WriteIndexedVector3(IndexedVector3 vector, BinaryWriter bw)
		{
			bw.Write(vector.X);
			bw.Write(vector.Y);
			bw.Write(vector.Z);
		}

		private static IndexedVector3 ReadIndexedVector3(BinaryReader br)
		{
			return new IndexedVector3(br.ReadSingle(), br.ReadSingle(), br.ReadSingle());
		}

		private static void WriteUShortVector3(UShortVector3 vector, BinaryWriter bw)
		{
			bw.Write(vector.X);
			bw.Write(vector.Y);
			bw.Write(vector.Z);
		}

		private static UShortVector3 ReadUShortVector3(BinaryReader br)
		{
			UShortVector3 result = default(UShortVector3);
			result.X = br.ReadUInt16();
			result.Y = br.ReadUInt16();
			result.Z = br.ReadUInt16();
			return result;
		}

		internal unsafe byte[] Save()
		{
<<<<<<< HEAD
			using (MemoryStream memoryStream = new MemoryStream())
			{
				using (BinaryWriter binaryWriter = new BinaryWriter(memoryStream))
				{
					binaryWriter.Write(m_num_nodes);
					WriteIndexedVector3(m_global_bound.m_min, binaryWriter);
					WriteIndexedVector3(m_global_bound.m_max, binaryWriter);
					WriteIndexedVector3(m_bvhQuantization, binaryWriter);
					for (int i = 0; i < m_num_nodes; i++)
					{
						BT_QUANTIZED_BVH_NODE bT_QUANTIZED_BVH_NODE = m_node_array[i];
						binaryWriter.Write((int)bT_QUANTIZED_BVH_NODE.m_size);
						for (int j = 0; j < bT_QUANTIZED_BVH_NODE.m_size; j++)
						{
							binaryWriter.Write(bT_QUANTIZED_BVH_NODE.m_escapeIndexOrDataIndex[j]);
						}
						WriteUShortVector3(bT_QUANTIZED_BVH_NODE.m_quantizedAabbMin, binaryWriter);
						WriteUShortVector3(bT_QUANTIZED_BVH_NODE.m_quantizedAabbMax, binaryWriter);
					}
					return memoryStream.ToArray();
				}
			}
=======
			using MemoryStream memoryStream = new MemoryStream();
			using BinaryWriter binaryWriter = new BinaryWriter(memoryStream);
			binaryWriter.Write(m_num_nodes);
			WriteIndexedVector3(m_global_bound.m_min, binaryWriter);
			WriteIndexedVector3(m_global_bound.m_max, binaryWriter);
			WriteIndexedVector3(m_bvhQuantization, binaryWriter);
			for (int i = 0; i < m_num_nodes; i++)
			{
				BT_QUANTIZED_BVH_NODE bT_QUANTIZED_BVH_NODE = m_node_array[i];
				binaryWriter.Write((int)bT_QUANTIZED_BVH_NODE.m_size);
				for (int j = 0; j < bT_QUANTIZED_BVH_NODE.m_size; j++)
				{
					binaryWriter.Write(bT_QUANTIZED_BVH_NODE.m_escapeIndexOrDataIndex[j]);
				}
				WriteUShortVector3(bT_QUANTIZED_BVH_NODE.m_quantizedAabbMin, binaryWriter);
				WriteUShortVector3(bT_QUANTIZED_BVH_NODE.m_quantizedAabbMax, binaryWriter);
			}
			return memoryStream.ToArray();
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		}

		internal unsafe void Load(byte[] byteArray)
		{
<<<<<<< HEAD
			using (MemoryStream input = new MemoryStream(byteArray))
			{
				using (BinaryReader binaryReader = new BinaryReader(input))
				{
					m_num_nodes = binaryReader.ReadInt32();
					IndexedVector3 min = ReadIndexedVector3(binaryReader);
					IndexedVector3 max = ReadIndexedVector3(binaryReader);
					m_global_bound = new AABB(ref min, ref max);
					m_bvhQuantization = ReadIndexedVector3(binaryReader);
					m_node_array = new GIM_QUANTIZED_BVH_NODE_ARRAY(m_num_nodes);
					for (int i = 0; i < m_num_nodes; i++)
					{
						int num = binaryReader.ReadInt32();
						if (num > 6)
						{
							throw new Exception();
						}
						BT_QUANTIZED_BVH_NODE item = default(BT_QUANTIZED_BVH_NODE);
						item.m_size = (byte)num;
						for (int j = 0; j < num; j++)
						{
							item.m_escapeIndexOrDataIndex[j] = binaryReader.ReadInt32();
						}
						item.m_quantizedAabbMin = ReadUShortVector3(binaryReader);
						item.m_quantizedAabbMax = ReadUShortVector3(binaryReader);
						m_node_array.Add(item);
					}
				}
=======
			using MemoryStream input = new MemoryStream(byteArray);
			using BinaryReader binaryReader = new BinaryReader(input);
			m_num_nodes = binaryReader.ReadInt32();
			IndexedVector3 min = ReadIndexedVector3(binaryReader);
			IndexedVector3 max = ReadIndexedVector3(binaryReader);
			m_global_bound = new AABB(ref min, ref max);
			m_bvhQuantization = ReadIndexedVector3(binaryReader);
			m_node_array = new GIM_QUANTIZED_BVH_NODE_ARRAY(m_num_nodes);
			for (int i = 0; i < m_num_nodes; i++)
			{
				int num = binaryReader.ReadInt32();
				if (num > 6)
				{
					throw new Exception();
				}
				BT_QUANTIZED_BVH_NODE item = default(BT_QUANTIZED_BVH_NODE);
				item.m_size = (byte)num;
				for (int j = 0; j < num; j++)
				{
					item.m_escapeIndexOrDataIndex[j] = binaryReader.ReadInt32();
				}
				item.m_quantizedAabbMin = ReadUShortVector3(binaryReader);
				item.m_quantizedAabbMax = ReadUShortVector3(binaryReader);
				m_node_array.Add(item);
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			}
		}

		private void CalcQuantization(GIM_BVH_DATA_ARRAY primitive_boxes)
		{
			CalcQuantization(primitive_boxes, 1f);
		}

		private void CalcQuantization(GIM_BVH_DATA_ARRAY primitive_boxes, float boundMargin)
		{
			AABB aABB = default(AABB);
			aABB.Invalidate();
			int count = primitive_boxes.Count;
			for (int i = 0; i < count; i++)
			{
				aABB.Merge(ref primitive_boxes.GetRawArray()[i].m_bound);
			}
			GImpactQuantization.CalcQuantizationParameters(out m_global_bound.m_min, out m_global_bound.m_max, out m_bvhQuantization, ref aABB.m_min, ref aABB.m_max, boundMargin);
		}

		private int SortAndCalcSplittingIndex(GIM_BVH_DATA_ARRAY primitive_boxes, int startIndex, int endIndex, int splitAxis)
		{
			int num = startIndex;
			int num2 = endIndex - startIndex;
			float num3 = 0f;
			IndexedVector3 zero = IndexedVector3.Zero;
			for (int i = startIndex; i < endIndex; i++)
			{
				IndexedVector3 indexedVector = 0.5f * (primitive_boxes[i].m_bound.m_max + primitive_boxes[i].m_bound.m_min);
				zero += indexedVector;
			}
			num3 = (zero * (1f / (float)num2))[splitAxis];
			for (int i = startIndex; i < endIndex; i++)
			{
				if ((0.5f * (primitive_boxes[i].m_bound.m_max + primitive_boxes[i].m_bound.m_min))[splitAxis] > num3)
				{
					primitive_boxes.Swap(i, num);
					num++;
				}
			}
			int num4 = num2 / 3;
			if (num <= startIndex + num4 || num >= endIndex - 1 - num4)
			{
				num = startIndex + (num2 >> 1);
			}
			return num;
		}

		private int CalcSplittingAxis(GIM_BVH_DATA_ARRAY primitive_boxes, int startIndex, int endIndex)
		{
			IndexedVector3 zero = IndexedVector3.Zero;
			IndexedVector3 a = IndexedVector3.Zero;
			int num = endIndex - startIndex;
			for (int i = startIndex; i < endIndex; i++)
			{
				IndexedVector3 indexedVector = 0.5f * (primitive_boxes[i].m_bound.m_max + primitive_boxes[i].m_bound.m_min);
				zero += indexedVector;
			}
			zero *= 1f / (float)num;
			for (int i = startIndex; i < endIndex; i++)
			{
				IndexedVector3 indexedVector2 = 0.5f * (primitive_boxes[i].m_bound.m_max + primitive_boxes[i].m_bound.m_min);
				IndexedVector3 indexedVector3 = indexedVector2 - zero;
				indexedVector3 *= indexedVector3;
				a += indexedVector3;
			}
			a *= 1f / ((float)num - 1f);
			return MathUtil.MaxAxis(ref a);
		}

		private void BuildSubTree(GIM_BVH_DATA_ARRAY primitive_boxes, int startIndex, int endIndex)
		{
			int num_nodes = m_num_nodes;
			m_num_nodes++;
			if (endIndex - startIndex <= 6)
			{
				int num = endIndex - startIndex;
				Span<int> dataIndices = stackalloc int[num];
				AABB bound = default(AABB);
				bound.Invalidate();
				for (int i = 0; i < num; i++)
				{
					dataIndices[i] = primitive_boxes[startIndex + i].m_data;
					bound.Merge(primitive_boxes.GetRawArray()[startIndex + i].m_bound);
				}
				SetNodeBound(num_nodes, ref bound);
				m_node_array.GetRawArray()[num_nodes].SetDataIndices(dataIndices);
				return;
			}
			int splitAxis = CalcSplittingAxis(primitive_boxes, startIndex, endIndex);
			splitAxis = SortAndCalcSplittingIndex(primitive_boxes, startIndex, endIndex, splitAxis);
			AABB bound2 = default(AABB);
			bound2.Invalidate();
			for (int j = startIndex; j < endIndex; j++)
			{
				bound2.Merge(ref primitive_boxes.GetRawArray()[j].m_bound);
			}
			SetNodeBound(num_nodes, ref bound2);
			BuildSubTree(primitive_boxes, startIndex, splitAxis);
			BuildSubTree(primitive_boxes, splitAxis, endIndex);
			m_node_array.GetRawArray()[num_nodes].SetEscapeIndex(m_num_nodes - num_nodes);
		}

		internal QuantizedBvhTree()
		{
			m_num_nodes = 0;
			m_node_array = new GIM_QUANTIZED_BVH_NODE_ARRAY();
		}

		internal void BuildTree(GIM_BVH_DATA_ARRAY primitive_boxes)
		{
			CalcQuantization(primitive_boxes);
			m_num_nodes = 0;
			m_node_array.Resize(primitive_boxes.Count * 2);
			BuildSubTree(primitive_boxes, 0, primitive_boxes.Count);
		}

		internal void QuantizePoint(out UShortVector3 quantizedpoint, ref IndexedVector3 point)
		{
			GImpactQuantization.QuantizeClamp(out quantizedpoint, ref point, ref m_global_bound.m_min, ref m_global_bound.m_max, ref m_bvhQuantization);
		}

		internal bool TestQuantizedBoxOverlap(int node_index, ref UShortVector3 quantizedMin, ref UShortVector3 quantizedMax)
		{
			return m_node_array[node_index].TestQuantizedBoxOverlapp(ref quantizedMin, ref quantizedMax);
		}

		internal int GetNodeCount()
		{
			return m_num_nodes;
		}

		internal bool IsLeafNode(int nodeindex)
		{
			return m_node_array[nodeindex].IsLeafNode();
		}

		internal BT_QUANTIZED_BVH_NODE GetNode(int nodeindex)
		{
			return m_node_array[nodeindex];
		}

		internal void GetNodeBound(int nodeindex, out AABB bound)
		{
			bound.m_min = GImpactQuantization.Unquantize(ref m_node_array.GetRawArray()[nodeindex].m_quantizedAabbMin, ref m_global_bound.m_min, ref m_bvhQuantization);
			bound.m_max = GImpactQuantization.Unquantize(ref m_node_array.GetRawArray()[nodeindex].m_quantizedAabbMax, ref m_global_bound.m_min, ref m_bvhQuantization);
		}

		private void SetNodeBound(int nodeindex, ref AABB bound)
		{
			GImpactQuantization.QuantizeClamp(out m_node_array.GetRawArray()[nodeindex].m_quantizedAabbMin, ref bound.m_min, ref m_global_bound.m_min, ref m_global_bound.m_max, ref m_bvhQuantization);
			GImpactQuantization.QuantizeClamp(out m_node_array.GetRawArray()[nodeindex].m_quantizedAabbMax, ref bound.m_max, ref m_global_bound.m_min, ref m_global_bound.m_max, ref m_bvhQuantization);
		}

		internal int GetEscapeNodeIndex(int nodeindex)
		{
			return m_node_array[nodeindex].GetEscapeIndex();
		}
	}
}
