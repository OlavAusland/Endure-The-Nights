using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class FloatingNumber : MonoBehaviour
{
    public float value { set { text.text = value.ToString(); value = value;} }
    public float speed = 1;
    public AnimationCurve fadeinCurve;
    public AnimationCurve fadeoutCurve;
    public Color color;
    public Vector4 spawnOffset;

    TextMeshProUGUI text { get{ return GetComponent<TextMeshProUGUI>(); } }
    RectTransform rect { get { return GetComponent<RectTransform>(); } }

    private void Start(){
        transform.position += new Vector3(Random.Range(spawnOffset.x, spawnOffset.y), 
            Random.Range(spawnOffset.z, spawnOffset.w), 0);
        StartCoroutine(AnimateFadein());
    }

    private IEnumerator AnimateFadein()
    {
        transform.localScale = Vector3.zero;
        text.color = color;
        float time = fadeinCurve[fadeinCurve.length - 1].time;
        Vector3 size = new Vector3(0.0075f, 0.0075f, 0.0075f);
        
        
        while (time > 0)
        {
            transform.localScale = Vector3.Lerp(transform.localScale, size, Time.deltaTime / time);
            time -= Time.deltaTime;
            yield return null;
        }
        StartCoroutine(AnimateFadeout());
        yield return null;
    }
    
    private IEnumerator AnimateFadeout()
    {
        text.color = color;
        float time = fadeoutCurve[fadeoutCurve.length - 1].time;
        while(time > 0){
            text.color = Color.Lerp(text.color, new Color(text.color.r, text.color.g, text.color.b, 0), Time.deltaTime / time);
            rect.anchoredPosition = Vector2.Lerp(rect.anchoredPosition, rect.anchoredPosition + new Vector2(0, (fadeoutCurve.Evaluate(fadeoutCurve[fadeoutCurve.length - 1].time - time) * speed)), Time.deltaTime);
            time -= Time.deltaTime;
            yield return null;
        }
        Destroy(this.gameObject);
        yield return null;
    }
    
}
