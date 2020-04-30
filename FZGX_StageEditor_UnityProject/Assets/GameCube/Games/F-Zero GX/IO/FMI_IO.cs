using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using GameCube.Games.FZeroGX.FileStructures.FMI;

namespace GameCube.Games.FZeroGX.IO
{
    [CreateAssetMenu(fileName = "FMI ImportExport", menuName = "FZGX IO/FMI ImportExport")]
    public class FMI_IO : ImportExportObject
    {
        [SerializeField]
        private string fileName = ".fmi";
        [SerializeField]
        private FMISobj exportFmi;

        public override string ImportMessage => "Fmi imported!";
        public override string ExportMessage => "Fmi exported!";
        public override string HelpBoxImport => "Imports all modules from 'fileName' in folder 'sourcePath' into folder 'importPath'";
        public override string HelpBoxExport => "Exports 'exportFmi' to folder 'exportPath'";

        public override void Import() {
            BinaryReaderWriterExtensions.PushEndianess(false);
            using (BinaryReader reader = OpenBinaryReaderWithFile(fileName)) {
                FMISobj fmi = CreateScriptableObject<FMISobj>(this.fileName);
                exportFmi = fmi;
                exportFmi.Fmi.Deserialize(reader);
            }
            BinaryReaderWriterExtensions.PopEndianess();
        }

        public override void Export() {
            string sourceFilePath = Path.Combine(SourcePathFull, fileName).PathToUnityPath();
            string exportFilePath = Path.Combine(ExportPathFull, fileName).PathToUnityPath();

            // Copy original file
            File.Copy(sourceFilePath, exportFilePath, true);

            BinaryReaderWriterExtensions.PushEndianess(false);
            using (BinaryWriter writer = new BinaryWriter(File.Open(exportFilePath, fileMode, fileAccess))) {
                exportFmi.Fmi.Serialize(writer);
            }
            EditorUtility.SetDirty(this);
            BinaryReaderWriterExtensions.PopEndianess();
        }
    }
}
