using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioSyncScale : AudioSyncer {

	private IEnumerator MoveToScale(Vector3 _target)
	{
		Vector3 pos = transform.localScale;
		Vector3 initital_pos = pos;
		float time_t = 0;

		while (pos != _target)
		{
			pos = Vector3.Lerp(initital_pos, _target, time_t / timeToBeat);
			time_t += Time.deltaTime;
			transform.localScale = pos;
			yield return null;
		}

		m_isBeat = false;
	}

	public override void OnUpdate()
	{
		base.OnUpdate();

		if (m_isBeat) return;

		transform.localScale = Vector3.Lerp(transform.localScale, restScale, restSmoothTime * Time.deltaTime);
	}

	public override void OnBeat()
	{
		base.OnBeat();

		StopCoroutine("MoveToScale");
		StartCoroutine("MoveToScale", beatScale);
	}

	public Vector3 beatScale;
	public Vector3 restScale;
}
