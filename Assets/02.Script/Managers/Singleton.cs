using UnityEngine;

public class Singleton<T> : MonoBehaviour where T : Singleton<T>
{
    [SerializeField]
    private bool isDontDestroy = true;

    private static T instance = null;
    protected virtual void Awake()
    {

        if (null == instance)
        {
            instance = (T)this;

            if (isDontDestroy)
                DontDestroyOnLoad(this.gameObject);
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
            if (null == instance)
            {
                return null;
            }
            return instance;
        }
    }
}