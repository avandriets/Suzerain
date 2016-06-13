using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public delegate void TimeEndDelegate(bool autoAnswer);

public class ClockObject : MonoBehaviour {

	public float startTime;
	public float finishTime;
	public float stopTime;
	public float stopTime4GameType4;

	private bool inProcess = false;

	public SpriteRenderer SR_ImgSec1;
	public SpriteRenderer SR_ImgSec2;
	public SpriteRenderer SR_ImgMillSec1;
	public SpriteRenderer SR_ImgMillSec2;

	private Sprite num0; 
	private Sprite num1;
	private Sprite num2;
	private Sprite num3;
	private Sprite num4;
	private Sprite num5;
	private Sprite num6;
	private Sprite num7;
	private Sprite num8;
	private Sprite num9;

	public GameObject blamba;

	TimeEndDelegate timeEndDelegate;

	[HideInInspector]
	public int		GameType 	= 0;
	public float	targetTime 	= 0;

	public bool invertTimer = true;
	public bool showMinutes;

	public void initClockImages(){

		Texture2D tex = (Texture2D)Resources.Load("LampClock/0");
		if (tex != null) {
			num0 = Sprite.Create (tex, new Rect (0, 0, tex.width, tex.height), new Vector2 (0.5f, 0.5f));
		}

		tex = (Texture2D)Resources.Load("LampClock/1");
		if (tex != null) {
			num1 = Sprite.Create (tex, new Rect (0, 0, tex.width, tex.height), new Vector2 (0.5f, 0.5f));
		}

		tex = (Texture2D)Resources.Load("LampClock/2");
		if (tex != null) {
			num2 = Sprite.Create (tex, new Rect (0, 0, tex.width, tex.height), new Vector2 (0.5f, 0.5f));
		}

		tex = (Texture2D)Resources.Load("LampClock/3");
		if (tex != null) {
			num3 = Sprite.Create (tex, new Rect (0, 0, tex.width, tex.height), new Vector2 (0.5f, 0.5f));
		}

		tex = (Texture2D)Resources.Load("LampClock/4");
		if (tex != null) {
			num4 = Sprite.Create (tex, new Rect (0, 0, tex.width, tex.height), new Vector2 (0.5f, 0.5f));
		}

		tex = (Texture2D)Resources.Load("LampClock/5");
		if (tex != null) {
			num5 = Sprite.Create (tex, new Rect (0, 0, tex.width, tex.height), new Vector2 (0.5f, 0.5f));
		}

		tex = (Texture2D)Resources.Load("LampClock/6");
		if (tex != null) {
			num6 = Sprite.Create (tex, new Rect (0, 0, tex.width, tex.height), new Vector2 (0.5f, 0.5f));
		}

		tex = (Texture2D)Resources.Load("LampClock/7");
		if (tex != null) {
			num7 = Sprite.Create (tex, new Rect (0, 0, tex.width, tex.height), new Vector2 (0.5f, 0.5f));
		}

		tex = (Texture2D)Resources.Load("LampClock/8");
		if (tex != null) {
			num8 = Sprite.Create (tex, new Rect (0, 0, tex.width, tex.height), new Vector2 (0.5f, 0.5f));
		}

		tex = (Texture2D)Resources.Load("LampClock/9");
		if (tex != null) {
			num9 = Sprite.Create (tex, new Rect (0, 0, tex.width, tex.height), new Vector2 (0.5f, 0.5f));
		}
	}

	public void StartTimer(TimeEndDelegate pTimerEnd){

		if(inProcess){
			return;
		}
			
		inProcess		= true;
		timeEndDelegate = pTimerEnd;

		StartCoroutine (showTimerProgress());
	}

	public void StopTimer(){
		
		inProcess = false;
	}

