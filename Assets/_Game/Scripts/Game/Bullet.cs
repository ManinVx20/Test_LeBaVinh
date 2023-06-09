using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : PoolableObject
{
    [Serializable]
    private enum Type
    {
        Bullet = 0,
        Orb = 1,
    }

    [SerializeField]
    private Type _type;
    [SerializeField]
    private float _flySpeed = 10.0f;
    [SerializeField]
    private float _despawnTime = 3.0f;
    [SerializeField]
    private TrailRenderer _trailRenderer;

    private Rigidbody2D _rb;
    private float _despawnTimer;
    private Character _owner;
    private float _damage;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        if (GameManager.Instance.IsGameWaited())
        {
            Destroy(gameObject);
        }
        else
        {
            _despawnTimer += Time.deltaTime;
            if (_despawnTimer >= _despawnTime)
            {
                Despawn();
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (Cache.TryGetHittableComponent2D(collider, out IHittable component))
        {
            if (component.IsHit(_owner, _damage))
            {
                Despawn();
            }
        }
    }

    public void Initialize(Vector3 position, Vector3 eulerAngles, Character owner, float damage)
    {
        GetTransform().position = position;
        GetTransform().eulerAngles = eulerAngles + GetTransform().eulerAngles;
        _owner = owner;
        _damage = damage;

        _rb.velocity = GetTransform().right * _flySpeed;
        _despawnTimer = 0.0f;
    }

    private void Despawn()
    {
        switch (_type)
        {
            case Type.Bullet:
                ResourceManager.Instance.BulletVFXPool.GetPrefabInstance().Initialize(GetTransform());

                break;
            case Type.Orb:
                ResourceManager.Instance.OrbtVFXPool.GetPrefabInstance().Initialize(GetTransform());

                break;
        }

        if (Origin.Exist())
        {
            if (_trailRenderer != null)
            {
                _trailRenderer.Clear();
            }

            ReturnToPool();
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
