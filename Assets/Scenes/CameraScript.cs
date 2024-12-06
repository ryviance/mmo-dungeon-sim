using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform player; // The player the camera will follow
    public float distance = 5.0f; // Default distance from the player
    public float minDistance = 2.0f; // Minimum distance for zooming in
    public float maxDistance = 10.0f; // Maximum distance for zooming out
    public float height = 3.0f; // Height offset from the player
    public float rotationSpeed = 5.0f; // Speed at which the camera rotates with the mouse
    public float zoomSpeed = 2.0f; // Speed for zooming in and out
    public float zoomSmoothTime = 0.1f; // Time to smooth the zoom transition

    public float yaw = 0.0f; // Horizontal rotation angle
    public float pitch = 0.0f; // Vertical rotation angle
    private bool isDragging = false; // To track if the mouse is being dragged
    private Vector3 lastMousePosition; // Store the last mouse position
    private float targetDistance; // Target distance for smooth zooming
    private float zoomVelocity; // Smooth damp velocity

    void Start()
    {
        // Initialize camera rotation based on current position relative to the player
        yaw = transform.eulerAngles.y;
        pitch = transform.eulerAngles.x;
        targetDistance = distance; // Set targetDistance to initial distance
    }

    void LateUpdate()
    {
        // Check if the left mouse button is pressed
        if (Input.GetMouseButtonDown(0)) // Left mouse button
        {
            isDragging = true;
            lastMousePosition = Input.mousePosition; // Store the mouse position
        }

        // Check if the left mouse button is released
        if (Input.GetMouseButtonUp(0))
        {
            isDragging = false; // Stop dragging
        }

        // If dragging, update yaw and pitch based on mouse movement
        if (isDragging)
        {
            Vector3 delta = Input.mousePosition - lastMousePosition; // Calculate the change in mouse position
            yaw += delta.x * rotationSpeed * Time.deltaTime; // Update yaw based on horizontal movement
            pitch -= delta.y * rotationSpeed * Time.deltaTime; // Update pitch based on vertical movement

            // Clamp the vertical rotation (pitch) to prevent flipping the camera upside down
            pitch = Mathf.Clamp(pitch, -20f, 89f);

            lastMousePosition = Input.mousePosition; // Update the last mouse position
        }

        // Handle mouse wheel input for zooming
        float scrollInput = Input.GetAxis("Mouse ScrollWheel");
        targetDistance -= scrollInput * zoomSpeed; // Adjust target distance based on mouse wheel scroll
        targetDistance = Mathf.Clamp(targetDistance, minDistance, maxDistance); // Clamp target distance

        // Smoothly interpolate the distance for a more fluid zoom experience
        distance = Mathf.SmoothDamp(distance, targetDistance, ref zoomVelocity, zoomSmoothTime);

        // Rotate the camera based on yaw and pitch
        Quaternion rotation = Quaternion.Euler(pitch, yaw, 0f);

        // Calculate the position of the camera based on the player's position, distance, and height
        Vector3 targetPosition = player.position - (rotation * Vector3.forward * distance) + Vector3.up * height;

        // Update camera position and look at the player
        transform.position = targetPosition;
        transform.LookAt(player.position + Vector3.up * height * 0.5f); // Look slightly above the player to keep them centered
    }
}
