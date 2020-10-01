using System.Collections;
using System.Collections.Generic;
using System.IO;
using PDollarDemo;
using PDollarGestureRecognizer;
using UnityEngine;
using UnityEngine.UI;

public class GestureAnalyzer : MonoBehaviour
{
    [SerializeField]
    [Tooltip("Margin of error concerning how sensitive the gesture recognition is.")]
    private float m_MarginOfError;

    [Header("Optional:")]
    [SerializeField] 
    [Tooltip("UI text element that will display the results of the gesture analysis.")]
    private Text m_UIText;

    private Gesture[] m_TrainingSet;   // training set loaded from XML files
    void Start()
    {
        m_TrainingSet = LoadTrainingSet();
    }
    public static List<Point> ConvertStrokeToPointCloud(GameObject drawing, Camera cam)
    {
        List<Point> output = new List<Point>();
        LineRenderer[] strokes = drawing.GetComponentsInChildren<LineRenderer>();
        for (int strokeIndex = 0; strokeIndex < strokes.Length; strokeIndex++)
        {
            LineRenderer stroke = strokes[strokeIndex];
            for (int i = 0; i < stroke.positionCount; i++)
            {
                Vector2 point = cam.WorldToScreenPoint(stroke.GetPosition(i));
                output.Add(new Point(point.x,point.y,strokeIndex));
            }
        }
        return output;
    }
    public void RecognizeGesture(List<Point> points)
    {
        if (points.Count > 0)
        {
            Gesture candidate = new Gesture(points.ToArray());
            string gestureClass = PointCloudRecognizerPlus.Classify(candidate, m_TrainingSet, m_MarginOfError);
            Debug.Log("Recognized as: " + gestureClass);
            if (m_UIText != null)
                m_UIText.text = "Recognized as: " + gestureClass;
        }
    }
    /// <summary>
    /// Loads training gesture samples from XML files
    /// </summary>
    /// <returns></returns>
    private Gesture[] LoadTrainingSet()
    {
        List<Gesture> gestures = new List<Gesture>();
        string[] gestureFolders = Directory.GetDirectories(Application.dataPath + "/RenderTarget/Scripts/Gesture/GestureSet");
        foreach (string folder in gestureFolders)
        {
            string[] gestureFiles = Directory.GetFiles(folder, "*.xml");
            foreach (string file in gestureFiles)
                gestures.Add(GestureIO.ReadGesture(file));
        }
        return gestures.ToArray();
    }
}
