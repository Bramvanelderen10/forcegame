using UnityEngine;
using System.Collections;

public class InputManager : MonoBehaviour {

    public float _baseVelocity = 5f;
    public float _maxChargeTime = 1.5f;
    public float _chargeTimeMultiplier = 8f;

    private float _startTime;
    private bool _isCharging = false;
    private float _chargeTime;

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            _isCharging = true;
            _startTime = Time.time;
        }

        if (Input.GetMouseButtonUp(0) && _isCharging)
        {
            _isCharging = false;
            _chargeTime = Time.time - _startTime;
            _chargeTime = (_chargeTime > _maxChargeTime) ? _maxChargeTime : _chargeTime;

            Vector3 pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            pos.z = 0;
            PushObjects(pos);
        }
    }

    void PushObjects(Vector2 pos)
    {

        float velocity = _chargeTime * _chargeTimeMultiplier + _baseVelocity;
        float radius = 1f + (_chargeTime * 2f);

        var hits = Physics2D.OverlapCircleAll(pos, radius, 1 << LayerMask.NameToLayer("Movable"));

        for (int i = 0; i < hits.Length; i++)
        {
            Transform tf = hits[i].transform;

            Vector2 v1 = pos;
            Vector2 v2 = tf.position;

            float distanceMultiplier = Vector2.Distance(v1, v2);

            var force = (v2 - v1) * (velocity / distanceMultiplier);

            tf.GetComponent<Rigidbody2D>().velocity = force;
        }
    }
}
