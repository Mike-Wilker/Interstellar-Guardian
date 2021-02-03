using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(CharacterController))]
[RequireComponent(typeof(Animator))]
public class Enemy : MonoBehaviour
{
    Vector3 momentum = new Vector3();
    const float ACCELERATION = 10.0f;
    const float JUMP_FORCE = 15.0f;
    const float DRAG = 1.0f;
    const float AIR_DRAG = 0.25f;
    const float AIR_CONTROL = 2.0f;
    const float VISION_RANGE = 20.0f;
    const float JUMP_THRESHOLD = 5.0f;
    const float FALL_THRESHOLD = 10.0f;
    CharacterController controller;
    Animator animator;
    Player player;
    Vector3 initialPosition;
    void Start()
    {
        controller = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();
        player = FindObjectOfType<Player>();
        initialPosition = new Vector3(transform.position.x, transform.position.y, transform.position.z);
    }
    void Update()
    {
        if(Mathf.Abs(player.transform.position.x - transform.position.x) < VISION_RANGE && transform.position.y - player.transform.position.y < FALL_THRESHOLD)
        {
            if (player.transform.position.x < transform.position.x)
            {
                momentum += Vector3.left * (controller.isGrounded ? ACCELERATION : AIR_CONTROL) * Time.deltaTime;
            }
            if (player.transform.position.x > transform.position.x)
            {
                momentum += Vector3.right * (controller.isGrounded ? ACCELERATION : AIR_CONTROL) * Time.deltaTime;
            }
            if (player.transform.position.y > transform.position.y + JUMP_THRESHOLD && controller.isGrounded)
            {
                momentum += Vector3.up * JUMP_FORCE;
            }
        }
        momentum += Physics.gravity * Time.deltaTime;
        momentum *= 1.0f - (controller.isGrounded ? DRAG * Time.deltaTime : AIR_DRAG * Time.deltaTime);
        controller.Move(momentum * Time.deltaTime);
        animator.SetBool("InAir", !controller.isGrounded);
        animator.SetBool("FacingLeft", momentum.x < 0);
    }
    public void OnTriggerEnter(Collider collider)
    {
        if (collider.GetComponent<Player>() != null)
        {
            transform.position = initialPosition;
            collider.GetComponent<Player>().hurt();
            momentum = Vector3.zero;
        }
    }
    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if (hit.normal.y > 0)
        {
            momentum.y = 0.0f;
        }
        else if (hit.normal.y < 0)
        {
            momentum.y = -Mathf.Abs(momentum.y);
        }
        momentum -= new Vector3(Mathf.Abs(hit.normal.x) * momentum.x, 0.0f, Mathf.Abs(hit.normal.z) * momentum.z);
    }
    public void kill()
    {
        Destroy(gameObject);
    }
}
