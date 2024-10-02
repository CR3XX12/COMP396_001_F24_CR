using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum NPCState {
    Idle,
    Patrol,
    Chase,
    Attack,
    Flee
}
public class SimpleFSM : MonoBehaviour
{
    public NPCState currentState;   // Current state of the NPC
    public Transform[] patrolPoints; // Array of patrol points

    private int patrolIndex = 0;    // Index for patrol points
    public float speed = 2f;        // Speed of NPC movement
    public Transform player;        // Reference to the player

    void Start() {
        currentState = NPCState.Idle;  // Start with Idle state
    }

    void Update() {
        switch (currentState) {
            case NPCState.Idle:
                IdleAction();
                break;
            case NPCState.Patrol:
                PatrolAction();
                break;
            case NPCState.Chase:
                ChaseAction();
                break;
            case NPCState.Attack:
                AttackAction();
                break;
            case NPCState.Flee:
                FleeAction();
                break;
        }
    }

    void IdleAction() {
        // Idle behavior here
        if (Random.value > 0.5f) {
            currentState = NPCState.Patrol;
        }
    }

    void PatrolAction() {
        // Patrol behavior here
        Transform targetPoint = patrolPoints[patrolIndex];
        transform.position = Vector3.MoveTowards(transform.position, targetPoint.position, speed * Time.deltaTime);

        if (Vector3.Distance(transform.position, targetPoint.position) < 0.5f) {
            patrolIndex = (patrolIndex + 1) % patrolPoints.Length;
        }

        if (Random.value > 0.7f) {
            currentState = NPCState.Idle;
        }
    }

    void ChaseAction() {
        // Chase player
        transform.position = Vector3.MoveTowards(transform.position, player.position, speed * Time.deltaTime);

        // Random chance to transition to attack or flee
        if (Vector3.Distance(transform.position, player.position) < 1.5f) {
            currentState = NPCState.Attack;
        } else if (Random.value > 0.8f) {
            currentState = NPCState.Flee;
        }
    }

    void AttackAction() {
        // Attack player logic
        Debug.Log("Attacking the player!");
        // Transition logic to flee (if low health, for example)
    }

    void FleeAction() {
        // Fleeing logic (e.g. move away from the player)
        Debug.Log("Fleeing from the player!");
    }
}
