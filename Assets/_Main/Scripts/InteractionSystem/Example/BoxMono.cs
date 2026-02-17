using UnityEngine;

namespace _Main.Scripts.InteractionSystem
{
    public class BoxMono : BaseAbstractInteractable
    {
            public override void Interact()
            {
                base.Interact();
                Debug.Log("Box Interacted !");
            }
    }
}