Shader "Graph/Point"
{
    Properties
    {
        _Smoothness("Smoothness", Range(0, 1)) = 0.5
        _A("a", Range(-1, 1)) = 0.5
        _B("b", Range(-5, 5)) = 0.5
    }

    SubShader
    {
        CGPROGRAM
        #pragma surface surf Standard fullforwardshadows
        #pragma target 3.0

        struct Input
        {
            float3 worldPos;
        };

        float _Smoothness;
        float _A;
        float _B;

        void surf(Input input, inout SurfaceOutputStandard surface)
        {
            surface.Albedo.rg = input.worldPos.xz * _A - _B;
            //surface.Albedo.rg = input.worldPos.xy / 2 + 0.5;
            surface.Smoothness = _Smoothness;
        }

        ENDCG
    }

    FallBack "Diffuse"
}
