using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rabbit : MonoBehaviour
{
    private Rigidbody rb;
    private Animator anim;
    [SerializeField] private float flyForce = 5f; // lực bay, chỉnh trong Inspector

    private void Awake()
    {
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody>();
    }

    public void Fly()
    {
        if (rb != null)
        {
            anim.Play("Armature|Action");

            // reset velocity để không bị cộng dồn quá mạnh
            rb.velocity = Vector3.zero;

            // Add lực bay theo trục Y
            rb.AddForce(Vector3.up * flyForce, ForceMode.Impulse);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Ground"))
        {
            UIManager.Ins.TransitionUI<ChangeUICanvas, MainCanvas>(0.5f,
                () =>
                {
                    LevelManager.Ins.DespawnLevel();
                    UIManager.Ins.OpenUI<LooseCanvas>();
                });
        }
    }
}
