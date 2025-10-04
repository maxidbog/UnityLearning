using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class DiceGame : MonoBehaviour
{
    [Header("Dice Settings")]
    [SerializeField] private GameObject dicePrefab;
    [SerializeField] private Transform diceSpawnPoint;
    [SerializeField] private int dicesCount = 2;
    [SerializeField] private Vector2 throwForceRange = new Vector2(10f, 15f);
    [SerializeField] private Vector2 torqueForceRange = new Vector2(5f, 10f);
    [SerializeField] private GameObject floorObject;
    [SerializeField][Range(1,10)] private int spawnSpace = 4;
    [SerializeField][Range(1, 10)] private int spawnHeight = 1;
    [SerializeField][Range(1, 5)] private int spawnStack = 3;
    [SerializeField] private int minDrawValue = 6;
    [SerializeField] private int minWinValue = 8;


    private bool isRolling = false;
    private List<GameObject> Dices = new List<GameObject>();
    private int diceStoppedCount = 0;
    private int totalScore = 0;

    public UnityEvent OnValuesChanged;

    public int DicesCount
    {
        get => dicesCount;
        set 
        {
            dicesCount = value; 
            CalculateDefaultConditions();
            OnValuesChanged.Invoke();
        }
    }
    public int TotalScore
    {
        get => totalScore;
    }
    public int MinDrawValue
    {
        get => minDrawValue;
        set { minDrawValue = value; OnValuesChanged.Invoke(); }
    }
    public int MinWinValue
    {
        get => minWinValue;
        set { minWinValue = value; OnValuesChanged.Invoke(); }
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    public void StartRoll()
    {
        Debug.Log("pressed");
        if (!isRolling)
            StartCoroutine(RollDices());
    }

    private void CalculateDefaultConditions()
    {
        minDrawValue = dicesCount * 3;
        minWinValue = dicesCount * 3 + 1;
    }

    private void CreateDices()
    {
        for (int i = 0; i < dicesCount; i++)
        {
            Vector3 spawnOffset = new Vector3(
                i % spawnStack * spawnSpace,
                spawnHeight,
                i / spawnStack * spawnSpace
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
            diceController.OnDiceStopped += OnDiceStopped;
        }
    }

    private void ForceDices()
    {
        foreach (var dice in Dices)
        {
            Rigidbody rb = dice.GetComponent<Rigidbody>();
            if (rb != null)
            {
                Vector3 Direction = new Vector3(
                    Random.Range(-1f, 1f),
                    Random.Range(0.8f, 1f),
                    Random.Range(-1f, 1f)
                ).normalized;

                float throwForce = Random.Range(throwForceRange.x, throwForceRange.y);
                float torqueForce = Random.Range(torqueForceRange.x, torqueForceRange.y);

                rb.AddForce(Direction * throwForce, ForceMode.Impulse);
                rb.AddTorque(Random.insideUnitSphere * torqueForce, ForceMode.Impulse);
            }
        }
    }

    private IEnumerator RollDices ()
    {
        isRolling = true;
        totalScore = 0;
        diceStoppedCount = 0;

        ClearCurrentDices();
        CreateDices();
        ForceDices();
        OnValuesChanged.Invoke();

        yield return new WaitUntil(() => diceStoppedCount >= dicesCount);

        Debug.Log($"Total score: {totalScore}");
        isRolling = false;
    }

    private void OnDiceStopped(int diceValue, GameObject dice)
    {
        diceStoppedCount++;
        totalScore += diceValue;
        OnValuesChanged.Invoke();
        Debug.Log($"Кубик остановился на {diceValue}");
    }

    private void ClearCurrentDices()
    {
        foreach (GameObject dice in Dices)
        {
            if (dice != null)
            {
                Destroy(dice);
            }
        }
        Dices.Clear();
    }
}
