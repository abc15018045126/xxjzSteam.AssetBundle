// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

// Shader created with Shader Forge v1.35 
// Shader Forge (c) Neat Corporation / Joachim Holmer - http://www.acegikmo.com/shaderforge/
// Note: Manually altering this data may prevent you from opening it in Shader Forge
/*SF_DATA;ver:1.35;sub:START;pass:START;ps:flbk:,iptp:0,cusa:False,bamd:0,lico:0,lgpr:1,limd:0,spmd:1,trmd:0,grmd:0,uamb:True,mssp:True,bkdf:False,hqlp:False,rprd:False,enco:False,rmgx:True,imps:True,rpth:0,vtps:0,hqsc:True,nrmq:1,nrsp:0,vomd:0,spxs:False,tesm:0,olmd:1,culm:2,bsrc:3,bdst:7,dpts:2,wrdp:False,dith:0,atcv:False,rfrpo:True,rfrpn:Refraction,coma:15,ufog:False,aust:True,igpj:True,qofs:0,qpre:3,rntp:2,fgom:False,fgoc:False,fgod:False,fgor:False,fgmd:0,fgcr:0.5,fgcg:0.5,fgcb:0.5,fgca:1,fgde:0.01,fgrn:0,fgrf:300,stcl:False,stva:128,stmr:255,stmw:255,stcp:6,stps:0,stfa:0,stfz:0,ofsf:0,ofsu:0,f2p0:False,fnsp:False,fnfb:False;n:type:ShaderForge.SFN_Final,id:3138,x:36491,y:32802,varname:node_3138,prsc:2|emission-3238-OUT,alpha-9050-OUT,voffset-691-OUT;n:type:ShaderForge.SFN_Tex2d,id:8152,x:32289,y:32806,ptovrint:False,ptlb:noise,ptin:_noise,varname:node_8152,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,ntxv:0,isnm:False|UVIN-8173-OUT;n:type:ShaderForge.SFN_TexCoord,id:7779,x:31517,y:32757,varname:node_7779,prsc:2,uv:0,uaff:False;n:type:ShaderForge.SFN_Time,id:467,x:31517,y:33013,varname:node_467,prsc:2;n:type:ShaderForge.SFN_Append,id:9851,x:31896,y:33004,varname:node_9851,prsc:2|A-4763-OUT,B-2697-OUT;n:type:ShaderForge.SFN_ValueProperty,id:7413,x:31517,y:32935,ptovrint:False,ptlb:noise_u,ptin:_noise_u,varname:node_7413,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,v1:0;n:type:ShaderForge.SFN_ValueProperty,id:4193,x:31517,y:33173,ptovrint:False,ptlb:noise_v,ptin:_noise_v,varname:_node_7413_copy,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,v1:0;n:type:ShaderForge.SFN_Multiply,id:4763,x:31737,y:32963,varname:node_4763,prsc:2|A-7413-OUT,B-467-T;n:type:ShaderForge.SFN_Multiply,id:2697,x:31737,y:33113,varname:node_2697,prsc:2|A-467-T,B-4193-OUT;n:type:ShaderForge.SFN_Add,id:8173,x:32083,y:32806,varname:node_8173,prsc:2|A-7779-UVOUT,B-9851-OUT;n:type:ShaderForge.SFN_Multiply,id:6110,x:32506,y:32831,varname:node_6110,prsc:2|A-8152-A,B-1939-OUT;n:type:ShaderForge.SFN_Slider,id:1939,x:32160,y:33013,ptovrint:False,ptlb:noise_qiangdu,ptin:_noise_qiangdu,varname:node_1939,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,min:0,cur:0,max:1;n:type:ShaderForge.SFN_Tex2d,id:7849,x:33043,y:32888,ptovrint:False,ptlb:Main,ptin:_Main,varname:node_7849,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,ntxv:0,isnm:False|UVIN-287-OUT;n:type:ShaderForge.SFN_TexCoord,id:1100,x:32495,y:32658,varname:node_1100,prsc:2,uv:0,uaff:False;n:type:ShaderForge.SFN_Time,id:4842,x:32096,y:33274,varname:node_4842,prsc:2;n:type:ShaderForge.SFN_Append,id:4313,x:32475,y:33265,varname:node_4313,prsc:2|A-1385-OUT,B-4141-OUT;n:type:ShaderForge.SFN_ValueProperty,id:8046,x:32096,y:33196,ptovrint:False,ptlb:Main_u,ptin:_Main_u,varname:_noise_u_copy,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,v1:0;n:type:ShaderForge.SFN_ValueProperty,id:8096,x:32096,y:33434,ptovrint:False,ptlb:Main_v,ptin:_Main_v,varname:_noise_v_copy,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,v1:0;n:type:ShaderForge.SFN_Multiply,id:1385,x:32316,y:33224,varname:node_1385,prsc:2|A-8046-OUT,B-4842-T;n:type:ShaderForge.SFN_Multiply,id:4141,x:32316,y:33374,varname:node_4141,prsc:2|A-4842-T,B-8096-OUT;n:type:ShaderForge.SFN_Add,id:287,x:32869,y:32853,varname:node_287,prsc:2|A-2898-OUT,B-4313-OUT;n:type:ShaderForge.SFN_Multiply,id:3238,x:33702,y:32660,varname:node_3238,prsc:2|A-8058-RGB,B-2043-OUT;n:type:ShaderForge.SFN_VertexColor,id:8058,x:33143,y:32605,varname:node_8058,prsc:2;n:type:ShaderForge.SFN_Tex2d,id:7049,x:33214,y:33435,ptovrint:False,ptlb:mask,ptin:_mask,varname:node_7049,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,ntxv:0,isnm:False|UVIN-1256-OUT;n:type:ShaderForge.SFN_Multiply,id:2682,x:33523,y:32855,varname:node_2682,prsc:2|A-8058-A,B-7849-A,C-7049-G;n:type:ShaderForge.SFN_Color,id:3963,x:33000,y:32210,ptovrint:False,ptlb:c1,ptin:_c1,varname:node_3963,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,c1:0.5,c2:0.5,c3:0.5,c4:1;n:type:ShaderForge.SFN_Color,id:3603,x:32999,y:32476,ptovrint:False,ptlb:c2,ptin:_c2,varname:node_3603,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,c1:0.5,c2:0.5,c3:0.5,c4:1;n:type:ShaderForge.SFN_Multiply,id:1707,x:33296,y:32257,varname:node_1707,prsc:2|A-3963-RGB,B-3103-OUT;n:type:ShaderForge.SFN_Multiply,id:969,x:33296,y:32478,varname:node_969,prsc:2|A-3103-OUT,B-3603-RGB;n:type:ShaderForge.SFN_Lerp,id:2043,x:33507,y:32463,varname:node_2043,prsc:2|A-1707-OUT,B-969-OUT,T-7849-RGB;n:type:ShaderForge.SFN_TexCoord,id:2214,x:32475,y:33111,varname:node_2214,prsc:2,uv:0,uaff:False;n:type:ShaderForge.SFN_Time,id:1897,x:32081,y:33610,varname:node_1897,prsc:2;n:type:ShaderForge.SFN_Append,id:4299,x:32460,y:33601,varname:node_4299,prsc:2|A-6548-OUT,B-7329-OUT;n:type:ShaderForge.SFN_ValueProperty,id:8756,x:32081,y:33532,ptovrint:False,ptlb:mask_u,ptin:_mask_u,varname:_Main_u_copy,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,v1:0;n:type:ShaderForge.SFN_ValueProperty,id:9519,x:32081,y:33770,ptovrint:False,ptlb:mask_v,ptin:_mask_v,varname:_Main_v_copy,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,v1:0;n:type:ShaderForge.SFN_Multiply,id:6548,x:32301,y:33560,varname:node_6548,prsc:2|A-8756-OUT,B-1897-T;n:type:ShaderForge.SFN_Multiply,id:7329,x:32301,y:33710,varname:node_7329,prsc:2|A-1897-T,B-9519-OUT;n:type:ShaderForge.SFN_Add,id:1256,x:32827,y:33421,varname:node_1256,prsc:2|A-2214-UVOUT,B-4299-OUT;n:type:ShaderForge.SFN_Multiply,id:2425,x:34115,y:33018,varname:node_2425,prsc:2|A-2682-OUT,B-7049-A;n:type:ShaderForge.SFN_Add,id:2898,x:32681,y:32658,varname:node_2898,prsc:2|A-1100-UVOUT,B-6110-OUT;n:type:ShaderForge.SFN_ValueProperty,id:3103,x:32999,y:32388,ptovrint:False,ptlb:yanseliangdu,ptin:_yanseliangdu,varname:node_3103,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,v1:2;n:type:ShaderForge.SFN_ViewVector,id:8050,x:32996,y:33531,varname:node_8050,prsc:2;n:type:ShaderForge.SFN_NormalVector,id:7223,x:32996,y:33717,prsc:2,pt:False;n:type:ShaderForge.SFN_Dot,id:6580,x:33214,y:33617,varname:node_6580,prsc:2,dt:3|A-8050-OUT,B-7223-OUT;n:type:ShaderForge.SFN_Power,id:5651,x:33429,y:33580,varname:node_5651,prsc:2|VAL-6580-OUT,EXP-5353-OUT;n:type:ShaderForge.SFN_Slider,id:5353,x:33182,y:33814,ptovrint:False,ptlb:MeshFade,ptin:_MeshFade,varname:node_5353,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,min:0,cur:0,max:10;n:type:ShaderForge.SFN_Multiply,id:1357,x:35201,y:33105,varname:node_1357,prsc:2|A-7020-OUT,B-6116-OUT;n:type:ShaderForge.SFN_SwitchProperty,id:6116,x:34451,y:33291,ptovrint:False,ptlb:MeshFadeSWC,ptin:_MeshFadeSWC,varname:node_6116,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,on:False|A-5637-OUT,B-5651-OUT;n:type:ShaderForge.SFN_Vector1,id:8677,x:33629,y:33345,varname:node_8677,prsc:2,v1:1;n:type:ShaderForge.SFN_Tex2d,id:565,x:32667,y:32992,ptovrint:False,ptlb:Masked,ptin:_Masked,varname:node_565,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,ntxv:0,isnm:False;n:type:ShaderForge.SFN_Smoothstep,id:3294,x:33211,y:33037,varname:node_3294,prsc:2|A-565-R,B-8491-OUT,V-1726-OUT;n:type:ShaderForge.SFN_Add,id:8491,x:32908,y:33055,varname:node_8491,prsc:2|A-565-R,B-3444-OUT;n:type:ShaderForge.SFN_ValueProperty,id:3444,x:32667,y:33179,ptovrint:False,ptlb:StepSmoothEdge,ptin:_StepSmoothEdge,varname:node_3444,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,v1:0.05;n:type:ShaderForge.SFN_Clamp01,id:6958,x:33618,y:33150,varname:node_6958,prsc:2|IN-3890-OUT;n:type:ShaderForge.SFN_Multiply,id:5637,x:34091,y:33305,varname:node_5637,prsc:2|A-6958-OUT,B-8677-OUT;n:type:ShaderForge.SFN_Slider,id:1726,x:32617,y:33265,ptovrint:False,ptlb:Step,ptin:_Step,varname:node_1726,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,min:-0.1,cur:0,max:1.1;n:type:ShaderForge.SFN_OneMinus,id:3890,x:33442,y:33135,varname:node_3890,prsc:2|IN-3294-OUT;n:type:ShaderForge.SFN_Time,id:5213,x:33822,y:32756,varname:node_5213,prsc:2;n:type:ShaderForge.SFN_Multiply,id:9089,x:34168,y:32797,varname:node_9089,prsc:2|A-5213-T,B-1578-OUT;n:type:ShaderForge.SFN_SwitchProperty,id:7020,x:34940,y:32967,ptovrint:False,ptlb:shijian,ptin:_shijian,varname:node_7020,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,on:False|A-2425-OUT,B-5223-OUT;n:type:ShaderForge.SFN_Slider,id:1578,x:33665,y:32997,ptovrint:False,ptlb:shijianshanshuo,ptin:_shijianshanshuo,varname:node_1578,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,min:0,cur:0,max:5;n:type:ShaderForge.SFN_Sin,id:8172,x:34337,y:32797,varname:node_8172,prsc:2|IN-9089-OUT;n:type:ShaderForge.SFN_Multiply,id:5223,x:34719,y:32988,varname:node_5223,prsc:2|A-6393-OUT,B-2682-OUT;n:type:ShaderForge.SFN_RemapRange,id:6393,x:34539,y:32760,varname:node_6393,prsc:2,frmn:0,frmx:1,tomn:1,tomx:1.5|IN-8172-OUT;n:type:ShaderForge.SFN_Multiply,id:9050,x:35489,y:33085,varname:node_9050,prsc:2|A-1357-OUT,B-5420-OUT;n:type:ShaderForge.SFN_ValueProperty,id:5420,x:35189,y:33298,ptovrint:False,ptlb:alpha,ptin:_alpha,varname:node_5420,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,v1:1;n:type:ShaderForge.SFN_Tex2d,id:1753,x:35082,y:33671,ptovrint:False,ptlb:dingdianpianyi,ptin:_dingdianpianyi,varname:node_1753,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,ntxv:0,isnm:False|UVIN-6563-OUT;n:type:ShaderForge.SFN_TexCoord,id:1683,x:34549,y:33661,varname:node_1683,prsc:2,uv:0,uaff:False;n:type:ShaderForge.SFN_Append,id:4380,x:34549,y:33819,varname:node_4380,prsc:2|A-1848-OUT,B-9231-OUT;n:type:ShaderForge.SFN_ValueProperty,id:1848,x:33967,y:33813,ptovrint:False,ptlb:dingdianpianyi_u,ptin:_dingdianpianyi_u,varname:node_1848,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,v1:0;n:type:ShaderForge.SFN_ValueProperty,id:9231,x:33967,y:33917,ptovrint:False,ptlb:dingdianpianyi_v,ptin:_dingdianpianyi_v,varname:node_9231,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,v1:0;n:type:ShaderForge.SFN_Time,id:3530,x:34549,y:33985,varname:node_3530,prsc:2;n:type:ShaderForge.SFN_Multiply,id:9908,x:34834,y:33918,varname:node_9908,prsc:2|A-4380-OUT,B-3530-T;n:type:ShaderForge.SFN_Add,id:6563,x:34901,y:33701,varname:node_6563,prsc:2|A-1683-UVOUT,B-9908-OUT;n:type:ShaderForge.SFN_Multiply,id:6411,x:35536,y:33725,varname:node_6411,prsc:2|A-1753-RGB,B-8392-OUT;n:type:ShaderForge.SFN_NormalVector,id:8392,x:35232,y:33943,prsc:2,pt:False;n:type:ShaderForge.SFN_Multiply,id:691,x:35829,y:33729,varname:node_691,prsc:2|A-6411-OUT,B-806-OUT;n:type:ShaderForge.SFN_ValueProperty,id:806,x:35535,y:33871,ptovrint:False,ptlb:dingdianpianyi_qiangdu,ptin:_dingdianpianyi_qiangdu,varname:node_806,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,v1:0;proporder:8152-7413-4193-1939-7849-8046-8096-7049-3963-3603-8756-9519-3103-5353-6116-565-3444-1726-7020-1578-5420-1753-1848-9231-806;pass:END;sub:END;*/

