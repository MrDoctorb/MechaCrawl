using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.Linq;

[CustomPropertyDrawer(typeof(Effect))]
public class EffectPropertyDrawer : PropertyDrawer
{
    int selectedType = -1;
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        //EditorGUI.BeginProperty(position, label, property);
        Type[] types = FindEffectSubClasses().ToArray();
        string[] typeNames = new string[types.Length];
        for (int i = 0; i < typeNames.Length; i++)
        {
            typeNames[i] = types[i].Name;
        }

        selectedType = EditorGUI.Popup(position, "Effect Type", selectedType, typeNames);
        try
        {
            property.managedReferenceValue = Activator.CreateInstance(types[selectedType]);
            Debug.Log("Option " + selectedType + " selected");
        }
        catch
        {
            Debug.LogError("That effect selection type is not valid for this variable");
            selectedType = -1;
        }
        //EditorGUI.EndProperty();
        //base.OnGUI(position, property,label);

    }

    public IEnumerable<Type> FindEffectSubClasses()
    {
        var baseType = typeof(Effect);
        var assembly = baseType.Assembly;

        return assembly.GetTypes().Where(t => t.IsSubclassOf(baseType));
    }
}
