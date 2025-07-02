using UnityEngine;

public class ThirdPersonCamera : MonoBehaviour
{
    public Transform target;       
    public Transform orientation;
    public Transform playerBody;

    public Transform camera;
     
    public float distance = 5f;      
    public float height = 2f;      
    public float sensitivity = 100f; 

    float yaw;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void LateUpdate()
    {
        float mouseX = Input.GetAxis("Mouse X") * sensitivity * Time.deltaTime;

        // On ne modifie que le yaw (rotation horizontale)
        yaw += mouseX;

        // Appliquer la rotation au pivot
        transform.rotation = Quaternion.Euler(17, yaw, 0f);
        playerBody.rotation = Quaternion.Euler(0f, yaw, 0f);

        // Positionner le pivot sur le joueur
        transform.position = target.position;

        // Calculer la position de la caméra
        camera.position = transform.position - transform.forward * distance + Vector3.up * height;
        camera.LookAt(target.position + Vector3.up * 1.5f);

        // Appliquer la direction au GameObject "Orientation"
        if (orientation != null)
            orientation.rotation = Quaternion.Euler(0, yaw, 0);

        

        
    }
}
