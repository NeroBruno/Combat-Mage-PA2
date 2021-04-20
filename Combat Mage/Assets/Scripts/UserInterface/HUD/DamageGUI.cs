using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class DamageGUI : HUD_DisplayerBehaviour
{
	// Blood Screen
	[SerializeField]
	private ImageFader _BloodScreenFader = null;

	[Space]

	// Damage Indicator
	[SerializeField]
	private RectTransform _DamageIndicatorRT = null;

	[SerializeField]
	private ImageFader _DamageIndicatorFader = null;

	[SerializeField]
	[Range(0f, 512f)]
	[Tooltip("Damage indicator distance (in pixels) from the screen center.")]
	private int _DamageIndicatorDistance = 128;

	private Vector3 _LastHitPoint;

	public override void OnPostAttachment()
	{
		Player.ChangeHealth.AddListener(OnHealthChanged);
	}

	private void Update()
	{
		if (!_DamageIndicatorFader.Fading)
			return;

		Vector3 lookDir = Vector3.ProjectOnPlane(Player.LookDirection.Get(), Vector3.up).normalized;
		Vector3 dirToPoint = Vector3.ProjectOnPlane(_LastHitPoint - Player.transform.position, Vector3.up).normalized;

		Vector3 rightDir = Vector3.Cross(lookDir, Vector3.up);

		float angle = Vector3.Angle(lookDir, dirToPoint) * Mathf.Sign(Vector3.Dot(rightDir, dirToPoint));

		_DamageIndicatorRT.localEulerAngles = Vector3.forward * angle;
		_DamageIndicatorRT.localPosition = _DamageIndicatorRT.up * _DamageIndicatorDistance;
	}

	private void OnHealthChanged(HealthEventData healthEventData)
	{
		if (healthEventData.Delta < 0f)
		{
			if (Player.Health.Val > 0)
				_BloodScreenFader.DoFadeCycle(this, healthEventData.Delta / 100f);

			if (healthEventData.Hitpoint != Vector3.zero)
			{
				_LastHitPoint = healthEventData.Hitpoint;
				_DamageIndicatorFader.DoFadeCycle(this, 1f);
			}
		}
	}

	[Serializable]
	public class ImageFader
	{
		public bool Fading { get; private set; }

		[SerializeField]
		private Image _Image = null;

		[SerializeField]
		[Range(0f, 1f)]
		private float _MinAlpha = 0.4f;

		[SerializeField]
		[Range(0f, 100f)]
		private float _FadeInSpeed = 25f;

		[SerializeField]
		[Range(0f, 100f)]
		private float _FadeOutSpeed = 0.3f;

		[SerializeField]
		[Range(0f, 10f)]
		private float _FadeOutPause = 0.5f;

		private Coroutine _FadeHandler;


		public void DoFadeCycle(MonoBehaviour parent, float targetAlpha)
		{
			if (_Image == null)
			{
				Debug.LogError("[ImageFader] - The image to fade is not assigned!");
				return;
			}

			targetAlpha = Mathf.Clamp01(Mathf.Max(Mathf.Abs(targetAlpha), _MinAlpha));

			if (_FadeHandler != null)
				parent.StopCoroutine(_FadeHandler);

			_FadeHandler = parent.StartCoroutine(C_DoFadeCycle(targetAlpha));
		}

		private IEnumerator C_DoFadeCycle(float targetAlpha)
		{
			Fading = true;

			while (Mathf.Abs(_Image.color.a - targetAlpha) > 0.01f)
			{
				_Image.color = Color.Lerp(_Image.color, new Color(_Image.color.r, _Image.color.g, _Image.color.b, targetAlpha), _FadeInSpeed * Time.deltaTime);

				yield return null;
			}

			_Image.color = new Color(_Image.color.r, _Image.color.g, _Image.color.b, targetAlpha);

			if (_FadeOutPause > 0f)
				yield return new WaitForSeconds(_FadeOutPause);

			while (_Image.color.a > 0.01f)
			{
				_Image.color = Color.Lerp(_Image.color, new Color(_Image.color.r, _Image.color.g, _Image.color.b, 0f), _FadeOutSpeed * Time.deltaTime);
				yield return null;
			}

			_Image.color = new Color(_Image.color.r, _Image.color.g, _Image.color.b, 0f);

			Fading = false;
		}
	}
}
