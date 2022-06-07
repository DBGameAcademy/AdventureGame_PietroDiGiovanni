using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// FSM 	: Finite State Machine System Class
/// </summary>
[System.Serializable]
public class FSMSystem : MonoBehaviour
{
    private List<FSMState> _states;

    private FSMState _previousState;
    private FSMState _previousStateForPop;

    public bool _statePushed;

    private FSMState _currentState;
    private FSMState _nextState;

    public FSMState CurrentState
    {
        get { return _currentState; }
    }

    public FSMState NextState
    {
        get { return _nextState; }
    }

    // In derivitive classes remember to declare and initialize any states for later
    // eg : FSMState WalkState = new CustomBuiltWalkState();

    public FSMSystem()
    {
        _states = new List<FSMState>();
    }

    /// <summary>
    /// This next region handles the Update calls for the currentState
    /// </summary>
    #region The Basic MonoBehaviour Methods

    public virtual void Update() { _currentState?.OnUpdate(); }
    public void FixedUpdate()
    {
        if (_currentState != null)
            _currentState?.OnFixedUpdate();
    }
    public void LateUpdate() { _currentState?.OnLateUpdate(); _currentState?.Reason(); }
    public void OnGUI() { _currentState?.OnGUI(); }
    public void OnPostRender() { _currentState?.OnPostRender(); }
    //public void OnCollisionEnter2D(Collision2D collision) { _currentState.OnCollisionEnter2D(collision); }

    #endregion

    /// <summary>
    /// This method places new states inside the FSM
    /// or prints an ERROR message if the state was already inside the List.
    /// First state added is also the initial state.
    /// Previous State will always be set to the previous Added state if there was one.
    /// </summary>
    public void AddState(FSMState inState)
    {
        Debug.Log("State added to machine: " + inState);

        // Check for Null reference before deleting
        if (inState == null)
        {
            Debug.LogError("FSM ERROR: Null reference is not allowed");
        }

        // First State inserted is also the initial State.
        // This is the state the machine is on when the simulation begins.
        if (_states.Count == 0)
        {
            _states.Add(inState);
            _currentState = inState;
            return;
        }

        // If the state is not the first state
        // Add the state to the List if its not already inside it
        foreach (FSMState st in _states)
        {
            if (st == inState)
            {
                Debug.LogError("Unable to Add State (" + inState.ToString() + ") because state has already been added.");
                return;
            }
        }

        _previousState = _currentState;
        _states.Add(inState);
    }

    /// <summary>
    /// This method is used to delete a state from the FSM List if it exists,
    /// otherwise it will print an ERROR message saying it was not in the List.
    /// </summary>
    public void DeleteState(FSMState inState)
    {
        // Check for NullState before deleting
        if (inState == null)
        {
            Debug.LogError("FSM ERROR: Null reference is not allowed");
            return;
        }

        // Search the List and delete the state if it is in the List
        foreach (FSMState st in _states)
        {
            if (st == inState)
            {
                _states.Remove(st);
                return;
            }
        }
        Debug.LogError("Unable to Delete State (" + inState.ToString() + ") because state is not in List.");
    }

    /// <summary>
    /// This method is used to move to a state within the FSM List
    /// otherwise it will print an ERROR message.
    /// </summary>
    public void GoToState(FSMState inState)
    {
        // Check for NullState before deleting
        if (inState == null)
        {
            Debug.LogError("FSM ERROR: Null reference is not allowed");
            return;
        }

        // Search the List and delete the state if it is in the List
        foreach (FSMState st in _states)
        {
            if (st == inState)
            {
                _nextState = st;

                _previousState = _currentState;
                _previousState.OnExit();

                _currentState = _nextState;
                _currentState.OnEnter();

                _nextState = null;
                return;
            }
        }
        Debug.LogError("Unable to GoToState (" + inState.ToString() + ") because state is not in List.");
    }

