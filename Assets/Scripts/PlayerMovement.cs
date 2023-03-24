using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("Movement")]
    public float moveSpeed;

    public float groundDrag;
    public float saveDrag;

    [Space]
    public int jumpCountMax;
    public int jumpCount;
    public float jumpForce;
    public float jumpCooldown;
    public float airMultiplier;
    public bool readyToJump = true;
    public float downwardForce;
    public float antiFloatiness;
    [Space]
    public int dashChargesMax;
    public int dashCharges;
    public float dashChargeCooldown;
    public float dashForce;
    public float dashCooldown;
    public bool readyToDash = true;
    public bool isDashing;

    [Header("Keybinds")]
    public KeyCode jumpKey = KeyCode.Space;
    public KeyCode dashKey = KeyCode.LeftShift;

    [Header("Ground Check")]
    public float playerHeight;
    public LayerMask whatIsGround;
    public bool grounded;
    public bool readyToLand;

    [Header("Slope Handling")]
    public float maxSlopeAngle;
    private RaycastHit slopeHit;
    private bool exitingSlope;

    [Header("Extra")]
    public Transform orientation;

    float horizontalInput;
    float verticalInput;

    Vector3 moveDirection;

    public Rigidbody rb;
    public float footstepSpeed;
    public PlayerSoundHandler sfxHandler;

    // Start is called before the first frame update
    void Start()
    {
        //set rigidbody for player
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;

        jumpCount = jumpCountMax;
        dashCharges = dashChargesMax;
        saveDrag = groundDrag;

        //set other stuff
        sfxHandler = GetComponent<PlayerSoundHandler>();
    }

    private void Update()
    {
        //ground check
        
        if (Physics.Raycast(transform.position, Vector3.down, playerHeight * 0.5f + 0.2f, whatIsGround))
        {
            if (!grounded)
            {
                readyToLand = true;
            }
            grounded = true;
        }
        else
        {
            grounded = false;
        }

        MyInput();
        SpeedControl();

        //handle/apply drag
        if (grounded)
        {
            rb.drag = groundDrag;
        }
        else
        {
            rb.drag = 0;
        }
    }

    private void FixedUpdate()
    {
        MovePlayer();
    }

    private void MyInput()
    {
        //register keyboard input
        horizontalInput = Input.GetAxisRaw("Horizontal");
        verticalInput = Input.GetAxisRaw("Vertical");

        //footstep sfx
        if(verticalInput != 0 && grounded && sfxHandler.soundIsPlaying == false)
        {
            sfxHandler.StartCoroutine(sfxHandler.PlaySFXwDelay(sfxHandler.footstep, footstepSpeed / moveSpeed, true));
        }

        //jumping from the ground
        if (Input.GetKey(jumpKey) && readyToJump && grounded && jumpCount >= 1)
        {
            readyToJump = false;
            jumpCount -= 1;

            Jump();

            Invoke("ResetJump", jumpCooldown); // this way you can continuously jump if you hold down the jump key.
        } 
        else if (Input.GetKey(jumpKey) && readyToJump && !grounded && jumpCount >= 1) //dirty to allow for double jumps
        {
            readyToJump = false;
            jumpCount -= 1;

            Jump();

            Invoke("ResetJump", jumpCooldown); // this way you can continuously jump if you hold down the jump key.
        }

        //dash go
        if (Input.GetKeyDown(dashKey) && readyToDash && dashCharges >= 1)
        {
            readyToDash = false;
            isDashing = true;
            dashCharges -= 1;

            Dash();

            groundDrag = 0;
            Invoke("ResetDash", dashCooldown);
        }
    }

    private void MovePlayer() //THIS IS THE PART THAT UPDATES THE PLAYER POSITION THIS IS THE PART THAT UPDATES THE PLAYER POSITION THIS IS THE PART THAT UPDATES THE PLAYER POSITION THIS IS THE PART THAT UPDATES THE PLAYER POSITION
    {
        //calculate movement direction
        moveDirection = orientation.forward * verticalInput + orientation.right * horizontalInput;

        //on ground
        if (grounded && !isDashing)
        {
            rb.AddForce(moveDirection.normalized * moveSpeed * 10f, ForceMode.Force);
            if (jumpCount != jumpCountMax && readyToJump == true)
            {
                jumpCount = jumpCountMax; //resets jumps
            }
        }
        //in air
        else if (!grounded && !isDashing)
        {
            rb.AddForce(moveDirection.normalized * moveSpeed * 10f * airMultiplier, ForceMode.Force);

            Vector3 checkNeg = new Vector3(0, rb.velocity.y, 0); //check for negative values on Y axis

            if (checkNeg.y <= 0)
            {
                rb.AddForce(transform.up * downwardForce, ForceMode.Force); //increase downward force
            }

            //constant downward force for less floatiness
            rb.AddForce(transform.up * antiFloatiness, ForceMode.Force);
        }

        //if dashing
        else if (isDashing)
        {
            rb.AddForce(moveDirection.normalized * moveSpeed * 10f * 5, ForceMode.Force);
            rb.velocity = new Vector3(rb.velocity.x, Mathf.Clamp(rb.velocity.y,-10,10), rb.velocity.z);
        }

        //on slope
        if (OnSlope() && !exitingSlope)
        {
            rb.AddForce(GetSlopeMoveDirection() * moveSpeed * 20f, ForceMode.Force);

            if (rb.velocity.y > 0)
            {
                rb.AddForce(Vector3.down * 20f, ForceMode.Force);
            }

        }
        //turn off gravity when on ground
        if (grounded)
        {
            rb.useGravity = false;
        }
        else
        {
            rb.useGravity = true;
        }
    }

    private void SpeedControl()
    {
        //limit speed on slope
        if (OnSlope() && !exitingSlope)
        {
            if(rb.velocity.magnitude > moveSpeed)
            {
                rb.velocity = rb.velocity.normalized * moveSpeed;
            }
        }

        //limiting speed on ground or in air
        else
        {
            //get flat velocity
            Vector3 flatVel = new Vector3(rb.velocity.x, 0, rb.velocity.z);

            //limit velocity if needed
            if (flatVel.magnitude > moveSpeed) //if you exceed the moveSpeed variable
            {
                Vector3 limitedVel = flatVel.normalized * moveSpeed; //calculate what max velocity would be
                rb.velocity = new Vector3(limitedVel.x, rb.velocity.y, limitedVel.z); //then apply that max velocity
            }
        }
    }

    private bool OnSlope()
    {
        if(Physics.Raycast(transform.position, Vector3.down, out slopeHit, playerHeight * 0.6f + 0.3f))
        {
            float angle = Vector3.Angle(Vector3.up, slopeHit.normal); //get angle of object hit by raycast
            return angle < maxSlopeAngle && angle != 0; //return true if angle is below max and not 0
        }

        return false; // when hitting nothing
    }

    private Vector3 GetSlopeMoveDirection()
    {
        return Vector3.ProjectOnPlane(moveDirection, slopeHit.normal).normalized; // It's a direction so normalize it https://docs.unity3d.com/ScriptReference/Vector3.ProjectOnPlane.html < explanation of the function, In short: This project our move direction onto the slope, adding force following it's vector angle
    }

    private void Jump()
    {
        sfxHandler.StartCoroutine(sfxHandler.PlaySFXwDelay(sfxHandler.jump, 0, true));

        exitingSlope = true;

        //reset y velocity, this way you always jump the exact same height
        rb.velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);

        rb.AddForce(transform.up * jumpForce, ForceMode.Impulse); //use forcemode.impulse because you're only applying the force once
    }

    private void ResetJump()
    {
        readyToJump = true;

        exitingSlope = false;
    }

    private void Dash()
    {
        rb.velocity = new Vector3(0, rb.velocity.y, 0);
        sfxHandler.StartCoroutine(sfxHandler.PlaySFXwDelay(sfxHandler.dash, 0, true));

        if (verticalInput == 0 && horizontalInput == 0)
        {
            rb.AddForce(orientation.forward * dashForce, ForceMode.Impulse);
        }
        else if (verticalInput != 0 || horizontalInput != 0)
        {
            rb.AddForce(moveDirection * dashForce, ForceMode.Impulse);
        }
    }

    private void ResetDash()
    {
        groundDrag = saveDrag;
        readyToDash = true;
        isDashing = false;
    }
}
