using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

// Manages UI elements, game state, and interface interactions
public class UI : MonoBehaviour
{
    public static UI instance;                         // Singleton instance for global access

    [SerializeField] private TextMeshProUGUI scoreText;    // Displays player's current score
    [SerializeField] private TextMeshProUGUI timerText;    // Displays elapsed game time
    [SerializeField] private TextMeshProUGUI ammoText;     // Displays current/maximum ammo count

    private int scoreValue;                            // Tracks the player's current score

    [SerializeField] private GameObject tryAgainButton;    // Button displayed at game over

    [Header("Reload details")]
    [SerializeField] private BoxCollider2D reloadWindow;   // Area where reload buttons can spawn
    [SerializeField] private GunController gunController;  // Reference to player's gun controller
    [SerializeField] private int reloadSteps;              // Number of buttons needed to reload
    [SerializeField] private UI_ReloadButton[] reloadButtons;  // Collection of reload buttons

    private void Awake()
    {
        // Set up singleton instance
        instance = this;
    }

    void Start()
    {
        // Find all reload buttons in the hierarchy
        reloadButtons = GetComponentsInChildren<UI_ReloadButton>(true);
    }

    void Update()
    {
        // Update timer display every frame after initial startup
        if (Time.time >= 1)
            timerText.text = Time.time.ToString("#,#");

        // Open reload UI when R key is pressed
        if (Input.GetKeyDown(KeyCode.R))
            OpenReloadUI();
    }

    public void OpenReloadUI()
    {
        // Activate and position each reload button randomly within bounds
        foreach (UI_ReloadButton button in reloadButtons)
        {
            button.gameObject.SetActive(true);

            // Generate random position within reload window
            float randomX = Random.Range(reloadWindow.bounds.min.x, reloadWindow.bounds.max.x);
            float randomY = Random.Range(reloadWindow.bounds.min.y, reloadWindow.bounds.max.y);

            // Position button at random location
            button.transform.position = new Vector2(randomX, randomY);
        }

        // Slow down game time during reload
        Time.timeScale = .4f;
        // Set number of buttons that need to be clicked
        reloadSteps = reloadButtons.Length;
    }

    public void AttemptToReload()
    {
        // Decrease remaining reload steps when button is clicked
        if (reloadSteps > 0)
            reloadSteps--;

        // Reload gun if all buttons have been clicked
        if (reloadSteps <= 0)
            gunController.ReloadGun();
    }

    public void AddScore()
    {
        // Increment score and update display
        scoreValue++;
        scoreText.text = scoreValue.ToString("#,#");
    }

    public void UpdateAmmoInfo(int currentBullets, int maxBullets)
    {
        // Update ammunition display with current/max format
        ammoText.text = currentBullets + "/" + maxBullets;
    }

    public void OpenEndScreen()
    {
        // Stop game time and show restart button
        Time.timeScale = 0;
        tryAgainButton.SetActive(true);
    }

    public void RestartGame()
    {
        // Reset time scale and reload the first scene
        Time.timeScale = 1;
        SceneManager.LoadScene(0);
    }
}
