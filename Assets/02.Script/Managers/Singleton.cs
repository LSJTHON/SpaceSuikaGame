using UnityEngine;

public class Singleton<T> : MonoBehaviour where T : Singleton<T>
{
    [SerializeField]
    private bool isDontDestroy = true;

    private static T instance = null;
    protected virtual void Awake()
    {

        if (instance == null)
        {
            instance = (T)this;

            if (isDontDestroy)
            {
                DontDestroyOnLoad(this.gameObject);
            }
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

    public static T Instance
    {
        get
        {
            if (instance == null)
            {
                return null;
            }
            return instance;
        }
    }
}