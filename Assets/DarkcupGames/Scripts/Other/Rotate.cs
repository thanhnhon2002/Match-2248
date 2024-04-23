using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DarkcupGames
{
    public class Rotate : MonoBehaviour
    {
        public float spinSpeed = 500f;
        private void Update()
        {
            transform.Rotate(new Vector3(0, 0, spinSpeed) * Time.deltaTime);
        }
    }
}