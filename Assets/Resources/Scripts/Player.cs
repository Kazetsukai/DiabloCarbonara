using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;
using System.Linq;
using RootMotion.FinalIK;

public class Player : MonoBehaviour
{
	private CharacterController _controller;

	[Header("Input Axis Names")]
	public string HorizontalAxis;
	public string VerticalAxis;
	public string InteractButtonAxis;

    [Header("Movement Parameters")]    
    public float MoveSpeed = 1f;
    public float RotationSpeed = 10f;
    public float RotateThreshold = 0.3f;
    public float RotateToTargetSpeed = 50f;
    public float InteractReach = 1f;
    public float InteractOffsetTarget = 1.5f;
    public AnimationCurve MoveSpeedVsAnimSpeed;

    [Header("Body parts")]
    public GameObject Head;
    public GameObject Chest;
    public GameObject ArmLeftUpper;
    public GameObject ArmRightUpper;
    public GameObject ArmLeftLower;
    public GameObject ArmRightLower;
    public GameObject HandLeft;
    public GameObject HandRight;
    public GameObject HeldObjectTransform;

    [Header("Animation Stuff")]
    public LimbIK IKArm_R;
    public LimbIK IKArm_L;
    public Transform ArmIKTarget_R;
    public Transform ArmIKTarget_L;
    public GameObject HandIKTarget_Idle_R;
    public GameObject HandIKTarget_Idle_L;
    public GameObject HandIKTarget_HoldItem_R;
    public GameObject HandIKTarget_HoldItem_L;
    public float TransitionToIdleDuration = 0.2f;
    public float TransitionToHoldDuration = 0.2f;

    public BenchBase CurrentInteractable { get; private set; }
	public IngredientBase HeldItem { get; private set; }

    public Color SelectColor;

    Vector3 lastVelocity;   //Used for working out rotation target direction
    Animator _anim;

    private List<BenchBase> _touchedBenches = new List<BenchBase>();
	private bool JustInteracted;
    public bool Interacting;

    void Start()
	{
		_controller = GetComponent<CharacterController>();
        _anim = GetComponentInChildren<Animator>();
	}

    Vector3 CameraTransformedInput()
    {
        var input = GetInput();
        var dirVect = new Vector3(input.x, 0, input.y);

        // Redirect based on camera angle
        dirVect = Quaternion.FromToRotation(Vector3.forward, TrimY(Camera.allCameras[0].transform.forward)) * dirVect;
        return dirVect;
    }

	void Update()
	{
        Interacting = false;

		if (Input.GetAxis(InteractButtonAxis) > 0)
		{
			if (!JustInteracted)
			{
				Interact();
                Interacting = true;
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

        if (!Interacting)
        {
            //Move player when horizontal or vertical input is given
            var input = CameraTransformedInput();
            if (input.magnitude > 0)
            {
                _controller.SimpleMove(input * MoveSpeed);  //    DON'T NEED TIME.DELTATIME HERE! SERSLY!      
            }

            //Update walk animation
            float speed = _controller.velocity.magnitude;
            _anim.SetFloat("MoveSpeed", speed);
            _anim.speed = MoveSpeedVsAnimSpeed.Evaluate(speed);
        }
        else
        {
            _anim.SetFloat("MoveSpeed", 0);
        }

        //Move towards bench interactTransform if interacting with the bench
        if ((CurrentInteractable != null) && (Interacting))
        {
            Vector3 targetPos = CurrentInteractable.transform.position + (CurrentInteractable.transform.right * InteractOffsetTarget);
            Vector3 dir = (new Vector3(targetPos.x, 0, targetPos.z) - new Vector3(transform.position.x, 0, transform.position.z)).normalized;

            float distanceToTarget = Vector3.Distance(targetPos, new Vector3(transform.position.x, 0, transform.position.z));

            if (distanceToTarget > 0.08f)
            {
                _controller.SimpleMove(dir * MoveSpeed);
            }
        }
        if ((CurrentInteractable == null) && (HeldItem == null) && !Interacting) 
        {
            //Reset IK targets
            IKArm_R.solver.target = ArmIKTarget_R;
            IKArm_L.solver.target = ArmIKTarget_L;
        }
	}

    void FixedUpdate()
    {
        //Rotate towards movement direction
        if ((lastVelocity.magnitude > 0.01f) && (CurrentInteractable == null))
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(lastVelocity, Vector3.up), RotationSpeed * Time.deltaTime);
        }
        else if (CurrentInteractable != null)
        {
            transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.LookRotation(new Vector3(CurrentInteractable.transform.position.x, 0, CurrentInteractable.transform.position.z) - new Vector3(transform.position.x, 0, transform.position.z), Vector3.up), RotateToTargetSpeed * Time.deltaTime);
        }
        transform.localEulerAngles = new Vector3(0, transform.localEulerAngles.y, 0);

