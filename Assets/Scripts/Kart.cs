using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Kart : MonoBehaviour
{
	[SerializeField]
	private float m_MaxSpeed = 100;
	[SerializeField]
	private float m_Speed = 100;
	[SerializeField]
	private float m_TurnSpeed = 2;
	[SerializeField]
	private float m_ModelTurnSpeed = 5;
	[SerializeField]
	private float m_BreakDrag = 5;
	[SerializeField]
	private float m_DriftDrag = 3;

	[SerializeField]
	private Transform m_Model;

	private Rigidbody m_RigidBody;
	private float m_DefaultDrag;

	private void Awake()
	{
		m_RigidBody = GetComponent<Rigidbody>();
		Debug.Assert(m_RigidBody, "Must have Rigidbody!");
		Debug.Assert(m_Model, "Must have Model!");

		m_DefaultDrag = m_RigidBody.drag;
	}

	private void FixedUpdate()
	{
		m_RigidBody.drag = m_DefaultDrag;

		float forward = Input.GetAxisRaw("Vertical");
		float turn = Input.GetAxis("Horizontal");

		{
			m_Model.localRotation = Quaternion.Euler(new Vector3 { y = turn * m_TurnSpeed * m_ModelTurnSpeed - 90 });
			transform.rotation = Quaternion.Euler(new Vector3 { y = turn * m_TurnSpeed } + transform.rotation.eulerAngles);
		}

		{
			Vector3 accel = transform.forward;
			accel *= m_Speed * forward;

			m_RigidBody.AddForce(accel);
		}

		if (forward > 0)
		{
			Vector3 accel = transform.right;
			accel.y = 0;
			accel.Normalize();
			accel *= m_Speed * turn;

			if (Input.GetKey(KeyCode.LeftShift))
			{
				accel = -accel;
				m_RigidBody.drag = m_DriftDrag;
			}

			m_RigidBody.AddForce(accel);
		}
		else if (Input.GetKey(KeyCode.LeftShift))
			m_RigidBody.drag = m_BreakDrag;

		if (m_RigidBody.velocity.magnitude > m_MaxSpeed)
			m_RigidBody.velocity = m_RigidBody.velocity.normalized * m_MaxSpeed;

		if (transform.rotation.eulerAngles.z < -80 || transform.rotation.eulerAngles.z > 80)
			transform.rotation = Quaternion.Euler(new Vector3(transform.rotation.eulerAngles.x, transform.rotation.eulerAngles.y, 0));
	}
}
