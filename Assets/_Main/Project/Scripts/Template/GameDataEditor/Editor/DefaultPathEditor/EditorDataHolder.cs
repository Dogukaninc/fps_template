using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/EditorDataHolder", order = 1)]
public class EditorDataHolder : ScriptableObject
{
    public List<string> DefaultDataPathes;

}
