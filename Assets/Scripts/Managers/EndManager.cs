using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndManager : MonoBehaviour
{
    public event System.Action OnEscape;

    // Singleton
    public static EndManager em;

    private void Awake()
    {
        // Singleton
        if (em == null)
        {
            em = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void Escaped()
    {
        OnEscape?.Invoke();
    }
}