using System;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;
using Vector3 = UnityEngine.Vector3;

public class ItemInformation : MonoBehaviour
{
    private SpriteRenderer _sr {get {return GetComponent<SpriteRenderer>();}}
    public Item item;

    public void Start(){ _sr.sprite = item.sprite; }

}