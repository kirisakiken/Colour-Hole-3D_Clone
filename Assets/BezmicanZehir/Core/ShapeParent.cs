using System;
using UnityEngine;

namespace BezmicanZehir.Core
{
    public class ShapeParent : MonoBehaviour
    {
        [SerializeField] private Rigidbody[] childRbArr;
        private bool _triggered;

        private void Start()
        {
            _triggered = false;
            childRbArr = GetComponentsInChildren<Rigidbody>();
        }

        private void OnTriggerEnter(Collider other)
        {
            if (_triggered) return;
            
            if (other.CompareTag("Hole"))
            {
                _triggered = true;
                
                foreach (var childRb in childRbArr)
                {
                    childRb.isKinematic = false;
                }
            }
        }
    }
}
