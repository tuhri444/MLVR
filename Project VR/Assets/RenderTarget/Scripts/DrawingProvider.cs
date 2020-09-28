using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;

public class DrawingProvider : MonoBehaviour
{
    public List<XRController> controllers = null;
    public GameObject brush;
    public RasterizeImage cam;
    public float spacing;
    public GameObject drawingPrefab;


    private bool canDraw = false;
    private bool drawingMode = false;

    private GameObject drawing;

    void Update()
    {
        CheckForInput();
    }
    private void CheckForInput()
    {
        foreach (XRController controller in controllers)
        {
            if (controller != null && controller.enableInputActions && controller.controllerNode == XRNode.LeftHand)
            {
                CheckToDraw(controller.inputDevice, controller.transform.position);
            }
            if (controller != null && controller.enableInputActions && controller.controllerNode == XRNode.RightHand)
            {
                CheckForDrawingActivation(controller.inputDevice);
            }
        }
    }
    private void CheckForDrawingActivation(InputDevice device)
    {
        if(device.TryGetFeatureValue(CommonUsages.triggerButton, out bool value))
        {
            if (drawingMode && !value)
            { 
                cam.SaveImage();
                Destroy(drawing);
                canDraw = false;
                Debug.Log("Stopping Drawing mode");
            }
            else if (!drawingMode && value)
            {
                Debug.Log("Start Drawing mode");
                drawing = Instantiate(drawingPrefab,transform.position,Quaternion.identity);
                drawing.transform.parent = transform.parent;
                canDraw = true;
            }

            drawingMode = value;
            if (value) Debug.Log("Drawing Mode on");
        }
    }
    private void CheckToDraw(InputDevice device, Vector3 pos)
    {
        if (device.TryGetFeatureValue(CommonUsages.triggerButton, out bool value))
        {
            Draw(value, pos);
        }
    }
    private void Draw(bool drawSomething, Vector3 pos)
    {
        if(drawSomething && canDraw)
        {
            Debug.Log("Drawing");
            GameObject temp = Instantiate(brush, pos, Quaternion.identity);
            temp.transform.parent = drawing.transform;
            temp.layer = 14;
            canDraw = false;
            StartCoroutine(DrawResetTimer(spacing));
        }
    }
    private IEnumerator DrawResetTimer(float timeToWait)
    {
        float timePassed = 0.0f;

        while(timePassed < timeToWait)
        {
            timePassed += Time.deltaTime;
            yield return null;
        }
        canDraw = true;
    }
}
