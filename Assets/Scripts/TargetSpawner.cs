using UnityEngine;

// Generates target objects at timed intervals with increasing difficulty
public class TargetSpawner : MonoBehaviour
{
    [SerializeField] private Sprite[] targetSprite;    // Collection of different target visuals

    [SerializeField] private BoxCollider2D cd;         // Area where targets can spawn horizontally
    [SerializeField] private GameObject targetPrefab;  // Template for creating new targets
    [SerializeField] private float cooldown;           // Time between target spawns
    private float timer;                               // Current countdown until next spawn

    private int sushiCreated;                          // Counter for total targets spawned
    private int sushiMilestone = 25;                   // Threshold for increasing difficulty

    void Update()
    {
        // Decrease spawn timer each frame
        timer -= Time.deltaTime;

        if (timer < 0)
        {
            // Reset timer and increment target counter
            timer = cooldown;
            sushiCreated++;

            // Increase game difficulty by reducing spawn time after reaching milestones
            if (sushiCreated > sushiMilestone && cooldown > .5f)
            {
                sushiMilestone += 10;
                cooldown -= .3f;
            }


            // Create a new target instance
            GameObject newTarget = Instantiate(targetPrefab);

            // Calculate random horizontal position within bounds
            float randomX = Random.Range(cd.bounds.min.x, cd.bounds.max.x);

            // Position target at spawn point with random horizontal offset
            newTarget.transform.position = new Vector2(randomX, transform.position.y);

            // Assign random sprite from available options
            int randomIndex = Random.Range(0, targetSprite.Length);
            newTarget.GetComponent<SpriteRenderer>().sprite = targetSprite[randomIndex];

        }
    }
}
