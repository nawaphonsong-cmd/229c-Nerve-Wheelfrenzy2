using UnityEngine;
using UnityEngine.SceneManagement; // Required to switch scenes

public class FinishedZone : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        // Only trigger if the object hitting the line is tagged "Player"
        if (other.CompareTag("Player"))
        {
            Debug.Log("Finish Line Reached!");
            // This must match the exact name of your second scene
            SceneManager.LoadScene("CreditScene");
        }
    }
}