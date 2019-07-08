using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerMovement : MonoBehaviour
{

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
            movementVector.y = Input.GetAxisRaw("Jump") * jumpForce; //Do the jump
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
