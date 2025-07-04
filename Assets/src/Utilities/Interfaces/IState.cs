namespace JamalArouna.Utilities.Interfaces
{
    /// <summary>
    /// Basic state interface for a state machine.
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

    /// <summary>
    /// Extended interface for states that require dynamic parameters on calls.
    /// </summary>
    /// <remarks>
    /// Created by Jamal Arouna, 2025.
    /// </remarks>
    public interface IParameterizedState
    {
        /// <summary>
        /// Called when entering the state with an optional parameter.
        /// </summary>
        /// <param name="parameter">Optional input data</param>
        void Enter(object parameter);

        /// <summary>
        /// Called every frame while the state is active, with an optional parameter.
        /// </summary>
        /// <param name="parameter">Optional input data</param>
        void Update(object parameter);

        /// <summary>
        /// Called when exiting the state with an optional parameter.
        /// </summary>
        /// <param name="parameter">Optional input data</param>
        void Exit(object parameter);
    }
}