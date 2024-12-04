using System.Collections;
using System.Linq;
using UnityEngine;

public class MagnetEffect : MonoBehaviour
{
    public float magnetDuration = 3f; // Duration of the magnet effect
    public float magnetInterval = 5f; // Interval between magnet activations
    public float magnetSpeed = 15f; // Speed at which pacdots are attracted

    private bool isMagnetActive = false;
    private GameObject[] pacdots;

    void Start()
    {
        // Start the magnet effect routine
        StartCoroutine(MagnetRoutine());
    }


    IEnumerator MagnetRoutine()
    {
        while (true)
        {
            // Wait for the specified interval
            yield return new WaitForSeconds(magnetInterval);

            Debug.Log("Magnet Activated");

            // Activate magnet effect
            ActivateMagnet();

            // Wait for the duration of the magnet effect
            yield return new WaitForSeconds(magnetDuration);

            // Deactivate magnet effect
            DeactivateMagnet();
        }
    }

    void ActivateMagnet()
    {
        isMagnetActive = true;

        // Find all remaining pacdots in the scene
        pacdots = GameObject.FindGameObjectsWithTag("pacdot");

        if (pacdots.Length == 0)
        {
            Debug.Log("No pacdots found to attract!");
            return;
        }

        Debug.Log($"Found {pacdots.Length} pacdots. Activating magnet effect.");

        // Shuffle the array to randomize selection
        pacdots = pacdots.OrderBy(x => Random.value).ToArray();

        // Select half of the pacdots
        int pacdotsToAttract = Mathf.CeilToInt(pacdots.Length / 2f);

        for (int i = 0; i < pacdotsToAttract; i++)
        {
            // Start moving each selected pacdot toward Pac-Man
            StartCoroutine(MovePacdotToPacman(pacdots[i]));
        }
    }

    void DeactivateMagnet()
    {
        isMagnetActive = false;
        Debug.Log("Magnet Deactivated");
    }

    IEnumerator MovePacdotToPacman(GameObject pacdot)
    {
        if (pacdot == null)
        {
            yield break;
        }

        while (isMagnetActive && pacdot != null)
        {
            // Move the pacdot toward Pac-Man's position
            pacdot.transform.position = Vector3.MoveTowards(
                pacdot.transform.position,
                transform.position, // Pac-Man's position
                magnetSpeed * Time.deltaTime
            );

            // Check if the pacdot is close enough to be "collected"
            if (Vector3.Distance(pacdot.transform.position, transform.position) < 0.5f)
            {
                Debug.Log($"Pacdot {pacdot.name} collected!");
                Destroy(pacdot); // Simulate collection
                yield break; // Stop moving this pacdot
            }

            yield return null;
        }
    }
}
