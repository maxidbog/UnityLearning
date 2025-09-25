using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class DiceGame : MonoBehaviour
{
    [Header("Dice Settings")]
    [SerializeField] private GameObject dicePrefab;
    [SerializeField] private Transform diceSpawnPoint;
    [SerializeField] private int numberOfDice = 2;
    [SerializeField] private Vector2 throwForceRange = new Vector2(10f, 15f);
    [SerializeField] private Vector2 torqueForceRange = new Vector2(5f, 10f);
    [SerializeField] private GameObject floorObject;

    private List<GameObject> Dices = new List<GameObject>();
    private int diceStoppedCount = 0;
    private int totalScore = 0;

    // Start is called once before the first execution of Update after the MonoBehaviour is created

    private void Awake()
    {
        CreateDices();
    }

    public void StartRoll()
    {
        Debug.Log("pressed");
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void CreateDices()
    {
        for (int i = 0; i < numberOfDice; i++)
        {
            Vector3 spawnOffset = new Vector3(
                i % 3 * 2,
                1,
                i / 3 * 2
            );

            GameObject dice = Instantiate(dicePrefab, diceSpawnPoint.position + spawnOffset, transform.rotation);
            dice.transform.SetParent(transform);
            Dices.Add(dice);
            DiceController diceController = dice.GetComponent<DiceController>();
            if (diceController == null)
            {
                diceController = dice.AddComponent<DiceController>();
            }
            diceController.Initialize(floorObject);
        }
    }
}
