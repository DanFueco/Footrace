using UnityEngine;

public class ResetPosModel : MonoBehaviour
{
    private Transform child;

    void Start()
    {
        if (transform.childCount > 0)
        {
            child = transform.GetChild(0);
            child.transform.localPosition = Vector3.zero;
        }
        
    }
}
