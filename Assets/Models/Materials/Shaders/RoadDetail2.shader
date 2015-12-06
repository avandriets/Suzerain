Shader "Buildings/RoadDetail2" {
Properties {
        _MainTex ("Base", 2D) = "white" {}       
        _AddTex ("Add texture", 2D) = "white" {}
        _AddMask ("Mask", 2D) = "white" {}   

}

SubShader {
        Tags {"IgnoreProjector"="True" "RenderType"="Opaque"}
        LOD 200
       
CGPROGRAM
#pragma surface surf Lambert alphatest:_Cutoff

sampler2D _MainTex;
sampler2D _AddTex;
sampler2D _AddMask;



struct Input {
        float2 uv_MainTex;
        float2 uv_AddTex;
        float2 uv_AddMask;
};

void surf (Input IN, inout SurfaceOutput o) {
        fixed4 c = tex2D(_MainTex, IN.uv_MainTex);
       
        fixed4 ca = tex2D(_AddTex, IN.uv_AddTex);
        float factor=tex2D(_AddMask, IN.uv_AddMask).r;
        c = lerp(ca, c, 1 - factor);
       
        o.Albedo = c.rgb;
        o.Alpha = c.a;

}
ENDCG
}

Fallback "Transparent/Cutout/VertexLit"
}