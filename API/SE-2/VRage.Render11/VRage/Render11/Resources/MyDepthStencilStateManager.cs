using SharpDX.Direct3D11;
using VRage.Render11.Resources.Internal;
using VRageRender;

namespace VRage.Render11.Resources
{
	internal class MyDepthStencilStateManager : MyPersistentResourceManager<MyDepthStencilState, DepthStencilStateDescription>
	{
		internal static IDepthStencilState DepthTestWrite;

		internal static IDepthStencilState DepthTestReadOnly;

		internal static IDepthStencilState DepthTestReadOnlyInverse;

		internal static IDepthStencilState DepthTestPassReadOnly;

		internal static IDepthStencilState IgnoreDepthStencil;

		internal static IDepthStencilState MarkEdgeInStencil;

		internal static IDepthStencilState WriteHighlightStencil;

		internal static IDepthStencilState WriteOverlappingHighlightStencil;

		internal static IDepthStencilState TestHighlightOuterStencil;

		internal static IDepthStencilState TestHighlightInnerStencil;

		internal static IDepthStencilState TestEdgeStencil;

		internal static IDepthStencilState TestDepthAndEdgeStencil;

		internal static IDepthStencilState[] MarkIfInsideCascade;

		internal static IDepthStencilState[] MarkIfInsideCascadeOld;

		internal static IDepthStencilState StereoDefaultDepthState;

		internal static IDepthStencilState StereoStencilMask;

		internal static IDepthStencilState StereoDepthTestReadOnly;

		internal static IDepthStencilState StereoDepthTestWrite;

		internal static IDepthStencilState StereoIgnoreDepthStencil;

		internal static IDepthStencilState DefaultDepthState
		{
			get
			{
				if (!MyRender11.UseComplementaryDepthBuffer)
				{
					return null;
				}
				return DepthTestWrite;
			}
		}

		internal static byte GetStereoMask()
		{
			return 16;
		}

		protected override int GetAllocResourceCount()
		{
			return 128;
		}

