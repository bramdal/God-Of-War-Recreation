using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponBehaviour : MonoBehaviour
{

    public bool isThrown;
    public float rotationSpeed= 5f;

    // Update is called once per frame
    void Update()
    {
        if(isThrown){
            transform.localEulerAngles += Vector3.forward * rotationSpeed * Time.deltaTime;
        }
    }

    

    private void OnCollisionEnter(Collision other) {
        if(other.gameObject.layer == 10){
            isThrown = false;
            GetComponent<Rigidbody>().collisionDetectionMode = CollisionDetectionMode.ContinuousSpeculative;
            GetComponent<Rigidbody>().isKinematic = true;
            GetComponent<Rigidbody>().Sleep();
        }
    }
}
