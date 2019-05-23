using System.IO;
using System;
using UnityEngine;

namespace GameCube.Games.FZeroGX.ImportExport
{
    [Obsolete]
    public abstract class FZGX_ImporterExporter : MonoBehaviour
    {
        public abstract void Import();
        public abstract void Export();

        public BinaryReader GetStreamFromFile(string filename)
        {
            return GetStreamFromFile(filename, System.Text.Encoding.ASCII);
        }
        public BinaryReader GetStreamFromFile(string filename, System.Text.Encoding encoding)
        {
            TextAsset file = Resources.Load<TextAsset>(filename);
            Stream stream = new MemoryStream(file.bytes);
            BinaryReader binaryReader = new BinaryReader(stream, encoding);

            return binaryReader;
        }
    }
}