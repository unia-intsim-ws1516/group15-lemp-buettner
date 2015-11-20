Shader "Hidden/GULTI/CVDSimulator"
{
	Properties
	{
		_MainTex ("Base (RGB)", 2D) = "white" {}
		_BlindIntensity ("Color-Blind Intensity", Range(0,1)) = 1
	}
	
	SubShader
	{
		Pass
		{
			CGPROGRAM
#include "UnityCG.cginc"

#pragma multi_compile CB_TYPE_ONE CB_TYPE_TWO
#pragma target 3.0
#pragma vertex vert_img
#pragma fragment frag
#pragma fragmentoption ARB_precision_hint_fastest
			
			uniform sampler2D _MainTex;
			uniform fixed _BlindIntensity;
			float4 _MainTex_TexelSize;
			
			fixed4 frag(v2f_img i) : COLOR
			{
				// Direct3D9 needs texel offset
				float2 uv = i.uv;
#ifdef UNITY_HALF_TEXEL_OFFSET
				uv.y += _MainTex_TexelSize.y;
#endif
				//Original Color
				fixed4 origColor = tex2D(_MainTex, uv);
				
				// no color vision deficiency is basically the identity
				fixed3x3 noCVD = fixed3x3
				(
				1.0, 0.0, 0.0,
				0.0, 1.0, 0.0,
				0.0, 0.0, 1.0
				);
				fixed3x3 severe = noCVD;
#if CB_TYPE_ONE
				severe = fixed3x3
				(
				 0.152286,  1.052583,  -0.204868,
				 0.114503,  0.786281,   0.099216,
				-0.003882, -0.048116,	1.051998
				);
#else
				severe = noCVD;
#endif

				fixed4 retColor;
				retColor.rgb = mul(severe, origColor.rgb);
				retColor.a = origColor.a;
				
				return retColor;
			}
			ENDCG
		}
	}
	FallBack off
}
