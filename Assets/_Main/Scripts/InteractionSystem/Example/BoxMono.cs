using UnityEngine;

namespace _Main.Scripts.InteractionSystem
{
    public class BoxMono : MonoBehaviour, IInteractable
    {
        public void Interact()
        {
            Debug.Log("Box Interacted");
        }
    }
}