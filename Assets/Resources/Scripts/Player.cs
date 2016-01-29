using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;
using System.Linq;

public class Player : MonoBehaviour
{
	private CharacterController _controller;

	[Header("Input Axis Names")]
	[SerializeField]
	string HorizontalAxis;
	[SerializeField]
	string VerticalAxis;
	[SerializeField]
	string InteractButtonAxis;

	[Header("Movement Parameters")]
    public float MoveSpeed = 1f;
    public float RotationSpeed = 10f;
    public float RotateThreshold = 0.3f;
    public float InteractReach = 1f;

    public BenchBase CurrentInteractable { get; private set; }
	public IngredientBase HeldItem { get; private set; }

    public Color SelectColor;

    Vector3 lastVelocity;   //Used for working out rotation target direction

    private List<BenchBase> _touchedBenches = new List<BenchBase>();
	private bool JustInteracted;

	void Start()
	{
		_controller = GetComponent<CharacterController>();
	}

	void Update()
	{
		//Move player when horizontal or vertical input is given
		var input = GetInput();
        var dirVect = new Vector3(input.x, 0, input.y);

        // Redirect based on camera angle
        dirVect = Quaternion.FromToRotation(Vector3.forward, TrimY(Camera.allCameras[0].transform.forward)) * dirVect;

        if (input.magnitude > 0)
		{
			_controller.SimpleMove(dirVect * MoveSpeed);  //    DON'T NEED TIME.DELTATIME HERE! SERSLY!      
		}

		if (Input.GetAxis(InteractButtonAxis) > 0)
		{
			if (!JustInteracted)
			{
				Interact(); 
			}
		}
		else
		{
			JustInteracted = false;
		}

		if (HeldItem != null)
		{
			//draw held item
			Debug.DrawLine(transform.position, transform.position + Vector3.up * 2, Color.red);
		}
	}

    void FixedUpdate()
    {
        //Rotate towards movement direction
        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(lastVelocity, Vector3.up), RotationSpeed * Time.deltaTime);
        transform.localEulerAngles = new Vector3(0, transform.localEulerAngles.y, 0);

        if (_controller.velocity.magnitude > RotateThreshold)
        {
            lastVelocity = _controller.velocity.normalized;
        }

        // Selection
        BenchBase touchedObject = _touchedBenches.Select(b => new
            {
                Me = b,
                Dist = (b.transform.position - transform.position).magnitude
            }).OrderBy(x => x.Dist).Select(x => x.Me).FirstOrDefault();


        // If the thing we are touching is not what we were touching
        if (touchedObject != CurrentInteractable)
        {
            // Do a cheap selection effect
            if (CurrentInteractable != null)
            {
                var mat = CurrentInteractable.GetComponentInChildren<Renderer>().material;
                mat.SetColor("_EmissionColor", Color.black);
                mat.DisableKeyword("_EMISSION");
            }
            if (touchedObject != null)
            {
                var mat = touchedObject.GetComponentInChildren<Renderer>().material;
                mat.SetColor("_EmissionColor", SelectColor);
                mat.EnableKeyword("_EMISSION");
            }

            CurrentInteractable = touchedObject;
        }
    }

    private Vector3 TrimY(Vector3 forward)
    {
        return new Vector3(forward.x, 0, forward.z);
    }

    private void Interact()
	{
		if (HeldItem != null)
		{
			print("put");
			if (CurrentInteractable != null && CurrentInteractable.Put(HeldItem))
			{
				// not holding anymore
				HeldItem = null;
				JustInteracted = true;
			}
			else
			{
				print("failed");
			}
		}
		else
		{
			print("get");
			var item = CurrentInteractable == null ? null : CurrentInteractable.Interact();
			if (item != null)
			{
				// hold item above head
				HeldItem = item;
				HeldItem.transform.position = transform.position + Vector3.up;
				HeldItem.transform.parent = transform;
				JustInteracted = true;
			}
			else
			{
				print("failed");
			}
		}
	}

	void OnTriggerEnter(Collider other)
	{
		var benchBase = other.gameObject.GetComponent<BenchBase>();
		if (benchBase != null && !_touchedBenches.Contains(benchBase))
		{
            _touchedBenches.Add(benchBase);
		}
    }

    void OnTriggerExit(Collider other)
    {
        var benchBase = other.gameObject.GetComponent<BenchBase>();
        if (benchBase != null && _touchedBenches.Contains(benchBase))
        {
            _touchedBenches.Remove(benchBase);
        }
    }

    Vector2 GetInput()
	{
		return new Vector2(Input.GetAxis(HorizontalAxis), Input.GetAxis(VerticalAxis));
	}
}
