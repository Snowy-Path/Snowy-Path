using System;
using System.Collections.Generic;

/// <summary>
/// State class using Action<T> delegates.
/// </summary>
/// <typeparam name="TEnum"></typeparam>
public class State<TEnum> where TEnum : System.Enum {

    #region Variables
    internal TEnum StateType { get; }
    private StateMachine<TEnum> Parent { get; }
    private List<Transition<TEnum>> m_transitions;

    private Action<State<TEnum>> onEntry;
    private Action<State<TEnum>> onExit;
    private Action<State<TEnum>> onUpdate;
    private Action<State<TEnum>> onFixedUpdate;
    #endregion

    #region Constructor
    /// <summary>
    /// Constructs the State of type <c>stateType</c>.
    /// Only the type TEnum and the parent StateMachine are required, allowing to declare only useful Action delegates.
    /// <example>
    /// <code>
    /// State<EWolfState> aggro = new State<EWolfState>(EWolfState.Aggro, parent,
    ///     onEntry: (state) => {
    ///         // Entry logic
    ///     },
    ///     onExit: (state) => {
    ///         // Exit logic
    ///     }
    /// );
    /// </code>
    /// </example>
    /// </summary>
    /// <param name="stateType">The type of the instantiated State.</param>
    /// <param name="parent">The StateMachine parent</param>
    /// <param name="onEntry">Action delegate called when entering this state.</param>
    /// <param name="onExit">Action delegate called when exiting this state.</param>
    /// <param name="onUpdate">Action delegate called at each frame update.</param>
    /// <param name="onFixedUpdate">Action delegate called at each physic system update.</param>
    public State(TEnum stateType, StateMachine<TEnum> parent, Action<State<TEnum>> onEntry = null, Action<State<TEnum>> onExit = null, Action<State<TEnum>> onUpdate = null, Action<State<TEnum>> onFixedUpdate = null) {
        this.StateType = stateType;
        this.Parent = parent;
        this.m_transitions = new List<Transition<TEnum>>();
        this.onEntry = onEntry;
        this.onExit = onExit;
        this.onUpdate = onUpdate;
        this.onFixedUpdate = onFixedUpdate;
    }
    #endregion

    #region State logic methods
    /// <summary>
    /// Called when entering this state. If <c>onEntry</c> Action delegate is not null, call it.
    /// </summary>
    internal virtual void OnEntry() {
        onEntry?.Invoke(this);
    }

    /// <summary>
    /// Called when exiting this state. If <c>onExit</c> Action delegate is not null, call it.
    /// </summary>
    internal virtual void OnExit() {
        onExit?.Invoke(this);
    }

    /// <summary>
    /// Called at each frame update. If <c>onUpdate</c> Action delegate is not null, call it.
    /// </summary>
    internal virtual void OnUpdate() {
        onUpdate?.Invoke(this);
    }

    /// <summary>
    /// Called at each physics update. If <c>onFixedUpdate</c> Action delegate is not null, call it.
    /// </summary>
    internal virtual void OnFixedUpdate() {
        onFixedUpdate?.Invoke(this);
    }

    /// <summary>
    /// Called at each LateUpdate. Test every transition stored in this state. The first one valid in the list makes the parent StateMachine switch state.
    /// </summary>
    internal virtual void OnLateUpdate() {
        foreach (Transition<TEnum> transition in m_transitions) {
            if (transition.ShouldTransition()) {
                Parent.SwitchState(transition.ToState);
                break;
            }
        }
    }
    #endregion

    #region Utility methods
    /// <summary>
    /// 
    /// </summary>
    /// <param name="transition"></param>
    internal virtual void AddTransition(Transition<TEnum> transition) {
        m_transitions.Add(transition);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    internal virtual TEnum GetCurrentState() {
        return StateType;
    }
    #endregion
}
