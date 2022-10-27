using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public List<SpriteRenderer> sprites = new List<SpriteRenderer>();

    private void Update()
    {
        foreach (SpriteRenderer sr in sprites)
        {
            sr.sortingOrder = -(int)(sr.transform.position.y * 100);
        }
    }
}
