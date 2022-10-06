using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private Transform sideMovementRoot;
    [SerializeField] private Transform leftLimit, rightLimit;

    [SerializeField] private float playerSpeed, sideMovementSensitivity, sideMovementLerpSpeed;

    [SerializeField] private Animator characterAnimator;

    [SerializeField] private Transform groundCheck;
    [SerializeField] private LayerMask floorLayerMask;

    [SerializeField] private LayerMask finishLayerMask;

    [SerializeField] private LayerMask detachStickLayerMask;
    [SerializeField] private GameObject stick;
    [SerializeField] private float throwSpeed;

    private Vector2 inputDrag;

    private Vector2 previousMousePosition;

    private float leftLimitX => leftLimit.localPosition.x;

    private float rightLimitX => rightLimit.localPosition.x;

    private float sideMovementTarget;

    private Vector2 mousePositionCM
    {
        get
        {
            Vector2 pixels = Input.mousePosition;
            var inches = pixels / Screen.dpi;
            var centimeters = inches * 2.54f;

            return centimeters;
        }
    }

    void Update()
    {
        HandleForwardMovement();
        HandleInput();
        HandleSideMovement();
        GroundCheck();
    }

    private void HandleForwardMovement()
    {
        if (isGroundedFinish() == false)
        {
            transform.position += transform.forward * Time.deltaTime * playerSpeed;
        }
    }

    private void HandleInput()
    {
        if (Input.GetMouseButtonDown(0))
        {
            previousMousePosition = mousePositionCM;
        }

        if (Input.GetMouseButton(0))
        {
            var deltaMouse = mousePositionCM - previousMousePosition;
            inputDrag = deltaMouse;
            previousMousePosition = mousePositionCM;
        }
        else
        {
            inputDrag = Vector2.zero;
        }
    }

    private void HandleSideMovement()
    {
        sideMovementTarget += inputDrag.x * sideMovementSensitivity;
        sideMovementTarget = Mathf.Clamp(sideMovementTarget, leftLimitX, rightLimitX);

        var localPosition = sideMovementRoot.localPosition;

        localPosition.x = Mathf.Lerp(localPosition.x, sideMovementTarget, Time.deltaTime * sideMovementLerpSpeed);

        sideMovementRoot.localPosition = localPosition;
    }

    private void GroundCheck()
    {
        if (isGrounded() == true)
        {
            characterAnimator.SetBool("isGrounded", true);
        }
        else
        {
            characterAnimator.SetBool("isGrounded", false);
        }

        if(isGroundedFinish() == true)
        {
            characterAnimator.SetBool("isGameFinish", true);
            GameManager.Instance.OnGameFinished();
        }

        if(isGroundedDetachStickWall() == true)
        {
            stick.transform.parent = null;
            stick.AddComponent<Rigidbody>().AddForce(0f, 0f, throwSpeed);
        }
    }

    private bool isGrounded()
    {
        return Physics.CheckSphere(groundCheck.position, 0.1f, floorLayerMask);
    }

    private bool isGroundedFinish()
    {
        return Physics.CheckSphere(groundCheck.position, 0.1f, finishLayerMask);
    }

    private bool isGroundedDetachStickWall()
    {
        return Physics.CheckSphere(groundCheck.position, 0.1f, detachStickLayerMask);
    }
}
