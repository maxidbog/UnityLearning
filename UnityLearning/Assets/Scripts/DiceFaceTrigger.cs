using UnityEngine;

public class DiceFaceTrigger : MonoBehaviour
{
    private DiceController diceController;
    private GameObject floorObject;
    private int value;

    public void Initialize(DiceController controller, int value, GameObject floorObject)
    {
        diceController = controller;
        this.value = value;
        this.floorObject = floorObject;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (diceController != null && other.gameObject == floorObject)
        {
            diceController.RegisterFloorContact(value, GetComponent<Collider>());
        }
    }
}
