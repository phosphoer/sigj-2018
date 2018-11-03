Shader "Custom/Opaque/PastelEmissive" {
	Properties {
		_Color ("Color", Color) = (1,1,1,1)
		_Lighting ("Light Ramp", 2D) = "grey" {}
		_ShadowColor ("Shadow Color", Color) = (0,0,0,1)
	}
	SubShader {
		Tags { "RenderType"="Opaque" }
		LOD 200
		
		CGPROGRAM
		#pragma surface surf Pastel

		sampler2D _MainTex;

		struct Input {
			float2 uv_MainTex;
		};

		fixed4 _Color;
		fixed4 _ShadowColor;

		struct SurfaceOutCustom{
			fixed3 Albedo;
			fixed3 Normal;
			fixed3 Emission;
			fixed Alpha;
			fixed2 UVs;
		};

		void surf (Input IN, inout SurfaceOutCustom o) {
			o.Albedo = _Color.rgb;

			o.UVs = IN.uv_MainTex;
		}

		sampler2D _Lighting;

		fixed4  LightingPastel (SurfaceOutCustom s, fixed3 lightDir, fixed atten){
			half NdotL = dot(s.Normal, lightDir);

			NdotL = tex2D(_Lighting, fixed2((s.UVs.x+s.UVs.y)/2,NdotL));

			fixed4 c = fixed4(0,0,0,0);
			c.rgb = (s.Albedo * _LightColor0.rgb * NdotL * atten * 2);

			fixed3 sdwCol = _LightColor0.rgb * (1-NdotL) * (1-atten) * 2 * _ShadowColor;//(1-c) * _ShadowColor;
			c.rgb += sdwCol;

			return c;
		}
		ENDCG
	}
	FallBack "Diffuse"
}
