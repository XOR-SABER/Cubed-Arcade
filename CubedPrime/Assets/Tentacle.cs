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

    private Vector3[] _segments;
    private Vector3[] _segmentV;

    private void Start() { 
        lineRen.positionCount = length;
        _segments = new Vector3[length];
        _segmentV = new Vector3[length];
        resetPos();
    }

    private void Update() {
        _segments[0] = target.position;

        for(int i = 1; i < _segments.Length; i++) {
            // segments[i] = Vector3.SmoothDamp(segments[i], segments[i-1] + target.right * targetDistance, ref segmentV[i], smoothSpeed + i / trailSpeed);
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
}
