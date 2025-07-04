using System.Collections;
using UnityEngine;

public class BaseMovement : MonoBehaviour
{
    public Transform cameraTransform;
    
    // Ground Movement
    private Rigidbody rb;
    public float CurrentMoveSpeed = 5f;

    private float MoveSpeed;

    private float moveHorizontal;
    private float moveForward;
    private int isBoosted;
    // Jumping
    public float jumpForce = 10f;
    public float fallMultiplier = 2.5f; // Multiplies gravity when falling down
    public float ascendMultiplier = 2f; // Multiplies gravity for ascending to peak of jump
    private bool isGrounded = true;
    public LayerMask groundLayer;
    private float groundCheckTimer = 0f;
    private float groundCheckDelay = 0.3f;
    private float playerHeight;
    private float raycastDistance;

    private Coroutine boostCoroutine;

    public void StartBoost(float speedMultiplier, float boostDuration)
    {

        if (boostCoroutine != null)
            StopCoroutine(boostCoroutine);

        boostCoroutine = StartCoroutine(BoostRoutine(speedMultiplier, boostDuration));
    }

    private IEnumerator BoostRoutine(float speedMultiplier, float boostDuration)
    {
        CurrentMoveSpeed = MoveSpeed;
        CurrentMoveSpeed *= speedMultiplier;

        if(isBoosted == 0)
            isBoosted = Random.Range(1, 3);

        yield return new WaitForSeconds(boostDuration);

        CurrentMoveSpeed = MoveSpeed;
        isBoosted = 0;

        boostCoroutine = null;
    }

    public int IsBoosted()
    {
        return isBoosted;
    }


    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;
        MoveSpeed = CurrentMoveSpeed;

        // Set the raycast to be slightly beneath the player's feet
        playerHeight = GetComponent<CapsuleCollider>().height * transform.localScale.y;
        raycastDistance = (playerHeight / 2) + 0.2f;

        // Hides the mouse
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
    public float GetOriginalSpeed()
    {
        return this.MoveSpeed;
    }
    void Update()
    {
        moveHorizontal = Input.GetAxisRaw("Horizontal");
        moveForward = Input.GetAxisRaw("Vertical");

        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            Jump();
        }

        // Checking when we're on the ground and keeping track of our ground check delay
        if (!isGrounded && groundCheckTimer <= 0f)
        {
            Vector3 rayOrigin = transform.position + Vector3.up * 0.1f;
            isGrounded = Physics.Raycast(rayOrigin, Vector3.down, raycastDistance, groundLayer);
        }
        else
        {
            groundCheckTimer -= Time.deltaTime;
        }

    }

    void FixedUpdate()
    {
        MovePlayer();
        ApplyJumpPhysics();
    }
    public float GetSpeed()
    {
        if (moveForward != 0)
        {
            Vector3 horizontalVelocity = rb.linearVelocity;
            horizontalVelocity.y = 0;

            // Avance ou recul par rapport à l'orientation du personnage
            float speed = Vector3.Dot(cameraTransform.forward, horizontalVelocity);

            return speed;
        }
        else return 0;
    }

    public float GetStrafeSpeed()
    {
        if (moveHorizontal != 0)
        {
            Vector3 horizontalVelocity = rb.linearVelocity;
            horizontalVelocity.y = 0;

            return Vector3.Dot(cameraTransform.right, horizontalVelocity);
        }
        else return 0;
    }


    void MovePlayer()
    {

        Vector3 camForward = cameraTransform.forward;
        Vector3 camRight = cameraTransform.right;

        camForward.y = 0f;
        camRight.y = 0f;

        camForward.Normalize();
        camRight.Normalize();

        // Mouvement basé sur la caméra
        Vector3 movement = (camRight * moveHorizontal + camForward * moveForward).normalized;

        Vector3 targetVelocity = movement * CurrentMoveSpeed;

        // Apply movement to the Rigidbody
        Vector3 velocity = rb.linearVelocity;
        velocity.x = targetVelocity.x;
        velocity.z = targetVelocity.z;
        rb.linearVelocity = velocity;

        // If we aren't moving and are on the ground, stop velocity so we don't slide
        if (isGrounded && moveHorizontal == 0 && moveForward == 0)
        {
            rb.linearVelocity = new Vector3(0, rb.linearVelocity.y, 0);
        }
    }

    void Jump()
    {
        isGrounded = false;
        groundCheckTimer = groundCheckDelay;
        rb.linearVelocity = new Vector3(rb.linearVelocity.x, jumpForce, rb.linearVelocity.z); // Initial burst for the jump
    }

    void ApplyJumpPhysics()
    {
        if (rb.linearVelocity.y < 0) 
        {
            // Falling: Apply fall multiplier to make descent faster
            rb.linearVelocity += Vector3.up * Physics.gravity.y * fallMultiplier * Time.fixedDeltaTime;
        } // Rising
        else if (rb.linearVelocity.y > 0)
        {
            // Rising: Change multiplier to make player reach peak of jump faster
            rb.linearVelocity += Vector3.up * Physics.gravity.y * ascendMultiplier  * Time.fixedDeltaTime;
        }
    }
}
