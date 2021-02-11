using UnityEngine;

namespace BezmicanZehir.Core
{
    public class Obstacle : MonoBehaviour
    {
        [SerializeField] private float impulsePower;
        private Rigidbody _rb;

        public bool isKiller;

        private void Start()
        {
            _rb = GetComponent<Rigidbody>();
        }

        private void OnTriggerStay(Collider other)
        {
            _rb.AddForce(Vector3.down * impulsePower, ForceMode.Impulse);
        }
    }
}
