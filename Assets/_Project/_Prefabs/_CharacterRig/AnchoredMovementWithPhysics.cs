using UnityEngine;
using System;
using BezierSolution;

[Serializable]
public struct AnchorPoint
{
	public Transform pointOfForce;
	public float trackingForce;
	public float offsetDistance;
	public Vector3 offset;
	public Vector3 offsetFromTarget;
	public Vector3 targetDirection;
}

[
	DisallowMultipleComponent,
	RequireComponent ( typeof ( Rigidbody ) )
]
public class AnchoredMovementWithPhysics : MonoBehaviour
{
	[SerializeField] private Rigidbody _itsRigidbody = null;
	public Rigidbody itsRigidbody => _itsRigidbody;

	[SerializeField] private Transform _anchorsTarget = null;
	public Transform anchorsTarget => _anchorsTarget;

	[SerializeField] private AnchorPoint [ ] _anchors = null;
	public AnchorPoint [ ] anchors => _anchors;

	[SerializeField] private float _trackingForceMultiplier = 1f;
	public float  trackingForceMultiplier => _trackingForceMultiplier;

	[SerializeField] private int _amountOfAnchors;
	private Vector3 _currentTargetPosition;
	private Quaternion _currrentTargetRotation;
	private AnchorPoint _currrentTrackingAnchor;
	private Vector3 _currrentPointOfForce;
	private Vector3 _currrenTrackingForce;


    public BezierWalkerWithSpeed bezierWalk;
    private void FixedUpdate ( ) { SpreadTrackingForceAcrossAnchors ( ); }

	public void UpdateAnchorsForceDistribution ( )
	{
		var amountOfForcePoints = anchors.Length;
		var center = new Vector3 ( );
		for ( int i = 0; i < amountOfForcePoints; i++ )
		{
			center.x += anchors [ i ].pointOfForce.localPosition.x;
			center.y += anchors [ i ].pointOfForce.localPosition.y;
			center.z += anchors [ i ].pointOfForce.localPosition.z;
		}

		center.x = center.x / amountOfForcePoints;
		center.y = center.y / amountOfForcePoints;
		center.z = center.z / amountOfForcePoints;

		for ( int i = 0; i < amountOfForcePoints; i++ )
		{
			var forcePointPosition = anchors [ i ].pointOfForce.localPosition;
			var direction = ( ( forcePointPosition - center ).normalized ) * anchors [ i ].offsetDistance;
			anchors [ i ].offset = forcePointPosition + direction;
		}
	}

    public void SpreadTrackingForceAcrossAnchors()
    {
        if (_amountOfAnchors < 1) return;
		if ( _anchorsTarget == null ) return;

		_currentTargetPosition = _anchorsTarget.position;
		_currrentTargetRotation = _anchorsTarget.rotation;

		for ( int i = 0; i < _amountOfAnchors; i++ )
		{
			_currrentTrackingAnchor = _anchors [ i ];
			_currrentPointOfForce = _currrentTrackingAnchor.pointOfForce.position;

			_currrenTrackingForce = 
				( ( _currentTargetPosition 
				+ (_currrentTargetRotation * _currrentTrackingAnchor.offset) ) 
				- _currrentPointOfForce).normalized;

			itsRigidbody.AddForceAtPosition
				( ( _currrentTrackingAnchor.trackingForce 
				* _trackingForceMultiplier ) 
				* _currrenTrackingForce, 
				_currrentPointOfForce, 
				ForceMode.Acceleration );

#if UNITY_EDITOR
			Debug.DrawLine (_currrentPointOfForce,
					( _currentTargetPosition + ( _currrentTargetRotation * _currrentTrackingAnchor.offset) ), Color.blue );
#endif
		}
	}

#if UNITY_EDITOR
	private void OnValidate ( )
	{
		if ( itsRigidbody == null )
			if ( GetComponentsInChildren<Rigidbody> ( ).Length > 0 )
				_itsRigidbody = GetComponentsInChildren<Rigidbody> ( ) [ 0 ];

		if ( _anchors == null )
			_anchors = new AnchorPoint [0];

		_amountOfAnchors = anchors.Length;

		UpdateAnchorsForceDistribution ( );
		SpreadTrackingForceAcrossAnchors ( );
	}
#endif
}
