Shader "Lines/Background" 
{ 
    Properties 
    { 
        _Color ("Main Color", Color) = (1,1,1,1) 
    }
    SubShader 
    { 
        Pass 
        {
            Blend SrcAlpha OneMinusSrcAlpha
            Colormask RGBA
            Lighting Off 
            Cull off
            Offset 1, 1 
            Color[_Color] 
        }
    }
}
