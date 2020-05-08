Shader "Unlit/GridMiniature"
{
	Properties
	{
		_GridColor ("Grid Color", Color) = (1,1,1,1)
		_BackColor ("Background Color", Color) = (0,0,0,1)
      	_GridSize("Grid Size", Range(0,1)) = 0.1
		_LineWidth ("Line Width", Range(0,0.01)) = 0.001
      	_Alpha ("Alpha", Range(0,1)) = 1
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
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			
			#include "UnityCG.cginc"

            float _GridSize;
			float _LineWidth;
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
    	    };
    	
    	    v2f vert (appdata v)
    	    {
        		v2f o;
        		o.vertex = UnityObjectToClipPos(v.vertex);
        		o.uv = v.uv; //mul(unity_ObjectToWorld, v.vertex).xz;
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
            
	    	fixed4 frag (v2f i) : SV_Target
    		{       
    			//fixed col = DrawGrid(i.uv, _GridSize, 0.0001);
				//float4 tCol = lerp(_BackColor, _GridColor, col);
    			//return float4(tCol.rgb, _Alpha);
				
				float x = i.uv.x % _GridSize;
				float y = i.uv.y % _GridSize;
				if (x < _LineWidth || y < _LineWidth)
				{
					return float4(_GridColor.rgb, _Alpha);
				}
    			return float4(_BackColor.rgb, _Alpha);
    		}
	    	ENDCG
		}
	}
}