using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

[RequireComponent(typeof(ARRaycastManager))]
public class ARMultipleObjectSpawning : MonoBehaviour
{
    [SerializeField] private int _maxPrefabsSpawnCount;
    private int _placedPrefabsCount;
    
    [SerializeField] private GameObject _prefabToInstantiate;
    private GameObject _spawnedPrefab;
    private List<GameObject> _placedPrefabsList = new List<GameObject>();
    
    private ARRaycastManager _arRaycastManager;
    private Vector2 _touchPosition;
    private static List<ARRaycastHit> _hits = new List<ARRaycastHit>();

    private void Awake()
    {
        _arRaycastManager = GetComponent<ARRaycastManager>();
    }

    public void SetPrefab(GameObject prefab)
    {
        _prefabToInstantiate = prefab;
    }
    
    private void Update()
    {
        if (!TryGetTouchPosition(out Vector2 touchPosition))
        {
            return;
        }

        if (_arRaycastManager.Raycast(touchPosition, _hits, TrackableType.PlaneWithinPolygon))
        {
            var hitPose = _hits[0].pose;

            if (_placedPrefabsCount < _maxPrefabsSpawnCount)
            {
                SpawnPrefab(hitPose);
            }
        }
    }

    private bool TryGetTouchPosition(out Vector2 touchPosition)
    {
        if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)
        {
            touchPosition = Input.GetTouch(0).position;
            return true;
        }

        touchPosition = default;
        return false;
    }

    private void SpawnPrefab(Pose hitPose)
    {
        _spawnedPrefab = Instantiate(_prefabToInstantiate, hitPose.position, hitPose.rotation);
        _placedPrefabsList.Add(_spawnedPrefab);
        _placedPrefabsCount++;
    }
}

