using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    //[SerializeField] private Transform groundCheckTransform = null;
    //[SerializeField] private LayerMask playerMask;
    [SerializeField] Transform oreintation;

    float playerHeight = 2f;

    private float horizontalInput;
    private float verticalInput;

    //private bool jumpKeyWasPressed;
    public float playerMovementSpeed;
    [SerializeField] float walkSpeed = 4f;
    [SerializeField] float sprintSpeed = 6f;
    [SerializeField] float acceleration = 10f; 

    public float moveSpeedMultiplier;
    public float airSpeedMultiplier;
    private float airTime;

    [SerializeField] private float jumpPower;

    //two drag values, one while grounded, the other while airborne
    [SerializeField] float groundDrag;
    [SerializeField] float airDrag;

    Vector3 moveDirection;
    Vector3 slopeMoveDirection;

    // all ground detection stuff
    bool isGrounded;
    bool canJump;
    //public bool has_speed_increase_perk;

    public float groundDistance;
    [SerializeField] LayerMask groundMask;
    [SerializeField] Transform groundCheck;

    [Header("KeyBinds")]
    [SerializeField] KeyCode sprintKey = KeyCode.LeftShift;

    [SerializeField] private AudioClip footstep_sound;
    [SerializeField] private AudioClip footstep_jump_sound;
    [SerializeField] private AudioClip footstep_jumpLand_sound;
    [SerializeField] private AudioClip footstep_run_sound;
    private float footstep_delay_between_sounds = 0f;
    [SerializeField] private float footstep_sound_speed;
    private AudioSource player_audiosource;

    RaycastHit slopeHit;

    private Rigidbody rb;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        player_audiosource = GetComponent<AudioSource>();
        canJump = true;

    }

    // Update is called once per frame
    void Update()
    {
        //uses a raycast to check if player is grounded
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);

        if (InputManager.IsInputEnabled)
        {
            PlayerMovements();
            ControlDrag();
            ControlSpeed();

        }

        slopeMoveDirection = Vector3.ProjectOnPlane(moveDirection, slopeHit.normal);

        //if airTime is greater than 0 and the player is grounded, then they just landed
        if (airTime > 0)
        {
            if (isGrounded)
            {
                player_audiosource.pitch = Random.Range(.75f, 1f);

                player_audiosource.PlayOneShot(footstep_jumpLand_sound);
            }
        }

        if (isGrounded)
        {
            airTime = 0f;
        }
        else
        {
            airTime += Time.deltaTime;
        }

    }
    private void FixedUpdate()
    {
        //moves player with respect to direction
        if (isGrounded && !OnSlope())
        {
            rb.AddForce(moveDirection.normalized * playerMovementSpeed * moveSpeedMultiplier, ForceMode.Acceleration);
        }
        else if (isGrounded && OnSlope())
        {
            rb.AddForce(slopeMoveDirection.normalized * playerMovementSpeed * moveSpeedMultiplier, ForceMode.Acceleration);
        }
        else if (!isGrounded)
        {
            rb.AddForce(moveDirection.normalized * playerMovementSpeed * airSpeedMultiplier, ForceMode.Acceleration);
        }

    }
    private void PlayerJump()
    {
        //increase jumpPower to jump higher 
        // *IMPORTANT: A high drag value will cause player too fall down too slowly
        rb.velocity = new Vector3(rb.velocity.x, 0, rb.velocity.z);
        rb.AddForce(transform.up * jumpPower, ForceMode.Impulse);
        

        //if (Physics.OverlapSphere(groundCheckTransform.position, 0.1f, playerMask).Length == 0)
        //{
        //return;
        // }
        //if (jumpKeyWasPressed)
        //{
        // jumpKeyWasPressed = false;
        //float jumpPower = 15f;
        // rb.AddForce(Vector3.up * jumpPower, ForceMode.VelocityChange);
        //}
    }
    private void ControlDrag()
    {
        if (isGrounded)
        {
            rb.drag = groundDrag;
        }
        else
        {
            rb.drag = airDrag;
        }

    }
    private void PlayerMovements()
    {
        //checks for jump input
        if (Input.GetButtonDown("Jump") && isGrounded && canJump)
        {
            if (player_audiosource.isPlaying)
            {
                player_audiosource.pitch = Random.Range(.85f, 1f);
                player_audiosource.Stop();
                player_audiosource.PlayOneShot(footstep_jump_sound);
            }
            else
            {
                player_audiosource.pitch = Random.Range(.85f, 1f);
                player_audiosource.PlayOneShot(footstep_jump_sound);
            }
            PlayerJump();
            StartCoroutine(PlayerJumpCooldown());

        }

        //checks for movement (walking) input (only works for keyboard)
        horizontalInput = Input.GetAxisRaw("Horizontal");
        verticalInput = Input.GetAxisRaw("Vertical");
        if ((Input.GetButton("Horizontal") || Input.GetButton("Vertical")) && Time.time > footstep_delay_between_sounds && isGrounded)
        {
            player_audiosource.pitch = Random.Range(.85f, 1f);
            //walk speed should be about 2.3f and sprint speed should be a bit higher
            if (Input.GetKey(sprintKey))
            {
                footstep_delay_between_sounds = Time.time + 1f / (footstep_sound_speed * 1.4f);
                player_audiosource.PlayOneShot(footstep_run_sound);
            }
            else
            {
                footstep_delay_between_sounds = Time.time + 1f / footstep_sound_speed;
                player_audiosource.PlayOneShot(footstep_sound);
            }
        }

        moveDirection = oreintation.forward * verticalInput + oreintation.right * horizontalInput;
    }
    IEnumerator PlayerJumpCooldown()
    {
        canJump = false;
        //Debug.Log("cant jump");
        yield return new WaitForSeconds(1.2f);
        //Debug.Log("You can now jump");
        canJump = true;
    }
    void ControlSpeed()
    {
        if(Input.GetKey(sprintKey) && isGrounded)
        {
            playerMovementSpeed = Mathf.Lerp(playerMovementSpeed, sprintSpeed, acceleration * Time.deltaTime);

        }
        else
        {
            playerMovementSpeed = Mathf.Lerp(playerMovementSpeed, walkSpeed, acceleration * Time.deltaTime);
        }
    }
    private bool OnSlope()
    {
        if (Physics.Raycast(transform.position, Vector3.down, out slopeHit, playerHeight / 2 + 0.5f))
        {
            //checking if the surface is a slope
            if (slopeHit.normal != Vector3.up)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        return false;
    }
    public void IncreaseSpeed()
    {
        walkSpeed += 5f;
        sprintSpeed += 5f;
    }
    public void DecreaseSpeed()
    {
        walkSpeed -= 5f;
        sprintSpeed -= 5f;
    }
    public void FreezePlayer()
    {
        //this function, when called, will freeze the player's position
        //this will mainly be called when the player is killed as to prevent any movement when dying
        rb.constraints = RigidbodyConstraints.FreezePosition;
    }
}
