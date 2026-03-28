using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target; // Drag your Truck here
    public Vector3 offset = new Vector3(0, 5, -10); // Position behind the truck
    public float smoothSpeed = 0.125f;

    // --- NEW VARIABLE FOR TILT ---
    // Increase this value in the Inspector (e.g., 2 or 3) to look higher and further ahead.
    public float lookAheadOffset = 1.5f; 
    // ----------------------------

    void LateUpdate()
    {
        // Safety check in case you forget to drag the truck in
        if (target == null) return;

        // Calculate the desired position
        Vector3 desiredPosition = target.position + target.TransformDirection(offset);
        
        // Smoothly move the camera
        transform.position = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);

        // --- NEW LOGIC TO TILT UPWARD ---
        // Instead of looking at target.position (the floor), we look higher.
        // This calculates a point slightly above the truck (target.up * lookAheadOffset)
        Vector3 lookPoint = target.position + (target.up * lookAheadOffset);
        
        // This makes the camera rotate to look at that higher point, effectively "tilting up."
        transform.LookAt(lookPoint);
        // ----------------------------------
    }
}