namespace JamalArouna.Utilities.Interfaces
{
    /// <summary>
    /// Interface for defining a state in a state machine.
    /// </summary>
    /// <remarks>
    /// Created by Jamal Arouna, 2025.
    /// </remarks>
    public interface IState
    {
        /// <summary>
        /// Called when entering the state.
        /// </summary>
        void Enter();

        /// <summary>
        /// Called every frame while the state is active.
        /// </summary>
        void Update();

        /// <summary>
        /// Called when exiting the state.
        /// </summary>
        void Exit();
    }
}