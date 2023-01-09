using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;
namespace PuppyScripts.EditorToolKit
{
    public class EditorButton : MonoBehaviour
    {
        public Button button;
        [ContextMenu("PressButton")]
        public void PressButton()
        {
            button.onClick.Invoke();
        }
    }
}
