using System.Collections.Generic;
using VRage.Profiler;

namespace VRage.Game
{
	public class MyObjectBuilder_ProfilerBlock
	{
		public int Id;

		public MyProfilerBlockKey Key;

		public string TimeFormat;

		public string ValueFormat;

		public string CallFormat;

		public CompactSerializedArray<float>? ProcessMemory;

		public CompactSerializedArray<long>? ManagedMemoryBytes;

		public CompactSerializedArray<float>? Allocations;

		public CompactSerializedArray<float>? Milliseconds;

		public CompactSerializedArray<float>? CustomValues;

		public CompactSerializedArray<int>? NumCallsArray;

		public List<MyProfilerBlockKey> Children;

		public MyProfilerBlockKey Parent;

		public static MyObjectBuilder_ProfilerBlock GetObjectBuilder(MyProfilerBlock profilerBlock, bool serializeAllocations)
		{
			MyProfilerBlock.MyProfilerBlockObjectBuilderInfo objectBuilderInfo = profilerBlock.GetObjectBuilderInfo(serializeAllocations);
			MyObjectBuilder_ProfilerBlock myObjectBuilder_ProfilerBlock = new MyObjectBuilder_ProfilerBlock();
			myObjectBuilder_ProfilerBlock.Id = objectBuilderInfo.Id;
			myObjectBuilder_ProfilerBlock.Key = objectBuilderInfo.Key;
			myObjectBuilder_ProfilerBlock.TimeFormat = objectBuilderInfo.TimeFormat;
			myObjectBuilder_ProfilerBlock.ValueFormat = objectBuilderInfo.ValueFormat;
			myObjectBuilder_ProfilerBlock.CallFormat = objectBuilderInfo.CallFormat;
			myObjectBuilder_ProfilerBlock.Allocations = objectBuilderInfo.Allocations;
			myObjectBuilder_ProfilerBlock.Milliseconds = objectBuilderInfo.Milliseconds;
			myObjectBuilder_ProfilerBlock.CustomValues = objectBuilderInfo.CustomValues;
			myObjectBuilder_ProfilerBlock.NumCallsArray = objectBuilderInfo.NumCallsArray;
			myObjectBuilder_ProfilerBlock.Children = new List<MyProfilerBlockKey>();
			foreach (MyProfilerBlock child in objectBuilderInfo.Children)
			{
				myObjectBuilder_ProfilerBlock.Children.Add(child.Key);
			}
			if (objectBuilderInfo.Parent != null)
			{
				myObjectBuilder_ProfilerBlock.Parent = objectBuilderInfo.Parent.Key;
			}
			return myObjectBuilder_ProfilerBlock;
		}

		public static MyProfilerBlock Init(MyObjectBuilder_ProfilerBlock objectBuilder, MyProfiler.MyProfilerObjectBuilderInfo profiler)
		{
			MyProfilerBlock.MyProfilerBlockObjectBuilderInfo myProfilerBlockObjectBuilderInfo = new MyProfilerBlock.MyProfilerBlockObjectBuilderInfo();
			myProfilerBlockObjectBuilderInfo.Id = objectBuilder.Id;
			myProfilerBlockObjectBuilderInfo.Key = objectBuilder.Key;
			myProfilerBlockObjectBuilderInfo.TimeFormat = objectBuilder.TimeFormat;
			myProfilerBlockObjectBuilderInfo.ValueFormat = objectBuilder.ValueFormat;
			myProfilerBlockObjectBuilderInfo.CallFormat = objectBuilder.CallFormat;
			CompactSerializedArray<float>? allocations = objectBuilder.Allocations;
			object allocations2;
			if (!allocations.HasValue)
			{
				allocations2 = null;
			}
			else
			{
				CompactSerializedArray<float> array = allocations.GetValueOrDefault();
				allocations2 = (float[])array;
			}
			myProfilerBlockObjectBuilderInfo.Allocations = (float[])allocations2;
			allocations = objectBuilder.Milliseconds;
			object milliseconds;
			if (!allocations.HasValue)
			{
				milliseconds = null;
			}
			else
			{
				CompactSerializedArray<float> array = allocations.GetValueOrDefault();
				milliseconds = (float[])array;
			}
			myProfilerBlockObjectBuilderInfo.Milliseconds = (float[])milliseconds;
			allocations = objectBuilder.CustomValues;
			object customValues;
			if (!allocations.HasValue)
			{
				customValues = null;
			}
			else
			{
				CompactSerializedArray<float> array = allocations.GetValueOrDefault();
				customValues = (float[])array;
			}
			myProfilerBlockObjectBuilderInfo.CustomValues = (float[])customValues;
			CompactSerializedArray<int>? numCallsArray = objectBuilder.NumCallsArray;
			object numCallsArray2;
			if (!numCallsArray.HasValue)
			{
				numCallsArray2 = null;
			}
			else
			{
				CompactSerializedArray<int> array2 = numCallsArray.GetValueOrDefault();
				numCallsArray2 = (int[])array2;
			}
			myProfilerBlockObjectBuilderInfo.NumCallsArray = (int[])numCallsArray2;
			myProfilerBlockObjectBuilderInfo.Children = new List<MyProfilerBlock>();
			foreach (MyProfilerBlockKey child in objectBuilder.Children)
			{
				myProfilerBlockObjectBuilderInfo.Children.Add(profiler.ProfilingBlocks[child]);
			}
			if (objectBuilder.Parent.File != null)
			{
				myProfilerBlockObjectBuilderInfo.Parent = profiler.ProfilingBlocks[objectBuilder.Parent];
			}
			MyProfilerBlock myProfilerBlock = profiler.ProfilingBlocks[myProfilerBlockObjectBuilderInfo.Key];
			myProfilerBlock.Init(myProfilerBlockObjectBuilderInfo);
			return myProfilerBlock;
		}
	}
}
