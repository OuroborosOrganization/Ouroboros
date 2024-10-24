Shader "Unlit/ParticleExp_Beta"
{
    Properties
    {
        // ϸ����ر���
        _Level("Level", int) = 0
        _DispDir("Displacement Direction", Vector) = (0, 0, 0)
        _uVelScale("VelScale", float) = 2
        // ���ӻ���Ч��ر���
        _Speed("Speed", Range(0, 3)) = 1
        _ShaderStartTime("Shader Start Time", float) = 0
        _FinalColor("Final Color", Color) = (1, 1, 1, 1)
    }

    SubShader
    {
        Tags { "RenderType" = "Transparent" "Queue" = "Transparent" }
        LOD 100

        Pass
        {
            Blend SrcAlpha OneMinusSrcAlpha // ʹ�� alpha ���
            Cull Off

            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #pragma geometry geom

            #include "UnityCG.cginc"

            // ϸ����ر���
            uniform int _Level;
            uniform float3 _DispDir;
            uniform float _uVelScale;
            // ���ӻ���Ч��ر���
            uniform float _Speed;            // ����λ���ٶ�
            uniform float _ShaderStartTime; // ���ӻ���ʼʱ��
            uniform fixed4 _FinalColor;      // ������ɫ

            float3 V0, V1, V2;
            float3 CG;
            float unityTime;

            struct appdata
            {
                float4 vertex : POSITION;
                float3 normal : NORMAL;
            };

            struct v2g
            {
                float4 vertex : SV_POSITION;
                fixed4 color : COLOR;
                float3 normal : NORMAL;
            };

            struct g2f
            {
                float4 vertex : SV_POSITION;
                fixed4 color : COLOR;
            };

            v2g vert(appdata v)
            {
                v2g o;
                o.vertex = v.vertex;
                o.normal = UnityObjectToWorldNormal(v.normal);
                return o;
            }

            [maxvertexcount(120)]
            void geom(inout PointStream<g2f> OutputStream, triangle v2g input[3])
            {
                float time_SinceBirth = (unityTime - _ShaderStartTime)*0.1f;
                g2f o = (g2f)0;
                V1 = (input[1].vertex - input[0].vertex).xyz;
                V2 = (input[2].vertex - input[0].vertex).xyz;
                V0 = input[0].vertex.xyz;
                CG = (input[0].vertex.xyz + input[1].vertex.xyz + input[2].vertex.xyz) / 3.0f;
                int numLayers = 1 << _Level; // 2^_Level
                float dt = 1.0f / float(numLayers);
                float t = 1.0f;

                for (int it = 0; it < numLayers; it++)
                {
                    float smax = 1.0f - t;
                    int nums = it + 1;
                    float ds = smax / float(nums - 1);
                    float s = 0;

                    for (int is = 0; is < nums; is++)
                    {
                        float3 v = V0 + s * V1 + t * V2;
                        float3 vel = _uVelScale * (v - CG);
                        v = CG + vel * (_Speed * time_SinceBirth + 1.0f) + 0.5f * _DispDir.xyz * sin(it * is) * (_Speed * time_SinceBirth) * (_Speed * time_SinceBirth);
                        o.vertex = UnityObjectToClipPos(float4(v, 1.0f));
                        o.color = _FinalColor;
                        OutputStream.Append(o);
                        s += ds;
                    }
                    t -= dt;
                }
            }

            fixed4 frag(g2f i) : SV_Target
            {
                return i.color;
            }
            ENDCG
        }
    }
}

