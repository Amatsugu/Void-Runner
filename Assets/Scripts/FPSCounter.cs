using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class FPSCounter : MonoBehaviour {

	public float updateRate = 4.0f;  // 4 updates per sec.
	
	private int frameCount = 0;
	private float dt = 0.0f;
	private float fps = 0.0f;
	private Text fpsText;

	void Start()
	{
		fpsText = GetComponent<Text>();
	}

	void Update()
	{
		frameCount++;
		dt += Time.deltaTime;
		if (dt > 1.0f / updateRate)
		{
			fps = frameCount / dt;
			frameCount = 0;
			dt -= 1.0f / updateRate;
		}
		fpsText.text = VoidUtils.Round(fps, 100)+" FPS";
	}
}
