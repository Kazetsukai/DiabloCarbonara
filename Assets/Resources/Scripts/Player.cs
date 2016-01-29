using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour
{
    CharacterController controller;

    [Header("Input Axis Names")]
    [SerializeField] string HorizontalAxis;
    [SerializeField] string VerticalAxis;
    [SerializeField] string InteractButtonAxis;

    [Header("Movement Parameters")]
    public float MoveSpeed = 1f;

	void Start ()
    {
        controller = GetComponent<CharacterController>();	
	}	

	void Update ()
    {
        //Move player when horizontal or vertical input is given
        Vector2 input = GetInput();
       
        if (input.magnitude > 0)
        {
            controller.SimpleMove(new Vector3(input.x, 0, input.y) * MoveSpeed);  //    DON'T NEED TIME.DELTATIME HERE! SERSLY!      
        }	

        //
	}

    Vector2 GetInput()
    {
        return new Vector2(Input.GetAxis(HorizontalAxis), -Input.GetAxis(VerticalAxis));    //INVERTED VERTICAL AXIS BECAUSE CAMERA ANGLE
    }

    void Move()
    {

    }
}
