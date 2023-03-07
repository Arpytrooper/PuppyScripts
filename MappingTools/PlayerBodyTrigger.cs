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
        //public LayerMask layersToCheckFor;
        public bool LooksForPlayerBody = false;
        public bool LooksForPlayerHand = false;
        public List<UnityEvent> OnPlayerHeadOrBodyEnter;
        public List<UnityEvent> OnPlayerHeadOrBodyExit;
        public List<UnityEvent> OnPlayerHandEnter;
        public List<UnityEvent> OnPlayerHandExit;
        private void OnTriggerEnter(Collider other)
        {
            if (LooksForPlayerBody && other.gameObject.layer == 15)
            {
                if (OnPlayerHeadOrBodyEnter != null)
                    foreach (UnityEvent e in OnPlayerHeadOrBodyEnter)
                    {
                        e.Invoke();
                    }
            }
            else if ( other.gameObject.layer == 9)
            {
                if (OnPlayerHandEnter != null) 
                    foreach (UnityEvent e in OnPlayerHandEnter)
                    e.Invoke();
                
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (LooksForPlayerBody && other.gameObject.layer == 15)
            {
                if (OnPlayerHeadOrBodyExit != null)
                    foreach (UnityEvent e in OnPlayerHeadOrBodyExit)
                    {
                        e.Invoke();
                    }
            }
            else if (LooksForPlayerHand && other.gameObject.layer == 9)
            {
                if (OnPlayerHeadOrBodyExit != null)
                    foreach (UnityEvent e in OnPlayerHandExit)
                        e.Invoke();
            }
        }


    }
}
