using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tentacle : MonoBehaviour
{
    public int length; 
    public LineRenderer lineRen;
    public Transform target;
    public float targetDistance;
    public float smoothSpeed;
    public Transform[] bodyParts;
    public GameObject ExplosionFX;
    private Vector3[] _segments;
    private Vector3[] _segmentV;
    private bool _is_dead = false; 
    private void Start() { 
        lineRen.positionCount = length;
        _segments = new Vector3[length];
        _segmentV = new Vector3[length];
        resetPos();
    }

    private void Update() {
        if(_is_dead) return;
        _segments[0] = target.position;

        for(int i = 1; i < _segments.Length; i++) {
            Vector3 targetPos = _segments[i - 1] + (_segments[i] - _segments[i-1]).normalized * targetDistance;
            _segments[i] = Vector3.SmoothDamp(_segments[i], targetPos, ref _segmentV[i], smoothSpeed);
            bodyParts[i-1].transform.position = _segments[i];
        }
        lineRen.SetPositions(_segments);
    }

    private void resetPos() {
        _segments[0] = target.position;
        for(int i = 1; i < length; i++) {
            _segments[i] = _segments[i-1] + target.right * targetDistance;
        }
        lineRen.SetPositions(_segments);
    }

    public void onDeath() {
        _is_dead = true;
    }

}
