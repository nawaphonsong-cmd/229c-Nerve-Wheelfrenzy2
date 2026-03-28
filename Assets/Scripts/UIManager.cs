using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    public TextMeshProUGUI statsDisplay;
    public Rigidbody truckRb;
    public Collider cargoZone;

    [Header("Win Settings")]
    public int pointsToWin = 100;
    public string winSceneName = "CreditScene";

    private float score = 0;
    private float timer = 0;
    private bool isFinished = false;
    private int cachedBoxCount = 0;
    private float lastBoxCountUpdate = 0f;
    private const float BOX_COUNT_UPDATE_INTERVAL = 0.5f;

    void Update()
    {
        if (isFinished) return;

        timer += Time.deltaTime;
        float speed = truckRb.linearVelocity.magnitude * 3.6f;

        // Update box count periodically to optimize performance
        if (Time.time - lastBoxCountUpdate > BOX_COUNT_UPDATE_INTERVAL)
        {
            cachedBoxCount = CountCrates();
            lastBoxCountUpdate = Time.time;
        }

        int multiplier = (cachedBoxCount > 0) ? cachedBoxCount * 2 : 1;

        statsDisplay.text =
            "TIME: " + timer.ToString("F1") + "s\n" +
            "SPEED: " + speed.ToString("F0") + " km/h\n" +
            "MULTIPLIER: x" + multiplier + "\n" +
            "SCORE: " + score;

        if (score >= pointsToWin)
        {
            CompleteLevel();
        }
    }

    void CompleteLevel()
    {
        isFinished = true;
        Debug.Log("Goal Reached! Loading Credits...");
        SceneManager.LoadScene(winSceneName);
    }

    public void AddScore(int points)
    {
        int multiplier = Mathf.Max(1, cachedBoxCount * 2);
        score += points * multiplier;
    }

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
}