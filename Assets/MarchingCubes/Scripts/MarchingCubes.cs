using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PavelKouril.MarchingCubesGPU
{
    public class MarchingCubes : MonoBehaviour
    {
        public int Resolution;
        public Material mat;
        public ComputeShader MarchingCubesCS;

        public Texture3D DensityTexture { get; set; }

        private int kernelMC;

        private ComputeBuffer appendVertexBuffer;
        private ComputeBuffer argBuffer;

        private void Awake()
        {
            kernelMC = MarchingCubesCS.FindKernel("MarchingCubes");
        }

        private void Start()
        {
            appendVertexBuffer = new ComputeBuffer(Resolution * Resolution * Resolution, sizeof(float) * 18, ComputeBufferType.Append);
            argBuffer = new ComputeBuffer(4, sizeof(int), ComputeBufferType.IndirectArguments);

            MarchingCubesCS.SetInt("_gridSize", Resolution);
            MarchingCubesCS.SetFloat("_isoLevel", 0.5f);
        }

        private void Update()
        {
            MarchingCubesCS.SetTexture(kernelMC, "_densityTexture", DensityTexture);
            appendVertexBuffer.SetCounterValue(0);

            MarchingCubesCS.SetBuffer(kernelMC, "triangleRW", appendVertexBuffer);
            MarchingCubesCS.Dispatch(kernelMC, Resolution / 8, Resolution / 8, Resolution / 8);

            int[] args = new int[] { 0, 1, 0, 0 };
            argBuffer.SetData(args);

            ComputeBuffer.CopyCount(appendVertexBuffer, argBuffer, 0);

            argBuffer.GetData(args);
            args[0] *= 3;
            argBuffer.SetData(args);

            Debug.Log("Vertex count:" + args[0]);
        }

        private void OnRenderObject()
        {
            mat.SetPass(0);
            mat.SetBuffer("triangles", appendVertexBuffer);
            mat.SetMatrix("model", transform.localToWorldMatrix);
            Graphics.DrawProceduralIndirect(MeshTopology.Triangles, argBuffer);
        }

        private void OnDestroy()
        {
            appendVertexBuffer.Release();
            argBuffer.Release();
        }
    }
}