	IEnumerator showTimerProgress(){

		float showTime;
		float startingTime = startTime;

		int sec1, sec2, milsec1, milsec2;
		int sec1OLD, sec2OLD, milsec1OLD, milsec2OLD;

		sec1OLD = -1; 
		sec2OLD = -1;
		milsec1OLD = -1; 
		milsec2OLD = -1;
		System.TimeSpan t;

		float partAfterPoint;

		while (startingTime <= finishTime && inProcess) {

			if (invertTimer) {
				showTime = finishTime - startingTime;
			} else {
				showTime = startingTime;
			}

			if (blamba != null && GameType == Utility.Reflex && showTime > targetTime - 3f && showTime < targetTime + 3f && !blamba.activeSelf)
				blamba.SetActive (true);

			if (blamba != null && GameType == Utility.Reflex && showTime < targetTime - 3f && blamba.activeSelf)
				blamba.SetActive (false);

			if (!showMinutes) {
				sec1 = (int)showTime / 10;
				sec2 = (int)showTime - sec1 * 10;
				partAfterPoint = (showTime - Mathf.Floor (showTime)) * 100;
				milsec1 = (int)partAfterPoint / 10;
				milsec2 = (int)partAfterPoint - milsec1 * 10;
			} else {

				t = System.TimeSpan.FromSeconds( (double)showTime );

				sec1 = (int)t.Minutes / 10;
				sec2 = (int)t.Minutes - sec1 * 10;
				milsec1 = (int)t.Seconds / 10;
				milsec2 = (int)t.Seconds - milsec1 * 10;
			}

			if(sec1 != sec1OLD )
				setImage (SR_ImgSec1, sec1);

			if(sec2 != sec2OLD )
				setImage (SR_ImgSec2, sec2);

			if(milsec1 != milsec1OLD )
				setImage (SR_ImgMillSec1, milsec1);

			if(milsec2 != milsec2OLD)
				setImage (SR_ImgMillSec2, milsec2);

			sec1OLD = sec1;
			sec2OLD = sec2;
			milsec1OLD = milsec1;
			milsec2OLD = milsec2;

			stopTime = startingTime;
			stopTime4GameType4 = (((int)(showTime*1000f))/10)*10;

			startingTime += Time.deltaTime;

			yield return null;
		}

		if (blamba != null) {
			blamba.SetActive (false);
		}
		
		if (inProcess == true) {
			timeEndDelegate (true);
		}

		inProcess = false;

	}

	public void SetTime(float pTime){
	
		float partAfterPoint;
		float showTime;

		int sec1, sec2, milsec1, milsec2;

		System.TimeSpan t;

		showTime = pTime;

		if (!showMinutes) {
			sec1 = (int)showTime / 10;
			sec2 = (int)showTime - sec1 * 10;
			partAfterPoint = (showTime - Mathf.Floor (showTime)) * 100;
			milsec1 = (int)partAfterPoint / 10;
			milsec2 = (int)partAfterPoint - milsec1 * 10;
		} else {

			t = System.TimeSpan.FromSeconds( (double)showTime );

			sec1 = (int)t.Minutes / 10;
			sec2 = (int)t.Minutes - sec1 * 10;
			milsec1 = (int)t.Seconds / 10;
			milsec2 = (int)t.Seconds - milsec1 * 10;
		}

//		sec1 = (int)showTime / 10;
//		sec2 = (int)showTime - sec1*10;
//		partAfterPoint = (showTime - Mathf.Floor (showTime)) * 100;
//		milsec1 = (int)partAfterPoint / 10;
//		milsec2 = (int)partAfterPoint - milsec1 * 10;

		setImage (SR_ImgSec1, sec1);
		setImage (SR_ImgSec2, sec2);
		setImage (SR_ImgMillSec1, milsec1);
		setImage (SR_ImgMillSec2, milsec2);

	}

	private void setImage(SpriteRenderer targImg, int number){

		if (number == 0) {
			targImg.sprite = num0;
		} else if (number == 1) {
			targImg.sprite = num1;
		} else if (number == 2) {
			targImg.sprite = num2;
		} else if (number == 3) {
			targImg.sprite = num3;
		} else if (number == 4) {
			targImg.sprite = num4;
		} else if (number == 5) {
			targImg.sprite = num5;
		} else if (number == 6) {
			targImg.sprite = num6;
		} else if (number == 7) {
			targImg.sprite = num7;
		} else if (number == 8) {
			targImg.sprite = num8;
		} else if (number == 9) {
			targImg.sprite = num9;
		}
	}

}
