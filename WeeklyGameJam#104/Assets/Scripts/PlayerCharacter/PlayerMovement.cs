using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerMovement : MonoBehaviour
{

    public Animator anim;
    public Transform playerBody;

    public float movementSmoothness = 5f;
    public float movementSpeed = 1f;
    public float jumpForce = 1;
    public float gravityForce = 1;

    [SerializeField]
    Vector2 movementVector;
    Vector2 input;
    CharacterController cc;

    private float movementChangeVel;

    void Start()
    {
        cc = GetComponent<CharacterController>();
    }

    void Update()
    {
        HandleInput(); 
        ApplyMovement();
        HandleAnimations();
    }

    void HandleInput()
    {
        input = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Jump")).normalized;
    }


    void ApplyMovement()
    {
        movementVector.x = Mathf.SmoothDamp(movementVector.x, input.x, ref movementChangeVel, movementSmoothness); //Smooth out the movement by movementSmoothness
        if (Mathf.Abs(movementVector.x) <= 0.001f) //Cut vector if too low
            movementVector.x = 0f;

        if (cc.isGrounded)
        {
            if (Input.GetAxisRaw("Jump") > 0)
            {
                anim.SetTrigger("Jump");
                movementVector.y = Input.GetAxisRaw("Jump") * jumpForce; //Do the jump
            }
            else
            {
                movementVector.y = 0;
            }
        }
        else
        {
            movementVector.y -= gravityForce; //Apply gravity if not grounded
        }

        //Scale the movement by speed and apply to CC
        if (movementVector != Vector2.zero)
        {
            Vector3 movementVectorScaled = movementVector;
            cc.Move(new Vector3(movementVectorScaled.x * movementSpeed, movementVectorScaled.y, 0f));
        }
    }

    void HandleAnimations()
    {
        if (anim)
        {
            anim.SetBool("Grounded", cc.isGrounded || movementVector.y == 0);
            anim.SetFloat("Speed", Mathf.Abs(movementVector.x));
            anim.SetFloat("VerticalVel", movementVector.y);
        }

        if(Mathf.Abs(movementVector.x) > 0) //A dirty way to handle rotations. Will move to a separate function later on
        {
            Quaternion targetRotation = movementVector.x > 0 ? Quaternion.Euler(0, 90, 0) : Quaternion.Euler(0, -90, 0);

            playerBody.rotation = Quaternion.Slerp(playerBody.rotation, targetRotation, 15f * Time.deltaTime);
        }
    }

    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        //Check if hit anything directly above the character. (Will make a function for it later)

        float dot = Vector3.Dot((hit.point - transform.position).normalized, transform.up);
        if (dot >= 0.99f && movementVector.y > 0)
        {
            movementVector.y = 0; //Clear jumpting force if so
        }
    }

}
