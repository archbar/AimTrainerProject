using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthComponent : MonoBehaviour
{
    // Needs to have TakeDamage called from something else that raycasts it
    
    #region Fields
    // Reference to the health bar prefab
    [SerializeField] GameObject healthBarPrefab;

    // Reference to the instantiated health bar and its filled part (Image)
    private GameObject healthBar;
    private Image filledPart;
    private MeshRenderer meshRenderer;

    // Health-related variables
    [SerializeField] private float maxHealth = 1f;
    [SerializeField] private float currentHealth = 1f;

    // How long we want the health bar to be visible for
    float healthBarVisibleTime = 2f;

    // Variables for scaling the health bar based on distance to the player
    float distanceFromPlayer;
    float scaleFactor;

    // Set + Get
    public float GetHealth() { return currentHealth; }
    public float GetMaxHealth() { return maxHealth; }
    public void SetHealth(float health) { currentHealth = health; }
    #endregion
    #region Methods
    #region UnityMethods
    void Start()
    {
        // Instantiate the health bar prefab as a child of the Canvas
        healthBar = Instantiate(healthBarPrefab, GameManager.Instance.CanvasTransform);
        
        // Get the filled part (Image) component from the health bar
        filledPart = healthBar.GetComponentInChildren<Image>();

        meshRenderer = GetComponent<MeshRenderer>();

        // Ensure we can't see the health bar until the target is damaged
        DisableHealthBar();
    }
    
    void Update()
    {
        // Manage the health bar appearance and behavior
        ManageHealthBar();
    }
    
    void OnDestroy()
    {
        // Destroy the health bar when the GameObject is destroyed
        if (healthBar != null) Destroy(healthBar.gameObject);
    }
    #endregion
    #region NonUnityMethods
    // Method to apply damage to the health
    public void TakeDamage(float damage)
    {
        // Decrease the current health and reset the health bar visibility timer
        currentHealth -= damage;
        
        if (currentHealth > 0) healthBar.SetActive(true);
        else healthBar.SetActive(false);
        if (healthBar.activeSelf) Invoke("DisableHealthBar", healthBarVisibleTime);
    }

    // Method to manage the appearance and behavior of the health bar
    private void ManageHealthBar()
    {
        if(meshRenderer.enabled == false) 
        {
            healthBar.SetActive(false);
            return;
        }
        // Show health fraction on the health bar
        ShowHealthFraction(currentHealth / maxHealth);
        
        // Scaling: Adjust health bar size based on the distance from the player
        distanceFromPlayer = Vector3.Distance(Camera.main.transform.position, transform.position);
        scaleFactor = Mathf.Clamp(1.0f / distanceFromPlayer, 0.75f, 1.0f);
        healthBar.transform.localScale = new Vector3(scaleFactor, scaleFactor, 1.0f);

        // Positioning: Set health bar position slightly below the GameObject
        healthBar.transform.position = Camera.main.WorldToScreenPoint(transform.position - new Vector3(0, .8f, 0));
    }

    public void DisableHealthBar() 
    {
        healthBar.SetActive(false);
    }

    // Method to show the health fraction on the health bar
    private void ShowHealthFraction(float fraction)
    {
        filledPart.rectTransform.localScale = new Vector3(fraction, 1, 1);
    }
    #endregion
    #endregion
}
