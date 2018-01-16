using System.Collections;
using UnityEngine;
using Wrld;
using Wrld.Space;

public class PositionObjectAtLatitudeAndLongitude: MonoBehaviour
{
    static LatLong pointA = LatLong.FromDegrees(40.025147, -105.285932);

    public GeographicTransform coordinateFrame;
    public Transform box;

    private void OnEnable()
    {
        Api.Instance.GeographicApi.RegisterGeographicTransform(coordinateFrame);
        StartCoroutine(Example());
    }

    IEnumerator Example()
    {
        Api.Instance.CameraApi.MoveTo(pointA, distanceFromInterest: 1000, headingDegrees: 0, tiltDegrees: 45);
        box.localPosition = new Vector3(0.0f, 40.0f, 0.0f);

        while (true)
        {
            yield return new WaitForSeconds(2.0f);
            coordinateFrame.SetPosition(pointA);
        }
    }

    private void OnDisable()
    {
        StopAllCoroutines();
        Api.Instance.GeographicApi.UnregisterGeographicTransform(coordinateFrame);
    }
}

