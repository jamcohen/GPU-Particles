using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwappableBuffer {
    public RenderTexture Input;
    public RenderTexture Output;

    public SwappableBuffer(int width, int height, int depth)
    {
        Input = new RenderTexture(width, height, depth, RenderTextureFormat.ARGBFloat);
        Output = new RenderTexture(width, height, depth, RenderTextureFormat.ARGBFloat);

        Input.useMipMap = false;
        Input.filterMode = FilterMode.Point;
        Output.useMipMap = false;
        Output.filterMode = FilterMode.Point;
    }

    public void Swap()
    {
        var tmp = Input;
        Input = Output;
        Output = tmp;
    }
}
