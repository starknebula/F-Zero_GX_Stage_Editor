using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace GameCube.Games.FZeroGX.FileStructures.FMI
{
    [CreateAssetMenu(fileName = "xxx.fmi", menuName = "FZGX ScriptableObject/FMI")]
    public class FMISobj : ScriptableObject
    {
        [SerializeField]
        protected FMI fmi;
        public FMI Fmi {
            get {
                return fmi;
            }
            internal set {
                fmi = value;
            }
        }
    }
}