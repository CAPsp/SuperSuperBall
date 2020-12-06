using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectExplosionBehaviour : MonoBehaviour
{
    #region フィールド

    private CircleCollider2D _Collider;

    private float _TimeSec;

    #endregion // フィールド

    // Start is called before the first frame update
    void Awake()
    {
        _Collider = gameObject.GetComponent<CircleCollider2D>();
        _TimeSec = 0.3f;
    }

    // Update is called once per frame
    void Update()
    {
        if(_TimeSec > 0f)
        {
            _TimeSec -= Time.deltaTime;
            if (_TimeSec <= 0f)
            {
                _Collider.enabled = true;
            }
        }
    }
}
