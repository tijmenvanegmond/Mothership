using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AsteroidSpawner : MonoBehaviour
{

    public GameObject AsteroidPrefab;
    public Vector3 SpawnFieldSize;
    public int Amount = 10;
    [Tooltip("Define the distribution of asteroid scales")]
    public AnimationCurve SizeDistribution = AnimationCurve.Linear(0, 1, 100, 100);
    public float TotalSize
    {
        get { return SpawnFieldSize.x * SpawnFieldSize.y * SpawnFieldSize.z; }
    }

    void Awake()
    {
        for (int i = 0; i < Amount; i++)
        {
            var rotation = Quaternion.Euler(new Vector3(Random.value * 360f, Random.value * 360f, Random.value * 360f));
            float length = Random.Range(0, 300f);
            GameObject asteroid = Instantiate(AsteroidPrefab, transform);

            asteroid.transform.localPosition = rotation * (Vector3.forward * length);
            asteroid.transform.rotation = rotation;
            float scale = SizeDistribution.Evaluate(Random.value * 100f);
            asteroid.transform.localScale = Vector3.one * scale;
            asteroid.GetComponent<Rigidbody>().mass *= scale;

        }
    }
}
