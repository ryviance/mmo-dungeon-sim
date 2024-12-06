using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float speed = 5f; // Speed of the player
    public float rotationSpeed = 700f; // Speed of rotation
    public float jumpHeight = 2f; // Height of the jump
    public float gravity = -9.81f; // Gravity value
    private Vector3 velocity; // Player's velocity
    private bool isGrounded; // Check if the player is on the ground
    private CharacterController controller;

    private Color gizmoColor = Color.green; // Color of the Gizmo
    private CharacterController characterController;
    public Animator animator; // Reference to the Animator component

    void Start()
    {
        controller = GetComponent<CharacterController>();
    }

    void Update()
    {
        MovePlayer();
        Jump();

        // Update the Animator parameters
        UpdateAnimator();
    }

    bool IsGrounded()
    {
        Ray ray = new Ray(transform.position, Vector3.down);
        float rayLength = 0.3f;

        return Physics.Raycast(ray, rayLength);
    }

    void MovePlayer()
    {
        // Get input 
        float moveHorizontal = Input.GetAxis("Horizontal");
        float moveVertical = Input.GetAxis("Vertical");

        Camera mainCamera = Camera.main; // Assuming the main camera is tagged as "MainCamera"
        Vector3 forward = mainCamera.transform.forward;
        Vector3 right = mainCamera.transform.right;

        forward.y = 0;
        right.y = 0;

        forward.Normalize();
        right.Normalize();

        Vector3 moveDirection = (forward * moveVertical + right * moveHorizontal).normalized;

        if (moveDirection.magnitude > 0.1f)
        {
            Quaternion targetRotation = Quaternion.LookRotation(moveDirection);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        }

        controller.Move(moveDirection * speed * Time.deltaTime);
    }

    void Jump()
    {
        if (IsGrounded() && velocity.y < 0)
        {
            velocity.y = -2f;
        }

        if (Input.GetButtonDown("Jump") && IsGrounded())
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
            animator.SetBool("IsJumping", true); // Set IsJumping to true when jumping
        }

        if (IsGrounded() && velocity.y < 0)
        {
            animator.SetBool("IsJumping", false); // Reset IsJumping when grounded
        }

        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);
    }

    void UpdateAnimator()
    {
        // Calculate the movement speed to update the Speed parameter
        Vector3 horizontalVelocity = new Vector3(controller.velocity.x, 0, controller.velocity.z);
        float currentSpeed = horizontalVelocity.magnitude;
        animator.SetFloat("Speed", currentSpeed); // Update Speed in the Animator

        // Update IsJumping in case Jump logic missed it
        animator.SetBool("IsJumping", !IsGrounded());
    }

    void OnDrawGizmos()
    {
        characterController = GetComponent<CharacterController>();

        if (characterController != null)
        {
            Gizmos.color = gizmoColor;

            Vector3 center = transform.position + characterController.center;
            float radius = characterController.radius;
            float height = characterController.height;

            Gizmos.DrawWireSphere(center + Vector3.up * (height / 2 - radius), radius);
            Gizmos.DrawWireSphere(center - Vector3.up * (height / 2 - radius), radius);
            Gizmos.DrawLine(center + Vector3.up * (height / 2 - radius), center - Vector3.up * (height / 2 - radius));
        }
    }
}