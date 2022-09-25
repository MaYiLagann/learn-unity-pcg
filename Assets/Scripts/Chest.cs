using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chest : MonoBehaviour
{
    public Sprite openSprite;
    public SpriteRenderer spriteRenderer;

    public void Open()
    {
        spriteRenderer.sprite = openSprite;

        // Todo: Create a random item

        gameObject.layer = 10;
        spriteRenderer.sortingLayerName = "Items";
    }
}
