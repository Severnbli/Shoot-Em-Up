using System.Collections.Generic;
using UnityEngine;

public class BackgroundController : MonoBehaviour
{
    [SerializeField] BackgroundCompliter[] _backgroundObjects;
    [SerializeField] float _treshHold = 2f;
    [SerializeField] float _cameraMovementFactor = 10f;

    private Vector3 _lastCameraPosition;
    private Vector3 _cameraMovement;
    private Dictionary<string, Vector3> _cameraBounds;
    

    private void Start()
    {
        _lastCameraPosition = Camera.main.transform.position;
        _cameraBounds = Utils.CalculateMainCameraBounds();
        SpawnBackground();
    }

    private void FixedUpdate() {
        if (Camera.main.transform.position != _lastCameraPosition) {
            _cameraBounds = Utils.CalculateMainCameraBounds();
            _cameraMovement = Camera.main.transform.position - _lastCameraPosition;
            _lastCameraPosition = Camera.main.transform.position;
        }

        foreach (var bObject in _backgroundObjects) {
            var objectsArray = bObject.GetEntities();

            for (int i = objectsArray.Count - 1; i >= 0; i--) {
                var entityController =  objectsArray[i].GetComponent<BackgroundEntityController>();

                if (entityController) {
                    entityController.SetHorizontalMovement(_cameraMovement.x * _cameraMovementFactor * -entityController.GetSpeed());
                }

                if (!IsInBounds(objectsArray[i])) {
                    ObjectPooling.PushObject(objectsArray[i]);
                    objectsArray.RemoveAt(i);
                    AddObject(bObject, GetRandomPreferentialSpawnPosition());
                }
            }
        }
    }

    private void SpawnBackground() {
        foreach(var bObject in _backgroundObjects) {
            for (int i = 0; i < bObject.GetTotalCount(); i++) {
                AddObject(bObject, GetRandomSpawnPosition()); 
            }
        }
    }

    private void AddObject(BackgroundCompliter bObject, Vector3 position) {
        GameObject newObject = ObjectPooling.PopObject(bObject.GetTag(), position);
                
        if (!newObject) {
            Debug.LogError($"{gameObject.name}: Object Pooling mistake. Tag: {bObject.GetTag()}!");
            return;
        }
        
        bObject.GetEntities().Add(newObject);

        var entityController = newObject.GetComponent<BackgroundEntityController>();

        if (entityController) {
            if (bObject.GetSpeed() != 0) {
                entityController.SetSpeed(bObject.GetSpeed());
            }

            if (bObject.GetScaleVariation() != 0) {
                entityController.SetScaleVariation(bObject.GetScaleVariation());
            }

            entityController.Setup();
        } else {
            Debug.LogError($"{gameObject.name}: Background Controller: {newObject.name} has no component " + 
            "Background Entity Controller!");
        }
    }

    private bool IsInBounds(GameObject entity) {
        return entity.transform.position.y > _cameraBounds["bottomLeft"].y - _treshHold &&
                entity.transform.position.x > _cameraBounds["bottomLeft"].x - _treshHold &&
                entity.transform.position.x < _cameraBounds["bottomRight"].x + _treshHold;
    }

    private Vector3 GetRandomSpawnPosition() {
        return new Vector3(
            Random.Range(_cameraBounds["bottomLeft"].x - _treshHold, _cameraBounds["bottomRight"].x + _treshHold),
            Random.Range(_cameraBounds["bottomLeft"].y, _cameraBounds["topLeft"].y + _treshHold),
            0
        );
    }

    Vector3 GetRandomPreferentialSpawnPosition() {
        SpawnDirection direction;

        if (_cameraMovement.x > 0) {
            direction = GetRandomSpawnDirectionBesideOne(SpawnDirection.LEFT);
        } else if (_cameraMovement.x < 0) {
            direction = GetRandomSpawnDirectionBesideOne(SpawnDirection.RIGHT);
        } else {
            direction = GetRandomSpawnDirection();
        }

        switch (direction) {
            default:
            case SpawnDirection.UP: {
                return new Vector3(
                    Random.Range(_cameraBounds["bottomLeft"].x - _treshHold, _cameraBounds["bottomRight"].x + _treshHold),
                    Random.Range(_cameraBounds["topRight"].y, _cameraBounds["topRight"].y + _treshHold),
                    0
                );
            }

            case SpawnDirection.LEFT: {
                return new Vector3(
                    Random.Range(_cameraBounds["bottomLeft"].x - _treshHold, _cameraBounds["bottomLeft"].x),
                    Random.Range(_cameraBounds["bottomLeft"].y - _treshHold, _cameraBounds["topLeft"].y + _treshHold),
                    0
                );
            }

            case SpawnDirection.RIGHT: {
                return new Vector3(
                    Random.Range(_cameraBounds["bottomRight"].x, _cameraBounds["bottomRight"].x + _treshHold),
                    Random.Range(_cameraBounds["bottomRight"].y - _treshHold, _cameraBounds["topRight"].y + _treshHold),
                    0
                );
            }
        }
    }

    private enum SpawnDirection {
        UP,
        RIGHT,
        LEFT
    }
    
    private SpawnDirection GetRandomSpawnDirection() {
        List<SpawnDirection> preferentialDirections = new List<SpawnDirection> {
            SpawnDirection.UP,
            SpawnDirection.RIGHT,
            SpawnDirection.LEFT
        };

        return preferentialDirections[Random.Range(0, preferentialDirections.Count)];
    }

    private SpawnDirection GetRandomSpawnDirectionBesideOne(SpawnDirection excludedDirection) {
        List<SpawnDirection> preferentialDirections = new List<SpawnDirection> {
            SpawnDirection.UP,
            SpawnDirection.RIGHT,
            SpawnDirection.LEFT
        };
        
        preferentialDirections.Remove(excludedDirection);

        return preferentialDirections[Random.Range(0, preferentialDirections.Count)];
    }


}
