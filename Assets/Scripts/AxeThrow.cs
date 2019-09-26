using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class AxeThrow : MonoBehaviour
{

    GameObject weapon;
    Rigidbody weaponRb;
    WeaponBehaviour weaponScript;

    [Header("State Control Bools")]
    public bool isAiming;
    public bool hasWeapon = true;
    public bool pulling = false;
    public float throwPower;
    [Space]
    [Header("Public References for Return Path")]
    //weapon pull variables
    public Transform curvePoint;
     public Transform hand;
    Vector3 originalLoc;
    Vector3 reachPoint;
    float time = 0f;

    //axe position in hand
    Vector3 originalHandLoc;
    Vector3 originalHandRot;



    PlayerMovementController thisPlayerMovementController;


    Animator thisPlayerAnimator;
    [Header("UI")]
    public Animator aimCursorAnimator;


    [Space]
    [Header("Public References for Effects")]
    //effect variables
    public CinemachineImpulseSource impulseSource;
    public TrailRenderer trail;
    void Start()
    {
        thisPlayerAnimator = GetComponentInChildren<Animator>();
        thisPlayerMovementController = GetComponent<PlayerMovementController>();
        
        //weapon init
        weapon = GameObject.FindGameObjectWithTag("Weapon");
        weaponRb = weapon.GetComponent<Rigidbody>();
        weaponScript = weapon.GetComponent<WeaponBehaviour>();
        originalHandLoc = weapon.transform.localPosition;
        originalHandRot = weapon.transform.localEulerAngles;
    }

    // Update is called once per frame
    void Update()
    {
        Aim();
        
        if(isAiming && hasWeapon && Input.GetButtonDown("Fire")){
            ThrowAxe();
        }

        if(!hasWeapon && Input.GetButtonDown("Fire2")){
            PullWeapon();
        }

        if(pulling){
            CalculateAxePath();
        }
             
        
    }

    void Aim(){
        if(Input.GetAxis("Aim")!=0f && hasWeapon){
            isAiming = true;
            thisPlayerAnimator.SetBool("Aiming", isAiming);
            thisPlayerMovementController.RotateToCameraWhileAiming();
            aimCursorAnimator.SetBool("Aim", isAiming);
        }
        else 
        {
            isAiming = false;
            thisPlayerAnimator.SetBool("Aiming", isAiming);
            aimCursorAnimator.SetBool("Aim", isAiming);
        }
    }

    public void ThrowWeapon(){
            hasWeapon = false;
            weaponScript.rotationSpeed *= -1;
            weaponScript.isThrown = true;
            weapon.transform.parent = null;
            weaponRb.isKinematic = false;
            weaponRb.collisionDetectionMode = CollisionDetectionMode.Continuous;
            weapon.transform.eulerAngles = new Vector3(0, -90 +transform.eulerAngles.y, 0);
            weapon.transform.position += transform.right/5;
            weaponRb.AddForce(Camera.main.transform.forward * throwPower + transform.up * 2, ForceMode.Impulse);
    }

    void ThrowAxe(){
        // isAiming=false;
        // thisPlayerAnimator.SetBool("Aiming", isAiming);
        
        thisPlayerAnimator.SetTrigger("Throw");
       

        //effects
        trail.emitting = true;
    }

    void PullWeapon(){
        pulling = true;
        originalLoc = weapon.transform.position;
        weaponScript.rotationSpeed *= -1;
        weaponRb.isKinematic = true;
        weaponRb.collisionDetectionMode = CollisionDetectionMode.ContinuousSpeculative;
        weaponScript.isThrown = true;

        thisPlayerAnimator.SetBool("Pulling", pulling);
    }

    void CalculateAxePath(){
        if(time<1f){
            weapon.transform.position = GetBezierQCCurvePoint(time, originalLoc, curvePoint.position, hand.position);
            time += Time.deltaTime;
        }
        else
        {
            CatchWeapon();
        }
    }

    
    void CatchWeapon(){
        weaponScript.isThrown = false;
        weapon.transform.parent = hand;
        pulling = false;
        thisPlayerAnimator.SetBool("Pulling", pulling);
        weapon.transform.localPosition = originalHandLoc;
        weapon.transform.localEulerAngles = originalHandRot;
        hasWeapon = true;
        time = 0f;

        //effects
        impulseSource.GenerateImpulse(Vector3.right);
        trail.emitting = false;
    }

    public Vector3 GetBezierQCCurvePoint(float t, Vector3 p0, Vector3 p1, Vector3 p2)
    {
        float u = 1 - t;
        float tt = t * t;
        float uu = u * u;
        return (uu * p0) + (2 * u * t * p1) + (tt * p2);
    }
}
