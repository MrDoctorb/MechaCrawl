//Code adapted from the codebase of Unity Forum User Leonidas85
//Forum with their codebase found Here: https://forum.unity.com/threads/how-to-set-serializedproperty-managedreferencevalue-to-null.746645/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using UnityEditor;
using UnityEngine;

/// <summary>
/// Add [SelectType] and [SerializeReference] to a base class reference to add a dropdown menu with all subclasses of the base class
/// </summary>
[CustomPropertyDrawer(typeof(SelectTypeAttribute))]
public class SelectTypeAttributeDrawer : PropertyDrawer
{
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        // Type is returned as "{assembly name} {type name}" from property.managedReferenceFieldTypename
        // Needs to be "{assembly name}, {type name}" for Type.GetType()
        string[] typeNameAndAssemblyName = property.managedReferenceFieldTypename.Split(' ');
        string typeName = typeNameAndAssemblyName[1]+", "+typeNameAndAssemblyName[0];
        Type baseType = Type.GetType(typeName);

        //Find all subclasses of the type provided
        List<Type> subclasses = FindSubClasses(baseType);

        //Create GUIContent for each type
        GUIContent[] menuOptions = new GUIContent[subclasses.Count];

        int menuIndex = 0;
        //Fill in the rest
        for (int i = 0; i < subclasses.Count; i++)
        {
            //Current type from the list
            Type type = subclasses[i];

            //Fill in the name of the type
            menuOptions[i] = new GUIContent(type.FullName);

            //If the current type that is being filled into the list is what is currently selected
            //then we make sure our current selection for the upcoming popup is set to what it should be
            //Build the string in the format .managedReferenceFullTypename is formatted
            string typeAndAssemblyName = $"{type.Assembly.GetName().Name} {type.Name}";
            if (property.managedReferenceFullTypename == typeAndAssemblyName)
            {
                menuIndex = i;
            }
        }

        //Check to see if we have selected a new type in our dropdown menu 
        //This also sets the appearance of the dropdown itslef
        int newSelectedIndex = EditorGUI.Popup(new Rect(position.x + 5, position.y, 75, EditorGUIUtility.singleLineHeight), menuIndex, menuOptions);
        if (menuIndex != newSelectedIndex)
        {
            //Allows us to Undo through each iteration of previous actions
            Undo.RegisterCompleteObjectUndo(property.serializedObject.targetObject, "Type Changed");

            //Assign the type to the given type
            Type selectedType = subclasses[newSelectedIndex];

            //Changes the type of our modified property to the selected type
            //FormatterServices allows us to set it to a type w/o errors
            property.managedReferenceValue = FormatterServices.GetUninitializedObject(selectedType);

        }

        //Give the field proper space to render so it doesn't get squashed
        EditorGUI.PropertyField(new Rect(position.x, position.y, position.width, position.height), property, true);
    }

    /// <summary>
    /// Default way to get the height of a property in the GUI
    /// </summary>
    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        return EditorGUI.GetPropertyHeight(property, true);
    }

    /// <summary>
    /// Way to find Subclasses of a type you know beforehand/explicitly
    /// </summary>
    public List<Type> FindSubClasses<T>()
    {
        Type baseType = typeof(T);
        var assembly = baseType.Assembly;

        return assembly.GetTypes().Where(t => t.IsSubclassOf(baseType)).ToList();
    }

    /// <summary>
    /// Way to generically find Subclasses of a Type you have found
    /// </summary>
    /// <param name="baseType"></param>
    /// <returns></returns>
    public List<Type> FindSubClasses(Type baseType)
    {
        var assembly = baseType.Assembly;
        return assembly.GetTypes().Where(t => t.IsSubclassOf(baseType)).ToList();
    }
}
