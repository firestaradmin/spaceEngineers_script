namespace Sandbox.ModAPI.Ingame
{
	/// <summary>
	/// Describes block, that has at least 1 text surface (PB scripting interface)
	/// </summary>
	public interface IMyTextSurfaceProvider
	{
<<<<<<< HEAD
		/// <summary>
		/// Whether generic LCD terminal controls should be created
		/// </summary>
		bool UseGenericLcd { get; }
=======
		int SurfaceCount { get; }
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d

		/// <summary>
		/// Get surfaces count
		/// </summary>
		int SurfaceCount { get; }

		/// <summary>
		/// Get surface by index
		/// </summary>
		/// <param name="index"></param>
		/// <returns>TextSurface if index in [0..SurfaceCount-1] and null if out of bounds</returns>
		IMyTextSurface GetSurface(int index);
	}
}
