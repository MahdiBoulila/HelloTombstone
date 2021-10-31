using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    // Start is called before the first frame update
    public Transform playerCamera = null;
    public float mouseSensitivity = 3.5f;
    public bool lockCursor = true;
    public float walkSpeed = 6.0f;
    float cameraPitch = 0.0f;
    float velocityY = 0f;
    CharacterController controller = null;
    // to smooth the camera movement
    [SerializeField][Range(0.0f, 0.05f)] float mouseSmoothTime = 0.03f;
    Vector2 currentMouseDelta = Vector2.zero;
    Vector2 currentMouseDeltaVelocity = Vector2.zero;

    // gravity
    public float gravity = -9.8f;
    private Vector3 movingDirection = Vector3.zero;
    // jumping
    public float jumpforce = 15f;
    public float jumpSpeed = 2.0f;
    Rigidbody rigidbody;
    void Start()
    {
        rigidbody = GetComponent<Rigidbody>();
        controller = GetComponent<CharacterController>();
        if(lockCursor){
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
    }

    // Update is called once per frame
    void Update()
    { 
        UpdateMouseLook();
        UpdateMovement();
        if (Input.GetKeyDown(KeyCode.Space) && controller.isGrounded)
        {
            controller.Move(Vector3.up * jumpforce * jumpSpeed* Time.deltaTime);
        }
        
    }
    void UpdateMouseLook()
    {
        Vector2 targetMouseDelta = new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"));
        currentMouseDelta = Vector2.SmoothDamp(currentMouseDelta, targetMouseDelta, ref currentMouseDelta, mouseSmoothTime);
        cameraPitch -= currentMouseDelta.y * mouseSensitivity; // invert y-axis for mouse movement
        cameraPitch = Mathf.Clamp(cameraPitch, -90.0f, 90.0f); // lock vertical screen rotation to 180
        playerCamera.localEulerAngles = Vector3.right * cameraPitch;
        transform.Rotate(Vector3.up * currentMouseDelta.x * mouseSensitivity);
        
    }

    void UpdateMovement()
    {
        Vector2 inputDir = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        inputDir.Normalize();
        float jump = 0f;
        if (controller.isGrounded)
        {
            velocityY = 0.0f;
        }

        velocityY += gravity * Time.deltaTime;
        Vector3 velocity = (transform.forward * inputDir.y + transform.right * inputDir.x) * walkSpeed + Vector3.up * velocityY;
        controller.Move(velocity * Time.deltaTime);
    }
}
