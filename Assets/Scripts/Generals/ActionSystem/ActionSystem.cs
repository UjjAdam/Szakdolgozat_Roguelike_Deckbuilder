using System.Collections.Generic;
using System.Collections;
using System;
using UnityEngine;

public class ActionSystem : Singleton<ActionSystem>
{
    private List<GameAction> reactions = null;

    public bool isPerforming { get; private set; } = false;

    private static Dictionary<Type, List<Action<GameAction>>> preSubs = new();
    private static Dictionary<Type, List<Action<GameAction>>> postSubs = new();
    private static Dictionary<Type, Func<GameAction,IEnumerator>> performers = new();

    // mapping to be able to remove the exact wrapper when unsubscribing
    private static Dictionary<Type, Dictionary<Delegate, Action<GameAction>>> preWrappedMap = new();
    private static Dictionary<Type, Dictionary<Delegate, Action<GameAction>>> postWrappedMap = new();

    public void Perform(GameAction action, System.Action OnPerformFinished = null)
    {
        if (isPerforming) return;
        isPerforming = true;
        StartCoroutine(Flow(action, () =>
         {
             isPerforming = false;
             OnPerformFinished?.Invoke();
         }
        ));

    }
    public void AddReaction(GameAction gameAction)
    {
        if (reactions == null)
            reactions = new List<GameAction>();
        reactions.Add(gameAction);
    }

    private IEnumerator Flow(GameAction action, Action OnFlowFinished = null)
    {
        reactions = action.PreReaction;
        PerformSubscribers(action, preSubs);
        yield return PerformReactions();

        reactions = action.PerformReaction;
        yield return PerformPerformer(action);
        yield return PerformReactions();

        reactions = action.PostReaction;
        PerformSubscribers(action, postSubs);
        yield return PerformReactions();

        OnFlowFinished?.Invoke();
    }

    private IEnumerator PerformPerformer(GameAction action)
    {
        Type type = action.GetType();
        if (performers.ContainsKey(type))
        {
            yield return performers[type](action);
        }
    }

    private void PerformSubscribers(GameAction action,Dictionary<Type,List<Action<GameAction>>> subs)
    {
        Type type = action.GetType();
        if (subs.ContainsKey(type))
        {
            // iterate over a copy in case subscribers modify subscriptions
            var copy = new List<Action<GameAction>>(subs[type]);
            foreach (var sub in copy)
            {
                sub(action);
            }
        }
    }

    private IEnumerator PerformReactions()
    {
        if (reactions == null || reactions.Count == 0)
            yield break;

        // iterate over a snapshot because reactions list may be modified during flow
        var snapshot = new List<GameAction>(reactions);
        // clear current reactions so newly added reactions will be collected fresh
        reactions.Clear();

        foreach (var reaction in snapshot)
        {
            yield return Flow(reaction);
        }
    }
    

    private IEnumerator PerformActions()
    {
        if (reactions == null || reactions.Count == 0)
            yield break;

        var snapshot = new List<GameAction>(reactions);
        reactions.Clear();
        foreach (var reaction in snapshot)
        {
            yield return Flow(reaction);
        }
    }

    public static void AttachPerformer<T>(Func<T, IEnumerator> performer) where T:GameAction
    {
        Type type = typeof(T);
        IEnumerator wrappedPerformer(GameAction action) => performer((T)action);
        if (performers.ContainsKey(type))
        {
            performers[type] = wrappedPerformer;
        }
        else
        {
            performers.Add(type, wrappedPerformer);
        }
    }

    public static void DetachPerformer<T>() where T : GameAction
    {
        Type type = typeof(T);
        if (performers.ContainsKey(type))
        {
            performers.Remove(type);
        }
    }

    public static void SubscribeReaction<T>(Action<T> reaction,ReactionTiming timing) where T : GameAction
    {
        Dictionary<Type, List<Action<GameAction>>> subs = timing == ReactionTiming.PRE ? preSubs : postSubs;
        var wrappedMap = timing == ReactionTiming.PRE ? preWrappedMap : postWrappedMap;
        void wrappedReaction(GameAction action) => reaction((T)action);

        Type t = typeof(T);

        if (subs.ContainsKey(t))
        {
            subs[t].Add(wrappedReaction);
        }
        else
        {
            subs.Add(t, new());
            subs[t].Add(wrappedReaction);
        }

        if (!wrappedMap.ContainsKey(t))
            wrappedMap[t] = new Dictionary<Delegate, Action<GameAction>>();

        // store mapping so we can remove exact wrapper later
        wrappedMap[t][reaction] = wrappedReaction;
    }

    public static void UnsubscribeReaction<T>(Action<T> reaction, ReactionTiming timing) where T : GameAction
    {
        Dictionary<Type, List<Action<GameAction>>> subs = timing == ReactionTiming.PRE ? preSubs : postSubs;
        var wrappedMap = timing == ReactionTiming.PRE ? preWrappedMap : postWrappedMap;
        Type t = typeof(T);

        if (subs.ContainsKey(t) && wrappedMap.ContainsKey(t) && wrappedMap[t].ContainsKey(reaction))
        {
            Action<GameAction> wrapped = wrappedMap[t][reaction];
            subs[t].Remove(wrapped);
            wrappedMap[t].Remove(reaction);

            // if no more entries, clean up
            if (subs[t].Count == 0)
                subs.Remove(t);
            if (wrappedMap.ContainsKey(t) && wrappedMap[t].Count == 0)
                wrappedMap.Remove(t);
        }
    }

    public void Reset()
    {
        preSubs.Clear();
        postSubs.Clear();
        performers.Clear();
        preWrappedMap.Clear();
        postWrappedMap.Clear();
        reactions?.Clear();
        reactions = null;
        isPerforming = false;
    }

}
