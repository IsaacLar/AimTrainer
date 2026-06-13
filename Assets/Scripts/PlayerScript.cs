using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerScript : MonoBehaviour
{

    public Rigidbody rb;
    public InputAction PlayerControls;
    public InputAction jumpAction;
    public InputAction sprintAction;

    public float defaultMoveSpeed;
    public float jumpForce;

    private Vector2 moveDirection = Vector2.zero;
    private float moveSpeed;
    private bool jumpRequested;
    private bool isGrounded = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        moveSpeed = defaultMoveSpeed;
        jumpRequested = false;
    }

    //Enable/Disable player controls
    private void OnEnable()
    {
        PlayerControls.Enable();
        jumpAction.Enable();
        sprintAction.Enable();
    }

    private void OnDisable()
    {
        PlayerControls.Disable();
        jumpAction.Disable();
        sprintAction.Disable();
    }

    // Update is called once per frame
    void Update()
    {
        moveDirection = PlayerControls.ReadValue<Vector2>();

        if(jumpAction.WasPressedThisFrame() && isGrounded)
        {
            jumpRequested = true;
        }

        if (sprintAction.WasPressedThisFrame())
        {
            moveSpeed *= 2;
        }

        if (sprintAction.WasReleasedThisFrame())
        {
            moveSpeed = defaultMoveSpeed;
        }
    }

    private void FixedUpdate()
    {
        rb.linearVelocity = new Vector3(moveDirection.x * moveSpeed *  Time.deltaTime,rb.linearVelocity.y, moveDirection.y * moveSpeed * Time.deltaTime);

        if (jumpRequested)
        {
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            jumpRequested = false;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.tag == "Ground")
        {
            isGrounded = true;
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.collider.tag == "Ground")
        {
            isGrounded = false;
        }
    }
}
