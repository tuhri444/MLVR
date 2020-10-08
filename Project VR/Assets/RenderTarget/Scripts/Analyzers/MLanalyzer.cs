using System.CodeDom.Compiler;
using Unity.Barracuda;
using UnityEngine;
using UnityEngine.UI;
using VRG;

public class MLanalyzer : MonoBehaviour
{
    [SerializeField]
    [Tooltip("Machine learning model used to identify images.")]
    private NNModel m_ModelAsset;

    [Header("Optional:")]
    [SerializeField]
    [Tooltip("UI text element that will display the results of the gesture analysis.")]
    private Text m_UIText;

    private Model m_RuntimeModel;

    [SerializeField] string[] m_ClassNames;

    void Start()
    {
        m_RuntimeModel = ModelLoader.Load(m_ModelAsset);
        RasterizeImage.OnRasterize += AnalyzeImage;
    }
    private void AnalyzeImage(Texture2D texture)
    {
        bool verbose = false;

        var additionalOutputs = new string[] { "StatefulPartitionedCall/sequential/dense/softmax" };
        var engine = WorkerFactory.CreateWorker(m_RuntimeModel, additionalOutputs/*, WorkerFactory.Device.GPU, verbose*/);

        //texture = Resize(texture, 45, 45);

        //You can treat input pixels as 1 (grayscale), 3 (color) or 4 (color with alpha) channels
        var channelCount = 1;
        var input = new Tensor(texture, channelCount);

        engine.Execute(input);
        var prediction = engine.PeekOutput("Identity");
        float[] predictionValues = prediction.AsFloats();

        int temp = 0;
        Debug.Log("----------------------");
        foreach (float f in predictionValues)
        {
            Debug.Log(m_ClassNames[temp] +": " + f*100.0f);
            temp++;
        }
        Debug.Log("----------------------");

        float highestValue = 0;
        int index = 0;
        int highestValueIndex = 0;
        foreach (float i in predictionValues)
        {
            if (i > highestValue)
            {
                highestValue = i;
                highestValueIndex = index;
            }
            index += 1;
        }

        if (highestValue > 0.75f)
            m_UIText.text = "Recognized: " + m_ClassNames[highestValueIndex];
        else
            m_UIText.text = "Unable to Recognize";
    }
    public static Texture2D Resize(Texture2D source, int newWidth, int newHeight)
    {
        source.filterMode = FilterMode.Point;
        RenderTexture rt = RenderTexture.GetTemporary(newWidth, newHeight);
        rt.filterMode = FilterMode.Point;
        RenderTexture.active = rt;
        Graphics.Blit(source, rt);
        Texture2D nTex = new Texture2D(newWidth, newHeight);
        nTex.ReadPixels(new Rect(0, 0, newWidth, newHeight), 0, 0);
        nTex.Apply();
        RenderTexture.active = null;
        RenderTexture.ReleaseTemporary(rt);
        return nTex;
    }
}
