using CrossProject.Core;
using UnityEngine;
using UnityEngine.Splines;

namespace RUNNER.Scripts.Core
{
    public class SplineRunner : CustomBehaviour
    {
        [SerializeField] private SplineAnimate splineAnimate;
        [SerializeField] private Transform viewRoot;

        protected override void OnAwake()
        {
            splineAnimate.Container ??= FindObjectOfType<SplineContainer>();
            float yPos = splineAnimate.Container.GetComponent<SplineExtrude>().Radius / 2;
            viewRoot.localPosition = new Vector3(0, yPos, 0);
        }
    }
}