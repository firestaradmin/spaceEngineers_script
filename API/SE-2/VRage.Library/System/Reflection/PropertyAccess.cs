using System.Linq.Expressions;

namespace System.Reflection
{
	public static class PropertyAccess
	{
		public static Func<T, TProperty> CreateGetter<T, TProperty>(this PropertyInfo propertyInfo)
		{
			Type typeFromHandle = typeof(T);
			Type typeFromHandle2 = typeof(TProperty);
<<<<<<< HEAD
			ParameterExpression parameterExpression = Expression.Parameter(typeFromHandle, "value");
			Expression expression = ((propertyInfo.DeclaringType == typeFromHandle) ? ((Expression)parameterExpression) : ((Expression)Expression.Convert(parameterExpression, propertyInfo.DeclaringType)));
			Expression expression2 = Expression.Property(expression, propertyInfo);
=======
			ParameterExpression val = Expression.Parameter(typeFromHandle, "value");
			Expression val2 = (Expression)((propertyInfo.DeclaringType == typeFromHandle) ? ((object)val) : ((object)Expression.Convert((Expression)(object)val, propertyInfo.DeclaringType)));
			Expression val3 = (Expression)(object)Expression.Property(val2, propertyInfo);
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			if (typeFromHandle2 != propertyInfo.PropertyType)
			{
				val3 = (Expression)(object)Expression.Convert(val3, typeFromHandle2);
			}
<<<<<<< HEAD
			return Expression.Lambda<Func<T, TProperty>>(expression2, new ParameterExpression[1] { parameterExpression }).Compile();
=======
			return Expression.Lambda<Func<T, TProperty>>(val3, (ParameterExpression[])(object)new ParameterExpression[1] { val }).Compile();
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		}

		public static Action<T, TProperty> CreateSetter<T, TProperty>(this PropertyInfo propertyInfo)
		{
			Type typeFromHandle = typeof(T);
			Type typeFromHandle2 = typeof(TProperty);
<<<<<<< HEAD
			ParameterExpression parameterExpression = Expression.Parameter(typeFromHandle);
			ParameterExpression parameterExpression2 = Expression.Parameter(typeFromHandle2);
			Expression expression = ((propertyInfo.DeclaringType == typeFromHandle) ? ((Expression)parameterExpression) : ((Expression)Expression.Convert(parameterExpression, propertyInfo.DeclaringType)));
			Expression right = ((propertyInfo.PropertyType == typeFromHandle2) ? ((Expression)parameterExpression2) : ((Expression)Expression.Convert(parameterExpression2, propertyInfo.PropertyType)));
			MemberExpression left = Expression.Property(expression, propertyInfo);
			return Expression.Lambda<Action<T, TProperty>>(Expression.Assign(left, right), new ParameterExpression[2] { parameterExpression, parameterExpression2 }).Compile();
=======
			ParameterExpression val = Expression.Parameter(typeFromHandle);
			ParameterExpression val2 = Expression.Parameter(typeFromHandle2);
			Expression val3 = (Expression)((propertyInfo.DeclaringType == typeFromHandle) ? ((object)val) : ((object)Expression.Convert((Expression)(object)val, propertyInfo.DeclaringType)));
			Expression val4 = (Expression)((propertyInfo.PropertyType == typeFromHandle2) ? ((object)val2) : ((object)Expression.Convert((Expression)(object)val2, propertyInfo.PropertyType)));
			MemberExpression val5 = Expression.Property(val3, propertyInfo);
			return Expression.Lambda<Action<T, TProperty>>((Expression)(object)Expression.Assign((Expression)(object)val5, val4), (ParameterExpression[])(object)new ParameterExpression[2] { val, val2 }).Compile();
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		}

