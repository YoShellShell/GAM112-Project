using UnityEngine;
using System.Collections;

public class CharacterHandler : MonoBehaviour {

	public float moveSpeed, rotSpeed, curHealth, maxHealth, curMana, maxMana, gravity;
	private int AttFlag;
	private CharacterController _char;
	private Animator _anim;
	private AnimatorStateInfo _info;

	// Use this for initialization
	void Start () {
		_char = GetComponent<CharacterController> ();
		_anim = GetComponent<Animator> ();
	}
	
	// Update is called once per frame
	void Update () {
		_info = _anim.GetCurrentAnimatorStateInfo(0); //what animation is playing?

		//////////////////////MOVEMENT//////////////////////

		float _x, _y, _r; // local variables get deleted at the function's end
		_x = Input.GetAxis("Horizontal"); // the horizontal axis (1 or -1)
		_y = Input.GetAxis("Vertical"); // the vertical axis
		_r = Input.GetAxis("Rotate"); // the rotation axis

		Vector3 moveDir = new Vector3 (_x, 0, _y)*(moveSpeed*Time.deltaTime); // the inputs multiplying the movement speed (1 * 30 or -1 * 30)
		moveDir.y = gravity*Time.deltaTime; //apply gravity
		moveDir = transform.TransformDirection (moveDir); //MY direction not the world direction
		_char.Move (moveDir); //apply movement


		float _curRot = transform.eulerAngles.y; //my y rotation
		float _wantRot = _curRot += ((rotSpeed * Time.deltaTime) * _r); //apply rotation to new var

		transform.rotation = Quaternion.Euler(0f,_wantRot, 0f); //rotation
		
		_anim.SetFloat ("Y", _y);//send the inputs to animator
		_anim.SetFloat ("X", _x);


		//////////////////////COMBAT//////////////////////

		if (!_info.IsTag ("InAttack")) { //am i not attacking?
			if(AttFlag != 0) //I'm not, but I have a flag remaining?
			{
				AttFlag=0; //remove the flag ready for the next attack
				_anim.SetInteger("AttackIdx", AttFlag);
			}
		}

		if (Input.GetButtonDown ("Fire1")) { //player has asked to attack
			_anim.SetBool("Down1", true);
			if(AttFlag == 0) //am I ready to attack?
			{
				AttFlag=1; //send the flag to the animator
				_anim.SetInteger("AttackIdx", AttFlag);
			}
		}

		if (Input.GetButtonUp ("Fire1")) { //player has released the mouse btn
			_anim.SetBool("Down1", false);
		}



	}
}
