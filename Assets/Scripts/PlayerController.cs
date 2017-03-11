using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
	private Vector3 pos;
	private float speed = 2f;
	private Animator anim;
	private bool isMoving;
	private Vector2 lastMove;

	public Transform rayUpStart, rayUpEnd;
	public Transform rayDownStart, rayDownEnd;
	public Transform rayLeftStart, rayLeftEnd;
	public Transform rayRightStart, rayRightEnd;

	public bool upBlocked;
	public bool downBlocked;
	public bool leftBlocked;
	public bool rightBlocked;

	void Start ()
	{
		pos = transform.position;
		anim = GetComponent<Animator> ();
	}
	void Update ()
	{
		Raycasting ();
		Move ();
	}
	void Move ()
	{
		if (Input.GetAxisRaw ("Vertical") > 0 && transform.position == pos && !upBlocked || Input.GetAxisRaw ("Vertical") < 0 && transform.position == pos && !downBlocked)
		{
			isMoving = true;
			pos += new Vector3 (0f, Input.GetAxisRaw ("Vertical"), 0f);
			lastMove = new Vector2 (0f, Input.GetAxisRaw ("Vertical"));
		}
		if (Input.GetAxisRaw ("Horizontal") < 0 && transform.position == pos && !leftBlocked || Input.GetAxisRaw ("Horizontal") > 0 && transform.position == pos && !rightBlocked)
		{
			isMoving = true;
			pos += new Vector3 (Input.GetAxisRaw ("Horizontal"), 0f, 0f);
			lastMove = new Vector2 (Input.GetAxisRaw ("Horizontal"), 0f);
		}
		transform.position = Vector3.MoveTowards (transform.position, pos, Time.deltaTime * speed);
		if (transform.position == Vector3.MoveTowards (transform.position, pos, Time.deltaTime * speed))
		{
			isMoving = false;
		}
		anim.SetFloat ("MoveX", Input.GetAxisRaw ("Horizontal"));
		anim.SetFloat ("MoveY", Input.GetAxisRaw ("Vertical"));
		anim.SetFloat ("LastMoveX", lastMove.x);
		anim.SetFloat ("LastMoveY", lastMove.y);
		anim.SetBool ("IsMoving", isMoving);
	}
	void Raycasting ()
	{
		StartCoroutine (Delay());
		upBlocked = Physics2D.Linecast (rayUpStart.position, rayUpEnd.position, 1 << LayerMask.NameToLayer ("Obstacle"));
		downBlocked = Physics2D.Linecast (rayDownStart.position, rayDownEnd.position, 1 << LayerMask.NameToLayer ("Obstacle"));
		leftBlocked = Physics2D.Linecast (rayLeftStart.position, rayLeftEnd.position, 1 << LayerMask.NameToLayer ("Obstacle"));
		rightBlocked = Physics2D.Linecast (rayRightStart.position, rayRightEnd.position, 1 << LayerMask.NameToLayer ("Obstacle"));
	}
	IEnumerator Delay()
	{
		yield return new WaitForSeconds (1);
	}
}