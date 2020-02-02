using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BezierSolution;

public class MoveWithinBoundaries2D : MonoBehaviour
{
    public Vector2 xAxisRange;
    public Vector2 yAxisRange;
    public Transform boundTarget;
    public float speed = 3f;

    public BezierWalkerWithSpeed bezierWalk;
    private void Update ( )
    {
        var currentVerticalInput = Input.GetAxis ( "Vertical" );
        var currentHorizontalInput = Input.GetAxis ( "Horizontal" );

        var smoothing = Time.smoothDeltaTime*speed;

        var currentLocalPotiision = boundTarget.transform.localPosition;
        var adjustedPosition = currentLocalPotiision;

        adjustedPosition.x = currentHorizontalInput == 0 ? currentLocalPotiision.x :
                currentHorizontalInput < 0 ? xAxisRange.x : xAxisRange.y;

        adjustedPosition.y = currentVerticalInput == 0 ? currentLocalPotiision.y :
                currentVerticalInput < 0 ? yAxisRange.x : yAxisRange.y;

        adjustedPosition.x = Mathf.Lerp ( currentLocalPotiision.x, adjustedPosition.x, smoothing);
        adjustedPosition.x = Mathf.MoveTowards ( currentLocalPotiision.x, adjustedPosition.x, smoothing );

        adjustedPosition.y = Mathf.Lerp ( currentLocalPotiision.y, adjustedPosition.y, smoothing );
        adjustedPosition.y = Mathf.MoveTowards ( currentLocalPotiision.y, adjustedPosition.y, smoothing );

        boundTarget.transform.localPosition = adjustedPosition;

        //if(!bezierWalk.walking)
        //    boundTarget.transform.localPosition = adjustedPosition;
    }
}
