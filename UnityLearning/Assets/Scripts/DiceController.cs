using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiceController : MonoBehaviour
{
    public System.Action<int, GameObject> OnDiceStopped;

    private Rigidbody rb;
    private bool hasStopped = false;
    private float stopThreshold = 0.1f;
    private float checkDelay = 1f;

    private GameObject floorObject;
    private int detectedValue = 0;
    private bool valueDetected = false;

    private Dictionary<Collider, int> faceTriggers = new Dictionary<Collider, int>();

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        StartCoroutine(CheckStopped());
    }

    public void Initialize(GameObject floor)
    {
        floorObject = floor;
        InitializeFaceTriggers();
    }

    private void InitializeFaceTriggers()
    {
        Collider[] colliders = GetComponentsInChildren<Collider>();
        int faceValue = 1;

        foreach (Collider collider in colliders)
        {
            if (collider.isTrigger)
            {
                faceTriggers.Add(collider, faceValue);
                //Debug.Log(collider.gameObject.name + faceValue);
                faceValue++;

                DiceFaceTrigger faceTrigger = collider.gameObject.AddComponent<DiceFaceTrigger>();
                faceTrigger.Initialize(this, faceTriggers[collider], floorObject);
            }
        }
    }

    private IEnumerator CheckStopped()
    {
        yield return new WaitForSeconds(checkDelay);

        while (!hasStopped)
        {
            if (rb.linearVelocity.magnitude < stopThreshold && rb.angularVelocity.magnitude < stopThreshold)
            {
                hasStopped = true;

                yield return new WaitForSeconds(1.5f);

                int diceValue = valueDetected ? detectedValue : 0;
                OnDiceStopped?.Invoke(diceValue, gameObject);
                yield break;
            }
            yield return new WaitForSeconds(0.1f);
        }
    }

    public void RegisterFloorContact(int faceValue, Collider triggerCollider)
    {
        if (faceTriggers.ContainsKey(triggerCollider))
        {
            float distanceToFloor = Mathf.Abs(triggerCollider.bounds.min.y - floorObject.transform.position.y);

            if (distanceToFloor <= 0.2)
            {
                detectedValue = faceValue;
                valueDetected = true;
            }
        }
    }


}
