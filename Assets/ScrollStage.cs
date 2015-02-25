using UnityEngine;

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

	[ContextMenu ("Align Children")]
	private void AlignChildren () {
		Vector3 origin = firstChild.localPosition;
		foreach (Transform child in transform) {
			child.localPosition = origin + transform.forward * unitSize * child.GetSiblingIndex();
		}
	}
}
