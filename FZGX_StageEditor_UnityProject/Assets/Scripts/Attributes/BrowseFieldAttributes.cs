// Created by Raphael "Stark" Tetreault 13/07/2017
// Copyright © 2017 Raphael Tetreault
// Last updated 17/07/2017

using UnityEngine;
using System.Text.RegularExpressions;

namespace UnityEngine
{
    /// <summary>
    /// Base Attribute to be derived for creating custom string browing fields.
    /// </summary>
    public abstract class BrowsePathAttribute : PropertyAttribute
    {
        protected string relativeDirectory = "Assets/";
        public string RelativeDirectory { get { return relativeDirectory; } }

        protected string panelTitle = "undefined";
        public string PanelTitle { get { return panelTitle; } }

        public BrowsePathAttribute()
        {

        }
        public BrowsePathAttribute(string relativeDirectory)
        {
            this.relativeDirectory = relativeDirectory;
        }
        public BrowsePathAttribute(string relativeDirectory, string panelTitle)
        {
            this.relativeDirectory = relativeDirectory;
            this.panelTitle = panelTitle;
        }
    }
    /// <summary>
    /// Attribute that turns string field into a browasable field that stores the path of the selected folder.
    /// </summary>
    public class BrowseFolderField : BrowsePathAttribute
    {
        public const string defaultPanelTitle = "Open Folder Path";

        public BrowseFolderField() : base()
        {
            this.panelTitle = defaultPanelTitle;
        }
        /// <param name="relativeDirectory">The relative directory to Regex.Match against</param>
        public BrowseFolderField(string relativeDirectory) : base(relativeDirectory)
        {
            this.relativeDirectory = relativeDirectory;
            this.panelTitle = defaultPanelTitle;
        }
        /// <param name="relativeDirectory">The relative directory to Regex.Match against</param>
        /// <param name="panelTitle">The title of the browsing window</param>
        public BrowseFolderField(string relativeDirectory, string panelTitle) : base(panelTitle)
        {
            this.relativeDirectory = relativeDirectory;
            this.panelTitle = panelTitle;
        }
    }
    /// <summary>
    /// Attribute that turns string field into a browasable field that stores the path of the selected file.
    /// </summary>
    public class BrowseFileField : BrowsePathAttribute
    {
        public const string defaultPanelTitle = "Open File Path";

        protected string[] fileTypes;
        protected string fileTypeArgs;
        public string FileTypeArgs { get { return fileTypeArgs; } }

        /// <summary>
        /// Converts string array into single string argument for EditorUtlility.OpenFilePanel
        /// </summary>
        private string CreateFileTypeArgs
        {
            get
            {
                string fileTypeArg = string.Empty;

                foreach (string fileType in fileTypes)
                    fileTypeArg += string.Format("{0}, ", fileType);

                return fileTypeArg;
            }
        }

        public BrowseFileField() : base()
        {
            this.panelTitle = defaultPanelTitle;
            fileTypeArgs = string.Empty;
        }
        /// <param name="relativeDirectory">The relative directory to Regex.Match against</param>
        public BrowseFileField(string relativeDirectory) : base()
        {
            this.relativeDirectory = relativeDirectory;
            this.panelTitle = defaultPanelTitle;
            fileTypeArgs = string.Empty;
        }
        /// <param name="fileTypes">The filetypes to be searched for by default</param>
        public BrowseFileField(string[] fileTypes) : base()
        {
            this.panelTitle = defaultPanelTitle;
            this.fileTypes = fileTypes;
            fileTypeArgs = CreateFileTypeArgs;
        }
        /// <param name="relativeDirectory">The relative directory to Regex.Match against</param>
        /// <param name="fileTypes">The filetypes to be searched for by default</param>
        public BrowseFileField(string relativeDirectory, string[] fileTypes) : base(relativeDirectory)
        {
            this.relativeDirectory = relativeDirectory;
            this.panelTitle = defaultPanelTitle;
            this.fileTypes = fileTypes;
            fileTypeArgs = CreateFileTypeArgs;
        }
        /// <param name="relativeDirectory">The relative directory to Regex.Match against</param>
        /// <param name="panelTitle">The title of the browsing window</param>
        /// <param name="fileTypes">The filetypes to be searched for by default</param>
        public BrowseFileField(string relativeDirectory, string panelTitle, string[] fileTypes) : base(panelTitle)
        {
            this.relativeDirectory = relativeDirectory;
            this.panelTitle = panelTitle;
            this.fileTypes = fileTypes;
            fileTypeArgs = CreateFileTypeArgs;
        }
    }
}

#if UNITY_EDITOR
namespace UnityEditor
{
    /// <summary>
    /// Default drawer for both BrowseFileField and BrowseFolderField
    /// </summary>
    [CustomPropertyDrawer(typeof(BrowseFileField))]
    [CustomPropertyDrawer(typeof(BrowseFolderField))]
    public class BrowseFieldDrawer : PropertyDrawer
    {
        const int browseFieldWidth = 60;
        string browsePath = string.Empty;

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            return (property.type == "string")
                ? base.GetPropertyHeight(property, label)       // [Browse] String
                : base.GetPropertyHeight(property, label) * 2f; // Helpbox Error
        }

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            // Error for Attribute misusage
            if (property.type != "string")
            {
                position = EditorGUI.PrefixLabel(position, new GUIContent(ObjectNames.NicifyVariableName(property.name)));
                EditorGUI.HelpBox(
                    position,
                    string.Format("The field '{0}' is not a string and cannot apply the {1} attribute.",
                        property.name,
                        attribute.GetType().Name),
                    MessageType.Error);
                return;
            }

            BrowsePathAttribute attributeTarget = attribute as BrowsePathAttribute;
            string value = property.stringValue;

            Rect contentPosition = EditorGUI.PrefixLabel(position, new GUIContent(ObjectNames.NicifyVariableName(property.name)));
            float textContentWidth = contentPosition.width - browseFieldWidth;
            contentPosition.width = browseFieldWidth;

            if (GUI.Button(contentPosition, "Browse"))
            {
                string str = (attribute is BrowseFileField)
                    ? EditorUtility.OpenFilePanel(attributeTarget.PanelTitle, browsePath, (attributeTarget as BrowseFileField).FileTypeArgs)
                    : EditorUtility.OpenFolderPanel(attributeTarget.PanelTitle, browsePath, string.Empty);

                // Min requirements for C:/, etc.
                // Prevents returning null on cancel.
                if (str.Length > 2)
                    value = str;
            }

            contentPosition.x += contentPosition.width;
            contentPosition.width = textContentWidth;

            // Only Regex if string contains desired directory
            // This can fail if the regex folder is contained twice in the directory
            if (value.Contains(attributeTarget.RelativeDirectory))
                // Get remainder of string after BrowsePathAttribute.RelativeDirectory
                // By default it's value is "Assets/"
                value = Regex.Match(value, string.Format("(?<={0}).*$", attributeTarget.RelativeDirectory)).Value;

            property.stringValue = GUI.TextField(contentPosition, value);
        }
    }
}
#endif