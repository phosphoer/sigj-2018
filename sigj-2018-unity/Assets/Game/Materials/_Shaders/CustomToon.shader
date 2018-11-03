Shader "Custom/Opaque/CustomToon" {
	Properties {
		_Color ("Color", Color) = (1,1,1,1)
		_MainTex ("Albedo (RGB)", 2D) = "white" {}
		_Lighting ("Light Ramp", 2D) = "grey" {}

		_FresColor ("Fresnel Color", Color) = (1,1,1,1)
		_FresRamp ("Fresel Ramp", 2D) = "grey" {}
		_FresStrength ("Fresnel Strength", Range(0,1)) = .5
		_FresPow ("Fresnel Power", Range(0,20)) = 3
	}
	SubShader {
		Tags { "RenderType"="Opaque" }
		LOD 200
		
		CGPROGRAM
		#pragma surface surf Toon

		sampler2D _MainTex;
		sampler2D _FresRamp;

		struct Input {
			float2 uv_MainTex;
			float3 viewDir;
		};

		fixed4 _Color;
		fixed4 _FresColor;
		half _FresStrength;
		half _FresPow;

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

			float edgeIn = ((IN.viewDir.x)+(IN.viewDir.y))/2;
			half NdotL = 1-saturate(dot(normalize(IN.viewDir), o.Normal));
			NdotL *= tex2D(_FresRamp, fixed2(edgeIn, NdotL));
			o.Emission = pow(NdotL,(1/_FresPow))*_FresStrength*_FresColor;
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
