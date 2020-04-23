using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class State : MonoBehaviour {
    public virtual IEnumerator wander() {//start
        yield break;
    }
    public virtual IEnumerator pursue() {//attack
        yield break;
    }
    public virtual IEnumerator face() {//face
        yield break;
    }
}
