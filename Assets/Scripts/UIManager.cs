using UnityEngine;
using TMPro;

public class UIManager : MonoBehaviour
{
    public TextMeshProUGUI statsDisplay;
    public Rigidbody truckRb; 
    public Collider cargoZone; // Drag your CargoZone trigger here in the Inspector

    private float score = 0;
    private float timer = 0;
    private int cachedBoxCount = 0;
    private float lastBoxCountUpdate = 0f;
    private const float BOX_COUNT_UPDATE_INTERVAL = 0.5f; // Update every 0.5 seconds

    void Update()
    {
        // 1. TIMER
        timer += Time.deltaTime;

        // 2. SPEED CALCULATION
        float speed = truckRb.linearVelocity.magnitude * 3.6f;

        // 3. WEIGHT CALCULATION
        // Update box count periodically to optimize performance
        if (Time.time - lastBoxCountUpdate > BOX_COUNT_UPDATE_INTERVAL)
        {
            cachedBoxCount = CountCrates();
            lastBoxCountUpdate = Time.time;
        }
        
        // Total Mass = Truck Mass + (Number of Boxes * Individual Box Mass)
        // Assuming each crate has a mass of 1.0f
        float totalMass = truckRb.mass + (cachedBoxCount * 1.0f); 
        
        float gravity = Mathf.Abs(Physics.gravity.y);
        float currentWeight = totalMass * gravity; // Theory D: W = mg

        // 4. MULTIPLIER LOGIC
        int multiplier = (cachedBoxCount > 0) ? cachedBoxCount * 2 : 1;

        // 5. UPDATE UI
        statsDisplay.text = 
            "TIME: " + timer.ToString("F1") + "s\n" +
            "SPEED: " + speed.ToString("F0") + " km/h\n" +
            "WEIGHT: " + currentWeight.ToString("F1") + " N\n" +
            "MULTIPLIER: x" + multiplier + "\n" +
            "SCORE: " + score;
    }

    // This checks how many boxes are still in the truck
    int CountCrates()
    {
        int count = 0;
        Collider[] hitColliders = Physics.OverlapBox(cargoZone.bounds.center, cargoZone.bounds.extents, cargoZone.transform.rotation);
        foreach (var hit in hitColliders)
        {
            if (hit.CompareTag("Crate")) count++;
        }
        return count;
    }

    // Updated AddScore to include the multiplier
    public void AddScore(int points) {
        int multiplier = Mathf.Max(1, cachedBoxCount * 2);
        score += points * multiplier;
    }
}