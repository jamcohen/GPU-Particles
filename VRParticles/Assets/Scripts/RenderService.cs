using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RenderService : Service<RenderService> {
    public int Resolution;

    public SwappableBuffer Velocity;
    public SwappableBuffer Position;

    public CameraController MainCam;

    public Material SimulateMat;
    public Material ParticleMat;

    public ParticleMesh Mesh;

    protected override void InitService()
    {
        Velocity = new SwappableBuffer(Resolution, Resolution, 0);
        Position = new SwappableBuffer(Resolution, Resolution, 0);

        Mesh.Resolution = Resolution;
        Mesh.Generate();
        var noise = Mesh.GenerateNoise();
        SimulateMat.SetTexture("_Noise", noise);
        SimulateMat.SetFloat("_PositionScale", 10.0f);
        Simulate(noise);
        SimulateMat.SetFloat("_PositionScale", 1.0f);
    }

    //simulates a particle step
    private void Update()
    {
        Simulate();
    }

    //Can override position to randomize starting pos
    private void Simulate(Texture overridePosition=null)
    {
        var positionIn = (overridePosition == null) ? Position.Input : overridePosition;

        SimulateMat.SetTexture("_VelocityIn", Velocity.Input);
        SimulateMat.SetTexture("_PositionIn", positionIn);
        SimulateMat.SetFloat("_DeltaTime", Time.deltaTime);
        SafeBlit(SimulateMat, Velocity.Output.depthBuffer, Velocity.Output.colorBuffer, Position.Output.colorBuffer);
        ParticleMat.SetTexture("_Position", Position.Output);
        Velocity.Swap();
        Position.Swap();
    }

    private void SafeBlit(Material mat, RenderBuffer depthBuffer, params RenderBuffer[] destinations)
    {
        // we have to perform our own custom blit on post render, otherwise we trash the render states
        Graphics.SetRenderTarget(destinations, depthBuffer);

        // draw a fullscreen quad
        GL.PushMatrix();
        GL.LoadOrtho();
        for (int pass = 0; pass < mat.passCount; pass++)
        {
            mat.SetPass(pass);
            GL.Begin(GL.QUADS);
            GL.TexCoord2(0, 0);
            GL.Vertex3(0, 0, 0);
            GL.TexCoord2(0, 1);
            GL.Vertex3(0, 1, 0);
            GL.TexCoord2(1, 1);
            GL.Vertex3(1, 1, 0);
            GL.TexCoord2(1, 0);
            GL.Vertex3(1, 0, 0);
            GL.End();
        }
        GL.PopMatrix();
    }


}