        var input = CameraTransformedInput();
        
        if ((input.magnitude > RotateThreshold) && (!Interacting || (HeldItem != null)))
        {
            lastVelocity = input.normalized;
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
       // Debug.Log("Current Interactable: " + CurrentInteractable);
       
        if (HeldItem != null)
		{
            //print("put");
			if (CurrentInteractable != null && CurrentInteractable.Put(HeldItem))
			{
				// not holding anymore
				HeldItem = null;
				JustInteracted = true;
                StartCoroutine(TransitionToPose_Idle(TransitionToIdleDuration));    //Animate arms                      
            }
			else
			{
				//print("failed");
			}
		}
		else
		{           
            var item = CurrentInteractable == null ? null : CurrentInteractable.Interact(this, GetInput());
			if (item != null)
			{
				// hold item above head
				HeldItem = item;
                HeldItem.transform.parent = HeldObjectTransform.transform;         
                StartCoroutine(DoItemPickupTransition(HeldItem, 0.2f));
                StartCoroutine(TransitionToPose_HoldItem(TransitionToHoldDuration));    //Animate arms
                JustInteracted = true;
			}
			else
			{
				//print("failed");                
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

    IEnumerator DoItemPickupTransition(IngredientBase itemToLerp, float duration)
    {
        Vector3 ItemStartPos = itemToLerp.transform.position;

        float t_elapsed = 0;
        do
        {
            t_elapsed += Time.deltaTime;
            itemToLerp.gameObject.transform.position = Vector3.Lerp(ItemStartPos, HeldObjectTransform.transform.position, t_elapsed / duration);
            yield return null;
        }
        while (t_elapsed / duration < 1f);        
    }
    
    IEnumerator TransitionToPose_Idle(float duration)
    {      
        float t_elapsed = 0;
        do
        {
            t_elapsed += Time.deltaTime;
            float t = t_elapsed / duration;

            ArmIKTarget_L.transform.position = Vector3.Lerp(ArmIKTarget_L.transform.position, HandIKTarget_Idle_L.transform.position, t);
            ArmIKTarget_R.transform.position = Vector3.Lerp(ArmIKTarget_R.transform.position, HandIKTarget_Idle_R.transform.position, t);

            yield return null;
        }
        while (t_elapsed / duration < 1f);
    }

    IEnumerator TransitionToPose_HoldItem(float duration)
    {
        float t_elapsed = 0;
        do
        {
            t_elapsed += Time.deltaTime;
            float t = t_elapsed / duration;

            ArmIKTarget_L.transform.position = Vector3.Lerp(ArmIKTarget_L.transform.position, HandIKTarget_HoldItem_L.transform.position, t);
            ArmIKTarget_R.transform.position = Vector3.Lerp(ArmIKTarget_R.transform.position, HandIKTarget_HoldItem_R.transform.position, t);

            yield return null;
        }
        while (t_elapsed / duration < 1f);
    }





}
