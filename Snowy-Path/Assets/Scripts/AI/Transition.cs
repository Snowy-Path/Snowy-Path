using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 
/// </summary>
/// <typeparam name="TEnum">Enumeration of states.</typeparam>
public class Transition<TEnum> where TEnum : System.Enum {

    /// <value>
    /// Gets the state ID to switch to when the transition is valid.
    /// </value>
    public TEnum ToState { get; }

    // Condition to validate the transition.
    private Predicate<Transition<TEnum>> condition;

    /// <summary>
    /// Constructor with parameters.
    /// </summary>
    /// <param name="to">The switch to switch to when the <c>condition</c> is valid.</param>
    /// <param name="condition">The condition to test. If valid, the FSM will switch state to <c>ToState</c></param>
    public Transition(TEnum to, Predicate<Transition<TEnum>> condition) {
        this.ToState = to;
        this.condition = condition;
    }

    /// <summary>
    /// Test the predicate <c>condition</c>.
    /// </summary>
    /// <returns>The predicate return value.</returns>
    public bool ShouldTransition() {
        return condition(this);
    }

}
