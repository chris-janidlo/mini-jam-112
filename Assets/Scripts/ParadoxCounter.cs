using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace mj112
{
    public class ParadoxCounter : MonoBehaviour
    {
        public TextMeshProUGUI Text;

        public void OnAteParadox (int count)
        {
            Text.text = count.ToString();
        }
    }
}
