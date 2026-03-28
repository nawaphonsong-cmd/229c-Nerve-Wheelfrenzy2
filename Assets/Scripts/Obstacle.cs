using UnityEngine;

public class Obstacle : MonoBehaviour
{
    public int pointsValue = 10;
    private bool hasBeenHit = false;

    // --- NEW VARIABLE ---
    // The obstacle will fall for 1 second before vanishing
    [SerializeField] private float lingerTime = 1.0f; 
    // --------------------

    private void OnCollisionEnter(Collision collision)
    {
        // Check if the truck (tagged "Player") hit us
        if (collision.gameObject.CompareTag("Player") && !hasBeenHit)
        {
            hasBeenHit = true;

            // 1. Update the UI Score
            UIManager ui = FindObjectOfType<UIManager>();
            if (ui != null)
            {
                ui.AddScore(pointsValue);
            }

            // --- THIS LOGIC CREATES THE FALL & VANISH ---
            
            // A. Make sure the Rigidbody is NOT Kinematic
            // This ensures gravity starts pulling it down instantly
            Rigidbody rb = GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.isKinematic = false; 
            }

            // B. Optional: Change the color to show it was hit
            GetComponent<Renderer>().material.color = Color.red;

            // C. This tells Unity: "Wait 1 second, THEN destroy this object"
            Destroy(gameObject, lingerTime);
            
            // ---------------------------------------------
        }
    }
}