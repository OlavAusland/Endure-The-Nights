using UnityEngine;

namespace Utilities
{
    public class Health : MonoBehaviour
    {
        private static readonly GameObject Canvas = GameObject.Find("Canvas");
        
        private static readonly GameObject FloatingNumber = Resources.Load<GameObject>("UI/FloatingNumber");
        private static readonly GameObject HealingEffect = Resources.Load<GameObject>("Particles/Heal");
        private static readonly GameObject HitEffect = Resources.Load<GameObject>("Particles/Blood");
        public static void TakeDamage(Transform transform, int value)
        {
            if(value < 0){Heal(transform, value * -1); return; }
            FloatingNumber number = Instantiate(FloatingNumber, transform.position, Quaternion.identity, GameObject.Find("Canvas").transform).GetComponent<FloatingNumber>();
            number.value = value;
            number.color = Color.red;
        }

        public static void Heal(Transform transform, int value)
        {
            FloatingNumber number = Instantiate(FloatingNumber, transform.position, Quaternion.identity, GameObject.Find("Canvas").transform).GetComponent<FloatingNumber>();
            number.value = value;
            number.color = Color.green;
            
            //HealEffect(transform, 0.5f);
        }

        private static void HealEffect(Transform transform, float ttl)
        {
            GameObject effect = Instantiate(HealingEffect, transform.position, Quaternion.identity, Canvas.transform);
            Destroy(effect, ttl);
        }
        
        private static void DamageEffect(Transform transform, float ttl)
        {
            GameObject effect = Instantiate(HitEffect, transform.position, Quaternion.identity, Canvas.transform);
            Destroy(effect, ttl);
        }
    }
}