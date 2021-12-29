using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class jayantCamera : MonoBehaviour
{
	public static jayantCamera instance;
	public Transform target;

	public float smoothSpeed = 0.125f;
	public Vector3 offset;

	bool backToNormal = false;
	Quaternion lastRot;// = new Vector3();
    private void Awake()
    {
		instance = this;
		target = FindObjectOfType<jayantPlayer>().gameObject.transform;
	}
    void Update()
	{
		Vector3 desiredPosition = target.position + offset;
		Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);
		transform.position = smoothedPosition;

        if (backToNormal)
        {
			transform.rotation = Quaternion.Slerp(transform.rotation, lastRot,Time.deltaTime*2);

		}
		//transform.LookAt(target);
	}

	public void flyingCamera() {
		offset.x = 0; offset.z = -20;
		lastRot = Quaternion.Euler(transform.rotation.x,0,transform.rotation.z);
		backToNormal = true;
		smoothSpeed = .7f;
	}

}
