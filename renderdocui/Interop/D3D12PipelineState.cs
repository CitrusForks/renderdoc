﻿/******************************************************************************
 * The MIT License (MIT)
 * 
 * Copyright (c) 2015-2016 Baldur Karlsson
 * Copyright (c) 2014 Crytek
 * 
 * Permission is hereby granted, free of charge, to any person obtaining a copy
 * of this software and associated documentation files (the "Software"), to deal
 * in the Software without restriction, including without limitation the rights
 * to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
 * copies of the Software, and to permit persons to whom the Software is
 * furnished to do so, subject to the following conditions:
 * 
 * The above copyright notice and this permission notice shall be included in
 * all copies or substantial portions of the Software.
 * 
 * THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
 * IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
 * FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
 * AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
 * LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
 * OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
 * THE SOFTWARE.
 ******************************************************************************/

using System;
using System.Runtime.InteropServices;
using System.Collections.Generic;

namespace renderdoc
{
    [StructLayout(LayoutKind.Sequential)]
    public class D3D12PipelineState
    {
        public ResourceId pipeline;
        public bool customName;
        [CustomMarshalAs(CustomUnmanagedType.UTF8TemplatedString)]
        public string PipelineName;

        public ResourceId rootSig;

        [StructLayout(LayoutKind.Sequential)]
        public class InputAssembler
        {
            [StructLayout(LayoutKind.Sequential)]
            public class LayoutInput
            {
                [CustomMarshalAs(CustomUnmanagedType.UTF8TemplatedString)]
                public string SemanticName;
                public UInt32 SemanticIndex;
                [CustomMarshalAs(CustomUnmanagedType.CustomClass)]
                public ResourceFormat Format;
                public UInt32 InputSlot;
                public UInt32 ByteOffset;
                public bool PerInstance;
                public UInt32 InstanceDataStepRate;
            };
            [CustomMarshalAs(CustomUnmanagedType.TemplatedArray)]
            public LayoutInput[] layouts;

            [StructLayout(LayoutKind.Sequential)]
            public class VertexBuffer
            {
                public ResourceId Buffer;
                public UInt64 Offset;
                public UInt32 Size;
                public UInt32 Stride;
            };
            [CustomMarshalAs(CustomUnmanagedType.TemplatedArray)]
            public VertexBuffer[] vbuffers;

            [StructLayout(LayoutKind.Sequential)]
            public class IndexBuffer
            {
                public ResourceId Buffer;
                public UInt64 Offset;
                public UInt32 Size;
            };
            [CustomMarshalAs(CustomUnmanagedType.CustomClass)]
            public IndexBuffer ibuffer;

            public UInt32 indexStripCutValue;
        };
        [CustomMarshalAs(CustomUnmanagedType.CustomClass)]
        public InputAssembler m_IA;

        [StructLayout(LayoutKind.Sequential)]
        public class ResourceView
        {
            public bool Immediate;
            public UInt32 RootElement;
            public UInt32 TableIndex;

            public ResourceId Resource;
            [CustomMarshalAs(CustomUnmanagedType.UTF8TemplatedString)]
            public string Type;
            [CustomMarshalAs(CustomUnmanagedType.CustomClass)]
            public ResourceFormat Format;

            [CustomMarshalAs(CustomUnmanagedType.FixedArray, FixedLength = 4)]
            public TextureSwizzle[] swizzle;

            public D3DBufferViewFlags BufferFlags;
            public UInt32 BufferStructCount;
            public UInt32 ElementSize;
            public UInt64 FirstElement;
            public UInt32 NumElements;

            public ResourceId CounterResource;
            public UInt64 CounterByteOffset;

            // Texture
            public UInt32 HighestMip;
            public UInt32 NumMipLevels;

            // Texture Array
            public UInt32 ArraySize;
            public UInt32 FirstArraySlice;

            public float MinLODClamp;
        };

        [StructLayout(LayoutKind.Sequential)]
        public class Sampler
        {
            public bool Immediate;
            public UInt32 RootElement;
            public UInt32 TableIndex;

