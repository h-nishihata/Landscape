using UnityEngine;
using System.Collections;

public class WireFrame : MonoBehaviour {
    Camera wireFrameCam;

    void Awake()
    {
        wireFrameCam = gameObject.GetComponent<Camera>();
        // Only render objects in the first layer (Default layer)
        wireFrameCam.cullingMask = 1 << 0;
    }

    void OnPreRender() {
        GL.wireframe = true;
    }

    void OnPostRender() {
        GL.wireframe = false;
    }
}