// Upgrade NOTE: replaced '_Object2World' with 'unity_ObjectToWorld'

Shader "Custom/Scene-AreaMask"
{
    Properties
    { 
		_MainTex("Texture", 2D) = "white" {}
		_Color("Tint", Color) = (1,1,1,1)
		_EdgeBlurLength("EdgeBlur", Float) = 5.0
		_Area00("Area00", Vector) = (0, 0, 0, 0)
		_Area01("Area01", Vector) = (0, 0, 0, 0)
		_Area02("Area02", Vector) = (0, 0, 0, 0)
		_Area03("Area03", Vector) = (0, 0, 0, 0)
		_Area04("Area04", Vector) = (0, 0, 0, 0)
		_Area05("Area05", Vector) = (0, 0, 0, 0)
		_Area06("Area06", Vector) = (0, 0, 0, 0)
		_Area07("Area07", Vector) = (0, 0, 0, 0)
		_Area08("Area08", Vector) = (0, 0, 0, 0)
		_Area09("Area09", Vector) = (0, 0, 0, 0)
		_Area10("Area10", Vector) = (0, 0, 0, 0)
		_Area11("Area11", Vector) = (0, 0, 0, 0)
		_Area12("Area12", Vector) = (0, 0, 0, 0)
		_Area13("Area13", Vector) = (0, 0, 0, 0)
		_Area14("Area14", Vector) = (0, 0, 0, 0)
		_Area15("Area15", Vector) = (0, 0, 0, 0)
		_UvSpeedX("uv X Speed", Float) = 0
		_UvSpeedY("uv Y Speed", Float) = 0
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
			float _EdgeBlurLength;

			fixed4 _Area00;
			fixed4 _Area01;
			fixed4 _Area02;
			fixed4 _Area03;
			fixed4 _Area04;
			fixed4 _Area05;
			fixed4 _Area06;
			fixed4 _Area07;
			fixed4 _Area08;
			fixed4 _Area09;
			fixed4 _Area10;
			fixed4 _Area11;
			fixed4 _Area12;
			fixed4 _Area13;
			fixed4 _Area14;
			fixed4 _Area15;

			fixed _UvSpeedX;
			fixed _UvSpeedY;

			float circleAlpha(float4 cp, float3 vp)
			{
				float d = distance(cp.xyz, vp);
				float r = cp.w;
				float b = r + _EdgeBlurLength;
				float a = max(r - _EdgeBlurLength, 0);
				float c = smoothstep(b, a, d) * step(0.001, r);
				return c;
			}

			fixed4 frag(v2f IN) : SV_Target
			{ 
				float uvOffX = _UvSpeedX * _Time.y;
				float uvOffY = _UvSpeedY * _Time.y;
				fixed4 texColor = tex2D(_MainTex, IN.uv + float2(uvOffX, uvOffY));
				float3 wp = IN.wp.xyz;
				float alpha = circleAlpha(_Area00, wp);
				alpha += circleAlpha(_Area01, wp);
				alpha += circleAlpha(_Area02, wp);
				alpha += circleAlpha(_Area03, wp);
				alpha += circleAlpha(_Area04, wp);
				alpha += circleAlpha(_Area05, wp);
				alpha += circleAlpha(_Area06, wp);
				alpha += circleAlpha(_Area07, wp);
				alpha += circleAlpha(_Area08, wp);
				alpha += circleAlpha(_Area09, wp);
				alpha += circleAlpha(_Area10, wp);
				alpha += circleAlpha(_Area11, wp);
				alpha += circleAlpha(_Area12, wp);
				alpha += circleAlpha(_Area13, wp);
				alpha += circleAlpha(_Area14, wp);
				alpha += circleAlpha(_Area15, wp);
				fixed4 color = _Color * (1.0 - saturate(alpha));
				return color * texColor;
			}

            ENDCG
        }
    }
}
