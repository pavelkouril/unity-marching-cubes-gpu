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
            for (var z = 0; z < Resolution; ++z)
            {
                for (var y = 0; y < Resolution; ++y)
                {
                    for (var x = 0; x < Resolution; ++x, ++idx)
                    {
                        var amount = Mathf.Pow(x - Resolution / 2, 2) + Mathf.Pow(y - Resolution / 2, 2) + Mathf.Pow(z - Resolution / 2, 2)
                            <= Mathf.Pow((Resolution - 2) / 2 * Mathf.Sin(0.25f * Time.time), 2) ? 1 : 0;
                        colors[idx] = new Color(amount, 0, 0);
                    }
                }
            }
            densityTexture.SetPixels(colors);
            densityTexture.Apply();

            mc.DensityTexture = densityTexture;
        }
    }
}