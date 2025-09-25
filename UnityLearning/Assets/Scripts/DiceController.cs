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


}
