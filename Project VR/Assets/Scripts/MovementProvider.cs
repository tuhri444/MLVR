using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;

public class MovementProvider : LocomotionProvider
{
    public float speed = 1.0f;
    public float gravity = 1.0f;
    public List<XRController> controllers = null;

    public float jumpSpeed;
    private float vspeed = 0.0f;
    public CharacterController characterController = null;
    private GameObject head = null;

    protected override void Awake()
    {
        //characterController = GetComponent<CharacterController>();
        head = system.xrRig.cameraGameObject;
    }
    void Start()
    {
        PositionController();
    }

    void FixedUpdate()
    {
        PositionController();
        CheckForInput();
        ApplyGravity();
    }

    private void PositionController()
    {
        float headHeight = Mathf.Clamp(head.transform.localPosition.y, 0.5f, 2.0f);
        characterController.height = headHeight;

        Vector3 newCenter = Vector3.zero;
        newCenter.y = characterController.height * 0.5f;
        newCenter.y += characterController.skinWidth;

        newCenter.x = head.transform.localPosition.x;
        newCenter.z = head.transform.localPosition.z;

        characterController.center = newCenter;
    }

    private void CheckForInput()
    {
        foreach(XRController controller in controllers)
        {
            if(controller != null && controller.enableInputActions && controller.controllerNode == XRNode.LeftHand)
            {
                CheckForMovement(controller.inputDevice);
            }
            if(controller != null && controller.enableInputActions && controller.controllerNode == XRNode.RightHand)
            {
                //CheckForJump(controller.inputDevice);
            }
        }
    }

    private void CheckForJump(InputDevice device)
    {
        if(device.TryGetFeatureValue(CommonUsages.triggerButton, out bool value) && characterController.isGrounded)
        {
            Debug.Log(value);
            Jump(value);
        }
    }

    private void Jump(bool jump)
    {
        if(jump)
        {
            Debug.Log("jumping now");
            Vector3 vel = characterController.velocity;
            vel.y += jumpSpeed * Time.fixedDeltaTime;
            characterController.Move(vel * Time.fixedDeltaTime);
        }
    }

    private void CheckForMovement(InputDevice device)
    {
        if (device.TryGetFeatureValue(CommonUsages.primary2DAxis, out Vector2 position))
            if (device.TryGetFeatureValue(CommonUsages.primary2DAxisClick, out bool value))
                StartMove(position, value);
            else StartMove(position, false);

    }

    private void StartMove(Vector2 position, bool sprint)
    {
        Vector3 dir = new Vector3(position.x, 0, position.y);
        Vector3 headRot = new Vector3(0, head.transform.eulerAngles.y, 0);

        dir = Quaternion.Euler(headRot) * dir;

        float spd = 0.0f;
        if (sprint) spd = speed * 2;
        else spd = speed;
        Vector3 movement = dir * spd;
        characterController.Move(movement * Time.fixedDeltaTime);
    }

    private void ApplyGravity()
    {
        Vector3 grav = new Vector3(0, Physics.gravity.y * gravity, 0);
        grav.y *= Time.fixedDeltaTime;
        if(characterController.isGrounded)
        {
            vspeed = 0;
        }
        Vector3 vel = characterController.velocity;
        vspeed -= gravity * Time.fixedDeltaTime;
        vel.y = vspeed;
        characterController.Move(vel * Time.fixedDeltaTime);
    }
}
