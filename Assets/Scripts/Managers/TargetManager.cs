using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetManager : MonoBehaviour
{
    // Class needs to instantiate targets and destroy targets as needed
    // Acts as object pooler
    #region Fields
    int noOfActiveTargets;

    float enableTimerValue;
    float startingEnableTimerValue;
    
    float timerValue;
    float startingTimerValue = 60f;
    
    [SerializeField] private int noOfTargets = 5;
    private GameObject[] targets = new GameObject[5];
    
    [SerializeField] private GameObject targetPrefab;
    [SerializeField] private GameObject movingTargetPrefab;
    [SerializeField] private GameObject complexTargetPrefab;
    

    #endregion
    #region Methods
    #region UnityMethods
    void Start()
    {
        timerValue = startingTimerValue;
        enableTimerValue = 5f;
        TaskInitialisation();
    }

    // Update is called once per frame
    void Update()
    {
        GameManager.Instance.timerValue = timerValue;
        GameManager.Instance.UpdateTimer(); 
        if (GameManager.Instance.taskPlaying)
        {
            TargetManagement();
        }
    }
    #endregion
    #region NonUnityMethods
    private void TaskInitialisation() 
    {
        switch (GameManager.Instance.taskType)
        {
            case 0:
                for (int i = 0; i < noOfTargets; i++)
                {
                    GameObject newTarget = Instantiate(targetPrefab, new Vector3(0, -10, 0), Quaternion.identity);
                    newTarget.SetActive(false);
                    targets[i] = newTarget;
                }
                break;
            case 1:
                for (int i = 0; i < noOfTargets; i++)
                {
                    GameObject newTarget = Instantiate(movingTargetPrefab, new Vector3(0, -10, 0), Quaternion.identity);
                    newTarget.SetActive(false);
                    targets[i] = newTarget;
                }
                break;
            case 2:
                for (int i = 0; i < noOfTargets; i++)
                {
                    GameObject newTarget = Instantiate(complexTargetPrefab, new Vector3(0, -10, 0), Quaternion.identity);
                    newTarget.SetActive(false);
                    targets[i] = newTarget;
                }
                break;
            default:
                Debug.Log("Something's wrong.");
                break;
        }
    }
    private void TargetManagement() 
    {
        timerValue -= Time.deltaTime;
        if (timerValue <= 0) 
        {
            for (int i = 0; i < targets.Length; i++)
            {
                targets[i].SetActive(false);
            }
            GameManager.Instance.taskPlaying = false;
            timerValue = startingTimerValue;
        }
        else
        {
            enableTimerValue += Time.deltaTime;
            if (enableTimerValue > 5f && noOfActiveTargets < targets.Length) 
            {
                targets[noOfActiveTargets].SetActive(true);
                enableTimerValue = startingEnableTimerValue;
                noOfActiveTargets++;
            }
        }
    }

    #endregion
    #endregion
}
