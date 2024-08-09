// Upgrade NOTE: replaced '_Object2World' with 'unity_ObjectToWorld'

Shader "Custom/Scene-WarFog"
{
    Properties
    { 
		_MainTex("Texture", 2D) = "white" {}
		_Color("Tint", Color) = (1,1,1,1)
    }

    SubShader
    {
		Tags
		{
			"Queue" = "Transparent"
			"IgnoreProjector" = "True"
			"RenderType" = "Transparent"
		}


        Cull Off ZWrite Off ZTest Always
		Blend SrcAlpha OneMinusSrcAlpha

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
				float4 wp : TEXCOORD1;
            };

            v2f vert (appdata v)
            {
                v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.wp = mul(unity_ObjectToWorld, v.vertex);
                o.uv = v.uv;
                return o;
            }

			sampler2D _MainTex;
			fixed4 _Color;

			fixed4 frag(v2f IN) : SV_Target
			{ 
				fixed4 texColor = tex2D(_MainTex, IN.uv);
				return _Color * (1.0 - texColor.a);
			}

            ENDCG
        }
    }
}