    /// <summary>
    /// Overloaded Method
    /// This method is used to move to a state within the FSM List
    /// this is used only when an object is being passed between the states
    /// otherwise it will print an ERROR message.
    /// </summary>
    public void GoToState(FSMState inState, object userData)
    {
        // Check for NullState before deleting
        if (inState == null)
        {
            Debug.LogError("FSM ERROR: Null reference is not allowed");
            return;
        }

        // Search the List and delete the state if it is in the List
        foreach (FSMState st in _states)
        {
            if (st == inState)
            {
                _nextState = st;

                _previousState = _currentState;
                _previousState.OnExit();

                _currentState = _nextState;
                _currentState.OnEnter(userData);

                _nextState = null;
                return;
            }
        }
        Debug.LogError(string.Format("{0}: {1} {2}",
                                     "Unable to GoToState(",
                                     inState.ToString(),
                                     ") because state is not in List."));
    }


    /// Previous State will still apply to the original previous state before the push
    public void GoToPreviousState()
    {
        // Check for NullState before deleting
        if (_previousState == null)
        {
            Debug.LogError("FSM ERROR: Null reference is not allowed. _previousState returned Null.");
            return;
        }

        // Search the List and delete the state if it is in the List
        foreach (FSMState st in _states)
        {
            if (st == _previousState)
            {
                FSMState tempHolderState = _currentState;

                _currentState.OnExit();
                _previousState.OnEnter();

                _currentState = _previousState;

                _previousState = tempHolderState;

                return;
            }
        }

        Debug.LogError(string.Format("{0}: {1} {2}",
                                     "Unable to GoToPrevious",
                                     _previousState.ToString(),
                                     " because state is not in List anymore."));
    }

    /// Push will push into a state without firing the OnExit of current state
    public void PushState(FSMState inState)
    {
        // Check for NullState before deleting
        if (inState == null)
        {
            Debug.LogError("FSM ERROR: Null reference is not allowed");
            return;
        }

        // Search the List and delete the state if it is in the List
        foreach (FSMState st in _states)
        {
            if (st == inState)
            {
                _previousStateForPop = _currentState;

                _currentState = st;
                _currentState.OnEnter();
                _statePushed = true;
                return;
            }
        }
        Debug.LogError(string.Format("{0}: {1} {2}",
                                     "Unable to PushState(",
                                     inState.ToString(),
                                     ") because state is not in List."));
    }

    /// Push will push into a state without firing the OnExit of current state
    public void PushState(FSMState inState, object userData)
    {
        // Check for NullState before deleting
        if (inState == null)
        {
            Debug.LogError("FSM ERROR: Null reference is not allowed");
            return;
        }

        // Search the List and delete the state if it is in the List
        foreach (FSMState st in _states)
        {
            if (st == inState)
            {
                _previousStateForPop = _currentState;

                _currentState = st;
                _currentState.OnEnter(userData);
                _statePushed = true;
                return;
            }
        }
        Debug.LogError(string.Format("{0}: {1} {2}",
                                     "Unable to PushState(",
                                     inState.ToString(),
                                     ") because state is not in List."));
    }

    /// Pop will allow you to pop back to the state you were in at the place you were
    public void PopState()
    {
        // Check for NullState before deleting
        if (_previousStateForPop == null)
        {
            Debug.LogError("FSM ERROR: Null reference is not allowed. _previousStateForPop returned Null.");
            return;
        }

        // Search the List and delete the state if it is in the List
        foreach (FSMState st in _states)
        {
            if (st == _previousStateForPop)
            {
                _currentState.OnExit();
                _currentState = _previousStateForPop;
                _statePushed = false;
                _previousStateForPop = null;
                return;
            }
        }

        Debug.LogError(string.Format("{0}: {1} {2}",
                                     "Unable to GoToPrevious",
                                     _previousStateForPop.ToString(),
                                     " because state is not in List anymore."));
    }
}
