using System;
using Cinemachine;
using UnityEngine;

namespace Platformer
{
    public class CameraController : MonoBehaviour
    {
        private bool isFollowing = false;
        
        public void OnEnable()
        {
            Debug.Log("oh hello");
            
            PhotonPlaying.OnCreateMyPlayer += OnStartFollowing;
        }

        public void OnDisable()
        {
            Debug.Log("oh bye");

            PhotonPlaying.OnCreateMyPlayer -= OnStartFollowing;
        }
        
        private void Start()
        {
        }

        private void OnStartFollowing(Transform transform)
        {
            Debug.Log(transform);
            var cam = gameObject.GetComponent<CinemachineVirtualCamera>();
            cam.Follow = transform;
        }
    }
}