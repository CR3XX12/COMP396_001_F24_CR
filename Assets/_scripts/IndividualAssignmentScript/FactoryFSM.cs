using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Abstract State class: Each state will inherit from this
public abstract class State {
    public abstract void Execute(FactoryFSM npc);  
}

public class FactoryFSM : MonoBehaviour {
    private State currentState;  // Holds the current state of the NPC
    public Transform[] patrolPoints;  // Patrol points for NPC to move between
    public Transform player;  // Player transform for chase behavior
    public float speed = 2f;  // Movement speed of the NPC
    public int health = 100;  // NPC's health

    private int patrolIndex = 0;  // Index for patrol points

    // Initialize the NPC with the starting state
    void Start() {
        currentState = new IdleState();  // Set the initial state to Idle
    }

    // Update the NPC every frame
    void Update() {
        currentState.Execute(this);  // Execute the current state's logic
    }

    // Set a new state for the NPC
    public void SetState(State newState) {
        currentState = newState;
    }

    // NPC-specific behavior: Patrol between points
    public void Patrol() {
        Transform targetPoint = patrolPoints[patrolIndex];
        transform.position = Vector3.MoveTowards(transform.position, targetPoint.position, speed * Time.deltaTime);

        // Move to the next patrol point if the NPC is close enough to the current one
        if (Vector3.Distance(transform.position, targetPoint.position) < 0.5f) {
            patrolIndex = (patrolIndex + 1) % patrolPoints.Length;
        }
    }

    // NPC-specific behavior: Chase the player
    public void ChasePlayer() {
        transform.position = Vector3.MoveTowards(transform.position, player.position, speed * Time.deltaTime);
    }

    // NPC-specific behavior: Flee from the player
    public void Flee() {
        Vector3 fleeDirection = (transform.position - player.position).normalized;
        transform.position += fleeDirection * speed * Time.deltaTime;
    }
}
// IdleState Class
public class IdleState : State {
    public override void Execute(FactoryFSM npc) {
        Debug.Log("NPC is Idle");

        // Randomly transition to Patrol after some time
        if (Random.value > 0.5f) {
            npc.SetState(new PatrolState());
        }
    }
}

// PatrolState Class
public class PatrolState : State {
    public override void Execute(FactoryFSM npc) {
        Debug.Log("NPC is Patrolling");

        // Patrol logic
        npc.Patrol();

        // Randomly transition back to Idle
        if (Random.value > 0.7f) {
            npc.SetState(new IdleState());
        }
        // Transition to Chase if the player is close
        else if (Vector3.Distance(npc.transform.position, npc.player.position) < 5.0f) {
            npc.SetState(new ChaseState());
        }
    }
}

// ChaseState Class
public class ChaseState : State {
    public override void Execute(FactoryFSM npc) {
        Debug.Log("NPC is Chasing the player");

        // Chase logic
        npc.ChasePlayer();

        // Transition to Attack or Flee based on conditions
        if (Vector3.Distance(npc.transform.position, npc.player.position) < 2.0f) {
            npc.SetState(new AttackState());
        } else if (npc.health < 20) {
            npc.SetState(new FleeState());
        }
    }
}

// AttackState Class
public class AttackState : State {
    public override void Execute(FactoryFSM npc) {
        Debug.Log("NPC is Attacking");

        // After attacking, transition to Flee or Chase based on health
        if (npc.health < 20) {
            npc.SetState(new FleeState());
        } else {
            npc.SetState(new ChaseState());
        }
    }
}

// FleeState Class
public class FleeState : State {
    public override void Execute(FactoryFSM npc) {
        Debug.Log("NPC is Fleeing");

        // Flee logic
        npc.Flee();

        // After fleeing, return to Idle
        if (Vector3.Distance(npc.transform.position, npc.player.position) > 10.0f) {
            npc.SetState(new IdleState());
        }
    }
}

