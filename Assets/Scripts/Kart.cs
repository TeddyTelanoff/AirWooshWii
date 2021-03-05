using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Kart : MonoBehaviour
{
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

        if (Input.GetKey(KeyCode.W))
        {
            Vector3 accel = transform.forward;
            accel.y = 0;
            accel.Normalize();
            accel *= m_Speed;

            m_RigidBody.AddForce(accel);
        }
        if (Input.GetKey(KeyCode.LeftShift))
        {
            if (Input.GetKey(KeyCode.W))
                m_RigidBody.drag = m_DriftDrag;
            else
                m_RigidBody.drag = m_BreakDrag;
        }

        float turn = Input.GetAxis("Horizontal");
        m_Model.localRotation = Quaternion.Euler(new Vector3 { y = turn * m_TurnSpeed * m_ModelTurnSpeed - 90 });
        transform.rotation = Quaternion.Euler(new Vector3 { y = turn * m_TurnSpeed } + transform.rotation.eulerAngles);
	}
}
