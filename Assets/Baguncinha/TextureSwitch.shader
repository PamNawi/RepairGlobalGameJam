Shader "Custom/TextureSwitch"
{
	Properties
	{
		_PosTex("Position Texture", 2D) = "white" {}
		_Dist("Distance", float) = 5.0
		_MainTex("Texture", 2D) = "white" {}
		_SecondayTex("Secondary texture", 2D) = "white"{}
		_NPoints("NPoints", int) = 1
	}
		SubShader
	{
		Tags { "RenderType" = "Opaque" }

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

			float4 _PlayerPos;
			sampler2D _MainTex;
			sampler2D _SecondayTex;
			sampler2D _PosTex;
			float _Dist;
			float _NPoints;

			fixed4 frag(v2f i) : SV_Target
			{

				float4 myPixelColor;
			[loop]
				for (int x = 0; x < _NPoints; x++)
				{
					float2 pixelPos = float2(x, 1);
					myPixelColor = tex2D(_PosTex, pixelPos / float2(_NPoints,1.0f));

					if (distance(myPixelColor.xyz, i.worldPos.xyz) > _Dist)
						return tex2D(_MainTex, i.uv) * myPixelColor;
				}
				return tex2D(_SecondayTex, i.uv);
			}

			ENDCG
		}
	}
}//