using UnityEngine;
using System.Collections;
using System;

public class Player : MonoBehaviour
{
	CharacterController controller;

	[Header("Input Axis Names")]
	[SerializeField]
	string HorizontalAxis;
	[SerializeField]
	string VerticalAxis;
	[SerializeField]
	string InteractButtonAxis;

	[Header("Movement Parameters")]
	public float MoveSpeed = 1f;

	public BenchBase CurrentInteractible { get; private set; }
	public IngredientBase HeldItem { get; private set; }

	void Start()
	{
		controller = GetComponent<CharacterController>();
	}

	void Update()
	{
		//Move player when horizontal or vertical input is given
		Vector2 input = GetInput();

		if (input.magnitude > 0)
		{
			controller.SimpleMove(new Vector3(input.x, 0, input.y) * MoveSpeed);  //    DON'T NEED TIME.DELTATIME HERE! SERSLY!      
		}

		if (Input.GetAxis(InteractButtonAxis) > 0)
		{
			Interact();
		}

		if (HeldItem != null)
		{
			//draw held item
			Debug.DrawLine(transform.position, transform.position + Vector3.up * 2, Color.red);
		}
	}

	private void Interact()
	{
		if (HeldItem != null)
		{
			print("put");
			if (CurrentInteractible.Put(HeldItem))
			{
				HeldItem = null;
			}
			else
			{
				print("failed");
			}
		}
		else
		{
			print("get");
			var item = CurrentInteractible.Interact();
			if (item != null)
			{
				HeldItem = item;
				HeldItem.transform.position = transform.position + Vector3.up;
				HeldItem.transform.parent = transform;
			}
		}
	}

	void OnTriggerEnter(Collider other)
	{
		print("hit");

		var benchBase = other.gameObject.GetComponent<BenchBase>();
		if (benchBase != null)
		{
			CurrentInteractible = benchBase;
		}
	}

	Vector2 GetInput()
	{
		return new Vector2(Input.GetAxis(HorizontalAxis), -Input.GetAxis(VerticalAxis));    //INVERTED VERTICAL AXIS BECAUSE CAMERA ANGLE
	}

	void Move()
	{

	}
}
