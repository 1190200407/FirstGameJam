using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Singleton<T> where T : class, new()
{
    // 静态实例
    private static T instance;

    // 锁对象，确保线程安全
    private static readonly object lockObj = new object();

    // 私有构造函数，防止外部实例化
    protected Singleton() { }

    // 获取实例
    public static T Instance
    {
        get
        {
            if (instance == null)
            {
                lock (lockObj)
                {
                    if (instance == null)
                    {
                        instance = new T();
                    }
                }
            }
            return instance;
        }
    }
}

public class MonoSingleton<T> : MonoBehaviour where T : MonoSingleton<T>
{
    // 静态实例
    private static T instance;

    // 获取实例
    public static T Instance
    {
        get
        {
            if (instance == null)
            {
                // 查找是否已有实例
                instance = FindObjectOfType<T>();

                if (instance == null)
                {
                    // 创建新的GameObject并附加该组件
                    GameObject singletonObj = new GameObject(typeof(T).Name);
                    instance = singletonObj.AddComponent<T>();
                }
            }
            return instance;
        }
    }

    // 当脚本被加载时，确保单例唯一性
    protected virtual void Awake()
    {
        if (instance == null)
        {
            instance = (T)this;
        }
        else if (instance != this)
        {
            Destroy(gameObject); // 如果已有实例，销毁当前对象
        }
    }
}
