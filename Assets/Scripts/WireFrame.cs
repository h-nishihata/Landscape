using UnityEngine;
using System.Collections;

public class WireFrame : MonoBehaviour {
    void OnPreRender() {
        GL.wireframe = true;
    }
    void OnPostRender() {
        GL.wireframe = false;
    }
}