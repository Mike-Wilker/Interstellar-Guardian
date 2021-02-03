using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
[RequireComponent(typeof(CharacterController))]
[RequireComponent(typeof(Animator))]
public class Player : MonoBehaviour
{
    KeyCode jump = KeyCode.W;
    KeyCode left = KeyCode.A;
    KeyCode right = KeyCode.D;
    KeyCode stab = KeyCode.Space;
    Vector3 momentum = new Vector3();
    int lives = 3;
    const float ACCELERATION = 20.0f;
    const float JUMP_FORCE = 15.0f;
    const float DRAG = 1.0f;
    const float AIR_DRAG = 0.20f;
    const float AIR_CONTROL = 10.0f;
    CharacterController controller;
    Animator animator;
    Vector3 initialPosition;
    void Start()
    {
        controller = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();
        initialPosition = new Vector3(transform.position.x, transform.position.y, transform.position.z);
    }
    void Update()
    {
        if (Input.GetKey(left))
        {
            momentum += Vector3.left * (controller.isGrounded ? ACCELERATION : AIR_CONTROL) * Time.deltaTime;
        }
        if (Input.GetKey(right))
        {
            momentum += Vector3.right * (controller.isGrounded ? ACCELERATION : AIR_CONTROL) * Time.deltaTime;
        }
        if (Input.GetKeyDown(jump) && controller.isGrounded)
        {
            momentum += Vector3.up * JUMP_FORCE;
        }
        if (Input.GetKeyDown(stab))
        {
            animator.SetTrigger("CanSwing");
        }
        momentum += Physics.gravity * Time.deltaTime;
        momentum *= 1.0f - (controller.isGrounded ? DRAG * Time.deltaTime : AIR_DRAG * Time.deltaTime);
        controller.Move(momentum * Time.deltaTime);
        animator.SetBool("InAir", !controller.isGrounded);
        animator.SetBool("FacingLeft", momentum.x < 0.0f);
    }
    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if(hit.normal.y > 0)
        {
            momentum.y = 0.0f;
        }
        else if (hit.normal.y < 0)
        {
            momentum.y = -Mathf.Abs(momentum.y);
        }
        momentum -= new Vector3(Mathf.Abs(hit.normal.x) * momentum.x, 0.0f, Mathf.Abs(hit.normal.z) * momentum.z);
    }
    public void hurt()
    {
        transform.position = initialPosition;
        momentum = Vector3.zero;
        lives--;
        if (lives == 2)
        {
            GameObject.Find("Life3").SetActive(false);
        }
        else if (lives == 1)
        {
            GameObject.Find("Life2").SetActive(false);
        }
        else if (lives <= 0)
        {
            SceneManager.LoadScene("GameOver");
        }
    }
}
