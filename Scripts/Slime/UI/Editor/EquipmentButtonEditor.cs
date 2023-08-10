#if UNITY_EDITOR

using Slime.UI.Character.Equipment;
using UnityEditor;
using UnityEditor.UI;

namespace Slime.UI.Editor
{
    [CustomEditor(typeof(EquipmentButton))]
    public class EquipmentButtonEditor : ButtonEditor
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