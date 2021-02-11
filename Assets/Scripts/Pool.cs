using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Represents a pool of <see cref="Component"/>.
/// </summary>
/// <typeparam name="T">The <see cref="System.Type"/> of the <see cref="Component"/>.</typeparam>
/// <remarks>
/// For more details, see: https://sourcemaking.com/design_patterns/object_pool
/// </remarks>
public sealed class Pool<T>
    where T : Component
{
    private readonly HashSet<T> actives = new HashSet<T>();
    private readonly Stack<T> availables = new Stack<T>();

    /// <summary>
    /// Creates a new <see cref="Pool{T}"/>.
    /// </summary>
    /// <remarks><see cref="Original"/> will be `null`.</remarks>
    public Pool() : this(null)
    {
    }

    /// <summary>
    /// Creates a new <see cref="Pool{T}"/> from an <see cref="Original"/> <see cref="Component"/>..
    /// </summary>
    /// <param name="original">The value of <see cref="Original"/>.</param>
    public Pool(T original)
    {
        Original = original;
    }

    /// <summary>
    /// Gets the active <see cref="Component"/> that have been <see cref="Get"/>.
    /// </summary>
    public IEnumerable<T> Actives => actives;

    /// <summary>
    /// Gets the availables <see cref="Component"/> to <see cref="Get"/>.
    /// </summary>
    public IEnumerable<T> Availables => availables;

    /// <summary>
    /// Gets the optional original <see cref="Component"/> to instantiate on <see cref="Get"/>.
    /// </summary>
    public T Original { get; }

    /// <summary>
    /// Calls <see cref="Object.Destroy(Object)"/> on each <see cref="Availables"/> <see cref="Component"/>.
    /// </summary>
    public void Clear()
    {
        var availablesCopy = new List<T>(availables);
        foreach (var obj in availablesCopy)
        {
            Object.Destroy(obj.gameObject);
        }
        availables.Clear();
    }

    /// <summary>
    /// Gets an available <see cref="Component"/> or instantiates it.
    /// </summary>
    /// <returns>A new instance of type <typeparamref name="T"/>.</returns>
    /// <remarks>
    /// <see cref="Component"/> instantiation is made with <see cref="Object.Instantiate(Object)"/>
    /// if <see cref="Original"/> is set otherwise with <see cref="GameObject.AddComponent{T}"/> on a new
    /// <see cref="GameObject"/>.
    /// </remarks>
    public T Get()
    {
        T obj;
        if (availables.Count > 0)
        {
            obj = availables.Pop();
        }
        else if (Original != null)
        {
            obj = Object.Instantiate(Original);
        }
        else
        {
            obj = new GameObject().AddComponent<T>();
        }

        actives.Add(obj);
        return obj;
    }

    /// <summary>
    /// Sets <see cref="Availables"/> an <see cref="Component"/> previously instantiated with <see cref="Get"/>.
    /// </summary>
    /// <param name="obj">The <see cref="Component"/> to return.</param>
    public void Return(T obj)
    {
        actives.Remove(obj);
        availables.Push(obj);
    }

    /// <summary>
    /// Sets <see cref="Availables"/> all the <see cref="Component"/> previously instantiated with <see cref="Get"/>.
    /// </summary>
    public void ReturnAll()
    {
        var activatedCopy = new List<T>(actives);
        foreach (var obj in activatedCopy)
        {
            Return(obj);
        }
    }
}
