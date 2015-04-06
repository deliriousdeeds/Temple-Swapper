using UnityEngine;
using System.Collections;

public class GaugeBar : MonoBehaviour {

	//public variables
	public float speed;
	public bool hasCompleted = false;

	//private variables
	private Vector3 gaugeOriginPosition;
	private RectTransform gaugeRect;

	void Awake()
	{
		gaugeRect = transform.FindChild("Gauge_Background/Gauge_Foreground").GetComponent<RectTransform>();
	}

	public void SetPosition(Vector3 pos)
	{
		transform.FindChild("Gauge_Background").position = new Vector3(pos.x - 25.0f, pos.y + 13.0f, pos.z);
	}

	public void ChargeUp()
	{
		if (gaugeRect.offsetMax.x < 0) {
			gaugeRect.offsetMax = new Vector2 (gaugeRect.offsetMax.x + speed, Mathf.Abs (gaugeRect.offsetMax.y));	
		} else {
			hasCompleted = true;
		}

	}

}
