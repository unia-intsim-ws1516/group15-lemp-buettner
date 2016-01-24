Shader "Hidden/CVDMachado"
{
	Properties
	{
		_MainTex ("Base (RGB)", 2D) = "white" {}
	}
	
	SubShader
	{
		Pass
		{
			CGPROGRAM
#include "UnityCG.cginc"

#pragma target 3.0
#pragma multi_compile CB_TYPE_ONE CB_TYPE_TWO
#pragma vertex vert_img
#pragma fragment frag
#pragma fragmentoption ARB_precision_hint_fastest
			
			uniform sampler2D _MainTex;
            float4x4 _CVD;
			float4 _MainTex_TexelSize;
			
			float4 frag(v2f_img i) : COLOR
			{
				// Direct3D9 needs texel offset
				float2 uv = i.uv;
#ifdef UNITY_HALF_TEXEL_OFFSET
				uv.y += _MainTex_TexelSize.y;
#endif
				//Original Color
				float4 origColor = tex2D(_MainTex, uv);

				float4 retColor;
				retColor = mul(_CVD, origColor);
                
                retColor = saturate(retColor);
				
				return retColor;
			}
			ENDCG
		}
	}
	FallBack off
}
