using System;
using System.Collections.Generic;

/// <summary>
/// Type-safe static event bus for decoupled communication between systems.
///
/// Usage:
///   struct PlayerDiedEvent { public int score; }
///
///   // Subscribe (call in OnEnable, unsubscribe in OnDisable)
///   EventBus.Subscribe<PlayerDiedEvent>(OnPlayerDied);
///   void OnPlayerDied(PlayerDiedEvent e) { ... }
///
///   // Publish
///   EventBus.Publish(new PlayerDiedEvent { score = 500 });
/// </summary>
public static class EventBus
{
    private static readonly Dictionary<Type, List<Delegate>> handlers =
        new Dictionary<Type, List<Delegate>>();

    public static void Subscribe<T>(Action<T> handler)
    {
        Type type = typeof(T);
        if (!handlers.ContainsKey(type))
            handlers[type] = new List<Delegate>();
        handlers[type].Add(handler);
    }

    public static void Unsubscribe<T>(Action<T> handler)
    {
        Type type = typeof(T);
        if (handlers.ContainsKey(type))
            handlers[type].Remove(handler);
    }

    public static void Publish<T>(T eventData)
    {
        Type type = typeof(T);
        if (!handlers.TryGetValue(type, out var list)) return;
        // Iterate over a copy so handlers can unsubscribe during dispatch
        foreach (Delegate d in list.ToArray())
            ((Action<T>)d)?.Invoke(eventData);
    }

    /// <summary>Call on scene unload to remove stale subscribers.</summary>
    public static void Clear()
    {
        handlers.Clear();
    }
}
