using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(Animator))]
public class PlayerController : MonoBehaviour
{
    public float maxHp;
    float hp;
    public float maxSpeed;
    public float acceleration;
    private float currentSpeed;
    public float rotationSpeed;
    private bool isDead;

    Rigidbody rb;
    Animator animCtrl;

    // Start is called before the first frame update
    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        animCtrl = GetComponent<Animator>();

        animCtrl.SetFloat("Speed", 0);
        animCtrl.SetFloat("HP", 1);

        hp = 10;
        isDead = false;
    }

    // Update is called once per frame
    void Update()
    {
        if(hp <= 0 && !isDead)
        {
            isDead = true;
            animCtrl.SetTrigger("Dead");
        }

        if (isDead)
        {
            if (Input.GetKey(KeyCode.Space))
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().name);
            }
        }
    }

    private void FixedUpdate()
    {
        if (!isDead)
        {
            UpdatePlayerVelocity();
            UpdateAnimationSpeed();
        }
    }

    private void UpdatePlayerVelocity()
    {
        //TODO -> add drag when turning and reducing when not
        if (Input.GetKey(KeyCode.W))
        {
            rb.AddRelativeForce(new Vector3(0, 0, acceleration) * Mathf.Lerp(maxSpeed, 0, Mathf.Clamp01(rb.velocity.magnitude / maxSpeed)), ForceMode.Force);
        }
        if (Input.GetKey(KeyCode.A))
        {
            transform.eulerAngles += new Vector3(0, -1, 0) * Time.fixedDeltaTime * rotationSpeed;
        }
        if (Input.GetKey(KeyCode.D))
        {
            transform.eulerAngles += new Vector3(0, 1, 0) * Time.fixedDeltaTime * rotationSpeed;
        }

        currentSpeed = rb.velocity.magnitude;
        currentSpeed = Mathf.Clamp(currentSpeed, 0, maxSpeed);
    }

    private void UpdateAnimationSpeed()
    {
        //Speed from physics on the RigidBody, multiplied by 1.5 for the speed of the animation
        animCtrl.SetFloat("Speed", (currentSpeed * 1.5f) / maxSpeed);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            hp = hp - 5f;
            animCtrl.SetFloat("HP", hp / maxHp);
            Debug.Log(hp/maxHp);
        }
    }
}
