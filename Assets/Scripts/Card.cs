using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Card : MonoBehaviour
{
	public CardStruct Definition;

	// State for card being dealt and flying out of deck.
	bool m_flying;
	float m_flyTime;
	float m_flyDuration;
	Vector3 m_flySource;
	Vector3 m_flyTarget;

    public void SetFlyTarget(Vector3 source, Vector3 target, float duration)
	{
		m_flying = true;
		m_flyTime = Time.time;
		m_flyDuration = duration;
		m_flySource = source;
		m_flyTarget = target;
	}
	
	// Update is called once per frame
	void Update ()
	{
	
		if (m_flying)
		{
			float t = Time.time - m_flyTime;
			Vector3 pos = transform.position;
			Quaternion rot = transform.rotation;
			if (t < m_flyDuration)
			{
				float tt = t/m_flyDuration;
				pos = Vector3.Lerp(m_flySource,m_flyTarget,tt);
				// parabolic arc to lift card of deck
				pos.z += -2*Mathf.Sin(tt*3.14f);				// delay card flip until 25% of flight
				float rt = Mathf.Clamp01(tt-0.25f)/0.75f;
				tt = 1-rt*rt;
				rot = Quaternion.Euler(0,180*rt*rt,0);
			}
			else
			{
				pos = m_flyTarget;
				rot = Quaternion.Euler(0, -180, 0);
                m_flying = false;
			}
			this.transform.position = pos;
			this.transform.rotation = rot;
		}
	}
}
