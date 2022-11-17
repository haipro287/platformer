using System;
using UnityEngine;

namespace Platformer
{
    public class CameraFollower: MonoBehaviour
    {
        public new GameObject camera;

        private void LateUpdate()
        {
            var camerax = camera.transform.position.x;
            var cameray = camera.transform.position.y;
            transform.position = new Vector3(camerax, cameray, transform.position.z);
        }
    }
}