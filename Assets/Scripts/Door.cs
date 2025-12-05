using UnityEngine;

public class Door : MonoBehaviour
{
    [SerializeField] private GameObject door;
    
    public void OpenDoor()
    {
        door.transform.localRotation = Quaternion.Euler(0, 42, 0);
    }

    public void CloseDoor()
    {
        door.transform.localRotation = Quaternion.Euler(0, -90, 0);
    }
}
