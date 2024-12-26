using System;

public class Singleton<T> where T : class, new()
{
    private static T instance;
    protected bool m_Init;

    protected Singleton()
    {
    }

    public static T Instance
    {
        get
        {
            if (Singleton<T>.instance == null)
            {
                Singleton<T>.CreateInstance();
            }

            return Singleton<T>.instance;
        }
    }

    public static T GetInstance()
    {
        if (Singleton<T>.instance == null)
        {
            Singleton<T>.CreateInstance();
        }

        return Singleton<T>.instance;
    }

    public static bool HasInstance()
    {
        return (Singleton<T>.instance != null);
    }

    public static void CreateInstance(bool bInit = true)
    {
        if (Singleton<T>.instance == null)
        {
            Singleton<T>.instance = Activator.CreateInstance<T>();
            if (bInit)
            {
                (Singleton<T>.instance as Singleton<T>).Init();
            }
        }
    }

    public static void DestroyInstance()
    {
        if (Singleton<T>.instance != null)
        {
            (Singleton<T>.instance as Singleton<T>).UnInit();
            Singleton<T>.instance = null;
        }
    }

    public virtual void Init()
    {
        m_Init = true;
    }

    public virtual void UnInit()
    {
        m_Init = false;
    }
}