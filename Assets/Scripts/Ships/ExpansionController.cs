using System.Collections.Generic;
using UnityEngine;

public class ExpansionController : MonoBehaviour
{
    [SerializeField] private List<SpawnController> _spawnControllers;
    [SerializeField] private string _scrollAxis = "Mouse ScrollWheel";
    [SerializeField] private float _maxExpansion;
    [SerializeField] private float _minExpansion = 0;
    [SerializeField] private float _expansionStep;

    private float _nowExpansion = 0;

    void Update()
    {
        float scroll = Input.GetAxis(_scrollAxis);
        float expansionParameter = 0;

        if (scroll > 0) {
            if (_nowExpansion < _maxExpansion) {
                expansionParameter = _expansionStep;
                _nowExpansion += _expansionStep;
            }
        } else if (scroll < 0) {
            if (_nowExpansion > -_minExpansion) {
                expansionParameter = -_expansionStep;
                _nowExpansion -= _expansionStep;
            }
        }

        foreach (var spawnController in _spawnControllers) {
            spawnController.Expansion(expansionParameter);
        }
    }
}
