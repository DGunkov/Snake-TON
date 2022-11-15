using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MiniMap : MonoBehaviour
{
    // [SerializeField] private Camera _mapCamera;
    // [SerializeField] private Image _map;
    // private int height = 1024;
    // private int width = 1024;
    // private int depth = 24;

    // private void Awake()
    // {
    //     _mapCamera = GameObject.FindGameObjectWithTag("MapCam").gameObject.GetComponent<Camera>();
    //     // StartCoroutine(UpdateMap());
    // // }

    // // private IEnumerator UpdateMap()
    // // {
    // //     while (true)
    // //     {
    // //         _map.sprite = CaptureScreen();
    // //         yield return new WaitForSeconds(2.5f);
    // //     }
    // // }

    // private Sprite CaptureScreen()
    // {
    //     RenderTexture renderTexture = new RenderTexture(width, height, depth);
    //     Rect rect = new Rect(0, 0, width, height);
    //     Texture2D texture = new Texture2D(width, height, TextureFormat.RGBA32, false);

    //     _mapCamera.targetTexture = renderTexture;
    //     _mapCamera.Render();

    //     RenderTexture currentRenderTexture = RenderTexture.active;
    //     RenderTexture.active = renderTexture;
    //     texture.ReadPixels(rect, 0, 0);
    //     texture.Apply();

    //     _mapCamera.targetTexture = null;
    //     RenderTexture.active = currentRenderTexture;
    //     Destroy(renderTexture);

    //     Sprite sprite = Sprite.Create(texture, rect, Vector2.zero);

    //     return sprite;
    // }
}
