namespace Sandbox.ModAPI
{
<<<<<<< HEAD
	/// <summary>
	/// Describes exhaust block (mods interface)
	/// </summary>
	public interface IMyExhaustBlock
	{
		/// <summary>
		/// Selects exhaust effect
		/// </summary>
		/// <param name="name">Name of effect</param>
		void SelectEffect(string name);

		/// <summary>
		/// Stop emitting effects 
		/// </summary>
		void StopEffects();

		/// <summary>
		/// Start emitting effects
		/// </summary>
=======
	public interface IMyExhaustBlock
	{
		void SelectEffect(string name);

		void StopEffects();

>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		void StartEffects();
	}
}
