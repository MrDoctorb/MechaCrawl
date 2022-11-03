using UnityEngine;
using Zanespace;

[ExecuteInEditMode]
[RequireComponent (typeof(UnitController))]
public class UnitOutline : MonoBehaviour
{
    Color color;

    private SpriteRenderer spriteRenderer;

    public void OnEnable()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        UnitController me = GetComponent<UnitController>();
        if(me is EnemyController)
        {
            color = References.enemyOutline;
        }
        else
        {
            color = References.allyOutline;
        }
        UpdateOutline(true);

    }

    void OnDisable()
    {
        UpdateOutline(false);
    }

    void Update()
    {
        UpdateOutline(true);
    }

    void UpdateOutline(bool outline)
    {
        MaterialPropertyBlock material = new MaterialPropertyBlock();
        spriteRenderer.GetPropertyBlock(material);
        material.SetColor("_OutlineColor", color);
        spriteRenderer.SetPropertyBlock(material);
    }
}