﻿using UnityEngine;
using UnityEngine.Experimental.UIElements;

namespace Assets.Scripts.Movement
{
    public class KuglaMovement : MonoBehaviour
    {
        private PlayerPrefs PlayerPrefs;
        private GameManager GameManager;

        [SerializeField]
        private Rigidbody kugla;

        private bool LeftSide = true;

        private GameObject Terrain;

        private Slider SpeedSlider;

        private Vector3 LastKuglaPosition;

        private void GoLeft(Rigidbody rb)
        {
            rb.velocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
            rb.AddForce(2 * PlayerPrefs.GetFloat("ForwardSpeed"), 0, 2 * PlayerPrefs.GetFloat("SideSpeed"), ForceMode.VelocityChange);
        }

        private void GoRight(Rigidbody rb)
        {
            rb.velocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
            rb.AddForce(2 * PlayerPrefs.GetFloat("ForwardSpeed"), 0, -2 * (PlayerPrefs.GetFloat("SideSpeed")), ForceMode.VelocityChange);
        }

        private void Start()
        {
            var msg = $"Side speed -> {PlayerPrefs.GetFloat("SideSpeed")}";
            var msgg = $"Forward speed -> {PlayerPrefs.GetFloat("ForwardSpeed")}";

            Debug.Log(msg);
            Debug.Log(msgg);

            GoLeft(kugla);
            Terrain = GameObject.FindGameObjectWithTag(Constants.TerrainTag);
            GameManager = FindObjectOfType<GameManager>();
        }

        private void OnCollisionEnter(Collision collision)
        {
            if (collision.collider.tag == Constants.WallTag)
            {
                if (LeftSide)
                    GoRight(kugla);
                else
                    GoLeft(kugla);
                LeftSide = !LeftSide;
            }
        }

        private void FixedUpdate()
        {
            if (LastKuglaPosition == transform.position)
                GameManager.GameOver();

            LastKuglaPosition = transform.position;

            if (kugla.transform.position.y < 0)
            {
                GameManager.GameOver();
            }

            if (kugla.transform.position.x % 20 < 1)
            {
                Terrain.transform.localScale += new Vector3(20, 0, 0);
            }
        }

        public void AdjustSideSpeed(float speed)
        {
            PlayerPrefs.SetFloat("SideSpeed", speed);
            PlayerPrefs.Save();
        }

        public void AdjustForwardSpeed(float speed)
        {
            PlayerPrefs.SetFloat("ForwardSpeed", speed);
            PlayerPrefs.Save();
        }
    }
}