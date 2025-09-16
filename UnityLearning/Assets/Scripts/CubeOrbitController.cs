using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;


public class CubeOrbitController : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    [SerializeField] private float orbitRadius = 5f;
    [Tooltip("Об/Мин")][SerializeField] private float rotationSpeed = 1f;
    [SerializeField] private int cubeCount = 4;
    [SerializeField] private GameObject cubePrefab;

    private List<GameObject> orbitingCubes = new List<GameObject>();
    private float rotationAngle;


    private void Awake()
    {
        for (int i = 0; i < cubeCount; i++)
        {
            GameObject cube = Instantiate(cubePrefab, transform);
            cube.transform.parent = transform;
            cube.transform.localPosition = CalculateOrbitPosition(i * 2 * Mathf.PI / cubeCount, orbitRadius);
            orbitingCubes.Add(cube);
        }
    }

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }

    void FixedUpdate()
    {
        rotationAngle = rotationSpeed * 360 / 60 / 50 * Mathf.Deg2Rad;
        transform.RotateAroundLocal(new Vector3(0, 1, 0), rotationAngle);
    }

    private Vector3 CalculateOrbitPosition (float angle, float radius)
    {
        float x = Mathf.Cos(angle) * radius;
        float z = Mathf.Sin(angle) * radius;
        return new Vector3(x, 0, z);
    }
}
