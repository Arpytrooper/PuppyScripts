using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.Events;
using FistVR;

namespace PuppyScripts
{
    public class PlayerBodyTrigger : MonoBehaviour
    {
        public LayerMask layersToCheckFor;
        public UnityEvent OnEnter;
        public UnityEvent OnExit;
        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.layer == layersToCheckFor)
            {
                if (OnEnter != null)
                    OnEnter.Invoke();
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.gameObject.layer == layersToCheckFor)
            {
                if (OnExit != null)
                    OnExit.Invoke();
            }
        }


    }
}
