using System;
using System.Globalization;
using VRage.Game;
using VRage.ObjectBuilders;

namespace Sandbox.ModAPI.Ingame
{
	public struct TerminalActionParameter
	{
		/// <summary>
		/// Gets an empty parameter.
		/// </summary>
		public static readonly TerminalActionParameter Empty;

		public readonly TypeCode TypeCode;

		public readonly object Value;

		public bool IsEmpty => TypeCode == TypeCode.Empty;

		private static Type ToType(TypeCode code)
		{
			return code switch
			{
				TypeCode.Boolean => typeof(bool), 
				TypeCode.Byte => typeof(byte), 
				TypeCode.Char => typeof(char), 
				TypeCode.DateTime => typeof(DateTime), 
				TypeCode.Decimal => typeof(decimal), 
				TypeCode.Double => typeof(double), 
				TypeCode.Int16 => typeof(short), 
				TypeCode.Int32 => typeof(int), 
				TypeCode.Int64 => typeof(long), 
				TypeCode.SByte => typeof(sbyte), 
				TypeCode.Single => typeof(float), 
				TypeCode.String => typeof(string), 
				TypeCode.UInt16 => typeof(ushort), 
				TypeCode.UInt32 => typeof(uint), 
				TypeCode.UInt64 => typeof(ulong), 
				_ => null, 
			};
		}

		/// <summary>
		/// Creates a <see cref="T:Sandbox.ModAPI.Ingame.TerminalActionParameter" /> from a serialized value in a string and a type code.
		/// </summary>
		/// <param name="serializedValue"></param>
		/// <param name="typeCode"></param>
		/// <returns></returns>
		public static TerminalActionParameter Deserialize(string serializedValue, TypeCode typeCode)
		{
			AssertTypeCodeValidity(typeCode);
			Type type = ToType(typeCode);
			if (type == null)
			{
				return Empty;
			}
			object value = Convert.ChangeType(serializedValue, typeCode, CultureInfo.InvariantCulture);
			return new TerminalActionParameter(typeCode, value);
		}

		/// <summary>
		/// Creates a <see cref="T:Sandbox.ModAPI.Ingame.TerminalActionParameter" /> from the given value.
		/// </summary>        
		/// <param name="value"></param>
		/// <returns></returns>
		public static TerminalActionParameter Get(object value)
		{
			if (value == null)
			{
				return Empty;
			}
			TypeCode typeCode = Type.GetTypeCode(value.GetType());
			AssertTypeCodeValidity(typeCode);
			return new TerminalActionParameter(typeCode, value);
		}

		private static void AssertTypeCodeValidity(TypeCode typeCode)
		{
			if ((uint)typeCode <= 2u)
			{
				throw new ArgumentException("Only primitive types are allowed for action parameters", "value");
			}
		}

		private TerminalActionParameter(TypeCode typeCode, object value)
		{
			TypeCode = typeCode;
			Value = value;
		}

		public MyObjectBuilder_ToolbarItemActionParameter GetObjectBuilder()
		{
			MyObjectBuilder_ToolbarItemActionParameter myObjectBuilder_ToolbarItemActionParameter = MyObjectBuilderSerializer.CreateNewObject<MyObjectBuilder_ToolbarItemActionParameter>();
			myObjectBuilder_ToolbarItemActionParameter.TypeCode = TypeCode;
			myObjectBuilder_ToolbarItemActionParameter.Value = ((Value == null) ? null : Convert.ToString(Value, CultureInfo.InvariantCulture));
			return myObjectBuilder_ToolbarItemActionParameter;
		}
	}
}
