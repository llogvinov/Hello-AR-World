using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

[RequireComponent(typeof(ARTrackedImageManager))]
public class ImageTracking : MonoBehaviour
{
    [SerializeField] private GameObject[] _placeablePrefabs;

    private Dictionary<string, GameObject> _spawnedPrefabs = new Dictionary<string, GameObject>();
    private ARTrackedImageManager _trackedImageManager;

    private void Awake()
    {
        _trackedImageManager = GetComponent<ARTrackedImageManager>();

        foreach (GameObject prefab in _placeablePrefabs)
        {
            GameObject newPrefab = Instantiate(prefab, Vector3.zero, Quaternion.identity);
            newPrefab.name = prefab.name;
            _spawnedPrefabs.Add(prefab.name, newPrefab);
            newPrefab.SetActive(false);
        }
    }

    private void OnEnable()
    {
        _trackedImageManager.trackedImagesChanged += OnImageChanged;
    }

    private void OnDisable()
    {
        _trackedImageManager.trackedImagesChanged -= OnImageChanged;
    }

    private void OnImageChanged(ARTrackedImagesChangedEventArgs eventArgs)
    {
        foreach (ARTrackedImage trackedImage in eventArgs.added)
        {
            UpdateImage(trackedImage);
        }
        foreach (ARTrackedImage trackedImage in eventArgs.updated)
        {
            UpdateImage(trackedImage);
        }
        foreach (ARTrackedImage trackedImage in eventArgs.removed)
        {
            _spawnedPrefabs[trackedImage.name].SetActive(false);
        }
    }

    private void UpdateImage(ARTrackedImage trackedImage)
    {
        string name = trackedImage.referenceImage.name;
        Vector3 position = trackedImage.transform.position;

        GameObject prefab = _spawnedPrefabs[name];
        prefab.transform.position = position;
        prefab.SetActive(true);

        foreach (GameObject go in _spawnedPrefabs.Values)
        {
            if (go.name != name || 
                trackedImage.trackingState == TrackingState.None || 
                trackedImage.trackingState == TrackingState.Limited) 
            {
                go.SetActive(false);
            }
        }
    }
}
