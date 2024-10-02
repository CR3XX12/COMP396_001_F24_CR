using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sight : Sense {
    public int FieldOfView = 45;
    public int ViewDistance = 100;
    private Transform playerTrans;
    private Vector3 rayDirection;
    protected override void Initialize() {
    //Find player position
    playerTrans = GameObject. FindGameObjectWithTag("Player").transform;
    }

    protected override void UpdateSense() {
    elapsedTime += Time.deltaTime;
    // Detect perspective sense if within the detection rate
    if (elapsedTime >= detectionRate) {
    DetectAspect();
    elapsedTime = 0.0f;
    }

    }

    //Detect perspective field of view for the AI Character
    void DetectAspect() {
    //Direction from current position to player position
    rayDirection = (playerTrans.position - transform.position).normalized;
    //Check the angle between the AI character's forward vector and the direction vector between
    //player and AI to detect if the Player is in the field of view.
    if ((Vector3.Angle(rayDirection, transform.forward)) < FieldOfView) {
    RaycastHit hit;
    if (Physics. Raycast(transform.position, rayDirection, out hit, ViewDistance)) {

    Aspect aspect = hit.collider.GetComponent<Aspect>();
    if (aspect != null) {
    //Check the aspect
    if (aspect.affiliation == targetAffiliation) {
    print("Enemy Detected");
    }
    }
    }
    }
    }
    void OnDrawGizmos() {
        if (!Application.isEditor|| playerTrans == null)
        return;
    Debug.DrawLine(transform.position, playerTrans.position, Color.red);
    Vector3 frontRayPoint = transform.position + (transform.forward * ViewDistance);
    //Approximate perspective visualization
    Vector3 leftRayPoint = Quaternion. Euler(0,FieldOfView * 0.5f ,0) * frontRayPoint;
    Vector3 rightRayPoint = Quaternion. Euler(0,- FieldOfView*0.5f, 0) * frontRayPoint;
    Debug.DrawLine(transform.position, frontRayPoint, Color.green);
    Debug.DrawLine(transform.position, leftRayPoint, Color.green);
    Debug.DrawLine(transform.position, rightRayPoint, Color.green);
    }
}