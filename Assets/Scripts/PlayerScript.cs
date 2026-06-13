using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class PlayerScript : MonoBehaviour
{

    public Rigidbody rb;
    public InputAction PlayerControls;
    public InputAction jumpAction;
    public InputAction sprintAction;
    public InputAction pauseAction;
    public InputAction shootAction;
    public InputAction interactAction;
    public Camera playerCam;

    //Default speed of the player
    public float defaultMoveSpeed;
    public float jumpForce;
    //Mouse x and y sensitivity
    public float sensX;
    public float sensY;

    //Direction the player should move in depending on input
    private Vector2 moveDirection = Vector2.zero;
    //Current speed of the player
    private float moveSpeed;
    //Whether the player has pressed space or not
    private bool jumpRequested;
    //If the player is colliding with the ground plane
    private bool isGrounded = false;
    //If the mouse is captured inside screen space
    private bool mouseCaptured;
    //X and Y rotation of the camera
    private float xRotation;
    private float yRotation;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //Capture mouse on screen
        CaptureMouse();

        moveSpeed = defaultMoveSpeed;
        jumpRequested = false;
    }

    //Enable/Disable player controls
    private void OnEnable()
    {
        PlayerControls.Enable();
        jumpAction.Enable();
        sprintAction.Enable();
        pauseAction.Enable();
        shootAction.Enable();
        interactAction.Enable();
    }

    private void OnDisable()
    {
        PlayerControls.Disable();
        jumpAction.Disable();
        sprintAction.Disable();
        pauseAction.Disable();
        shootAction.Disable();
        interactAction.Disable();
    }

    // Update is called once per frame
    void Update()
    {
        //Get current movement vector
        moveDirection = PlayerControls.ReadValue<Vector2>();

        //Get mouse input
        Vector2 mouseDelta = Mouse.current.delta.ReadValue();
        float mouseX = mouseDelta.x * Time.deltaTime * sensX;
        float mouseY = mouseDelta.y * Time.deltaTime * sensY;
        
        //Get relevant x and y rotation
        yRotation += mouseX;
        xRotation -= mouseY;
        //Clamp up/down rotation
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        //Turn player camera up/down/left/right
        playerCam.transform.rotation = Quaternion.Euler(xRotation, yRotation, 0);
        //Turn player body left/right
        transform.rotation = Quaternion.Euler(0, yRotation, 0);


        //Called if player presses space
        if(jumpAction.WasPressedThisFrame() && isGrounded)
        {
            //Queues a jump to be processed
            jumpRequested = true;
        }

        //Sprint mechanic
        if (sprintAction.WasPressedThisFrame())
        {
            moveSpeed *= 2;
        }

        if (sprintAction.WasReleasedThisFrame())
        {
            moveSpeed = defaultMoveSpeed;
        }

        //Pause/unpause and release/capture mouse
        if (pauseAction.WasPressedThisFrame())
        {
            if (mouseCaptured)
            {
                ReleaseMouse();
            } else
            {
                CaptureMouse();
            }
        }

        //Shooting logic
        if (shootAction.WasPressedThisFrame())
        {
            //Generate a new raycast straight forward from current cameras position
            Ray ray = new Ray(playerCam.transform.position, playerCam.transform.forward);
            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                //If the collider hits a target
                if (hit.collider.CompareTag("Target"))
                {
                    Debug.Log("Target Hit");
                    //Send a message to the target telling it its been hit
                    Target target = hit.collider.GetComponent<Target>();
                    target.OnHit();
                }
            }
        }

        //Interact logic
        if (interactAction.WasPressedThisFrame())
        {
            Ray interactRay = new Ray(playerCam.transform.position, playerCam.transform.forward);
            if (Physics.Raycast(interactRay, out RaycastHit hit))
            {
                if (hit.collider.CompareTag("Button"))
                {
                    Button button = hit.collider.GetComponent<Button>();
                    button.OnHit();
                }
            }
        }
    }

    private void FixedUpdate()
    {
        //Find movement vector relevant to the direction the player is facing
        Vector3 move = (transform.forward * moveDirection.y + transform.right * moveDirection.x) * moveSpeed;
        //Sets player velocity depending on input
        rb.linearVelocity = new Vector3(move.x,rb.linearVelocity.y, move.z);

        if (jumpRequested)
        {
            //Add jump force
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

    private void CaptureMouse()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        mouseCaptured = true;
    }

    private void ReleaseMouse()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        mouseCaptured = false;
    }
}
