using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlatesCounterVisual : MonoBehaviour
{
    [SerializeField] private PlatesCounter platesCounter;
    [SerializeField] private Transform counterTop;
    [SerializeField] private GameObject plateVisualPrefab;

    private List<GameObject> _platesList;

    private void Awake()
    {
        _platesList = new List<GameObject>();
    }

    private void Start()
    {
        platesCounter.OnPlateSpawned += SpawnPlate;
        platesCounter.OnPlateRemoved += RemovePlate;
    }

    private void SpawnPlate()
    {
        var plateVisualSpawned = Instantiate(plateVisualPrefab, counterTop);

        var plateOffsetY = 0.1f;

        plateVisualSpawned.transform.localPosition = new Vector3(0, plateOffsetY * _platesList.Count, 0);

        _platesList.Add(plateVisualSpawned);
    }

    private void RemovePlate()
    {
        var lastItem = _platesList.Last();
        _platesList.Remove(lastItem);
        Destroy(lastItem);
    }
}
