using System;
using System.Linq.Expressions;

namespace VRage.Library.Extensions
{
	/// <summary>
	/// Activator factory that uses expression trees.
	/// </summary>
	/// <remarks>This is the default activator factory.</remarks>
	public sealed class ExpressionBaseActivatorFactory : IActivatorFactory
	{
		/// <inheritdoc />
		public Func<T> CreateActivator<T>()
		{
			return CreateActivator<T>(typeof(T));
		}

		/// <inheritdoc />
		public Func<T> CreateActivator<T>(Type subtype)
		{
<<<<<<< HEAD
			return Expression.Lambda<Func<T>>(Expression.New(subtype), Array.Empty<ParameterExpression>()).Compile();
=======
			return Expression.Lambda<Func<T>>((Expression)(object)Expression.New(subtype), Array.Empty<ParameterExpression>()).Compile();
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		}
	}
}
