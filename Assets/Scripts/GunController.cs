using UnityEngine;
using UnityEngine.EventSystems;

// Controls weapon behavior, positioning, and ammo management
public class GunController : MonoBehaviour
{
    // References and configuration
    [SerializeField] private Animator gunAnim;         // Animation controller for gun visuals
    [SerializeField] private Transform gun;            // Transform of the gun object
    [SerializeField] private float gunDistance = 1.5f; // Distance gun appears from player

    private bool gunFacingRight = true;                // Tracks current gun orientation

    [Header("Bullet")]
    [SerializeField] private GameObject bulletPrefab;  // Prefab instantiated when shooting
    [SerializeField] private float bulletSpeed;        // Velocity of fired bullets
    [SerializeField] private int maxBullets = 15;      // Maximum ammo capacity
    [SerializeField] private int currentBullets;       // Current available ammo count

    private void Start()
    {
        // Initialize gun with full ammo
        ReloadGun();
    }

    void Update()
    {
        // Get mouse position in world space
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        // Calculate direction from player to mouse cursor
        Vector3 direction = mousePos - transform.position;

        // Rotate gun to point toward mouse position
        gun.rotation = Quaternion.Euler(new Vector3(0, 0, Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg));

        // Calculate angle for positioning gun around player
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        // Position gun at fixed distance from player in direction of mouse
        gun.position = transform.position + Quaternion.Euler(0, 0, angle) * new Vector3(gunDistance, 0, 0);

        // Fire bullet when left mouse button is clicked and ammo is available
        if (Input.GetKeyDown(KeyCode.Mouse0) && HaveBullets())
            Shoot(direction);

        //if (Input.GetKeyDown(KeyCode.R))
        //    ReloadGun();

        // Update gun orientation based on mouse position
        GunFlipController(mousePos);
    }

    private void GunFlipController(Vector3 mousePos)
    {
        // Flip gun sprite when mouse crosses to opposite side of gun
        if (mousePos.x < gun.position.x && gunFacingRight)
            GunFlip();
        else if (mousePos.x > gun.position.x && !gunFacingRight)
            GunFlip();
    }

    private void GunFlip()
    {
        // Toggle facing direction flag
        gunFacingRight = !gunFacingRight;
        // Flip gun sprite by inverting Y scale
        gun.localScale = new Vector3(gun.localScale.x, gun.localScale.y * -1, gun.localScale.z);
    }

    private void Shoot(Vector3 direction)
    {
        // Prevent shooting when cursor is over UI elements
        if (EventSystem.current.IsPointerOverGameObject())
            return;

        // Trigger gun firing animation
        gunAnim.SetTrigger("Shoot");

        // Create bullet instance at gun position
        GameObject newBullet = Instantiate(bulletPrefab, gun.position, Quaternion.identity);
        // Apply velocity to bullet in direction of mouse cursor
        newBullet.GetComponent<Rigidbody2D>().linearVelocity = direction.normalized * bulletSpeed;
        // Destroy bullet after 7 seconds to prevent clutter
        Destroy(newBullet, 7);

        // Update ammo display in UI
        UI.instance.UpdateAmmoInfo(currentBullets, maxBullets);
    }

    public void ReloadGun()
    {
        // Refill ammo to maximum capacity
        currentBullets = maxBullets;
        // Update ammo counter in UI
        UI.instance.UpdateAmmoInfo(currentBullets, maxBullets);
        // Restore normal game speed after reload mini-game
        Time.timeScale = 1;
    }

    private bool HaveBullets()
    {
        // Check if gun is out of ammo
        if (currentBullets <= 0)
        {
            return false;
        }

        // Decrement bullet count and confirm shot can be fired
        currentBullets--;
        return true;
    }
}
