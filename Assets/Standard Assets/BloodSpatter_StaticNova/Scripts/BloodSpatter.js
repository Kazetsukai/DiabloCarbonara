var bloodPrefab:Transform;
var bloodPosition:Transform;
var bloodRotation:Transform;
var bloodLocalRotationYOffset:int = 0;
var maxAmountBloodPrefabs:int = 1;
private var bloodInstances : GameObject[];

function Update () {
	if(Input.GetMouseButtonDown(0)){		
		bloodRotation.Rotate(0,bloodLocalRotationYOffset,0);
		var newBlood:Transform = Instantiate (bloodPrefab, bloodPosition.position, bloodRotation.rotation)as Transform;
		bloodInstances = GameObject.FindGameObjectsWithTag("blood");
		if(bloodInstances.length >= maxAmountBloodPrefabs){
			Destroy(bloodInstances[0]);
		}
	}
}