Shader "YB/common_uvliudong" {
    Properties {
        _noise ("noise", 2D) = "white" {}
        _noise_u ("noise_u", Float ) = 0
        _noise_v ("noise_v", Float ) = 0
        _noise_qiangdu ("noise_qiangdu", Range(0, 1)) = 0
        _Main ("Main", 2D) = "white" {}
        _Main_u ("Main_u", Float ) = 0
        _Main_v ("Main_v", Float ) = 0
        _mask ("mask", 2D) = "white" {}
        _c1 ("c1", Color) = (0.5,0.5,0.5,1)
        _c2 ("c2", Color) = (0.5,0.5,0.5,1)
        _mask_u ("mask_u", Float ) = 0
        _mask_v ("mask_v", Float ) = 0
        _yanseliangdu ("yanseliangdu", Float ) = 2
        _MeshFade ("MeshFade", Range(0, 10)) = 0
        [MaterialToggle] _MeshFadeSWC ("MeshFadeSWC", Float ) = 1
        _Masked ("Masked", 2D) = "white" {}
        _StepSmoothEdge ("StepSmoothEdge", Float ) = 0.05
        _Step ("Step", Range(-0.1, 1.1)) = 0
        [MaterialToggle] _shijian ("shijian", Float ) = 0
        _shijianshanshuo ("shijianshanshuo", Range(0, 5)) = 0
        _alpha ("alpha", Float ) = 1
        _dingdianpianyi ("dingdianpianyi", 2D) = "white" {}
        _dingdianpianyi_u ("dingdianpianyi_u", Float ) = 0
        _dingdianpianyi_v ("dingdianpianyi_v", Float ) = 0
        _dingdianpianyi_qiangdu ("dingdianpianyi_qiangdu", Float ) = 0
        [HideInInspector]_Cutoff ("Alpha cutoff", Range(0,1)) = 0.5
    }
    SubShader {
        Tags {
            "IgnoreProjector"="True"
            "Queue"="Transparent"
            "RenderType"="Transparent"
        }
        Pass {
            Name "FORWARD"
            Tags {
                "LightMode"="ForwardBase"
            }
            Blend SrcAlpha OneMinusSrcAlpha
            Cull Off
            ZWrite Off
            
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #define UNITY_PASS_FORWARDBASE
            #include "UnityCG.cginc"
            #pragma multi_compile_fwdbase
            #pragma only_renderers d3d9 d3d11 glcore gles gles3 metal d3d11_9x 
            #pragma target 3.0
            uniform float4 _TimeEditor;
            uniform sampler2D _noise; uniform float4 _noise_ST;
            uniform float _noise_u;
            uniform float _noise_v;
            uniform float _noise_qiangdu;
            uniform sampler2D _Main; uniform float4 _Main_ST;
            uniform float _Main_u;
            uniform float _Main_v;
            uniform sampler2D _mask; uniform float4 _mask_ST;
            uniform float4 _c1;
            uniform float4 _c2;
            uniform float _mask_u;
            uniform float _mask_v;
            uniform float _yanseliangdu;
            uniform float _MeshFade;
            uniform fixed _MeshFadeSWC;
            uniform sampler2D _Masked; uniform float4 _Masked_ST;
            uniform float _StepSmoothEdge;
            uniform float _Step;
            uniform fixed _shijian;
            uniform float _shijianshanshuo;
            uniform float _alpha;
            uniform sampler2D _dingdianpianyi; uniform float4 _dingdianpianyi_ST;
            uniform float _dingdianpianyi_u;
            uniform float _dingdianpianyi_v;
            uniform float _dingdianpianyi_qiangdu;
            struct VertexInput {
                float4 vertex : POSITION;
                float3 normal : NORMAL;
                float2 texcoord0 : TEXCOORD0;
                float4 vertexColor : COLOR;
            };
            struct VertexOutput {
                float4 pos : SV_POSITION;
                float2 uv0 : TEXCOORD0;
                float4 posWorld : TEXCOORD1;
                float3 normalDir : TEXCOORD2;
                float4 vertexColor : COLOR;
            };
            VertexOutput vert (VertexInput v) {
                VertexOutput o = (VertexOutput)0;
                o.uv0 = v.texcoord0;
                o.vertexColor = v.vertexColor;
                o.normalDir = UnityObjectToWorldNormal(v.normal);
                float4 node_3530 = _Time + _TimeEditor;
                float2 node_6563 = (o.uv0+(float2(_dingdianpianyi_u,_dingdianpianyi_v)*node_3530.g));
                float4 _dingdianpianyi_var = tex2Dlod(_dingdianpianyi,float4(TRANSFORM_TEX(node_6563, _dingdianpianyi),0.0,0));
                v.vertex.xyz += ((_dingdianpianyi_var.rgb*v.normal)*_dingdianpianyi_qiangdu);
                o.posWorld = mul(unity_ObjectToWorld, v.vertex);
                o.pos = UnityObjectToClipPos(v.vertex );
                return o;
            }
            float4 frag(VertexOutput i, float facing : VFACE) : COLOR {
                float isFrontFace = ( facing >= 0 ? 1 : 0 );
                float faceSign = ( facing >= 0 ? 1 : -1 );
                i.normalDir = normalize(i.normalDir);
                i.normalDir *= faceSign;
                float3 viewDirection = normalize(_WorldSpaceCameraPos.xyz - i.posWorld.xyz);
                float3 normalDirection = i.normalDir;
////// Lighting:
////// Emissive:
                float4 node_467 = _Time + _TimeEditor;
                float2 node_8173 = (i.uv0+float2((_noise_u*node_467.g),(node_467.g*_noise_v)));
                float4 _noise_var = tex2D(_noise,TRANSFORM_TEX(node_8173, _noise));
                float4 node_4842 = _Time + _TimeEditor;
                float2 node_287 = ((i.uv0+(_noise_var.a*_noise_qiangdu))+float2((_Main_u*node_4842.g),(node_4842.g*_Main_v)));
                float4 _Main_var = tex2D(_Main,TRANSFORM_TEX(node_287, _Main));
                float3 emissive = (i.vertexColor.rgb*lerp((_c1.rgb*_yanseliangdu),(_yanseliangdu*_c2.rgb),_Main_var.rgb));
                float3 finalColor = emissive;
                float4 node_1897 = _Time + _TimeEditor;
                float2 node_1256 = (i.uv0+float2((_mask_u*node_1897.g),(node_1897.g*_mask_v)));
                float4 _mask_var = tex2D(_mask,TRANSFORM_TEX(node_1256, _mask));
                float node_2682 = (i.vertexColor.a*_Main_var.a*_mask_var.g);
                float4 node_5213 = _Time + _TimeEditor;
                float4 _Masked_var = tex2D(_Masked,TRANSFORM_TEX(i.uv0, _Masked));
                return fixed4(finalColor,((lerp( (node_2682*_mask_var.a), ((sin((node_5213.g*_shijianshanshuo))*0.5+1.0)*node_2682), _shijian )*lerp( (saturate((1.0 - smoothstep( _Masked_var.r, (_Masked_var.r+_StepSmoothEdge), _Step )))*1.0), pow(abs(dot(viewDirection,i.normalDir)),_MeshFade), _MeshFadeSWC ))*_alpha));
            }
            ENDCG
        }
        Pass {
            Name "ShadowCaster"
            Tags {
                "LightMode"="ShadowCaster"
            }
            Offset 1, 1
            Cull Off
            
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #define UNITY_PASS_SHADOWCASTER
            #include "UnityCG.cginc"
            #include "Lighting.cginc"
            #pragma fragmentoption ARB_precision_hint_fastest
            #pragma multi_compile_shadowcaster
            #pragma only_renderers d3d9 d3d11 glcore gles gles3 metal d3d11_9x 
            #pragma target 3.0
            uniform float4 _TimeEditor;
            uniform sampler2D _dingdianpianyi; uniform float4 _dingdianpianyi_ST;
            uniform float _dingdianpianyi_u;
            uniform float _dingdianpianyi_v;
            uniform float _dingdianpianyi_qiangdu;
            struct VertexInput {
                float4 vertex : POSITION;
                float3 normal : NORMAL;
                float2 texcoord0 : TEXCOORD0;
            };
            struct VertexOutput {
                V2F_SHADOW_CASTER;
                float2 uv0 : TEXCOORD1;
                float4 posWorld : TEXCOORD2;
                float3 normalDir : TEXCOORD3;
            };
            VertexOutput vert (VertexInput v) {
                VertexOutput o = (VertexOutput)0;
                o.uv0 = v.texcoord0;
                o.normalDir = UnityObjectToWorldNormal(v.normal);
                float4 node_3530 = _Time + _TimeEditor;
                float2 node_6563 = (o.uv0+(float2(_dingdianpianyi_u,_dingdianpianyi_v)*node_3530.g));
                float4 _dingdianpianyi_var = tex2Dlod(_dingdianpianyi,float4(TRANSFORM_TEX(node_6563, _dingdianpianyi),0.0,0));
                v.vertex.xyz += ((_dingdianpianyi_var.rgb*v.normal)*_dingdianpianyi_qiangdu);
                o.posWorld = mul(unity_ObjectToWorld, v.vertex);
                o.pos = UnityObjectToClipPos(v.vertex );
                TRANSFER_SHADOW_CASTER(o)
                return o;
            }
            float4 frag(VertexOutput i, float facing : VFACE) : COLOR {
                float isFrontFace = ( facing >= 0 ? 1 : 0 );
                float faceSign = ( facing >= 0 ? 1 : -1 );
                i.normalDir = normalize(i.normalDir);
                i.normalDir *= faceSign;
                float3 viewDirection = normalize(_WorldSpaceCameraPos.xyz - i.posWorld.xyz);
                float3 normalDirection = i.normalDir;
                SHADOW_CASTER_FRAGMENT(i)
            }
            ENDCG
        }
    }
    FallBack "Diffuse"
    CustomEditor "ShaderForgeMaterialInspector"
}