		public MyDepthStencilStateManager()
		{
			DepthStencilStateDescription depthStencilStateDescription = new DepthStencilStateDescription
			{
				DepthComparison = (MyRender11.UseComplementaryDepthBuffer ? Comparison.Greater : Comparison.Less),
				DepthWriteMask = DepthWriteMask.All,
				IsDepthEnabled = true,
				IsStencilEnabled = false
			};
			DepthStencilStateDescription desc = depthStencilStateDescription;
			DepthTestWrite = CreateResource("DepthTestWrite", ref desc);
			depthStencilStateDescription = new DepthStencilStateDescription
			{
				DepthComparison = (MyRender11.UseComplementaryDepthBuffer ? Comparison.Greater : Comparison.Less),
				DepthWriteMask = DepthWriteMask.Zero,
				IsDepthEnabled = true,
				IsStencilEnabled = false
			};
			DepthStencilStateDescription desc2 = depthStencilStateDescription;
			DepthTestReadOnly = CreateResource("DepthTestReadOnly", ref desc2);
			depthStencilStateDescription = new DepthStencilStateDescription
			{
				DepthComparison = (MyRender11.UseComplementaryDepthBuffer ? Comparison.Greater : Comparison.Less),
				DepthWriteMask = DepthWriteMask.Zero,
				IsDepthEnabled = true,
				IsStencilEnabled = false
			};
			DepthStencilStateDescription desc3 = depthStencilStateDescription;
			DepthTestReadOnlyInverse = CreateResource("DepthTestReadOnlyInverse", ref desc3);
			depthStencilStateDescription = new DepthStencilStateDescription
			{
				DepthComparison = Comparison.Always,
				DepthWriteMask = DepthWriteMask.Zero,
				IsDepthEnabled = true,
				IsStencilEnabled = false
			};
			DepthStencilStateDescription desc4 = depthStencilStateDescription;
			DepthTestPassReadOnly = CreateResource("DepthTestPass", ref desc4);
			depthStencilStateDescription = new DepthStencilStateDescription
			{
				DepthComparison = (MyRender11.UseComplementaryDepthBuffer ? Comparison.Greater : Comparison.Less),
				DepthWriteMask = DepthWriteMask.Zero,
				IsDepthEnabled = false,
				IsStencilEnabled = false
			};
			DepthStencilStateDescription desc5 = depthStencilStateDescription;
			IgnoreDepthStencil = CreateResource("IgnoreDepthStencil", ref desc5);
			depthStencilStateDescription = new DepthStencilStateDescription
			{
				IsDepthEnabled = false,
				IsStencilEnabled = true,
				StencilReadMask = byte.MaxValue,
				StencilWriteMask = 128,
				BackFace = 
				{
					Comparison = Comparison.Always,
					DepthFailOperation = StencilOperation.Replace,
					FailOperation = StencilOperation.Replace,
					PassOperation = StencilOperation.Replace
				}
			};
			DepthStencilStateDescription desc6 = depthStencilStateDescription;
			desc6.FrontFace = desc6.BackFace;
			MarkEdgeInStencil = CreateResource("MarkEdgeInStencil", ref desc6);
			depthStencilStateDescription = new DepthStencilStateDescription
			{
				IsDepthEnabled = false,
				DepthComparison = Comparison.Always,
				DepthWriteMask = DepthWriteMask.Zero,
				IsStencilEnabled = true,
				StencilReadMask = 0,
				StencilWriteMask = 64,
				BackFace = 
				{
					Comparison = Comparison.Always,
					DepthFailOperation = StencilOperation.Keep,
					FailOperation = StencilOperation.Replace,
					PassOperation = StencilOperation.Replace
				}
			};
			DepthStencilStateDescription desc7 = depthStencilStateDescription;
			desc7.FrontFace = desc7.BackFace;
			WriteHighlightStencil = CreateResource("WriteHighlightStencil", ref desc7);
			depthStencilStateDescription = new DepthStencilStateDescription
			{
				IsDepthEnabled = true,
				DepthComparison = Comparison.Equal,
				DepthWriteMask = DepthWriteMask.Zero,
				IsStencilEnabled = true,
				StencilReadMask = 0,
				StencilWriteMask = 128,
				BackFace = 
				{
					Comparison = Comparison.Always,
					DepthFailOperation = StencilOperation.Keep,
					FailOperation = StencilOperation.Replace,
					PassOperation = StencilOperation.Replace
				}
			};
			DepthStencilStateDescription desc8 = depthStencilStateDescription;
			desc8.FrontFace = desc8.BackFace;
			WriteOverlappingHighlightStencil = CreateResource("WriteOverlappingHighlightStencil", ref desc8);
			depthStencilStateDescription = new DepthStencilStateDescription
			{
				IsDepthEnabled = false,
				IsStencilEnabled = true,
				StencilReadMask = 192,
				StencilWriteMask = 0,
				BackFace = 
				{
					Comparison = Comparison.Equal,
					DepthFailOperation = StencilOperation.Keep,
					FailOperation = StencilOperation.Keep,
					PassOperation = StencilOperation.Keep
				}
			};
			DepthStencilStateDescription desc9 = depthStencilStateDescription;
			desc9.FrontFace = desc9.BackFace;
			TestHighlightOuterStencil = CreateResource("TestHighlightOuterStencil", ref desc9);
			depthStencilStateDescription = new DepthStencilStateDescription
			{
				IsDepthEnabled = false,
				IsStencilEnabled = true,
				StencilReadMask = 192,
				StencilWriteMask = 0,
				BackFace = 
				{
					Comparison = Comparison.Equal,
					DepthFailOperation = StencilOperation.Keep,
					FailOperation = StencilOperation.Keep,
					PassOperation = StencilOperation.Keep
				}
			};
			DepthStencilStateDescription desc10 = depthStencilStateDescription;
			desc10.FrontFace = desc10.BackFace;
			TestHighlightInnerStencil = CreateResource("TestHighlightInnerStencil", ref desc10);
			depthStencilStateDescription = new DepthStencilStateDescription
			{
				IsDepthEnabled = false,
				IsStencilEnabled = true,
				StencilReadMask = 128,
				StencilWriteMask = 0,
				BackFace = 
				{
					Comparison = Comparison.Equal,
					DepthFailOperation = StencilOperation.Keep,
					FailOperation = StencilOperation.Keep,
					PassOperation = StencilOperation.Keep
				}
			};
			DepthStencilStateDescription desc11 = depthStencilStateDescription;
			desc11.FrontFace = desc11.BackFace;
			TestEdgeStencil = CreateResource("TestEdgeStencil", ref desc11);
			depthStencilStateDescription = new DepthStencilStateDescription
			{
				DepthComparison = (MyRender11.UseComplementaryDepthBuffer ? Comparison.Greater : Comparison.Less),
				DepthWriteMask = DepthWriteMask.Zero,
				IsDepthEnabled = true,
				IsStencilEnabled = true,
				StencilReadMask = 128,
				StencilWriteMask = 0,
				BackFace = 
				{
					Comparison = Comparison.Equal,
					DepthFailOperation = StencilOperation.Keep,
					FailOperation = StencilOperation.Keep,
					PassOperation = StencilOperation.Keep
				}
			};
			DepthStencilStateDescription desc12 = depthStencilStateDescription;
			desc12.FrontFace = desc12.BackFace;
			TestDepthAndEdgeStencil = CreateResource("TestDepthAndEdgeStencil", ref desc12);
			IDepthStencilState[] array = (MarkIfInsideCascade = new MyDepthStencilState[8]);
			depthStencilStateDescription = new DepthStencilStateDescription
			{
				IsDepthEnabled = true,
				DepthComparison = (MyRender11.UseComplementaryDepthBuffer ? Comparison.Less : Comparison.Greater),
				DepthWriteMask = DepthWriteMask.Zero,
				IsStencilEnabled = true,
				StencilReadMask = 15,
				BackFace = 
				{
					DepthFailOperation = StencilOperation.Keep,
					FailOperation = StencilOperation.Keep,
					PassOperation = StencilOperation.Invert
				}
			};
			DepthStencilStateDescription desc13 = depthStencilStateDescription;
			for (int i = 0; i < MarkIfInsideCascade.Length; i++)
			{
				desc13.StencilWriteMask = (byte)(15 - i);
				desc13.BackFace.Comparison = ((i == 0) ? Comparison.Always : Comparison.GreaterEqual);
				desc13.FrontFace = desc13.BackFace;
				MarkIfInsideCascade[i] = CreateResource("MarkIfInsideCascade_" + i, ref desc13);
			}
			array = (MarkIfInsideCascadeOld = new MyDepthStencilState[8]);
			depthStencilStateDescription = new DepthStencilStateDescription
			{
				IsDepthEnabled = true,
				DepthComparison = (MyRender11.UseComplementaryDepthBuffer ? Comparison.Less : Comparison.Greater),
				DepthWriteMask = DepthWriteMask.Zero,
				IsStencilEnabled = true,
				StencilReadMask = 15,
				BackFace = 
				{
					DepthFailOperation = StencilOperation.Keep,
					FailOperation = StencilOperation.Keep,
					PassOperation = StencilOperation.Invert
				}
			};
			DepthStencilStateDescription desc14 = depthStencilStateDescription;
			for (int j = 0; j < MarkIfInsideCascadeOld.Length; j++)
			{
				desc14.StencilWriteMask = (byte)(15 - j);
				desc14.BackFace.Comparison = ((j == 0) ? Comparison.Always : Comparison.GreaterEqual);
				desc14.FrontFace = desc14.BackFace;
				MarkIfInsideCascadeOld[j] = CreateResource("MarkIfInsideCascade_" + j, ref desc14);
			}
			depthStencilStateDescription = new DepthStencilStateDescription
			{
				DepthComparison = (MyRender11.UseComplementaryDepthBuffer ? Comparison.Greater : Comparison.Less),
				DepthWriteMask = DepthWriteMask.All,
				IsDepthEnabled = true,
				IsStencilEnabled = true,
				StencilWriteMask = GetStereoMask(),
				StencilReadMask = GetStereoMask(),
				BackFace = 
				{
					Comparison = Comparison.GreaterEqual,
					DepthFailOperation = StencilOperation.Keep,
					FailOperation = StencilOperation.Keep,
					PassOperation = StencilOperation.Replace
				}
			};
			DepthStencilStateDescription desc15 = depthStencilStateDescription;
			desc15.FrontFace = desc15.BackFace;
			StereoDefaultDepthState = CreateResource("StereoDefaultDepthState", ref desc15);
			depthStencilStateDescription = new DepthStencilStateDescription
			{
				DepthComparison = (MyRender11.UseComplementaryDepthBuffer ? Comparison.Greater : Comparison.Less),
				DepthWriteMask = DepthWriteMask.Zero,
				IsDepthEnabled = false,
				IsStencilEnabled = true,
				StencilWriteMask = GetStereoMask(),
				StencilReadMask = GetStereoMask(),
				BackFace = 
				{
					Comparison = Comparison.Always,
					DepthFailOperation = StencilOperation.Replace,
					FailOperation = StencilOperation.Replace,
					PassOperation = StencilOperation.Replace
				}
			};
			DepthStencilStateDescription desc16 = depthStencilStateDescription;
			desc16.FrontFace = desc16.BackFace;
			StereoStencilMask = CreateResource("StereoStencilMask", ref desc16);
			depthStencilStateDescription = new DepthStencilStateDescription
			{
				DepthComparison = (MyRender11.UseComplementaryDepthBuffer ? Comparison.Greater : Comparison.Less),
				DepthWriteMask = DepthWriteMask.Zero,
				IsDepthEnabled = true,
				IsStencilEnabled = true,
				StencilWriteMask = GetStereoMask(),
				StencilReadMask = GetStereoMask(),
				BackFace = 
				{
					Comparison = Comparison.GreaterEqual,
					DepthFailOperation = StencilOperation.Keep,
					FailOperation = StencilOperation.Keep,
					PassOperation = StencilOperation.Replace
				}
			};
			DepthStencilStateDescription desc17 = depthStencilStateDescription;
			desc17.FrontFace = desc17.BackFace;
			StereoDepthTestReadOnly = CreateResource("StereoDepthTestReadOnly", ref desc17);
			depthStencilStateDescription = new DepthStencilStateDescription
			{
				DepthComparison = (MyRender11.UseComplementaryDepthBuffer ? Comparison.Greater : Comparison.Less),
				DepthWriteMask = DepthWriteMask.All,
				IsDepthEnabled = true,
				IsStencilEnabled = true,
				StencilWriteMask = GetStereoMask(),
				StencilReadMask = GetStereoMask(),
				BackFace = 
				{
					Comparison = Comparison.GreaterEqual,
					DepthFailOperation = StencilOperation.Keep,
					FailOperation = StencilOperation.Keep,
					PassOperation = StencilOperation.Replace
				}
			};
			DepthStencilStateDescription desc18 = depthStencilStateDescription;
			desc18.FrontFace = desc18.BackFace;
			StereoDepthTestWrite = CreateResource("StereoDepthTestWrite", ref desc18);
			depthStencilStateDescription = new DepthStencilStateDescription
			{
				DepthComparison = (MyRender11.UseComplementaryDepthBuffer ? Comparison.Greater : Comparison.Less),
				DepthWriteMask = DepthWriteMask.Zero,
				IsDepthEnabled = false,
				IsStencilEnabled = true,
				StencilWriteMask = GetStereoMask(),
				StencilReadMask = GetStereoMask(),
				BackFace = 
				{
					Comparison = Comparison.GreaterEqual,
					DepthFailOperation = StencilOperation.Keep,
					FailOperation = StencilOperation.Keep,
					PassOperation = StencilOperation.Replace
				}
			};
			DepthStencilStateDescription desc19 = depthStencilStateDescription;
			desc19.FrontFace = desc19.BackFace;
			StereoIgnoreDepthStencil = CreateResource("StereoIgnoreDepthStencil", ref desc19);
		}
	}
}
