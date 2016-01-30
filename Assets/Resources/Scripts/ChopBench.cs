using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using RootMotion.FinalIK;

public class ChopBench : BenchBase
{
    [Header("Chop Bench")]
	public float progress;
	public int HitsRequired;
	public GameObject progressImagePrefab;
    public float InputValue_Up = 0f;
    public float InputValue_Down = -0.9f;   
    public bool upTargetReached = false;
    public string TaskType = "Chop";
    public GameObject Knife;

    [Header("Animation Stuff")]
    public Transform ChopUpTransform;
    public Transform ChopDownTransform;
    public float MaxVarianceZ = 0.38f;
    public float MinVarianceZ = 0.06f;

    private bool progressingThisFrame;
	private GameObject progressImage;

    Vector3 KnifeIdlePosition;
    Vector3 KnifeIdleRotation;
    float InteractKnifeResetTime = 0.1f;
    float InteractKnifeResetElapsed;
	private ParticleSystem ParticleSystem;

	public override IngredientBase Interact(Player player, Vector2 input)
	{       
		if (contents == null)
		{
			return null;
		}

        LastInteractedPlayer = player;

        if (input.y >= InputValue_Up)
        {
            upTargetReached = true;

            //Move ChopDownTransform to a new random place on the board (within limits)
            ChopDownTransform.localPosition = new Vector3(ChopDownTransform.localPosition.x, ChopDownTransform.localPosition.y, Random.Range(MinVarianceZ, MaxVarianceZ));
        }
        if ((input.y <= InputValue_Down) && upTargetReached)
        {
            progress += 1f / HitsRequired;
            upTargetReached = false;
			ParticleSystem.Emit(30);
		}

        //Do animation for players arms
        player.IKArm_R.solver.target = HandIKTarget_R;  //Set IK target R of player to be IK transform R of this bench

        //Move IK target based on joystick input
        float movePercent = Mathf.Abs(Mathf.Clamp(input.y, InputValue_Down, InputValue_Up));        
        float t = movePercent  / Mathf.Abs(InputValue_Down - InputValue_Up);       
        HandIKTarget_R.transform.position = Vector3.Lerp(ChopUpTransform.position, ChopDownTransform.position, t);

        //Move knife accordingly
        InteractKnifeResetElapsed = 0;
        Knife.transform.position = player.HandRight.transform.position;        
        Knife.transform.forward = -player.ArmRightLower.transform.right;

        if (progress >= 1)
		{
            upTargetReached = false;
            contents.TasksDone.Add(TaskType);
            contents.Process();

            //Reset player arm IK targets
            player.IKArm_R.solver.target = player.ArmIKTarget_R;

            progress = 0;
			var temp = contents;
			contents = null;
			return temp;
		}	        
		else
		{			
			return null;
		}
	}

	public override bool CanIReceive(IngredientBase item)
	{
		ParticleSystem.startColor = item.Color;
		return true;
	}

	public void Start()
	{
		ParticleSystem = GetComponentInChildren<ParticleSystem>();
		progressImage = Instantiate(progressImagePrefab);
		progressImage.GetComponent<Image>().color = Color.blue;
		var canvas = FindObjectOfType<Canvas>();
		progressImage.transform.SetParent(canvas.transform);

        KnifeIdlePosition = Knife.transform.position;
        KnifeIdleRotation = Knife.transform.eulerAngles;
    }

	public new void Update()
	{
		Vector3 pos = transform.position + Vector3.up * 3;
		Vector3 screenPos = Camera.main.WorldToScreenPoint(pos);
		progressImage.GetComponent<RectTransform>().position = screenPos;
		progressImage.GetComponent<Image>().fillAmount = progress;

        //Update knife position
        InteractKnifeResetElapsed += Time.deltaTime;
        if (InteractKnifeResetElapsed >= InteractKnifeResetTime)
        {
            Knife.transform.position = KnifeIdlePosition;
            Knife.transform.eulerAngles = KnifeIdleRotation;

            //Reset player arm IK targets
            if (LastInteractedPlayer != null)
            {
                LastInteractedPlayer.IKArm_R.solver.target = LastInteractedPlayer.ArmIKTarget_R;
                LastInteractedPlayer.IKArm_L.solver.target = LastInteractedPlayer.ArmIKTarget_L;
                LastInteractedPlayer = null;
            }
        }

		base.Update();
	}

	public void FixedUpdate()
	{
		if (progressingThisFrame)
		{
			progress += 1f/HitsRequired;
			progressingThisFrame = false;
		}
	}
}
