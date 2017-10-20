namespace GameCube.GFX.GX.Enumerated_Types
{
    public enum GXAlphaOp
    {
        GX_AOP_AND = 0,
        GX_AOP_OR,
        GX_AOP_XOR,
        GX_AOP_XNOR,
        GX_MAX_ALPHAOP,
    }
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
    public enum GXAnisotropy
    {
        GX_ANISO_1 = 0,
        GX_ANISO_2,
        GX_ANISO_4,
        GX_MAX_ANISOTROPY,
    }
    /// <summary>
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
    /// GX Attenuation Function
    /// </summary>
    public enum GXAttnFn
    {
        GX_AF_SPEC = 0,
        GX_AF_SPOT,
        GX_AF_NONE,
    }
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
    public enum GXAttrType
    {
        GX_NONE = 0,
        GX_DIRECT,
        GX_INDEX8,
        GX_INDEX16,
    }
    /// <summary>
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
    public enum VertexAttributes
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

}