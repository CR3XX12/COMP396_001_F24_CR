using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    public Transform targetTransform;
    [SerializeField]
    private float movementSpeed = 10.0f;
    [SerializeField]
    private float rotSpeed = 2.0f;
    [SerializeField]
    private float targetReactionRadius = 5.0f;

    private void Start()
    {
        Aspect aspect = gameObject.AddComponent<Aspect>(); // Add Aspect component
        aspect.affiliation = Aspect.Affiliation.Player; // Set affiliation
    }
    
    void Update () {
    //Stop once you reached near the target position
    if (Vector3.Distance(transform.position, targetTransform.position) < targetReactionRadius)

    return;
    //Calculate direction vector from current position to target position
    Vector3 tarPos = targetTransform.position;
    tarPos.y = transform. position.y;
    Vector3 dirRot = tarPos - transform.position;
    //Build a Quaternion for this new rotation vector using LookRotation method
    Quaternion tarRot = Quaternion. LookRotation(dirRot);
    //Move and rotate with interpolation
    transform.rotation= Quaternion. Slerp(transform.rotation, tarRot, rotSpeed * Time.deltaTime);
    transform. Translate(new Vector3(0, 0, movementSpeed * Time.deltaTime));
        
    }
}
