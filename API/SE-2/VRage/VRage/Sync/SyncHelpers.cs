using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using VRage.Network;
using VRage.Serialization;

namespace VRage.Sync
{
	public static class SyncHelpers
	{
		internal delegate ISyncType Composer(object instance, int id, ISerializerInfo serializeInfo);

		private static Dictionary<Type, List<Tuple<FieldInfo, Composer, MySerializeInfo>>> m_composers = new Dictionary<Type, List<Tuple<FieldInfo, Composer, MySerializeInfo>>>();

		private static FastResourceLock m_composersLock = new FastResourceLock();

		public static SyncType Compose(object obj, int firstId = 0)
		{
			List<SyncBase> list = new List<SyncBase>();
			Compose(obj, firstId, list);
			return new SyncType(list);
		}

		/// <summary>
		/// Takes objects and creates instances of Sync fields.
		/// </summary>
		public static void Compose(object obj, int startingId, List<SyncBase> resultList)
		{
			Type type = obj.GetType();
			List<Tuple<FieldInfo, Composer, MySerializeInfo>> value;
			using (m_composersLock.AcquireExclusiveUsing())
			{
				if (!m_composers.TryGetValue(type, out value))
				{
					value = CreateComposer(type);
					m_composers.Add(type, value);
				}
			}
			foreach (Tuple<FieldInfo, Composer, MySerializeInfo> item in value)
			{
				SyncBase syncBase = (SyncBase)item.Item2(obj, startingId++, item.Item3);
				syncBase.DebugName = item.Item1.Name;
				resultList.Add(syncBase);
			}
		}

		private static List<Tuple<FieldInfo, Composer, MySerializeInfo>> CreateComposer(Type type)
		{
			List<Tuple<FieldInfo, Composer, MySerializeInfo>> list = new List<Tuple<FieldInfo, Composer, MySerializeInfo>>();
<<<<<<< HEAD
			foreach (FieldInfo item in type.GetDataMembers(fields: true, properties: false, nonPublic: true, inherited: true, _static: false, instance: true, read: true, write: true).OfType<FieldInfo>())
=======
			foreach (FieldInfo item in Enumerable.OfType<FieldInfo>((IEnumerable)type.GetDataMembers(fields: true, properties: false, nonPublic: true, inherited: true, _static: false, instance: true, read: true, write: true)))
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			{
				if (typeof(SyncBase).IsAssignableFrom(item.FieldType))
				{
					list.Add(new Tuple<FieldInfo, Composer, MySerializeInfo>(item, CreateFieldComposer(item), MyFactory.CreateInfo(item)));
				}
			}
			return list;
		}

		private static Composer CreateFieldComposer(FieldInfo field)
		{
			ISyncComposer syncComposer = CodegenUtils.GetSyncComposer(field);
			if (syncComposer != null)
			{
				return syncComposer.Compose;
			}
			ConstructorInfo constructor = field.FieldType.GetConstructor(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, null, new Type[2]
			{
				typeof(int),
				typeof(ISerializerInfo)
			}, null);
			if (field.IsInitOnly)
			{
				return delegate(object instance, int id, ISerializerInfo info)
				{
					object obj = Activator.CreateInstance(field.FieldType, id, info);
					field.SetValue(instance, obj);
					return (ISyncType)obj;
				};
			}
<<<<<<< HEAD
			ParameterExpression parameterExpression = Expression.Parameter(typeof(object), "instance");
			ParameterExpression parameterExpression2 = Expression.Parameter(typeof(int), "id");
			ParameterExpression parameterExpression3 = Expression.Parameter(typeof(ISerializerInfo), "serializeInfo");
			UnaryExpression expression = Expression.Convert(parameterExpression, field.DeclaringType);
			ParameterExpression parameterExpression4 = Expression.Parameter(field.FieldType, "syncInstance");
			NewExpression right = Expression.New(constructor, parameterExpression2, parameterExpression3);
			return Expression.Lambda<Composer>(Expression.Block(new ParameterExpression[1] { parameterExpression4 }, Expression.Assign(parameterExpression4, right), Expression.Assign(Expression.Field(expression, field), parameterExpression4), parameterExpression4), new ParameterExpression[3] { parameterExpression, parameterExpression2, parameterExpression3 }).Compile();
=======
			ParameterExpression val = Expression.Parameter(typeof(object), "instance");
			ParameterExpression val2 = Expression.Parameter(typeof(int), "id");
			ParameterExpression val3 = Expression.Parameter(typeof(ISerializerInfo), "serializeInfo");
			UnaryExpression val4 = Expression.Convert((Expression)(object)val, field.DeclaringType);
			ParameterExpression val5 = Expression.Parameter(field.FieldType, "syncInstance");
			NewExpression val6 = Expression.New(constructor, (Expression[])(object)new Expression[2]
			{
				(Expression)val2,
				(Expression)val3
			});
			return Expression.Lambda<Composer>((Expression)(object)Expression.Block((IEnumerable<ParameterExpression>)(object)new ParameterExpression[1] { val5 }, (Expression[])(object)new Expression[3]
			{
				(Expression)Expression.Assign((Expression)(object)val5, (Expression)(object)val6),
				(Expression)Expression.Assign((Expression)(object)Expression.Field((Expression)(object)val4, field), (Expression)(object)val5),
				(Expression)val5
			}), (ParameterExpression[])(object)new ParameterExpression[3] { val, val2, val3 }).Compile();
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		}
	}
}
