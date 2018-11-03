Shader "Custom/Opaque/CustomLightDiff" {
	Properties {
		_Color ("Color", Color) = (1,1,1,1)
		_MainTex ("Albedo (RGB)", 2D) = "white" {}
		_Lighting ("Light Ramp", 2D) = "grey" {}
	}
	SubShader {
		Tags { "RenderType"="Opaque" }
		LOD 200
		
		CGPROGRAM
		#pragma surface surf Toon

		sampler2D _MainTex;

		struct Input {
			float2 uv_MainTex;
		};

		fixed4 _Color;

		struct SurfaceOutCustom{
			fixed3 Albedo;
			fixed3 Normal;
			fixed3 Emission;
			fixed Alpha;
			fixed2 UVs;
		};

		void surf (Input IN, inout SurfaceOutCustom o) {
			fixed4 c = tex2D (_MainTex, IN.uv_MainTex) * _Color;
			o.Albedo = c.rgb;
			o.UVs = IN.uv_MainTex;
		}

		sampler2D _Lighting;

		fixed4  LightingToon (SurfaceOutCustom s, fixed3 lightDir, fixed atten){
			half NdotL = dot(s.Normal, lightDir);

			NdotL = tex2D(_Lighting, fixed2((s.UVs.x+s.UVs.y)/2,NdotL));

			fixed4 c = fixed4(0,0,0,0);
			c.rgb = s.Albedo * _LightColor0.rgb * NdotL * atten * 2;

			return c;
		}
		ENDCG
	}
	FallBack "Diffuse"
}
