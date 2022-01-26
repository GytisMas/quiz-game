using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Array/List with quicker item removal.
/// </summary>
/// <typeparam name="E">Item type</typeparam>
public class Container<E>
{
    private const int defaultCapacity = 20;

    public int Count;
    private int Capacity;
    private E[] elements;

    public Container(int capacity = defaultCapacity)
    {
        Capacity = capacity;
        Count = 0;
        elements = new E[Capacity];
    }

    public void CheckCapacity()
    {
        if (Count == Capacity)
        {
            Capacity *= 2;
            E[] newArray = new E[Capacity];
            for (int i = 0; i < elements.Length; i++)
            {
                newArray[i] = elements[i];
            }
            elements = newArray;
        }
    }

    public E Get(int index)
    {
        return elements[index];
    }

    public void Add(E element)
    {
        CheckCapacity();
        elements[Count++] = element;
    }

    public E RemoveByIndex(int index)
    {
        if (index >= Count || index < 0)
        {
            Debug.LogWarning("Could not remove element, index value invalid");
            return default;
        }
        E elementToRemove = elements[index];
        if (index != Count - 1)
            elements[index] = elements[Count - 1];
        Count--;
        return elementToRemove;
    }
}
