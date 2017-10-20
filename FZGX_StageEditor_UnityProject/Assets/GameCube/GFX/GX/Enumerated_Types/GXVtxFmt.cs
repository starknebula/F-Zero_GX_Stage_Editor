namespace GameCube.GFX.GX.Enumerated_Types
{
    /// <summary>
    /// Alpha combine control.
    /// </summary>
    public enum GXAlphaOp
    {
        GX_AOP_AND = 0,
        GX_AOP_OR,
        GX_AOP_XOR,
        GX_AOP_XNOR,
        GX_MAX_ALPHAOP,
    }
    /// <summary>
    /// Maximum anisotropic filter control.
    /// </summary>
    public enum GXAnisotropy
    {
        GX_ANISO_1 = 0,
        GX_ANISO_2,
        GX_ANISO_4,
        GX_MAX_ANISOTROPY,
    }
    /// <summary>
    /// Name of vertex attribute or array. Attributes are listed in the ascending order vertex data is required to be sent to the GP.
    /// 
    /// Notes:
    /// Tells GX what to expect from oncoming vertex information.
    /// The data provided should be 32-byte aligned. Refer to GX FIFO.
    /// </summary>
    public enum GXAttr
    {
        /// <summary>
        /// An unsigned 8-bit index into matrix memory. This number indicates the first row of matrix memory where the matrix was loaded.
        /// </summary>
        GX_VA_PNMTXIDX = 0,
        GX_VA_TEX0MTXIDX,
        GX_VA_TEX1MTXIDX,
        GX_VA_TEX2MTXIDX,
        GX_VA_TEX3MTXIDX,
        GX_VA_TEX4MTXIDX,
        GX_VA_TEX5MTXIDX,
        GX_VA_TEX6MTXIDX,
        GX_VA_TEX7MTXIDX,
        GX_VA_POS,
        GX_VA_NRM_or_GX_VA_NBT,
        GX_VA_CLR0,
        GX_VA_CLR1,
        GX_VA_TEX0,
        GX_VA_TEX1,
        GX_VA_TEX2,
        GX_VA_TEX3,
        GX_VA_TEX4,
        GX_VA_TEX5,
        GX_VA_TEX6,
        GX_VA_TEX7,
        GX_VA_POS_MTX_ARRAY,
        GX_VA_NRM_MTX_ARRAY,
        GX_VA_TEX_MTX_ARRAY,
        GX_VA_LIGHT_ARRAY,
        GX_VA_NBT,
        GX_VA_MAX_ATTR,

        GX_VA_NULL = 0xff,
    }
    /// <summary>
    /// Lighting attenuation control.
    /// </summary>
    public enum GXAttnFn
    {
        GX_AF_SPEC = 0,
        GX_AF_SPOT,
        GX_AF_NONE,
    }
    /// <summary>
    /// Texture Environment (Tev) control.
    /// </summary>
    public enum GXAlphaReadMode
    {
        /// <summary>
        /// Always read 0x00
        /// </summary>
        GX_READ_00 = 0,
        /// <summary>
        /// Always read 0xFF
        /// </summary>
        GX_READ_FF,
        /// <summary>
        /// Always read the real alpha value
        /// </summary>
        GX_READ_NONE,
    }
    /// <summary>
    /// Type of attribute reference.
    /// </summary>
    public enum GXAttrType
    {
        GX_NONE = 0,
        GX_DIRECT,
        GX_INDEX8,
        GX_INDEX16,
    }

    /// <summary>
    /// Blending type.
    /// </summary>
    public enum GXBlendMode
    {
        /// <summary>
        /// Write input directly to EFB
        /// </summary>
        GX_BM_NONE,
        /// <summary>
        /// Blend using blending equation
        /// </summary>
        GX_BM_BLEND,
        /// <summary>
        /// Blend using bitwise operation
        /// </summary>
        GX_BM_LOGIC,
        /// <summary>
        /// (HW2 only)
        /// Input subtracts from existing pixel
        /// </summary>
        GX_BM_SUBTRACT,
        GX_MAX_BLENDMODE,
    }
    /// <summary>
    /// Blending controls.
    /// 
    /// Note:
    /// WARNING: it appears that liboGC and the Nintendo GameCube SDK may use different enumerations for these values
    /// </summary>
    public enum GXBlendFactor
    {
        /// <summary>
        /// 0.0
        /// </summary>
        GX_BL_ZERO,
        /// <summary>
        /// 1.0
        /// </summary>
        GX_BL_ONE,
        /// <summary>
        /// Source color
        /// </summary>
        GX_BL_SRCCLR,
        /// <summary>
        /// 1.0 - (source color)
        /// </summary>
        GX_BL_INVSRCCLR,
        /// <summary>
        /// source alpha
        /// </summary>
        GX_BL_SRCALPHA,
        /// <summary>
        /// 1.0 - (source alpha)
        /// </summary>
        GX_BL_INVSRCALPHA,
        /// <summary>
        /// FrameBuffer alpha
        /// </summary>
        GX_BL_DSTALPHA,
        /// <summary>
        /// 1.0 - (FrameBuffer alpha)
        /// </summary>
        GX_BL_INVDSTALPHA,

        /// <summary>
        /// Same as GX_BL_SRCCLR. Source color
        /// </summary>
        GX_BL_DSTCLR = GX_BL_SRCCLR,
        /// <summary>
        /// Same as GX_BL_INVSRCCLR. 1.0 (source color)
        /// </summary>
        GX_BL_INVDSTCLR = GX_BL_INVSRCCLR,
    }
    /// <summary>
    /// Boolean type.
    /// </summary>
    public enum GXBool
    {
        GX_FALSE = 0,
        GX_TRUE = 1,
        GX_DISABLE = 0,
        GX_ENABLE = 1,
    }

    /// <summary>
    /// Name of color channel used for lighting and to specify raster color input to a Tev stage.
    /// </summary>
    public enum GXChannelID
    {
        GX_COLOR0 = 0,
        GX_COLOR1,
        GX_ALPHA0,
        GX_ALPHA1,
        GX_COLOR1A1,
        GX_COLOR_ZERO,
        GX_ALPHA_BUMP,
        GX_ALPHA_BUMPN,

        GX_COLOR0A0 = 0,
        GX_COLOR_NULL = 0xFF,
    }
    /// <summary>
    /// WARNING: COULD NOT INDICATE WHAT VALUES THESE ARE.
    /// Color index texture format types.
    /// </summary>
    public enum GXCITexFmt
    {
        GX_TF_C4 = int.MaxValue,
        GX_TF_C8 = int.MaxValue,
        GX_TF_C14X2 = int.MaxValue,
    }
    /// <summary>
    /// Clipping modes.  Note that they are backwards of the typical enable/disable enums. This is by design.
    /// </summary>
    public enum GXClipMode
    {
        GX_CLIP_ENABLE = 0,
        GX_CLIP_DISABLE = 1,
    }
    /// <summary>
    /// Source of incoming color.
    /// </summary>
    public enum GXColorSrc
    {
        GX_SRC_REG,
        GX_SRC_VTX,
    }
    /// <summary>
    /// Compare types.
    /// </summary>
    public enum GXCompare
    {
        GX_NEVER = 0,
        GX_LESS,
        GX_LEQUAL,
        GX_EQUAL,
        GX_NEQUAL,
        GX_GEQUAL,
        GX_GREATER,
        GX_ALWAYS,
    }
    /// <summary>
    /// GX Component Count
    /// </summary>
    public enum GXCompCnt
    {
        /// <summary>
        /// X,Y position
        /// </summary>
        GX_POS_XY = 0,
        /// <summary>
        /// X,Y,Z position
        /// </summary>
        GX_POS_XYZ,


        /// <summary>
        /// X,Y,Z normal
        /// </summary>
        GX_NRM_XYZ = 0,
        /// <summary>
        /// Normal, Binormal, Tangent
        /// </summary>
        GX_NRM_NBT,
        /// <summary>
        /// Normal, Binormal, Tangent x3 (HW2 only)
        /// </summary>
        GX_NRM_NBT3,

        /// <summary>
        /// RGB color
        /// </summary>
        GX_CLR_RGB = 0,
        /// <summary>
        /// RGBA color
        /// </summary>
        GX_CLR_RGBA,

        /// <summary>
        /// One texture dimension
        /// </summary>
        GX_TEX_S = 0,
        /// <summary>
        /// Two texture dimensions
        /// </summary>
        GX_TEX_ST,
    }
    /// <summary>
    /// GX Component Type
    /// Related to GXVtxFmt
    /// </summary>
    public enum GXCompType
    {
        /// <summary>
        /// Unsigned 8-bit integer
        /// </summary>
        GX_U8 = 0,
        /// <summary>
        /// Signed 8-bit integer
        /// </summary>
        GX_S8,
        /// <summary>
        /// Unsigned 16-bit integer
        /// </summary>
        GX_U16,
        /// <summary>
        /// Signed 16-bit integer
        /// </summary>
        GX_S16,
        /// <summary>
        /// 32-bit floating-point
        /// </summary>
        GX_F32,

        /// <summary>
        /// 16-bit RGB
        /// </summary>
        GX_RGB565 = 0,
        /// <summary>
        /// 24-bit RGB
        /// </summary>
        GX_RGB8,
        /// <summary>
        /// 32-bit RGBX
        /// </summary>
        GX_RGBX8,
        /// <summary>
        /// 16-bit RGBA
        /// </summary>
        GX_RGBA4,
        /// <summary>
        /// 24-bit RGBA
        /// </summary>
        GX_RGBA6,
        /// <summary>
        /// 32-bit RGBA
        /// </summary>
        GX_RGBA8,
    }
    /// <summary>
    /// Controls whether all lines, only even lines, or only odd lines are copied from the EFB
    /// </summary>
    public enum GXCopyMode
    {
        GX_COPY_PROGRESSIVE = 0,
        GX_COPY_INTLC_EVEN = 2,
        GX_COPY_INTLC_ODD = 3,
    }
    /// <summary>
    /// Backface culling modes.
    /// </summary>
    public enum GXCullMode
    {
        GX_CULL_NONE,
        GX_CULL_FRONT,
        GX_CULL_BACK,
        GX_CULL_ALL,
    }

    /// <summary>
    /// DiffuseFunction
    /// </summary>
    public enum GXDiffuseFn
    {
        GX_DF_NONE = 0,
        GX_DF_SIGN,
        GX_DF_CLAMP,
    }
    /// <summary>
    /// Type of the brightness decreasing function by distance.
    /// </summary>
    public enum GXDistAttnFn
    {
        GX_DA_OFF,
        GX_DA_GENTLE,
        GX_DA_MEDIUM,
        GX_DA_STEEP,
    }

    /// <summary>
    /// Frame buffer clamp modes on copy.
    /// </summary>
    public enum GXFBClamp
    {
        GX_CLAMP_NONE,
        GX_CLAMP_TOP,
        GX_CLAMP_BOTTOM,
    }
    /// <summary>
    /// Fog equation control.
    /// </summary>
    public enum GXFogType
    {
        GX_FOG_NONE = 0,
        GX_FOG_LIN = 2,
        GX_FOG_EXP = 4,
        GX_FOG_EXP2 = 5,
        GX_FOG_REVEXP = 14,
        GX_FOG_REVEXP2 = 15,
    }

    /// <summary>
    /// Indirect texture "bump" alpha select.  Indicates which offset component should provide the "bump" alpha output for the given TEV stage.  Bump alpha is not available for TEV stage 0.
    /// </summary>
    public enum GXIndTexAlphaSel
    {
        GX_ITBA_OFF,
        GX_ITBA_S,
        GX_ITBA_T,
        GX_ITBA_U,
        GX_MAX_ITBALPHA,
    }
    /// <summary>
    /// Indirect texture bias select.  Indicates which components of the indirect offset should receive a bias value.  The bias is fixed at -128 for GX_ITF_8 and +1 for the other formats.  The bias happens prior to the indirect matrix multiply.
    /// </summary>
    public enum GXIndTexBiasSel
    {
        GX_ITB_NONE,
        GX_ITB_S,
        GX_ITB_T,
        GX_ITB_ST,
        GX_ITB_U,
        GX_ITB_SU,
        GX_ITB_TU,
        GX_ITB_STU,
        GX_MAX_ITBIAS,
    }
    /// <summary>
    /// Indirect texture formats.  Bits for the indirect offsets are extracted from the high end of each component byte.  Bits for the bump alpha are extraced off the low end of the byte.  For GX_ITF_8, the byte is duplicated for the offset and the bump alpha.
    /// </summary>
    public enum GXIndTexFormat
    {
        GX_ITF_8,
        GX_ITF_5,
        GX_ITF_4,
        GX_ITF_3,
        GX_MAX_ITFORMAT,
    }
    /// <summary>
    /// Indirect texture stage ID.  Specifies which of the four indirect hardware stages to use.
    /// </summary>
    public enum GXIndTexStageID
    {
        GX_INDTEXSTAGE0,
        GX_INDTEXSTAGE1,
        GX_INDTEXSTAGE2,
        GX_INDTEXSTAGE3,
        GX_MAX_INDTEXSTAGE,
    }
    /// <summary>
    /// Indirect texture wrap value.  This indicates whether the regular texture coordinate should be wrapped before being added to the offset.  GX_ITW_OFF specifies no wrapping.  GX_ITW_0 will zero out the regular texture coordinate.
    /// </summary>
    public enum GXIndTexWrap
    {
        GX_ITW_OFF,
        GX_ITW_256,
        GX_ITW_128,
        GX_ITW_64,
        GX_ITW_32,
        GX_ITW_16,
        GX_ITW_0,
        GX_MAX_ITWRAP,
    }
    /// <summary>
    /// Indirect texture scale value.  Specifies an additional scale value that may be applied to the texcoord used for an indirect initial lookup (not a TEV stage regular lookup).  The scale value is a fraction; thus GX_ITS_32 means to divide the texture coordinate values by 32.
    /// </summary>
    public enum GXIndTexScale
    {
        GX_ITS_1,
        GX_ITS_2,
        GX_ITS_4,
        GX_ITS_8,
        GX_ITS_16,
        GX_ITS_32,
        GX_ITS_64,
        GX_ITS_128,
        GX_ITS_256,
        GX_MAX_ITSCALE,
    }
    /// <summary>
    /// Indirect texture matrix ID.  Indicates which indirect texture matrix and associated scale value should be used for a given TEV stage offset computation.   Three static matrices are available as well as two types of dynamic matrices.   Each dynamic matrix shares the scale values used with the static matrices.
    /// </summary>
    public enum GXIndTexMtxID
    {
        /// <summary>
        /// Specifies a matrix of all zeros.
        /// </summary>
        GX_ITM_OFF,
        /// <summary>
        /// Specifies indirect matrix 0, indirect scale 0.
        /// </summary>
        GX_ITM_0,
        /// <summary>
        /// Specifies indirect matrix 1, indirect scale 1.
        /// </summary>
        GX_ITM_1,
        /// <summary>
        /// Specifies indirect matrix 2, indirect scale 2.
        /// </summary>
        GX_ITM_2,
        /// <summary>
        /// Specifies dynamic S-type matrix, indirect scale 0.
        /// </summary>
        GX_ITM_S0 = 5,
        /// <summary>
        /// Specifies dynamic S-type matrix, indirect scale 1.
        /// </summary>
        GX_ITM_S1,
        /// <summary>
        /// Specifies dynamic S-type matrix, indirect scale 2.
        /// </summary>
        GX_ITM_S2,
        /// <summary>
        /// Specifies dynamic T-type matrix, indirect scale 0.
        /// </summary>
        GX_ITM_T0 = 9,
        /// <summary>
        /// Specifies dynamic T-type matrix, indirect scale 1.
        /// </summary>
        GX_ITM_T1,
        /// <summary>
        /// Specifies dynamic T-type matrix, indirect scale 2.
        /// </summary>
        GX_ITM_T2,   
    }

    /// <summary>
    /// Gamma values.
    /// </summary>
    public enum GXGamma
    {
        GX_GM_1_0,
        GX_GM_1_7,
        GX_GM_2_2,
    }

    /// <summary>
    /// Name of light.
    /// </summary>
    public enum GXLightID
    {
        GX_LIGHT0 = 1 << 0,
        GX_LIGHT1 = 1 << 1,
        GX_LIGHT2 = 1 << 2,
        GX_LIGHT3 = 1 << 3,
        GX_LIGHT4 = 1 << 4,
        GX_LIGHT5 = 1 << 5,
        GX_LIGHT6 = 1 << 6,
        GX_LIGHT7 = 1 << 7,
        GX_MAX_LIGHT = 1 << 8, // UNSURE
        GX_LIGHT_NULL = 0,
    }
    /// <summary>
    /// WARNING: UNSURE IF ORDER IS CORRECT, IT DIFFERS FROM liboGC
    /// </summary>
    public enum GXLogicOp
    {
        GX_LO_CLEAR = 0,
        GX_LO_SET,
        GX_LO_COPY,
        GX_LO_INVCOPY,
        GX_LO_NOOP,
        GX_LO_INV,
        GX_LO_AND,
        GX_LO_NAND,
        GX_LO_OR,
        GX_LO_NOR,
        GX_LO_XOR,
        GX_LO_EQUIV,
        GX_LO_REVAND,
        GX_LO_INVAND,
        GX_LO_REVOR,
        GX_LO_INVOR,
    }

    /// <summary>
    /// Miscellaneous control setting values.
    /// </summary>
    public enum GXMiscToken
    {
        GX_MT_XF_FLUSH = 1,
        GX_MT_DL_SAVE_CONTEXT = 2,
    }

    /// <summary>
    /// Primitive type.
    /// </summary>
    public enum GXPrimitive
    {
        GX_QUADS = 0x80,
        GX_TRIANGLES = 0x90,
        GX_TRAINGLE_STRIP = 0x98,
        /// <summary>
        /// Specific to F-Zero GX GMA files
        /// </summary>
        GX_UINT16_INDEXED_TRIANGLE_STRIP = 0x99,

        GX_TRIANGLE_FAN = 0xA0,
        GX_LINES = 0xA8,
        GX_LINE_STRIP = 0xB0,
        GX_POINTS = 0xB8,

    }

    /// <summary>
    /// Vertex format number.
    /// </summary>
    public enum GXVtxFmt
    {
        GX_VTXFMT0 = 0,
        GX_VTXFMT1,
        GX_VTXFMT2,
        GX_VTXFMT3,
        GX_VTXFMT4,
        GX_VTXFMT5,
        GX_VTXFMT6,
        GX_VTXFMT7,
        GX_MAX_VTXFMT,
    }

    // @ GXPerf0
}