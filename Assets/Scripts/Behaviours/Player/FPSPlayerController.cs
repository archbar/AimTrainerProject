using System.Collections;
using System.Collections.Generic;
using UnityEngine;
// A class for controlling the player
public class FPSPlayerController : MonoBehaviour
{
    #region Fields
    [SerializeField] private LayerMask ground; // contains ground layer mask, currently layer 6
    [SerializeField] private Transform groundCheck; // At base of player by default
    [SerializeField] private float moveSpeed; // 4f by default
    [SerializeField] private float jumpHeight; //1.25f by default
    private float x; // takes x input
    private float z; // takes z input
    private float gravity = -9.81f; // is gravity
    private Vector3 moveDirectionHorizontal; // takes inputs for char controller method
    private Vector3 jumpDirection; // takes inputs for char controller method
    private bool isGrounded; // bool for if we're on the ground
    [SerializeField] private CharacterController charCont; // contains char controller on player
    #endregion
    #region Methods
    #region UnityMethods
    void Update()
    {
        HorizontalMovement();
        VerticalMovement();
    }
    #endregion
    #region NonUnityMethods
    // Enacts horizontal movement and checks for input
    void HorizontalMovement()
    {
        x = Input.GetAxis("Horizontal");
        z = Input.GetAxis("Vertical");
        moveDirectionHorizontal = transform.right * x + transform.forward * z;
        if (Input.GetKey(KeyCode.LeftShift)) charCont.Move(moveDirectionHorizontal * (moveSpeed * 2f) * Time.deltaTime);
        else charCont.Move(moveDirectionHorizontal * (moveSpeed * 2f) * Time.deltaTime);
    }
    // Enacts vertical movement and checks for input
    void VerticalMovement()
    {
        groundCheck.position = transform.position + new Vector3(0, -1.44f, 0);
        isGrounded = Physics.CheckSphere(groundCheck.position, 0.3f, ground);
        if (isGrounded && jumpDirection.y < 0)
        {
            jumpDirection.y=-10f;
        }
        jumpDirection.y += gravity * Time.deltaTime;
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            jumpDirection.y = Mathf.Sqrt(jumpHeight * -2 * gravity);
        }
        charCont.Move(jumpDirection * jumpHeight * Time.deltaTime);
    }
    #endregion
    #endregion
}