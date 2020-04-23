using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class StateMachine : MonoBehaviour {
    protected State state;

    public void setState(State _state) {
        state = _state;
        StartCoroutine(state.wander());
    }
}
