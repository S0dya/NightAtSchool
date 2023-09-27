using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : SingletonMonobehaviour<Player>
{
    [Header("Settings")]
    public float movementSpeed;
    public float rotationSpeed;
    public float gravityForce;

    [Header("SerializeFields")]
    [SerializeField] CharacterController controller;
    [SerializeField] Camera playerCamera;
    [SerializeField] FixedJoystick movemenetJoystick;
    [SerializeField] FloatingJoystick POVJoystick;
    [SerializeField] Animator animator;
    [SerializeField] BoxCollider raycastCollider;


    //local
    float verticalAngle;
    Vector3 characterVelocity;
    Vector3 gravityVelocity;

    //cors
    Coroutine movementCor;
    Coroutine povCor;

    protected override void Awake()
    {
        base.Awake();

        
    }

    //Input
    void Update()
    {
        if (!controller.isGrounded)
        {
            gravityVelocity.y -= gravityForce * Time.deltaTime;
        }
        else
        {
            gravityVelocity.y = 0;
        }

        controller.Move((characterVelocity * movementSpeed + gravityVelocity) * Time.deltaTime);
    }

    public void StartMoving()
    {
        animator.Play("Walking");
        movementCor = StartCoroutine(MovementCor());
    }
    public void StopMoving()
    {
        animator.Play("IDLE");
        StopCoroutine(movementCor);
        characterVelocity = Vector3.zero;
    }

    public void StartLooking()
    {
        povCor = StartCoroutine(POVCor());
    }
    public void StopLooking()
    {
        StopCoroutine(povCor);
    }

    IEnumerator MovementCor()
    {
        while (true)
        {
            Vector3 localDirection = transform.forward * movemenetJoystick.Direction.y + transform.right * movemenetJoystick.Direction.x;
            characterVelocity = new Vector3(localDirection.x, 0f, localDirection.z);

            yield return null;
        }
    }

    IEnumerator POVCor()
    {
        while (true)
        {
            //x
            transform.Rotate(new Vector3(0f, (POVJoystick.Direction.x * rotationSpeed), 0f), Space.Self);
            //y
            verticalAngle += -POVJoystick.Direction.y * rotationSpeed;
            verticalAngle = Mathf.Clamp(verticalAngle, -89f, 89f);
            playerCamera.transform.localEulerAngles = new Vector3(verticalAngle, 0, 0);
            yield return null;
        }
    }

    //sound
    public void PlayStepSound()
    {
        if (controller.isGrounded)
        {
            //PlaySound;
        }
    }

    //hide interaction
    public void ToggleHide(bool val)
    {
        controller.enabled = !val;
        movemenetJoystick.gameObject.SetActive(!val);
        raycastCollider.enabled = !val;
    }

    //gameover
    public void Die()
    {
        GameMenuUI.I.GameOver();
    }


}
