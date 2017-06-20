using System;
using System.Collections.Generic;
using UnityEngine;

namespace PavelKouril.MarchingCubesGPU
{
    public class DensityFieldGenerator : MonoBehaviour
    {
        public int Resolution;

        private MarchingCubes mc;

        private Texture3D densityTexture;
        private Color[] colors;

        private void Awake()
        {
            mc = GetComponent<MarchingCubes>();
            densityTexture = new Texture3D(Resolution, Resolution, Resolution, TextureFormat.RFloat, false);
            densityTexture.wrapMode = TextureWrapMode.Clamp;
            colors = new Color[Resolution * Resolution * Resolution];

            for (int i = 0; i < colors.Length; i++) colors[i] = Color.white;
        }

        private void Start()
        {
            GenerateSoil();
        }

        private void Update()
        {
            GenerateSoil();
        }

        private void GenerateSoil()
        {
            var idx = 0;
            float sx, sy, sz;
            float resol = (Resolution - 2) / 2 * Mathf.Sin(0.25f * Time.time);
            for (var z = 0; z < Resolution; ++z)
            {
                for (var y = 0; y < Resolution; ++y)
                {
                    for (var x = 0; x < Resolution; ++x, ++idx)
                    {
                        sx = x - Resolution / 2;
                        sy = y - Resolution / 2;
                        sz = z - Resolution / 2;
                        var amount = (sx * sx + sy * sy + sz * sz) <= resol * resol ? 1 : 0;
                        colors[idx].r = amount;
                    }
                }
            }
            densityTexture.SetPixels(colors);
            densityTexture.Apply();

            mc.DensityTexture = densityTexture;
        }
    }
}