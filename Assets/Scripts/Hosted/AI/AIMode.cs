using System;
using UnityEngine;

[Serializable]
public class AIMode : ICloneable
{
    [SerializeField] private Modes _mode;
    [SerializeField] private float _probability;
    [SerializeField] private float _duration;
    [SerializeField] private float _maxDurationDeviation;
    [SerializeField] private float _entitySpeed;
    [SerializeField] private float _entityShootDelay;
    [SerializeField] private float _entityShootSpeed;
    [SerializeField] private GameObject _entityAttackPrefab;
    [SerializeField] private float _increaseFactorForSpeed = 10f;
    [SerializeField] private bool _isEntityCanDodge = false;

    public Modes Mode => _mode;
    public float Probability => _probability;
    public float Duration => _duration;
    public float MaxDurationDeviation => _maxDurationDeviation;
    public float EntitySpeed => _entitySpeed;
    public float EntityShootDelay => _entityShootDelay;
    public float IncreaseFactorForSpeed => _increaseFactorForSpeed;
    public bool IsEntityCanDodge => _isEntityCanDodge;
    public GameObject EntityAttackPrefab => _entityAttackPrefab; 
    public float EntityShootSpeed => _entityShootSpeed;
    public float _nowDuration { get; set; }

    public enum Modes {
        PURSUIT,
        FREE,
        MEGAFIRE
    }

    public void RandomizeNowDuration() {
        _nowDuration = UnityEngine.Random.Range(_duration - _maxDurationDeviation, _duration + _maxDurationDeviation); 
    }

    public object Clone() {
        return new AIMode(
            _mode, _probability, _duration, _maxDurationDeviation, _entitySpeed, _entityShootDelay,
            _increaseFactorForSpeed, _isEntityCanDodge, _entityAttackPrefab, _entityShootSpeed
        );
    }

    public AIMode() {}

    public AIMode(
        Modes mode, float probability, float duration, float maxDurationDeviation, float entitySpeed, float entityShootDelay, 
        float increaseFactorForSpeed, bool isEntityCanDodge, GameObject entityAttackPrefab, float entityShootSpeed
    ) {
        _mode = mode;
        _probability = probability;
        _duration = duration;
        _maxDurationDeviation = maxDurationDeviation;
        _entitySpeed = entitySpeed;
        _entityShootDelay = entityShootDelay;
        _increaseFactorForSpeed = increaseFactorForSpeed;
        _isEntityCanDodge = isEntityCanDodge;
        _entityAttackPrefab = entityAttackPrefab;
        _entityShootSpeed = entityShootSpeed;
    }
}
