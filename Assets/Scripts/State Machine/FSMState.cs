using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

/// <summary>
/// FSM 	: Finite State Machine System Class
/// Deterministic Finite State Machine (based on chapter 3.1 of Game Programming Gems 1 by Eric Dybsend.)
/// </summary>
public abstract class FSMState
{
    public FSMSystem Parent;

    // This method is called before the state is made the current state
    public virtual void OnEnter()
    {
        Debug.Log("Enter State " + this);
    }

    // Overloaded OnEnter Method w/ user data
    public virtual void OnEnter(object userData)
    {
        Debug.Log("Enter State " + this);
    }

    // This method is used to determine if the current state is the correct state to be in
    // If the current state is not the correct state to be in it can be used to change state
    public virtual void Reason() { }

    // This method is called on every Update by the FSM
    public virtual void OnUpdate() { }

    // This method is called on every FixedUpdate by the FSM
    public virtual void OnFixedUpdate() { }

    // This method is called on every LateUpdate by the FSM
    public virtual void OnLateUpdate() { }

    // This method is called before leaving the current State by the FSM
    public virtual void OnExit() { }

    // This method is called during GUI call by the FSM
    public virtual void OnGUI() { }

    // This method is called during PostRender by the FSM
    public virtual void OnPostRender() { }

    // Addition methods for collision and triggers can be added in this format too.
    //public virtual void OnCollisionEnter2D(Collision2D collision) { }
}
