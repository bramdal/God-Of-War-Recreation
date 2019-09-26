using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParentCommunication : MonoBehaviour
{

    AxeThrow axeThrowScript;
    // Start is called before the first frame update
    void Start()
    {
        axeThrowScript = GetComponentInParent<AxeThrow>();
    }

    void ThrowWeapon(){
        axeThrowScript.ThrowWeapon();
    }

    
}
