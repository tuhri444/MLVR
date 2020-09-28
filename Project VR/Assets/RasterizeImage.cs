using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.Rendering;

public class RasterizeImage : MonoBehaviour
{
    private Texture2D targetTexture2D = new Texture2D(200, 200);
    private RenderTexture renderTarget;
    private Camera selfCamera;

    private bool saveImage;
    void Awake()
    {
        HandleTargetRender();
        selfCamera = GetComponent<Camera>();

        //Setting up render texture
        renderTarget = new RenderTexture(targetTexture2D.width, targetTexture2D.height, 0);
        RenderTexture.active = renderTarget;
        Graphics.Blit(targetTexture2D, renderTarget);

        selfCamera.targetTexture = renderTarget;
    }

    void OnEnable()
    {
        RenderPipelineManager.endCameraRendering += RenderPipelineManager_endCameraRendering;
    }
    void OnDisable()
    {
        RenderPipelineManager.endCameraRendering -= RenderPipelineManager_endCameraRendering;
    }
    private void RenderPipelineManager_endCameraRendering(ScriptableRenderContext context, Camera camera)
    {
        OnPostRender();
    }

    private void OnPostRender()
    {
        if (saveImage)
        {
            saveImage = false;
            SaveTargetImage();
        }
    }




    /// <summary>
    /// Tries to look a texture2D asset to set as target. 
    /// If it can't find anything, it will create one and use that.
    /// </summary>
    private void HandleTargetRender()
    {
        try
        {
            byte[] bytes = File.ReadAllBytes(Application.dataPath + "/RenderTarget/target.png");

            if (targetTexture2D == null)
                targetTexture2D = new Texture2D(200, 200);

            ImageConversion.LoadImage(targetTexture2D, bytes);
            if (targetTexture2D == null) throw new System.ArgumentException("Did not find a target image, will now create one");
            return;
        }
        catch (Exception e)
        {
            Texture2D temp = new Texture2D(200, 200, TextureFormat.ARGB32, true);

            if (!Directory.Exists(Application.dataPath + "/RenderTarget/"))
                Directory.CreateDirectory(Application.dataPath + "/RenderTarget/");

            byte[] bytes = temp.EncodeToPNG();
            File.WriteAllBytes(Application.dataPath + "/RenderTarget/target.png", bytes);
            targetTexture2D = temp;
            UnityEditor.AssetDatabase.Refresh();
            Debug.Log("Succesfully made the target image file, have fun with it!");
        }
    }

    /// <summary>
    /// Will save all the pixels from the render target onto a texture2D (png file (inside of the asset/rendertarget folder))
    /// </summary>
    private void SaveTargetImage()
    {
        targetTexture2D.ReadPixels(new Rect(0, 0, renderTarget.width, renderTarget.height), 0, 0, true);
        byte[] bytes = targetTexture2D.EncodeToPNG();
        File.WriteAllBytes(Application.dataPath + "/RenderTarget/target.png", bytes);
        UnityEditor.AssetDatabase.Refresh();
    }

    public void SaveImage()
    {
        saveImage = true;
    }
}
