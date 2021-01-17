using System;
using System.Collections;
using System.Collections.Generic;
using NUnit.Framework.Internal.Commands;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
	public float fireRate = 0.5F;
	public float speed;
	public float tilt;
	public Boundary boundary;
	public GameObject shot;
	public Transform shotSpawn;

    private DestroyByContact destroyByContact = new DestroyByContact();
	private float myTime = 0.0F;
	private float standbyTime = 0.5F;
	private Rigidbody rb;
	private AudioSource audioSource;

	void Start()
	{
		rb = GetComponent<Rigidbody>();
		audioSource = GetComponent<AudioSource>();
	}

	void Update()
	{
		myTime = myTime + Time.deltaTime;

        foreach (Touch touch in Input.touches)
        {
            if (Input.touchCount == 1)
            {
                if (touch.phase == TouchPhase.Began && myTime > standbyTime)
                {
                    standbyTime = myTime + fireRate;
                    Instantiate(shot, shotSpawn.position, shotSpawn.rotation);

                    audioSource.Play();

                    standbyTime -= myTime;
                    myTime = 0.1F;
                }
            }
        }

    }

    void FixedUpdate()
	{
		if (Input.touchCount > 0 && destroyByContact.moveLock == false)
        {
            Touch touch = Input.GetTouch(0);
            if (touch.phase==TouchPhase.Moved)
            {
                
				Vector3 vec = touch.deltaPosition;
				transform.Translate(new Vector3(vec.x*speed,0,vec.y*speed));

                transform.position = new Vector3(
                    Mathf.Clamp(transform.position.x, boundary.xMin, boundary.xMax),
                    0,
                    Mathf.Clamp(transform.position.z, boundary.zMax, boundary.zMin));

                transform.rotation = Quaternion.Euler(
                    0.0f,
                    0.0f,
                    rb.velocity.x * -tilt
                );
            }

		}

    }
}
