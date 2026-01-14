using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TaskStarter : MonoBehaviour
{
    void OnCollisionStay(Collision collision) 
    {
        GameManager.Instance.taskPlaying = true;
        Destroy(gameObject);
    }
}
