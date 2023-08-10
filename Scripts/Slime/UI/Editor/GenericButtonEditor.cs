#if UNITY_EDITOR

using Slime.UI.Common;
using UnityEditor;
using UnityEditor.UI;

namespace Slime.UI.Editor
{
    [CustomEditor(typeof(GenericButton))]
    public class GenericButtonEditor : ButtonEditor
    {
        public override void OnInspectorGUI()
        {
            // NOTE: hide unnecessary properties
            
            // Show default inspector property editor
            DrawDefaultInspector();
        }
    }
}

#endif