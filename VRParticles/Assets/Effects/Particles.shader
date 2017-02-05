Shader "Trigger/Particles"
{
	Properties
	{
		_MainTex ("Texture", 2D) = "white" {}
		_Position("Position", 2D) = "white" {}
		_Velocity("Velocity", 2D) = "white" {}
		_Resolution("Resolution", Float) = 0
	}

	SubShader
	{
		Tags { "RenderType"="Transparent" "Queue"="Transparent"}
		Blend One One
		LOD 100
		ZWrite On
		ZTest LEqual

		Pass
		{
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			
			#include "UnityCG.cginc"

			struct appdata
			{
				float4 vertex : POSITION;
				float2 uv : TEXCOORD0;
			};

			struct v2f
			{
				float2 uv : TEXCOORD0;
				float4 vertex : SV_POSITION;
			};

			sampler2D _MainTex;
			sampler2D_float _Position;
			sampler2D_float _Velocity;
			float4 _MainTex_ST;
			float _Resolution;
			
			v2f vert (appdata v)
			{
				v2f o;
				float x = frac(v.uv.x * _Resolution);
				float y = floor(v.uv.x * _Resolution) / _Resolution;
				float2 particleCoord = float2(x, y);
				float3 pos = tex2Dlod(_Position, float4(particleCoord, 0, 0)).xyz;
				v.vertex.xyz += pos;
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.uv = v.uv;
				return o;
			}
			
			fixed4 frag (v2f i) : SV_Target
			{
				// sample the texture
				fixed4 col = fixed4(0.005,0.01,0.002,0.1);
				return col;
			}
			ENDCG
		}
	}
}
