using UnityEngine;
using System.Collections;

public class RotateTransform : MonoBehaviour {
    [Header("Settings")]
    public float Speed = 1.0f;

	void Update () {

        this.transform.Rotate(transform.position.x, Speed, transform.position.z);
	}
}
