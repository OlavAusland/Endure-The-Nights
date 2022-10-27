using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrainingDummy : MonoBehaviour
{
    private int _health = 100;

    public int Health
    {
        get { return _health; }

        set
        {
            Utilities.Health.TakeDamage(transform, Health - value);
            _health = value;
        }
    }
}
