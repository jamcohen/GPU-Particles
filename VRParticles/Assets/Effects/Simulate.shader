Shader "Cohen/Simulate"
{
	Properties
	{
		_PositionIn ("Position", 2D) = "white" {}
		_VelocityIn ("Velocity", 2D) = "white" {}
		_Noise("Noise", 2D) = "white" {}
		_AttractPos ("Attract Position", Vector) = (1,1,1,1)
		_AttractSpeed ("Attract Speed", Float) = 1
		_DeltaTime ("Delta Time", Float) = 0
	}
	SubShader
	{
		Tags { "RenderType"="Opaque" }
		LOD 100

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

			struct PixelOutput
			{
				float4 pos : COLOR1;
				float4 vel : COLOR0;
			};

			sampler2D_float _PositionIn;
			float4 _PositionIn_ST;
			sampler2D_float _VelocityIn;
			sampler2D _Noise;
			float4 _AttractPos;
			float _AttractSpeed;
			float _DeltaTime;

			v2f vert (appdata v)
			{
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.uv = TRANSFORM_TEX(v.uv, _PositionIn);
				return o;
			}
			
			PixelOutput frag(v2f i){
				float4 posIn = tex2D(_PositionIn, i.uv);
				float4 velIn = tex2D(_VelocityIn, i.uv);
				float4 noise = tex2D(_Noise, i.uv);

				float rand = (noise.r + noise.g + noise.b) * 0.33333;
				float4 attractDir = (_AttractPos - posIn) * rand * _AttractSpeed;

				float4 vel = velIn + attractDir * _DeltaTime;

				PixelOutput o;
				o.vel = vel;
				o.pos = posIn + vel *_DeltaTime;
				return o;
			}
			ENDCG
		}
	}
}
