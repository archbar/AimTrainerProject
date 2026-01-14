using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FPSCameraController : MonoBehaviour
{
    #region Fields
    // Sensitivity controls the speed of camera rotation
    private float sensitivity = 2.0f; // Default sensitivity

    // Limit the vertical rotation angle of the camera
    public float verticalRotationLimit = 80.0f;

    // Store the rotation around the X (vertical) axis
    private float verticalRotation = 0.0f;
    // Store the rotation around the Y (horizontal) axis
    private float horizontalRotation = 0.0f;

    // Extra Rotations
    private Vector3 currentRotation;
    private Vector3 targetRotation;

    // Hipfire Recoil
    [SerializeField] private float recoilX;
    [SerializeField] private float recoilY;
    [SerializeField] private float recoilZ;

    // ADS Recoil
    [SerializeField] private float aimRecoilX;
    [SerializeField] private float aimRecoilY;
    [SerializeField] private float aimRecoilZ;

    // Settings
    [SerializeField] private float snappiness;
    [SerializeField] private float returnSpeed;

    #endregion

    #region UnityMethods
    // Start is called before the first frame update
    void Start()
    {
        // Check if the current scene is the MainGame scene
        if (SceneManager.GetActiveScene().name == "mainGame")
        {
            // Hide the cursor and lock its position
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
    }

    // Update is called once per frame
    void Update()
    {
        // Check if the current scene is the MainGame scene
        if (SceneManager.GetActiveScene().name != "mainGame")
        {
            // If not in MainGame scene, show the cursor and unlock it
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            return;
        }

        // Get the mouse input for rotation
        float mouseX = Input.GetAxis("Mouse X");
        float mouseY = Input.GetAxis("Mouse Y");

        // Calculate the new vertical rotation angle based on mouse input and sensitivity
        verticalRotation -= mouseY * sensitivity;
        // Calculate the new horizontal rotation angle based on mouse input and sensitivity
        horizontalRotation += mouseX * sensitivity;

        // Limit the vertical rotation to avoid camera flipping
        verticalRotation = Mathf.Clamp(verticalRotation, -verticalRotationLimit, verticalRotationLimit);

        // Apply the vertical rotation to the camera
        Vector3 totalRot = new Vector3(verticalRotation, horizontalRotation, 0);

        targetRotation = Vector3.Lerp(targetRotation, Vector3.zero, returnSpeed * Time.deltaTime);
        currentRotation = Vector3.Slerp(currentRotation, targetRotation, snappiness * Time.fixedDeltaTime);

        transform.localRotation = Quaternion.Euler(currentRotation + totalRot);
    }
    #endregion

    #region NonUnityMethods
    public void RecoilFire(bool ads)
    {
        if (ads) targetRotation += new Vector3(aimRecoilX, Random.Range(-aimRecoilY, aimRecoilY), Random.Range(-aimRecoilZ, aimRecoilZ));
        else targetRotation += new Vector3(recoilX, Random.Range(-recoilY, recoilY), Random.Range(-recoilZ, recoilZ));
    }

    public void SetRecoilValues(float t_returnSpeed, float t_snappiness, float t_recoilX, float t_recoilY, float t_recoilZ, float t_aimRecoilX, float t_aimRecoilY, float t_aimRecoilZ)
    {
        recoilX = t_recoilX;
        recoilY = t_recoilY;
        recoilZ = t_recoilZ;
        aimRecoilX = t_aimRecoilX;
        aimRecoilY = t_aimRecoilY;
        aimRecoilZ = t_aimRecoilZ;
        returnSpeed = t_returnSpeed;
        snappiness = t_snappiness;
    }


    #endregion
}
