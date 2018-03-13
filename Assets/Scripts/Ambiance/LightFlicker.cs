using UnityEngine;
using System.Collections;

public class LightFlicker : MonoBehaviour
{
	public float MaxReduction;
	public float MaxIncrease;
	public float RateDamping;
	public float Strength;
	public bool StopFlickering;
	public float _baseIntensity;

	private Light _lightSource;
	private bool _flickering;

	// Color
	private Color minColor;
	private Color maxColor;
	private float colorTimer;


	public void Reset()
	{
		MaxReduction = 0.2f;
		MaxIncrease = 0.2f;
		RateDamping = 0.1f;
		Strength = 300;
	}

	public void Start()
	{
		_lightSource = GetComponent<Light>();
		if (_lightSource == null)
		{
			Debug.LogError("Flicker script must have a Light Component on the same GameObject.");
			return;
		}
		_baseIntensity = _lightSource.intensity;
		StartCoroutine(DoFlicker());

		minColor = new Color(1f, 0.8f, 0.3f, 1);
		maxColor = new Color(1f, 0.8f, 0, 1);
	}

	void Update()
	{
		if (!StopFlickering && !_flickering)
		{
			StartCoroutine(DoFlicker());
		}

		// Color
		colorTimer += Time.deltaTime;
	}

	private IEnumerator DoFlicker()
	{
		_flickering = true;
		while (!StopFlickering)
		{
			_lightSource.intensity = Mathf.Lerp(_lightSource.intensity, Random.Range(_baseIntensity - MaxReduction, _baseIntensity + MaxIncrease), Strength * Time.deltaTime);
			yield return new WaitForSeconds(RateDamping);
		}
		_flickering = false;
	}
}