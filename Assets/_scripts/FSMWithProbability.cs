using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;

public class FSMWithProbability : MonoBehaviour
{
   [Serializable]
    public enum FSMState {
        Chase,
        Flee,
        SelfDestruct,
    }

    [Serializable]
    public struct FSMProbability {
        public FSMState state;
        public int weight;
    }

    public FSMProbability[] states;

    FSMState selectState() {
        var weightSum = states.Sum(state => state.weight);
        var randomNumber = UnityEngine.Random.Range(0, weightSum);
        var i = 0;
        while (randomNumber >= 0) {
            var state = states[i];
            randomNumber -= state.weight;
            if (randomNumber <= 0) {
                return state.state;
            }
            i++;
        }
        throw new Exception("Something is wrong in the selectState algorithm!");
    }

    void Update() {
        if (Input.GetKeyDown(KeyCode.Space)) {
            FSMState randomState = selectState();
            Debug.Log(randomState.ToString());
        }
    }
}
