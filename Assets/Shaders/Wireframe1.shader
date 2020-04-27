Shader "SuperSystems/Wireframe1"

{
    Properties
    {
        [PowerSlider(3.0)]
        _WireframeVal ("Wireframe width", Range(0., 0.5)) = 0.05
        _FrontColor ("Front color", color) = (1., 1., 1., 1.)
        _BackColor ("Back color", color) = (1., 1., 1., 1.)
        [Toggle] _RemoveDiag("Remove diagonals?", Float) = 0.
    }
    SubShader
    {
        Tags {
            "IgnoreProjector"="True"
            "Queue"="Transparent"
            "RenderType"="Transparent"
        }
       
        Pass
        {
			Blend SrcAlpha OneMinusSrcAlpha
            ZWrite Off
            Cull Back
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #pragma geometry geom
 
            // Change "shader_feature" with "pragma_compile" if you want set this keyword from c# code
            #pragma shader_feature __ _REMOVEDIAG_ON
 
            #include "UnityCG.cginc"
 
            struct v2g {
                float4 worldPos : SV_POSITION;
                bool visible : TEXCOORD0;
            };
 
            struct g2f {
                float4 pos : SV_POSITION;
                float3 bary : TEXCOORD0;
                bool visible : TEXCOORD1;
            };
            
            bool in_frustum(float4x4 M, float4 p) 
            {
                float4 Pclip = mul(M, float4(p.x, p.y, p.z, 1.0));
                return abs(Pclip.x) < Pclip.w && abs(Pclip.y) < Pclip.w && 0 < Pclip.z && Pclip.z < Pclip.w;
            }
            
            float4x4 _CameraMatrix;
 
            v2g vert(appdata_base v) {
                v2g o;
                o.worldPos = mul(unity_ObjectToWorld, v.vertex);
                o.visible = in_frustum(_CameraMatrix, o.worldPos);
                return o;
            }
 
            [maxvertexcount(3)]
            void geom(triangle v2g IN[3], inout TriangleStream<g2f> triStream) {
                float3 param = float3(0., 0., 0.);
 
                #if _REMOVEDIAG_ON
                float EdgeA = length(IN[0].worldPos - IN[1].worldPos);
                float EdgeB = length(IN[1].worldPos - IN[2].worldPos);
                float EdgeC = length(IN[2].worldPos - IN[0].worldPos);
               
                if(EdgeA > EdgeB && EdgeA > EdgeC)
                    param.y = 1.;
                else if (EdgeB > EdgeC && EdgeB > EdgeA)
                    param.x = 1.;
                else
                    param.z = 1.;
                #endif
 
                g2f o;
                o.pos = mul(UNITY_MATRIX_VP, IN[0].worldPos);
                o.bary = float3(1., 0., 0.) + param;
                o.visible = IN[0].visible;
                triStream.Append(o);
                o.pos = mul(UNITY_MATRIX_VP, IN[1].worldPos);
                o.bary = float3(0., 0., 1.) + param;
                o.visible = IN[1].visible;
                triStream.Append(o);
                o.pos = mul(UNITY_MATRIX_VP, IN[2].worldPos);
                o.bary = float3(0., 1., 0.) + param;
                o.visible = IN[2].visible;
                triStream.Append(o);
            }
 
            float _WireframeVal;
            fixed4 _BackColor;
            fixed4 _FrontColor;
 
            fixed4 frag(g2f i) : SV_Target 
            {
                if (!any(bool3(i.bary.x < _WireframeVal, i.bary.y < _WireframeVal, i.bary.z < _WireframeVal)))
                    return _BackColor;
 
                return _FrontColor;
            }
 
            ENDCG
        }
 
        
    }
}
