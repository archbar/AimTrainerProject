using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Gunsway : MonoBehaviour
{
    #region Fields
    [SerializeField] private float intensity;
    [SerializeField] private float smooth;

    private Quaternion originalRotation;
    #endregion
    #region UnityMethods
    private void Start()
    {
        originalRotation = transform.localRotation;
    }
    private void Update()
    {
        UpdateSway();
    }
    #endregion
    #region Methods
    private void UpdateSway()
    {
        //controls
        float t_x_mouse = Input.GetAxis("Mouse X");
        float t_y_mouse = Input.GetAxis("Mouse Y");

        //calculate target rotation
        Quaternion t_x_adj = Quaternion.AngleAxis(intensity * t_x_mouse, Vector3.up);
        Quaternion t_y_adj = Quaternion.AngleAxis(-intensity * t_y_mouse, Vector3.right);
        Quaternion target_rotation = originalRotation * t_x_adj * t_y_adj;

        //rotate towards target rotation
        transform.localRotation = Quaternion.Lerp(transform.localRotation, target_rotation, Time.deltaTime * smooth);
    }
    #endregion
}

