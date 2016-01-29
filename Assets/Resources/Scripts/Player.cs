using UnityEngine;
using System.Collections;
using System;

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
    public float InteractReach = 1f;

    public BenchBase CurrentInteractible { get; private set; }
	public IngredientBase HeldItem { get; private set; }

    public Color SelectColor;
    
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
			Interact();
		}

		if (HeldItem != null)
		{
			//draw held item
			Debug.DrawLine(transform.position, transform.position + Vector3.up * 2, Color.red);
		}
	}

    void FixedUpdate()
    {
        // Change direction to facing
        var redirectSpeed = (float)Math.Tanh(_controller.velocity.magnitude / 10f);

        if (redirectSpeed > 0.001f)
            transform.forward = Vector3.RotateTowards(transform.forward, _controller.velocity, redirectSpeed * Time.fixedDeltaTime * RotationSpeed, 100);

        // Selection
        RaycastHit rayHit;
        
        BenchBase touchedObject = null;
        for (int down = 0; down < 3; down++)
        {
            if (touchedObject == null)
                for (int i = 0; i < 36; i++)
                {
                    // Scan outwards for object
                    var ray = new Ray(transform.position,
                        Quaternion.Euler(0, i * 10, 0) * 
                        (transform.forward + transform.up * (-down / 4)));

                    Debug.DrawRay(transform.position, ray.direction * InteractReach);

                    if (Physics.Raycast(ray, out rayHit, InteractReach))
                    {
                        touchedObject = rayHit.collider.gameObject.GetComponent<BenchBase>();
                        if (touchedObject != null)
                            break;
                    }
                }
        }


        // If the thing we are touching is not what we were touching
        if (touchedObject != CurrentInteractible)
        {
            // Do a cheap selection effect
            if (CurrentInteractible != null)
            {
                var mat = CurrentInteractible.GetComponent<Renderer>().material;
                mat.SetColor("_EmissionColor", Color.black);
                mat.DisableKeyword("_EMISSION");
            }
            if (touchedObject != null)
            {
                var mat = touchedObject.GetComponent<Renderer>().material;
                mat.SetColor("_EmissionColor", SelectColor);
                mat.EnableKeyword("_EMISSION");
            }

            CurrentInteractible = touchedObject;
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
			if (CurrentInteractible != null && CurrentInteractible.Put(HeldItem))
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
			var item = CurrentInteractible == null ? null : CurrentInteractible.Interact();
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
		return new Vector2(Input.GetAxis(HorizontalAxis), Input.GetAxis(VerticalAxis));
	}

	void Move()
	{

	}
}
