Shader "Custom/EdgeDetection"
{
    Properties
    {
        _MainTex ("Main Texture", 2D) = "white" {}  // 主纹理
        _LightMask ("Light Mask", 2D) = "white" {}  // 光照遮罩
        _Color ("Edge Color", Color) = (1, 1, 1, 1) // 边缘颜色
        _Alpha ("Alpha", Range(0,1)) = 1            // 透明度
        _EdgeThreshold ("Edge Threshold", Range(0.01, 1)) = 0.05  // 边缘检测阈值
    }
    SubShader
    {
        Tags { "Queue" = "Transparent" "RenderType" = "Transparent" }
        LOD 200

        Pass
        {
            ZWrite Off
            Blend SrcAlpha OneMinusSrcAlpha

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
                float4 pos : SV_POSITION;
                float2 uv : TEXCOORD0;
                float4 screenPos : TEXCOORD1;  // 屏幕坐标
            };

            sampler2D _LightMask;  // 光照遮罩
            float4 _Color;         // 给定的边缘颜色
            float _Alpha;          // 透明度控制
            float _EdgeThreshold;  // 边缘检测阈值
            float4 _LightMask_TexelSize;  // 像素大小，用于计算卷积

            // 5x5 Sobel 算子 (卷积核)
            float EdgeDetectionWithSmoothing(float2 uv)
            {
                // 5x5 卷积核的采样偏移
                float2 offset[25] = {
                    float2(-2, -2), float2(-1, -2), float2(0, -2), float2(1, -2), float2(2, -2),
                    float2(-2, -1), float2(-1, -1), float2(0, -1), float2(1, -1), float2(2, -1),
                    float2(-2,  0), float2(-1,  0), float2(0,  0), float2(1,  0), float2(2,  0),
                    float2(-2,  1), float2(-1,  1), float2(0,  1), float2(1,  1), float2(2,  1),
                    float2(-2,  2), float2(-1,  2), float2(0,  2), float2(1,  2), float2(2,  2)
                };

                // 5x5 Sobel 核，X 和 Y 方向的卷积核
                float sobelX[25] = {
                    -2, -1,  0,  1,  2,
                    -2, -1,  0,  1,  2,
                    -4, -2,  0,  2,  4,
                    -2, -1,  0,  1,  2,
                    -2, -1,  0,  1,  2
                };
                
                float sobelY[25] = {
                    -2, -2, -4, -2, -2,
                    -1, -1, -2, -1, -1,
                     0,  0,  0,  0,  0,
                     1,  1,  2,  1,  1,
                     2,  2,  4,  2,  2
                };

                float gradientX = 0.0;
                float gradientY = 0.0;

                // 遍历 5x5 卷积核，计算 X 和 Y 方向梯度
                for (int i = 0; i < 25; i++)
                {
                    float2 sampleUV = uv + offset[i] * _LightMask_TexelSize.xy;
                    float sampleValue = tex2D(_LightMask, sampleUV).r;

                    gradientX += sampleValue * sobelX[i];
                    gradientY += sampleValue * sobelY[i];
                }
                gradientX /= 25;
                gradientY /= 25;

                // 计算边缘强度
                float edgeStrength = sqrt(gradientX * gradientX + gradientY * gradientY);

                // 返回平滑过渡的边缘强度
                return smoothstep(_EdgeThreshold, 1.0, edgeStrength);
            }

            // 3x3 中值滤波 (抗锯齿)
            float ApplyMedianFilter(float2 uv)
            {
                float values[9];

                float2 offset[9] = {
                    float2(-1, -1), float2( 0, -1), float2( 1, -1),
                    float2(-1,  0), float2( 0,  0), float2( 1,  0),
                    float2(-1,  1), float2( 0,  1), float2( 1,  1)
                };

                for (int i = 0; i < 9; i++)
                {
                    float2 sampleUV = uv + offset[i] * _LightMask_TexelSize.xy;
                    values[i] = tex2D(_LightMask, sampleUV).r;
                }

                // 对9个采样值进行排序，取中值
                for (int i = 0; i < 8; i++)
                {
                    for (int j = i + 1; j < 9; j++)
                    {
                        if (values[i] > values[j])
                        {
                            float temp = values[i];
                            values[i] = values[j];
                            values[j] = temp;
                        }
                    }
                }

                // 返回排序后的中间值，即中值滤波结果
                return values[4];
            }

            v2f vert (appdata v)
            {
                v2f o;
                o.pos = UnityObjectToClipPos(v.vertex);  // 转换到裁剪空间
                o.uv = v.uv;
                o.screenPos = ComputeScreenPos(o.pos);  // 保存屏幕坐标
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                // 获取屏幕坐标
                float2 screenUV = i.screenPos.xy / i.screenPos.w;

                // 确保屏幕 UV 在有效范围内
                if (screenUV.x < 0.0 || screenUV.x > 1.0 || screenUV.y < 0.0 || screenUV.y > 1.0)
                {
                    return float4(0, 0, 0, 0);
                }

                // 应用中值滤波抗锯齿
                //float filteredValue = ApplyMedianFilter(screenUV);

                // 使用 5x5 卷积核进行边缘检测并平滑处理
                float edge = EdgeDetectionWithSmoothing(screenUV);

                // 动态调整不透明度：边缘处显示给定的颜色和透明度，非边缘部分透明
                float finalAlpha = edge * _Alpha;
                return float4(_Color.rgb, finalAlpha);
            }
            ENDCG
        }
    }
    FallBack "Transparent"
}