            [CustomMarshalAs(CustomUnmanagedType.UTF8TemplatedString)]
            public string AddressU, AddressV, AddressW;
            [CustomMarshalAs(CustomUnmanagedType.FixedArray, FixedLength = 4)]
            public float[] BorderColor;
            [CustomMarshalAs(CustomUnmanagedType.UTF8TemplatedString)]
            public string Comparison;
            [CustomMarshalAs(CustomUnmanagedType.UTF8TemplatedString)]
            public string Filter;
            public bool UseBorder;
            public bool UseComparison;
            public UInt32 MaxAniso;
            public float MaxLOD;
            public float MinLOD;
            public float MipLODBias;
        };

        [StructLayout(LayoutKind.Sequential)]
        public class CBuffer
        {
            public bool Immediate;
            public UInt32 RootElement;
            public UInt32 TableIndex;

            public ResourceId Buffer;
            public UInt64 Offset;
            public UInt32 ByteSize;

            [CustomMarshalAs(CustomUnmanagedType.TemplatedArray)]
            public UInt32[] RootValues;
        };

        [StructLayout(LayoutKind.Sequential)]
        public class ShaderStage
        {
            private void PostMarshal()
            {
                if (_ptr_ShaderDetails != IntPtr.Zero)
                    ShaderDetails = (ShaderReflection)CustomMarshal.PtrToStructure(_ptr_ShaderDetails, typeof(ShaderReflection), false);
                else
                    ShaderDetails = null;

                _ptr_ShaderDetails = IntPtr.Zero;
            }

            public ResourceId Shader;
            private IntPtr _ptr_ShaderDetails;
            [CustomMarshalAs(CustomUnmanagedType.Skip)]
            public ShaderReflection ShaderDetails;
            [CustomMarshalAs(CustomUnmanagedType.CustomClass)]
            public ShaderBindpointMapping BindpointMapping;

            public ShaderStageType stage;

            [StructLayout(LayoutKind.Sequential)]
            public class RegisterSpace
            {
                [CustomMarshalAs(CustomUnmanagedType.TemplatedArray)]
                public CBuffer[] ConstantBuffers;
                [CustomMarshalAs(CustomUnmanagedType.TemplatedArray)]
                public Sampler[] Samplers;
                [CustomMarshalAs(CustomUnmanagedType.TemplatedArray)]
                public ResourceView[] SRVs;
                [CustomMarshalAs(CustomUnmanagedType.TemplatedArray)]
                public ResourceView[] UAVs;
            };

            [CustomMarshalAs(CustomUnmanagedType.TemplatedArray)]
            public RegisterSpace[] Spaces;
        };
        [CustomMarshalAs(CustomUnmanagedType.CustomClass)]
        public ShaderStage m_VS, m_HS, m_DS, m_GS, m_PS, m_CS;

        [StructLayout(LayoutKind.Sequential)]
        public class Streamout
        {
            [StructLayout(LayoutKind.Sequential)]
            public class Output
            {
                public ResourceId Buffer;
                public UInt64 Offset;
                public UInt64 Size;

                public ResourceId WrittenCountBuffer;
                public UInt64 WrittenCountOffset;
            };
            [CustomMarshalAs(CustomUnmanagedType.TemplatedArray)]
            public Output[] Outputs;
        };
        [CustomMarshalAs(CustomUnmanagedType.CustomClass)]
        public Streamout m_SO;

        [StructLayout(LayoutKind.Sequential)]
        public class Rasterizer
        {
            public UInt32 SampleMask;

            [StructLayout(LayoutKind.Sequential)]
            public class Viewport
            {
                [CustomMarshalAs(CustomUnmanagedType.FixedArray, FixedLength = 2)]
                public float[] TopLeft;
                public float Width, Height;
                public float MinDepth, MaxDepth;
            };
            [CustomMarshalAs(CustomUnmanagedType.TemplatedArray)]
            public Viewport[] Viewports;

            [StructLayout(LayoutKind.Sequential)]
            public class Scissor
            {
                public Int32 left, top, right, bottom;
            };
            [CustomMarshalAs(CustomUnmanagedType.TemplatedArray)]
            public Scissor[] Scissors;

            [StructLayout(LayoutKind.Sequential)]
            public class RasterizerState
            {
                public TriangleFillMode FillMode;
                public TriangleCullMode CullMode;
                public bool FrontCCW;
                public Int32 DepthBias;
                public float DepthBiasClamp;
                public float SlopeScaledDepthBias;
                public bool DepthClip;
                public bool MultisampleEnable;
                public bool AntialiasedLineEnable;
                public UInt32 ForcedSampleCount;
                public bool ConservativeRasterization;
            };
            [CustomMarshalAs(CustomUnmanagedType.CustomClass)]
            public RasterizerState m_State;
        };
        [CustomMarshalAs(CustomUnmanagedType.CustomClass)]
        public Rasterizer m_RS;

