using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerEquipment : MonoBehaviour
{
    private SpriteRenderer _sr {get{return GetComponent<SpriteRenderer>();}}
    
    private Texture2D _result;
    public Sprite baseSprite;

    public List<Sprite> equipment;

    private void Start() { _result = new Texture2D(_sr.sprite.texture.width, _sr.sprite.texture.height);}

    public void Combine(List<Sprite> sprites)
    {
        print("Equipment");
        _sr.sprite = baseSprite;

        foreach (Sprite sprite in sprites)
        {
            for (int x = 0; x < sprite.texture.width; x++)
            {
                for (int y = 0; y < sprite.texture.height; y++)
                {
                    var colorA = sprite.texture.GetPixel(x, y);
                    var colorB = _sr.sprite.texture.GetPixel(x, y);
                    
                    if(colorA.a != 0) {_result.SetPixel(x, y, colorA);}
                    else if (colorB.a != 0) {_result.SetPixel(x, y, colorB);}
                    else{_result.SetPixel(x, y, Color.clear);}
                }
            }

            _result.filterMode = FilterMode.Point;
            _result.Apply();

            _sr.sprite = Sprite.Create(_result, new Rect(0, 0, _result.width, _result.height), 
                new Vector2(0.5f, 0.5f), baseSprite.pixelsPerUnit);
        }
    }
}
