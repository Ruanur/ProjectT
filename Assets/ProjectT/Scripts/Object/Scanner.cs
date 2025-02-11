using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scanner : MonoBehaviour
{
    [SerializeField]
    private float _scanRange;
    public float ScanRange
    {
        get { return _scanRange; }
    }
    [SerializeField]
    private LayerMask _targetLayer;
    public LayerMask TargetLayer
    {
        get { return _targetLayer; }
    }
    [SerializeField]
    private RaycastHit2D[] _targets;
    public RaycastHit2D[] Targets
    {
        get { return _targets; }
    }
    [SerializeField]
    private Transform _nearestTarget;
    public Transform NearestTarget
    {
        get { return _nearestTarget; }
    }

    private void FixedUpdate()
    {
        _targets = Physics2D.CircleCastAll(transform.position, _scanRange, Vector2.zero, 0, _targetLayer);
        _nearestTarget = GetNearest();
    }

    private Transform GetNearest()
    {
        Transform result = null;
        float diff = 100;

        foreach (var target in _targets) 
        {
            Vector3 myPos = transform.position;
            Vector3 targetPos = target.transform.position;
            float curDiff = Vector3.Distance(myPos, targetPos);

            if (curDiff < diff)
            {
                diff = curDiff;
                result = target.transform;
            }
        }

        return result;
    }
}
