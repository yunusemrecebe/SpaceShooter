using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletController : MonoBehaviour
{
	public float speed;

	private Rigidbody bullet;

	void Start()
	{
		bullet = GetComponent<Rigidbody>();

		bullet.velocity = transform.forward * speed;
	}
}
