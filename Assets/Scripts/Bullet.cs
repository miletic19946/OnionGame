using UnityEngine;

// Controls bullet behavior, including movement and collision responses.
public class Bullet : MonoBehaviour
{
    // Use expression-bodied property to get Rigidbody2D component
    private Rigidbody2D rb => GetComponent<Rigidbody2D>();

    // Reference to dust effect prefab that appears on impact
    [SerializeField] private GameObject dustPrefab;

    // Update is called once per frame. Aligns bullet direction with its velocity.
    void Update() => transform.right = rb.linearVelocity;

    // Handles collision with other 2D triggers.
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Target")
        {
            // Create dust effect at the collision point
            GameObject newDust = Instantiate(dustPrefab, collision.transform.position, Quaternion.identity);
            // Rotate dust effect randomly for visual variation
            newDust.transform.Rotate(new Vector3(0, 0, Random.Range(0, 360)));
            // Destroy dust effect after 3 seconds
            Destroy(newDust, 3);

            // Destroy the hit target
            Destroy(collision.gameObject);
            // Destroy this bullet
            Destroy(gameObject);

            // Increase player's score
            UI.instance.AddScore();
        }
    }
}
