Shader "Custom/TextureSwitch"
{
	Properties
	{
		_Dist("Distance", float) = 5.0
		_PosTex("Position Texture", 2D) = "white" {}
		_MainTex("Texture", 2D) = "white" {}
		_SecondaryTex("Secondary texture", 2D) = "white"{}
		_NPoints("NPoints", int) = 1
		_Cutoff("Alpha cutoff", Range(0,1)) = 0.5
	}
		SubShader
	{
		Tags { "Queue" = "AlphaTest"  "RenderType" = "TransparentCutout" }

		Pass
		{
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#include "UnityCG.cginc"

			struct v2f {
				float4 pos : SV_POSITION;
				float2 uv : TEXCOORD0;
				float4 worldPos : TEXCOORD1;
				float4 col : COLOR;
			};

			v2f vert(appdata_base v)
			{
				v2f o;
				// We compute the world position to use it in the fragment function
				o.worldPos = mul(unity_ObjectToWorld, v.vertex);
				o.pos = UnityObjectToClipPos(v.vertex);
				o.uv = v.texcoord;
				return o;
			}
			sampler2D _MainTex;
			sampler2D _SecondaryTex;
			sampler2D _PosTex;
			float _Dist;
			float _NPoints;
			float _Cutoff;

			fixed4 frag(v2f i) : SV_Target
			{

				float4 myPixelColor;
				fixed4 col = tex2D(_SecondaryTex, i.uv);
				clip(col.a - _Cutoff);

				if (_NPoints <= 0)
					return col;

				[loop]
				for (int x = 0; x < _NPoints; x++)
				{
					float2 pixelPos = float2(x, 1.0f);
					
					myPixelColor = tex2D(_PosTex, float2(x/_NPoints, 1.0f) );
					if(distance(myPixelColor * 255, i.worldPos.xyz) < _Dist)
					{
						col = tex2D(_MainTex, i.uv);
						return col;
					}
				}

				clip(col.a - _Cutoff);
				return col;
			}

			ENDCG
		}
	}
}//