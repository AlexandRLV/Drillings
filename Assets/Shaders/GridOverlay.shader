Shader "Unlit/GridOverlay"
{
	Properties
	{
		_GridColor ("Grid Color", Color) = (1,1,1,1)
		_BackColor ("Background Color", Color) = (0,0,0,1)
      	_GridSize("Grid Size", Float) = 1
      	_Alpha ("Alpha", Range(0,1)) = 1
      	_DistFromCenter ("Dist From Center", Float) = 1
      	_Offset ("Offset", Float) = 0
	}
	SubShader
	{
        Tags 
        {
            "Queue"="Transparent"
            "RenderType"="Transparent"
        }
		LOD 100

		Pass
		{
         	Blend SrcAlpha OneMinusSrcAlpha
			Cull off
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			
			#include "UnityCG.cginc"

            float _GridSize;
            float _Alpha;
            float _DistFromCenter;
            float _Offset;
            float4 _CenterPoint;
			float4 _GridColor;
			float4 _BackColor;

	        struct appdata
   	        {
    	    	float4 vertex : POSITION;
    	    	float2 uv : TEXCOORD0;
    	    };

        	struct v2f
    	    {
    		    float2 uv : TEXCOORD0;
    		    float4 vertex : SV_POSITION;
    		    float3 wPos : TEXCOORD1;
    	    };
    	
    	    v2f vert (appdata v)
    	    {
        		v2f o;
        		o.vertex = UnityObjectToClipPos(v.vertex);
        		o.uv = v.uv; //mul(unity_ObjectToWorld, v.vertex).xz;
        		float3 worldPos = mul (unity_ObjectToWorld, v.vertex).xyz;
        		o.wPos = worldPos;
        		return o;
        	}
        	
            float DrawGrid(float2 uv, float sz, float aa)
            {
                uv += _Offset;
                float aaThresh = aa;
                float aaMin = aa*0.1;
    
                float2 gUV = uv / sz + aaThresh;
             
                float2 fl = floor(gUV);
                gUV = frac(gUV);
                gUV -= aaThresh;
                gUV = smoothstep(aaThresh, aaMin, abs(gUV));
                float d = max(gUV.x, gUV.y);

                return d;
            }
            
            float dist(float3 vertex)
            {
                float x = vertex.x - _CenterPoint.x;
                float y = vertex.y - _CenterPoint.y;
                float z = vertex.z - _CenterPoint.z;
                return sqrt(x * x + y * y + z * z);
            }

	    	fixed4 frag (v2f i) : SV_Target
    		{       
    			fixed col = DrawGrid(i.uv, _GridSize, 0.1);
    			float d = dist(i.wPos);
    			if (d > _DistFromCenter)
    			    return float4(0, 0, 0, 0);
    			else
    			{
					float4 tCol = lerp(_BackColor, _GridColor, col);
    			    float t = 1 - d / _DistFromCenter;
    			    t = t * t;
    			    return float4(tCol.rgb, _Alpha * t);
    			}
    		}
	    	ENDCG
		}
	}
}