		public static Getter<T, TProperty> CreateGetterRef<T, TProperty>(this PropertyInfo propertyInfo)
		{
			Type typeFromHandle = typeof(T);
			Type typeFromHandle2 = typeof(TProperty);
<<<<<<< HEAD
			ParameterExpression parameterExpression = Expression.Parameter(typeFromHandle.MakeByRefType(), "value");
			Expression expression = ((propertyInfo.DeclaringType == typeFromHandle) ? ((Expression)parameterExpression) : ((Expression)Expression.Convert(parameterExpression, propertyInfo.DeclaringType)));
			Expression expression2 = Expression.Property(expression, propertyInfo);
=======
			ParameterExpression val = Expression.Parameter(typeFromHandle.MakeByRefType(), "value");
			Expression val2 = (Expression)((propertyInfo.DeclaringType == typeFromHandle) ? ((object)val) : ((object)Expression.Convert((Expression)(object)val, propertyInfo.DeclaringType)));
			Expression val3 = (Expression)(object)Expression.Property(val2, propertyInfo);
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			if (typeFromHandle2 != propertyInfo.PropertyType)
			{
				val3 = (Expression)(object)Expression.Convert(val3, typeFromHandle2);
			}
<<<<<<< HEAD
			ParameterExpression parameterExpression2 = Expression.Parameter(typeFromHandle2.MakeByRefType(), "out");
			BinaryExpression body = Expression.Assign(parameterExpression2, expression2);
			return Expression.Lambda<Getter<T, TProperty>>(body, new ParameterExpression[2] { parameterExpression, parameterExpression2 }).Compile();
=======
			ParameterExpression val4 = Expression.Parameter(typeFromHandle2.MakeByRefType(), "out");
			BinaryExpression val5 = Expression.Assign((Expression)(object)val4, val3);
			return Expression.Lambda<Getter<T, TProperty>>((Expression)(object)val5, (ParameterExpression[])(object)new ParameterExpression[2] { val, val4 }).Compile();
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		}

		public static Setter<T, TProperty> CreateSetterRef<T, TProperty>(this PropertyInfo propertyInfo)
		{
			Type typeFromHandle = typeof(T);
			Type typeFromHandle2 = typeof(TProperty);
<<<<<<< HEAD
			ParameterExpression parameterExpression = Expression.Parameter(typeFromHandle.MakeByRefType());
			ParameterExpression parameterExpression2 = Expression.Parameter(typeFromHandle2.MakeByRefType());
			Expression expression = ((propertyInfo.DeclaringType == typeFromHandle) ? ((Expression)parameterExpression) : ((Expression)Expression.Convert(parameterExpression, propertyInfo.DeclaringType)));
			Expression right = ((propertyInfo.PropertyType == typeFromHandle2) ? ((Expression)parameterExpression2) : ((Expression)Expression.Convert(parameterExpression2, propertyInfo.PropertyType)));
			MemberExpression left = Expression.Property(expression, propertyInfo);
			return Expression.Lambda<Setter<T, TProperty>>(Expression.Assign(left, right), new ParameterExpression[2] { parameterExpression, parameterExpression2 }).Compile();
=======
			ParameterExpression val = Expression.Parameter(typeFromHandle.MakeByRefType());
			ParameterExpression val2 = Expression.Parameter(typeFromHandle2.MakeByRefType());
			Expression val3 = (Expression)((propertyInfo.DeclaringType == typeFromHandle) ? ((object)val) : ((object)Expression.Convert((Expression)(object)val, propertyInfo.DeclaringType)));
			Expression val4 = (Expression)((propertyInfo.PropertyType == typeFromHandle2) ? ((object)val2) : ((object)Expression.Convert((Expression)(object)val2, propertyInfo.PropertyType)));
			MemberExpression val5 = Expression.Property(val3, propertyInfo);
			return Expression.Lambda<Setter<T, TProperty>>((Expression)(object)Expression.Assign((Expression)(object)val5, val4), (ParameterExpression[])(object)new ParameterExpression[2] { val, val2 }).Compile();
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		}
	}
}
