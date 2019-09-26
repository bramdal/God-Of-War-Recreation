using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovementController : MonoBehaviour
{
    CharacterController thisPlayerController;
    Animator thisPlayerAnimator;
    
    //Movement variables
    float inputZaxis;
    float inputXaxis;
    float inputYaxis;

    Vector3 movementDirection = Vector3.zero;
    bool moving;
    public float gravity = 9.8f;

    public float movementSpeed = 5f;
    public float rotationSpeed = 1f;
    public float aimingRotationSpeed = 0.1f;
    public float jumpSpeed = 5f;

    

    //Camera variables
    Camera cam;

    
    void Start()
    {
        thisPlayerController = GetComponent<CharacterController>();
        thisPlayerAnimator = GetComponentInChildren<Animator>();
        cam = Camera.main;

        
    }

    
    void Update()
    {
        GetInput();
    }

    void GetInput(){
        
            //taking inputs
            inputXaxis = Input.GetAxis("Horizontal");
            inputXaxis = Mathf.Clamp(inputXaxis, -0.5f, 0.5f);
            inputZaxis = Input.GetAxis("Vertical");
            inputYaxis = 0f;

        
            if(thisPlayerController.isGrounded){
                movementDirection = new Vector3(inputXaxis, inputYaxis, inputZaxis);
                movementDirection = transform.TransformDirection(movementDirection);
                movementDirection *= movementSpeed;
                if(movementDirection.x + movementDirection.z == 0f){
                        moving = false;
                        thisPlayerAnimator.SetBool("Moving", moving);
                }
                else{
                        moving = true;
                        thisPlayerAnimator.SetBool("Moving", moving);
                        RotateToCamera();
                }
                if(Input.GetButtonDown("Jump")){
                    movementDirection.y = jumpSpeed;
                }
            } 
            else{
                movementDirection = new Vector3(inputXaxis, inputYaxis, inputZaxis);
                movementDirection = transform.TransformDirection(movementDirection);
                movementDirection.x *= movementSpeed;
                movementDirection.z *= movementSpeed;
            }   

            movementDirection.y -= gravity * Time.deltaTime;

            
                thisPlayerController.Move(movementDirection * Time.deltaTime);
            
                if(movementDirection.magnitude>0f){
                thisPlayerAnimator.SetFloat("VelocityX", Vector3.Dot(movementDirection, transform.forward));
                thisPlayerAnimator.SetFloat("VelocityZ", Vector3.Dot(movementDirection, transform.right));
                }
                else{
                thisPlayerAnimator.SetFloat("VelocityX", Vector3.Dot(movementDirection, transform.forward));
                thisPlayerAnimator.SetFloat("VelocityZ", Vector3.Dot(movementDirection, transform.right));
                }
         
    }

    void RotateToCamera(){
        if(moving){
            Vector3 camForward = cam.transform.forward;
            Vector3 camRight = cam.transform.right;
            camForward.y = camRight.y = 0f;
            transform.rotation = Quaternion.Slerp (transform.rotation, Quaternion.LookRotation (camForward), rotationSpeed * Time.deltaTime);
        }
    }

    public void RotateToCameraWhileAiming(){
            Vector3 camForward = cam.transform.forward;
            Vector3 camRight = cam.transform.right;
            camForward.y = camRight.y = 0f;
            transform.rotation = Quaternion.Slerp (transform.rotation, Quaternion.LookRotation (camForward), (rotationSpeed*3) * Time.deltaTime);
    }
}