        [StructLayout(LayoutKind.Sequential)]
        public class OutputMerger
        {
            [StructLayout(LayoutKind.Sequential)]
            public class DepthStencilState
            {
                public bool DepthEnable;
                public bool DepthWrites;
                [CustomMarshalAs(CustomUnmanagedType.UTF8TemplatedString)]
                public string DepthFunc;
                public bool StencilEnable;
                public byte StencilReadMask;
                public byte StencilWriteMask;

                [StructLayout(LayoutKind.Sequential)]
                public class StencilOp
                {
                    [CustomMarshalAs(CustomUnmanagedType.UTF8TemplatedString)]
                    public string FailOp;
                    [CustomMarshalAs(CustomUnmanagedType.UTF8TemplatedString)]
                    public string DepthFailOp;
                    [CustomMarshalAs(CustomUnmanagedType.UTF8TemplatedString)]
                    public string PassOp;
                    [CustomMarshalAs(CustomUnmanagedType.UTF8TemplatedString)]
                    public string Func;
                };
                [CustomMarshalAs(CustomUnmanagedType.CustomClass)]
                public StencilOp m_FrontFace, m_BackFace;

                public UInt32 StencilRef;
            };
            [CustomMarshalAs(CustomUnmanagedType.CustomClass)]
            public DepthStencilState m_State;

            [StructLayout(LayoutKind.Sequential)]
            public class BlendState
            {
                public bool AlphaToCoverage;
                public bool IndependentBlend;

                [StructLayout(LayoutKind.Sequential)]
                public class RTBlend
                {
                    [StructLayout(LayoutKind.Sequential)]
                    public class BlendOp
                    {
                        [CustomMarshalAs(CustomUnmanagedType.UTF8TemplatedString)]
                        public string Source;
                        [CustomMarshalAs(CustomUnmanagedType.UTF8TemplatedString)]
                        public string Destination;
                        [CustomMarshalAs(CustomUnmanagedType.UTF8TemplatedString)]
                        public string Operation;
                    };
                    [CustomMarshalAs(CustomUnmanagedType.CustomClass)]
                    public BlendOp m_Blend, m_AlphaBlend;

                    [CustomMarshalAs(CustomUnmanagedType.UTF8TemplatedString)]
                    public string LogicOp;

                    public bool Enabled;
                    public bool LogicEnabled;
                    public byte WriteMask;
                };
                [CustomMarshalAs(CustomUnmanagedType.TemplatedArray)]
                public RTBlend[] Blends;

                [CustomMarshalAs(CustomUnmanagedType.FixedArray, FixedLength = 4)]
                public float[] BlendFactor;
            };
            [CustomMarshalAs(CustomUnmanagedType.CustomClass)]
            public BlendState m_BlendState;

            [CustomMarshalAs(CustomUnmanagedType.TemplatedArray)]
            public ResourceView[] RenderTargets;

            [CustomMarshalAs(CustomUnmanagedType.CustomClass)]
            public ResourceView DepthTarget;
            public bool DepthReadOnly;
            public bool StencilReadOnly;

            public UInt32 multiSampleCount;
            public UInt32 multiSampleQuality;
        };
        [CustomMarshalAs(CustomUnmanagedType.CustomClass)]
        public OutputMerger m_OM;

        [StructLayout(LayoutKind.Sequential)]
        public class ResourceData
        {
            public ResourceId id;

            [StructLayout(LayoutKind.Sequential)]
            public class ResourceState
            {
                [CustomMarshalAs(CustomUnmanagedType.UTF8TemplatedString)]
                public string name;
            };

            [CustomMarshalAs(CustomUnmanagedType.TemplatedArray)]
            public ResourceState[] states;
        };

        [CustomMarshalAs(CustomUnmanagedType.TemplatedArray)]
        private ResourceData[] Resources_;

        // add to dictionary for convenience
        private void PostMarshal()
        {
            Resources = new Dictionary<ResourceId, ResourceData>();

            foreach (ResourceData i in Resources_)
                Resources.Add(i.id, i);
        }

        [CustomMarshalAs(CustomUnmanagedType.Skip)]
        public Dictionary<ResourceId, ResourceData> Resources;
    };
}
