/// http://hitmen.c02.at/files/yagcd/yagcd/chap15.html
namespace GameCube.GFX.GX.Texture
{
    public enum TextureFormat
    {
        /// <summary>
        /// 4 bit intensity, 8x8 tiles
        /// </summary>
        I4 = 0x00,
        /// <summary>
        /// 8 bit intensity, 8x4 tiles
        /// </summary>
        I8 = 0x01,
        /// <summary>
        /// 4 bit intensity with 4 bit alpha, 8x4 tiles
        /// </summary>
        IA4 = 0x02,
        /// <summary>
        /// 8 bit intensity with 8 bit alpha, 4x4 tiles
        /// </summary>
        IA8 = 0x03,
        /// <summary>
        /// 4x4 tiles
        /// </summary>
        RGB565 = 0x04,
        /// <summary>
        /// 4x4 tiles
        /// </summary>
        RGB5A3 = 0x05,
        /// <summary>
        /// 4x4 tiles in two cache lines - first is AR and second is GB
        /// </summary>
        RGBA8 = 0x06,
        /// <summary>
        /// 4 bit color index, 8x8 tiles
        /// </summary>
        CI4 = 0x08,
        /// <summary>
        /// 8 bit color index, 8x4 tiles
        /// </summary>
        CI8 = 0x09,
        /// <summary>
        /// 14 bit color index, 4x4 tiles
        /// </summary>
        CI14X2 = 0x0A, // 10
        /// <summary>
        /// S3TC compressed, 2x2 blocks of 4x4 tiles
        /// </summary>
        CMPR = 0x0E, // 14
    }

    //public class GameCubeTexture
    //{
    //    protected Texture2D texture;
    //    protected byte[] rawTexture;
    //    protected TextureFormat inputTextureFormat;
    //    protected TextureFormat outputTextureFormat;

    //    public void LoadFromRaw(TextureFormat format)
    //    {
    //        throw new NotImplementedException();
    //    }
    //}

}
