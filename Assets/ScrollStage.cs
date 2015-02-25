using UnityEngine;
#if UNITY_EDITOR
using System.Linq;
using System.Collections.Generic;
#endif

public class ScrollStage : MonoBehaviour {

	public float speed = 10f;
	public float unitSize = 1;

	private Transform m_transform;
	new protected Transform transform {
		get {
			if (m_transform == null) m_transform = base.transform;
			return m_transform;
		}
	}

	protected Transform firstChild { get { return transform.GetChild (0); } }
	protected Transform lastChild { get { return transform.GetChild (transform.childCount - 1); } }

	protected float edge;

	public void Start () {
		edge = firstChild.localPosition.z - unitSize;
	}
	
	public void Update () {
		foreach(Transform child in transform) {
			child.Translate(Vector3.back * speed * Time.deltaTime);
		}
	}

	public void LateUpdate () {
		if (firstChild.localPosition.z < edge) {
			OnEdge ();
		}
	}

	virtual protected void OnEdge () {
		firstChild.localPosition = lastChild.localPosition + Vector3.forward * unitSize;
		firstChild.SetAsLastSibling ();
	}

#if UNITY_EDITOR
	[ContextMenu ("Sort Children By LocalPosition Z")]
	private void SortChildren () {
		List<Transform> temp = transform.Cast<Transform>()
			.OrderBy( (c) => c.localPosition.z )
			.ToList();

		for(int i = 0, len = temp.Count; i < len; i++) {
			Transform child = temp[i];
			child.SetSiblingIndex (i);
		}
	}
#endif
}
