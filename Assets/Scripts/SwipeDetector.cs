using UnityEngine;
using System.Collections;
using VRTK;

public class SwipeDetector : MonoBehaviour
{
	public int magicValue = 1;

	private readonly Vector2 mXAxis = new Vector2(1, 0);
	private readonly Vector2 mYAxis = new Vector2(0, 1);
	private bool startChecking = false;
	private bool upChecking = false;
	private bool trackingSwipe = false;
	private bool checkSwipe = false;

	private readonly string[] mMessage = {
		"",
		"Swipe Left",
		"Swipe Right",
		"Swipe Top",
		"Swipe Bottom"
	};

	// The angle range for detecting swipe
	private const float mAngleRange = 30;

	// To recognize as swipe user should at lease swipe for this many pixels
	private const float mMinSwipeDist = 0.2f;

	// To recognize as a swipe the velocity of the swipe
	// should be at least mMinVelocity
	// Reduce or increase to control the swipe speed
	private const float mMinVelocity = 4.0f;

	private Vector2 mStartPosition;
	private Vector2 endPosition;

	private float mSwipeStartTime;


	private void Start()
	{
		GetComponent<VRTK_ControllerEvents>().TouchpadAxisChanged += new ControllerInteractionEventHandler(DoTouchpadAxisChanged);
		GetComponent<VRTK_ControllerEvents>().TouchpadTouchEnd += new ControllerInteractionEventHandler(DoTouchpadTouchEnd);
	}

	private void DoTouchpadAxisChanged(object sender, ControllerInteractionEventArgs e)
	{
		if (!trackingSwipe)
		{
			startChecking = true;
			Debug.Log("START");
		}
	}

	private void DoTouchpadTouchEnd(object sender, ControllerInteractionEventArgs e)
	{
		upChecking = true;
		Debug.Log("STOP");
	}

	void Update()
	{
		// Touch down, possible chance for a swipe
		if (startChecking)
		{
			trackingSwipe = true;
			// Record start time and position
			mStartPosition = new Vector2(GetComponent<VRTK_ControllerEvents>().GetTouchpadAxis().x,
											GetComponent<VRTK_ControllerEvents>().GetTouchpadAxis().y);
			mSwipeStartTime = Time.time;
			startChecking = false;
		}
		// Touch up , possible chance for a swipe
		else if (upChecking)
		{
			trackingSwipe = false;
			trackingSwipe = true;
			checkSwipe = true;
		}
		else if (trackingSwipe)
		{
			endPosition = new Vector2(GetComponent<VRTK_ControllerEvents>().GetTouchpadAxis().x,
										GetComponent<VRTK_ControllerEvents>().GetTouchpadAxis().y);
		}

		if (checkSwipe)
		{
			checkSwipe = false;
			upChecking = false;
			trackingSwipe = false;

			float deltaTime = Time.time - mSwipeStartTime;

			Vector2 swipeVector = endPosition - mStartPosition;

			float velocity = swipeVector.magnitude / deltaTime;
			Debug.Log(velocity);
			if (velocity > mMinVelocity &&
				swipeVector.magnitude > mMinSwipeDist)
			{
				// if the swipe has enough velocity and enough distance
				swipeVector.Normalize();

				float angleOfSwipe = Vector2.Dot(swipeVector, mXAxis);
				angleOfSwipe = Mathf.Acos(angleOfSwipe) * Mathf.Rad2Deg;

				// Detect left and right swipe
				if (angleOfSwipe < mAngleRange)
				{
					OnSwipeRight();
				}
				else if ((180.0f - angleOfSwipe) < mAngleRange)
				{
					OnSwipeLeft();
				}
			}
		}
	}

	private void OnSwipeLeft()
	{
		Debug.Log("Swipe Left");
		if (magicValue > 1)
			magicValue--;
	}

	private void OnSwipeRight()
	{
		Debug.Log("Swipe right");
		if (magicValue < 4)
			magicValue++;
	}

}