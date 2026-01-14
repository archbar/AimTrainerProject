using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using Microsoft.Unity.VisualStudio.Editor;

// Class to manage the game state
public class GameManager : MonoBehaviour
{
    #region Fields
    // Singleton
    private static GameManager instance = null;
    private TargetManager targetManager;
    // Canvas reference
    private Canvas canvasReference;
    
    // Task values
    public int taskType = 0;
    public float targetSizeMultiplier = 0;
    // Is the task playing?
    public bool taskPlaying;

    // Score
    private TextMeshProUGUI scoreText;
    private TextMeshProUGUI accuracyText;
    public float scoreValue, accuracyValue, avgTimeBetweenShots;
    public int noOfShotsFired, noOfShotsMissed, noOfShotsHit;

    //Timer
    public TextMeshProUGUI timerText;
    public float timerValue;
    
    //Crosshair
    public GameObject crosshair;
    private RectTransform crosshairRectTransform;


    #endregion
    #region Properties
    public static GameManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<GameManager>();
            }
            return instance;
        }
    }
    public Canvas CanvasReference()
    {
        return canvasReference;
    }
    public Transform CanvasTransform => canvasReference != null ? canvasReference.transform : null;
    #endregion
    #region Methods
    #region UnityMethods
    void Awake()
    {
        Initialise();
        SceneManager.sceneLoaded += OnSceneLoaded;
    }
    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        switch (SceneManager.GetActiveScene().buildIndex)
        {
            case 0:
                accuracyValue = 0; noOfShotsHit = 0; noOfShotsFired = 0; noOfShotsMissed = 0; avgTimeBetweenShots = 0;
                break;
            case 1:
                taskPlaying = false;
                targetManager = FindObjectOfType<TargetManager>();
                canvasReference = FindObjectOfType<Canvas>();
                Transform hudTransform = canvasReference.transform.Find("HUD");

                if (hudTransform != null)
                {

                    GameObject instantiatedCrosshair = Instantiate(GameManager.Instance.crosshair, Vector3.zero, Quaternion.identity);
                    Transform parentTransform = canvasReference.transform.GetChild(0);

                    // Set parent
                    instantiatedCrosshair.transform.SetParent(parentTransform);

                    // Set local position to (0, 0, 0) relative to its parent
                    instantiatedCrosshair.transform.localPosition = Vector3.zero;

                    // Move it to the last sibling in the hierarchy
                    instantiatedCrosshair.transform.SetAsLastSibling();


                    scoreText = hudTransform.GetChild(0).GetComponent<TextMeshProUGUI>();
                    timerText = hudTransform.GetChild(1).GetComponent<TextMeshProUGUI>();
                    accuracyText = hudTransform.GetChild(2).GetComponent<TextMeshProUGUI>();

                    scoreText = hudTransform.GetChild(0).GetComponent<TextMeshProUGUI>();
                    timerText = hudTransform.GetChild(1).GetComponent<TextMeshProUGUI>();
                    accuracyText = hudTransform.GetChild(2).GetComponent<TextMeshProUGUI>();
                }
                else
                {
                    Debug.LogError("HUD object not found under Canvas.");
                }
                break; 
            case 2:
                canvasReference = FindObjectOfType<Canvas>();
                canvasReference.transform.GetChild(0).GetChild(2).GetComponent<TextMeshProUGUI>().text = "Accuracy: " + accuracyValue.ToString("F2") + "%";
                canvasReference.transform.GetChild(0).GetChild(3).GetComponent<TextMeshProUGUI>().text = "Shots Fired: " + noOfShotsFired.ToString();
                canvasReference.transform.GetChild(0).GetChild(4).GetComponent<TextMeshProUGUI>().text = "Shots Hit: " + noOfShotsHit.ToString();
                canvasReference.transform.GetChild(0).GetChild(5).GetComponent<TextMeshProUGUI>().text = "Shots Missed: " + noOfShotsMissed.ToString();
                canvasReference.transform.GetChild(0).GetChild(6).GetComponent<TextMeshProUGUI>().text = "Average Time Between Accurate Shots: " + avgTimeBetweenShots.ToString("F2") + "s";
                canvasReference.transform.GetChild(0).GetChild(7).GetComponent<TextMeshProUGUI>().text = "Score: " + scoreValue.ToString("F2");
                taskPlaying = false;
            
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
                break;
            default: 
                break;
        }
    }
    #endregion
    #region NonUnityMethods
    private void Initialise()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this);
        } else
        {
            Destroy(this);
        }
    }
    public void UpdateScore() 
    {
        scoreText.text = "Score: " + scoreValue.ToString();
        accuracyValue = (noOfShotsFired > 0) ? ((float)noOfShotsHit / noOfShotsFired) * 100f : 0f;
        accuracyText.text = "Accuracy: " + accuracyValue.ToString("F2") + "%";
    }
    public void UpdateTimer()
    {
        // Decrease the timer over time
        timerValue -= Time.deltaTime;

        // Clamp the timer to never go below 0
        timerValue = Mathf.Max(timerValue, 0f);

        // Update the UI text
        timerText.text = "Time: " + timerValue.ToString("F2");

        // Check if the timer has reached 0
        if (timerValue <= 0)
        {
            HandleTimeUp();
        }
    }
    private void HandleTimeUp()
    {
        SceneManager.LoadScene("gameOver");
        avgTimeBetweenShots = Mathf.Round((60f / noOfShotsHit) * Mathf.Pow(10f, 5)) / Mathf.Pow(10f, 5);
        
    }
    public void ResetGame()
    {
        FPSGunController.Instance.ResetGunController();

    }
    #endregion
    #endregion
}
