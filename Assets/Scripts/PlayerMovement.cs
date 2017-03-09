using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
	private float moveSpeed = 2f;
	private float gridSize = 1f;
	private enum Orientation {Horizontal, Vertical};
	private Orientation gridOrientation = Orientation.Vertical;
	private bool allowDiagonals = false;
	private bool correctDiagonalSpeed = true;
	private Vector2 input;
	private bool isMoving = false;
	private Vector3 startPosition;
	private Vector3 endPosition;
	private float t;
	private float factor;

	private Animator anim;
	private Vector2 lastMove;

	void Start ()
	{
		anim = GetComponent<Animator> ();
	}
	public void Update ()
	{
		if (!isMoving)
		{
			input = new Vector2 (Input.GetAxis ("Horizontal"), Input.GetAxis ("Vertical"));
			if (!allowDiagonals)
			{
				if (Mathf.Abs (input.x) > Mathf.Abs (input.y))
				{
					input.y = 0;
				}
				else
				{
					input.x = 0;
				}
			}

			if (input != Vector2.zero)
			{
				StartCoroutine (Move (transform));
				lastMove = input; // new.
			}
		}
		anim.SetFloat ("MoveX", Input.GetAxis ("Horizontal"));
		anim.SetFloat ("MoveY", Input.GetAxis ("Vertical"));
		anim.SetFloat ("LastMoveX", lastMove.x);
		anim.SetFloat ("LastMoveY", lastMove.y);
		anim.SetBool ("IsMoving", isMoving);
	}
	public IEnumerator Move (Transform transform)
	{
		isMoving = true;
		startPosition = transform.position;
		t = 0;

		if (gridOrientation == Orientation.Horizontal)
		{
			endPosition = new Vector3 (startPosition.x + System.Math.Sign (input.x) * gridSize,
				startPosition.y, startPosition.z + System.Math.Sign (input.y) * gridSize);
		}
		else
		{
			endPosition = new Vector3 (startPosition.x + System.Math.Sign (input.x) * gridSize,
				startPosition.y + System.Math.Sign (input.y) * gridSize, startPosition.z);
		}
		if (allowDiagonals && correctDiagonalSpeed && input.x != 0 && input.y != 0)
		{
			factor = 0.7071f;
		}
		else
		{
			factor = 1f;
		}
		while (t < 1f)
		{
			t += Time.deltaTime * (moveSpeed / gridSize) * factor;
			transform.position = Vector3.Lerp (startPosition, endPosition, t);
			yield return null;
		}
		isMoving = false;
		yield return 0;
	}
}