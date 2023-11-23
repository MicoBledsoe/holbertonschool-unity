using UnityEngine;
//Directives ^

public class Cameracontroller : MonoBehaviour
{
    public Transform player;
    public float followSpeed = 5f; //camera will follow the OGPlayer at a certain speed
    public float rotationSpeed = 2f; //rotation speed ^
    public bool isInverted = false;
    public float minY = 0f;
    public float mouseSensor = 50f;
    private Vector3 offset;
    private bool isRotating;
    private float mouseX;
    private float mouseY;

    private void Start()
    {
        Debug.Log("Player Position: " + player.position);
        Debug.Log("Camera Position: " + transform.position);
        Debug.Log("Offset: " + offset);
        offset = transform.position - player.position;
    }

    private void LateUpdate() //updates every frame contacntly within the game 
    {
    // Camera follows the player
    Debug.Log("CameraController Update");
    Vector3 targetPosition = player.position + offset; // Calculate the target position based on the OGPlayers position and the initial offset.
    targetPosition.y = Mathf.Max(targetPosition.y, minY); // Ensure the camera doesn't go below the minimum Y value.
    transform.position = Vector3.Lerp(transform.position, targetPosition, followSpeed * Time.deltaTime); // gives more of a smooth interpolate the camera's position towards the target position.
    Debug.Log("POS: " + Vector3.Lerp(transform.position, targetPosition, followSpeed * Time.deltaTime)); // gives more of a smooth interpolate the camera's position towards the target position.)
    //caculates linearly between two vectors

    // Handling camera rotation
    if (Input.GetMouseButton(0)) // If the left mouse button is held down...
    {
        mouseX = Input.GetAxis("Mouse X") * mouseSensor * 1;
        isRotating = true; // enables camera rotation.
    }

    else if (Input.GetMouseButtonUp(0)) // When the left mouse button is released...
    {
        isRotating = false; // stopss the from foing camera rotation.
    }

    if (isRotating) // If the camera is set to rotate
    {
        Quaternion rotation = Quaternion.Euler(0f, mouseX, 0f); //Quaternion for rotation around the y-axis based on mouse input
        offset = rotation * offset; // Updating the camera's offset
        Vector3 newPosition = player.position + offset; //Calculating the new position of the camera
        transform.position = Vector3.Lerp(transform.position, newPosition, followSpeed * Time.deltaTime); //interp the camera's position from its current position to the new position
        transform.LookAt(player.position); // making sure the camera always faces the OG
    }
    }
}
