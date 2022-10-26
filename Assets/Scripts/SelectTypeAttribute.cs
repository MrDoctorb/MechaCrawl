using UnityEngine;

/// <summary>
/// Add [SelectType] and [SerializeReference] to a base class reference to add a dropdown menu with all subclasses of the base class
/// </summary>
public class SelectTypeAttribute : PropertyAttribute
{
	public SelectTypeAttribute() : base()
	{
	}
}
