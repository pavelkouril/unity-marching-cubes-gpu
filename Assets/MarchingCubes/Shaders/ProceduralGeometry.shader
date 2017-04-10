Shader "PavelKouril/Marching Cubes/Procedural Geometry"
{
	SubShader
	{
		Cull Back

		Pass
		{
			CGPROGRAM
			#pragma target 5.0
			#pragma vertex vert
			#pragma fragment frag

			#include "UnityCG.cginc"

			struct Vertex
			{
				float3 vPosition;
				float3 vNormal;
			};

			struct Triangle
			{
				Vertex v[3];
			};

			uniform StructuredBuffer<Triangle> triangles;
			uniform float4x4 model;

			struct v2f
			{
				float4 vertex : SV_POSITION;
				float3 normal : NORMAL;
			};

			v2f vert(uint id : SV_VertexID)
			{
				uint pid = id / 3;
				uint vid = id % 3;

				v2f o;
				o.vertex = mul(UNITY_MATRIX_VP, mul(model, float4(triangles[pid].v[vid].vPosition, 1)));
				o.normal = mul(unity_ObjectToWorld, triangles[pid].v[vid].vNormal);
				return o;
			}

			float4 frag(v2f i) : SV_Target
			{
				float d = max(dot(normalize(_WorldSpaceLightPos0.xyz), i.normal), 0);
				return float4(d,d,d, 1);
			}
			ENDCG
		}
	}
}
