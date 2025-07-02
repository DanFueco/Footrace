using Unity.VisualScripting;
using UnityEditor.Animations;
using UnityEngine;

public class AnimationController : MonoBehaviour
{
    private Transform child;
    public RuntimeAnimatorController animatorController;
    public Avatar avatar;

    public BaseMovement movementScript;


    void Start()
    {
        if (transform.childCount > 0)
        {
            child = transform.GetChild(0);
            child.AddComponent<Animator>();

            var animator = child.GetComponent<Animator>();
            animator.avatar = avatar;
            animator.runtimeAnimatorController = animatorController;

        }
    }

    void Update()
    {
        var velocity = movementScript.GetSpeed();
        child.GetComponent<Animator>().SetFloat("Speed", velocity);
    }
}
