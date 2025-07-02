using UnityEngine;

public class PlayerRotation : MonoBehaviour
{
    public Transform orientation;
    public Transform playerModel;
    void Update()
    {
        playerModel.rotation = orientation.rotation;
    }
}
