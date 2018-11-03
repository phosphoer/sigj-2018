Shader "Custom/Unlit"
{
	Properties
	{
    _Color ("Color", Color) = (1,1,1,1)
    _MainTex ("Texture", 2D) = "white" {}

		[Enum(Off,0,On,1)] 
		_ZWrite ("ZWrite", Float) = 1
		
		[Enum(Always, 0, Less, 2, Equal, 3, LEqual, 4, GEqual, 5)] 
		_ZTest ("ZTest", Float) = 4
	}
	SubShader
	{
		Tags { "RenderType"="Opaque" }
		ZWrite [_ZWrite]
		ZTest [_ZTest]
		
		Pass
		{
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag

			#include "UnityCG.cginc"

			struct appdata
			{
				float4 vertex : POSITION;
				fixed4 color : COLOR;
        float2 uv : TEXCOORD0;
			};

			struct v2f
			{
				float4 vertex : SV_POSITION;
				fixed4 color : COLOR;
        float2 uv : TEXCOORD0;
			};

      sampler2D _MainTex;
      float4 _Color;

			v2f vert (appdata v)
			{
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);
        o.uv = v.uv;
				o.color = v.color;
				return o;
			}

			fixed4 frag (v2f i) : SV_Target
			{
        return _Color * tex2D(_MainTex, i.uv) * i.color;
			}
			ENDCG
		}
	}

  FallBack "VertexLit"
}
