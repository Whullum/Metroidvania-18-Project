using System;
using System.Linq;
using UnityEngine;

// Credits: https://www.reddit.com/r/Unity3D/comments/fdc2on/easily_generate_unique_ids_for_your_game_objects/
public class UniqueGUID : MonoBehaviour
{
    [SerializeField] private UniqueID _GUID;

    public string ID
    {
        get { return _GUID.Value; }
    }

    [ContextMenu("Force reset ID")]
    private void ResetId()
    {
        _GUID.Value = Guid.NewGuid().ToString();
        Debug.Log("Setting new ID on object: " + gameObject.name, gameObject);
    }

    //Need to check for duplicates when copying a gameobject/component
    public static bool IsUnique(string ID)
    {
        return Resources.FindObjectsOfTypeAll<UniqueGUID>().Count(x => x.ID == ID) == 1;
    }

    protected void OnValidate()
    {
        //If scene is not valid, the gameobject is most likely not instantiated (ex. prefabs)
        if (!gameObject.scene.IsValid())
        {
            _GUID.Value = string.Empty;
            return;
        }

        if (string.IsNullOrEmpty(ID) || !IsUnique(ID))
        {
            ResetId();
        }
    }

    [Serializable]
    private struct UniqueID
    {
        public string Value;
    }
}