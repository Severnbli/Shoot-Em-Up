using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class BackgroundCompliter
{
    [SerializeField] private GameObject _entity;
    [SerializeField] private int _totalCount;
    [SerializeField] private float _speed;
    [SerializeField] private float _scaleVariation;

    private List<GameObject> _entities = new List<GameObject>();

    public string GetTag() => _entity.tag;

    public int GetTotalCount() => _totalCount;

    public float GetSpeed() => _speed;

    public float GetScaleVariation() => _scaleVariation;

    public List<GameObject> GetEntities() => _entities;
}
