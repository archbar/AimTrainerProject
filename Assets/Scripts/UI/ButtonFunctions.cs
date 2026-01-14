using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class ButtonFunctions : MonoBehaviour
{
    bool difficultyWasSet = false, taskTypeWasSet = false;
    [SerializeField] GameObject crosshair1, crosshair2, crosshair3, crosshair4;

    // Variable to store the selected crosshair temporarily
    private GameObject selectedCrosshair;

    public void LoadTaskScene()
    {
        if (difficultyWasSet && taskTypeWasSet)
        {
            SceneManager.LoadScene(1);
        }
    }

    public void BasicButtonClickEvent()
    {
        GameManager.Instance.taskType = 0;
        taskTypeWasSet = true;
    }

    public void MovingButtonClickEvent()
    {
        GameManager.Instance.taskType = 1;
        taskTypeWasSet = true;
    }

    public void MovingPlusButtonClickEvent()
    {
        GameManager.Instance.taskType = 2;
        taskTypeWasSet = true;
    }

    public void EasyButtonClickEvent()
    {
        GameManager.Instance.targetSizeMultiplier = 1;
        difficultyWasSet = true;
    }

    public void NormalButtonClickEvent()
    {
        GameManager.Instance.targetSizeMultiplier = 0.8f;
        difficultyWasSet = true;
    }

    public void HardButtonClickEvent()
    {
        GameManager.Instance.targetSizeMultiplier = 0.6f;
        difficultyWasSet = true;
    }

    public void Crosshair1Changer()
    {
        selectedCrosshair = crosshair1;
        Debug.Log("Selected Crosshair: Crosshair1");
    }

    public void Crosshair2Changer()
    {
        selectedCrosshair = crosshair2;
        Debug.Log("Selected Crosshair: Crosshair2");
    }

    public void Crosshair3Changer()
    {
        selectedCrosshair = crosshair3;
        Debug.Log("Selected Crosshair: Crosshair3");
    }

    public void Crosshair4Changer()
    {
        selectedCrosshair = crosshair4;
        Debug.Log("Selected Crosshair: Crosshair4");
    }

    public void ApplyCrosshair()
    {
        if (selectedCrosshair != null)
        {
            GameManager.Instance.crosshair = selectedCrosshair;
            Debug.Log("Crosshair Applied: " + selectedCrosshair.name);
        }
        else
        {
            Debug.LogError("No crosshair selected. Please select a crosshair before applying.");
        }
    }
}
