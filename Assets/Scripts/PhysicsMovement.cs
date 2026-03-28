using UnityEngine;

public class PhysicsMovement : MonoBehaviour
{
    private Rigidbody rb;
    
    [Header("Drive Settings")]
    public float acceleration = 50f; 
    public float maxSpeed = 30f;
    public float turnSpeed = 100f;
    
    [Header("Drift Settings")]
    public KeyCode driftKey = KeyCode.LeftShift;
    public float normalDrag = 1f;
    public float driftDrag = 0.2f;

    void Start() {
        rb = GetComponent<Rigidbody>();
        rb.linearDamping = normalDrag;
        // This stops the car from flipping over easily
        rb.centerOfMass = new Vector3(0, -0.5f, 0); 
    }

    void FixedUpdate() {
        float moveInput = Input.GetAxis("Vertical"); // W/S or Up/Down
        float turnInput = Input.GetAxis("Horizontal"); // A/D or Left/Right

        // 1. FORWARD/BACKWARD (Theory D: F=ma)
        if (rb.linearVelocity.magnitude < maxSpeed) {
            // We multiply by moveInput directly so W is (+) and S is (-)
            rb.AddForce(transform.forward * moveInput * acceleration, ForceMode.Acceleration);
        }

        // 2. TURNING (Theory F: Rotational Motion)
        // We only turn if the car is moving to feel more realistic
        if (rb.linearVelocity.magnitude > 0.1f) {
            float turnMultiplier = (moveInput < 0) ? -1 : 1; // Reverse steering when backing up
            float rotation = turnInput * turnSpeed * turnMultiplier * Time.fixedDeltaTime;
            Quaternion turnRotation = Quaternion.Euler(0f, rotation, 0f);
            rb.MoveRotation(rb.rotation * turnRotation);
        }

        // 3. DOWNFORCE (Keeps wheels on road)
        rb.AddForce(Vector3.down * 15f, ForceMode.Acceleration);

        // 4. DRIFT LOGIC (Theory E: Friction)
        if (Input.GetKey(driftKey)) {
            rb.linearDamping = driftDrag;
        } else {
            rb.linearDamping = normalDrag;
        }
    }
}