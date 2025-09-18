using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;


public class CubeOrbitController : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    [SerializeField] private float orbitRadius = 5f;
    [Tooltip("Об/Мин")][SerializeField] private float rotationSpeed = 1f;
    [SerializeField] private bool isReversed = false;
    [SerializeField] private int cubeCount = 4;
    [SerializeField] private GameObject cubePrefab;
    [SerializeField] private DistributionType distributionType;
    [SerializeField] private int cubeInterval = 5;


    //private List<GameObject> orbitingCubes = new List<GameObject>();


    private void Awake()
    {
        for (int i = 0; i < cubeCount; i++)
        {
            GameObject cube = Instantiate(cubePrefab, transform);
            cube.transform.parent = transform;
            cube.transform.localPosition = CalculateOrbitPosition(GetDeviationAngle(i), orbitRadius);
            //orbitingCubes.Add(cube);
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
        int rotationDirection = isReversed ? -1 : 1;
        float rotationAngle = rotationSpeed * 360 / 60 / 50 * Mathf.Deg2Rad * rotationDirection;
        transform.RotateAroundLocal(new Vector3(0, 1, 0), rotationAngle);
    }

    private Vector3 CalculateOrbitPosition (float angle, float radius)
    {
        float x = Mathf.Cos(angle) * radius;
        float z = Mathf.Sin(angle) * radius;
        return new Vector3(x, 0, z);
    }

    private float GetDeviationAngle(int i)
    {
        float deviationAngle = 0;
        if (distributionType == DistributionType.Uniform)
            deviationAngle = i * 2 * Mathf.PI / cubeCount;
        else if (distributionType == DistributionType.Sequential)
            deviationAngle = i * cubeInterval * 2 * Mathf.PI / 360;
        return deviationAngle;
    }

    private enum DistributionType
    {
        Uniform,
        Sequential
    }
}
