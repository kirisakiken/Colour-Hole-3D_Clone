using System;
using UnityEngine;

namespace BezmicanZehir.Core
{
    public class HoleController : MonoBehaviour
    {
        [SerializeField] private float holeDiameter;
        
        [Header("Mesh Fields")]
        [SerializeField] private PolygonCollider2D holeCollider;
        [SerializeField] private PolygonCollider2D groundCollider;
        [SerializeField] private MeshCollider generatedMeshCollider;
        public Collider groundMeshCollider;
        private Mesh _generatedMesh;

        public MeshCollider firstStageGroundCollider;
        public MeshCollider middleGroundCollider;
        public MeshCollider secondStageGroundCollider;

        private void Start()
        {
            var objects = FindObjectsOfType(typeof(GameObject)) as GameObject[];

            foreach (var obj in objects)
            {
                if (obj.layer == LayerMask.NameToLayer("Obstacle"))
                {
                    Physics.IgnoreCollision(obj.GetComponent<Collider>(), generatedMeshCollider, true);
                }
            }
        }

        private void FixedUpdate()
        {
            if (transform.hasChanged)
            {
                transform.hasChanged = false;
                var currentPos = transform.position;

                holeCollider.transform.position = new Vector2(currentPos.x, currentPos.z);
                holeCollider.transform.localScale = transform.localScale * holeDiameter;
                
                Debug.Log("Generated Hole");
                GenerateHole();
                GenerateMeshCollider();
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            Physics.IgnoreCollision(other, groundMeshCollider, true);
            Physics.IgnoreCollision(other, generatedMeshCollider, false);
        }

        private void OnTriggerExit(Collider other)
        {
            Physics.IgnoreCollision(other, groundMeshCollider, false);
            Physics.IgnoreCollision(other, generatedMeshCollider, true);
        }

        private void GenerateHole()
        {
            var pointPositions = holeCollider.GetPath(0);

            for (int i = 0; i < pointPositions.Length; i++)
            {
                pointPositions[i] = holeCollider.transform.TransformPoint(pointPositions[i]);
            }

            groundCollider.pathCount = 2;
            groundCollider.SetPath(1, pointPositions);
        }

        private void GenerateMeshCollider()
        {
            if (!(_generatedMesh is null)) Destroy(_generatedMesh);
            
            _generatedMesh = groundCollider.CreateMesh(true, true);
            generatedMeshCollider.sharedMesh = _generatedMesh;
        }

        public void SwitchStageMesh(MeshCollider collider)
        {
            groundMeshCollider = collider;
        }
    }
}
