using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;

public class DrawingProvider : MonoBehaviour
{
    public List<XRController> controllers = null;
    public RasterizeImage cam;
    public float spacing;
    public GameObject drawingPrefab;

    private bool canDraw = false;
    private bool drawingMode = false;

    public GameObject camObject;
    private GameObject drawing;
    private LineRenderer line;
    private int numberOfPoints = 0;

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
                cam.SaveImage(drawing);                
                canDraw = false;
                numberOfPoints = 0;
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
        if (device.TryGetFeatureValue(CommonUsages.triggerButton, out bool value) && drawingMode)
        {
            Draw(value, pos);
        }
    }
    private void Draw(bool drawSomething, Vector3 pos)
    {
        if(drawSomething && canDraw)
        {
            Debug.Log("Drawing");
            Vector3 tempPos = pos;
            LineRenderer line = drawing.GetComponent<LineRenderer>();
            line.positionCount++;
            line.SetPosition(numberOfPoints, tempPos);
            canDraw = false;
            StartCoroutine(DrawResetTimer(spacing));
            numberOfPoints++;
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
