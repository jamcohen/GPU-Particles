using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class CameraController : MonoBehaviour {
    public event Action OnPostRender_evt = delegate { };

    private Camera _cam;
    public Camera Cam
    {
        get
        {
            if(_cam == null)
            {
                _cam = GetComponent<Camera>();
            }
            return _cam;
        }
    }

    public void OnPostRender()
    {
        OnPostRender_evt();
    }


}
