using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class BreakBoxScript : MonoBehaviour
{

    public GameObject brokenBox;

    public void Break()
    {
        try{
            GameObject broken = Instantiate(brokenBox, transform.position, transform.rotation);
            Rigidbody[] rbs = broken.GetComponentsInChildren<Rigidbody>();
            foreach(Rigidbody rb in rbs)
            {
                rb.AddExplosionForce(150, transform.position, 30);
            }
            Destroy(gameObject);
        }
        catch(Exception ex){

        }
    }
}
