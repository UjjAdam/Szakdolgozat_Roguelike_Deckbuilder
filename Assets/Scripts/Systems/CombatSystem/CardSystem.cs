using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardSystem : Singleton<CardSystem>
{
    [SerializeField] private HandView handView;
    [SerializeField] private Transform drawPilePoint;
    [SerializeField] private Transform discardPilePoint;

    private readonly List<Card> drawPile = new();
    private readonly List<Card> hand = new();
    private readonly List<Card> discardPile = new();

    private void OnEnable()
    {
        ActionSystem.AttachPerformer<DrawCardsGameAction>(DrawCardPerformer);
        ActionSystem.AttachPerformer<DiscardAllCardsGameAction>(DiscardAllCardPerformer);
        ActionSystem.AttachPerformer<PlayCardGameAction>(PlayCardPerformer);

    }

    private void OnDisable()
    {
        ActionSystem.DetachPerformer<DrawCardsGameAction>();
        ActionSystem.DetachPerformer<DiscardAllCardsGameAction>();
        ActionSystem.DetachPerformer<PlayCardGameAction>();

    }
    //Publics
    public void Setup(List<CardData> deckData)
    {
        foreach (var cardData in deckData)
        {
            Card card = new(cardData);
            drawPile.Add(card);
        }
    }

    // Helper to check whether a runtime deck already exists (draw/hand/discard)
    public bool HasAnyCards()
    {
        return drawPile.Count + hand.Count + discardPile.Count > 0;
    }

    // Recreate visual CardView instances for cards currently in hand.
    // Use when a scene reload destroyed previous CardView GameObjects but runtime hand (Card models) still exists.
    public IEnumerator RecreateHandViews()
    {
        // wait for scene binder to attach HandView and for CardViewCreator prefab to be available
        int attempts = 0;
        while ((handView == null || CardViewCreator.Instance == null || !CardViewCreator.Instance.HasPrefab) && attempts < 120)
        {
            attempts++;
            yield return null;
        }

        if (handView == null || CardViewCreator.Instance == null || !CardViewCreator.Instance.HasPrefab)
        {
            Debug.LogWarning("CardSystem.RecreateHandViews: required scene objects or CardView prefab not ready. Aborting recreate.");
            yield break;
        }

        // Clear any leftover visuals then recreate
        handView.ClearAllCards();

        foreach (var card in hand)
        {
            CardView cardView = CardViewCreator.Instance.CreateCardView(card, drawPilePoint.position, drawPilePoint.rotation);
            if (cardView != null)
                yield return handView.AddCard(cardView);
            else
                yield return null;
        }
    }

    //Performers

    private IEnumerator DrawCardPerformer(DrawCardsGameAction drawCardGA)
    {
        // Draw safely: attempt to draw requested amount, refill when needed,
        // but abort gracefully if both draw and discard piles are empty.
        int remaining = drawCardGA.Amount;
        for (int i = 0; i < remaining; i++)
        {
            if (drawPile.Count == 0)
            {
                RefillDeck();
                if (drawPile.Count == 0)
                {
                    // Nothing left to draw
                    yield break;
                }
            }

            yield return DrawCard();
        }
    }

    private IEnumerator DiscardAllCardPerformer(DiscardAllCardsGameAction discardAllCardGA)
    {
        // iterate snapshot because we remove visuals during the loop
        var snapshot = new List<Card>(hand);
        foreach (var card in snapshot)
        {
            CardView cardView = handView != null ? handView.RemoveCard(card) : null;
            yield return DiscardCard(cardView);
        }
        hand.Clear();
    }

    private IEnumerator PlayCardPerformer(PlayCardGameAction playCardGA)
    {
        hand.Remove(playCardGA.Card);
        CardView cardview = handView != null ? handView.RemoveCard(playCardGA.Card) : null;
        yield return DiscardCard(cardview);

        SpendEnergyGameAction spendEnergyGA = new(playCardGA.Card.Energy);
        ActionSystem.Instance.AddReaction(spendEnergyGA);



        if (playCardGA.Card.ManualTargetEffect != null)
        {
            PerformEffectGameAction performEffectGA = new(playCardGA.Card.ManualTargetEffect, new() { playCardGA.ManualTarget });
            ActionSystem.Instance.AddReaction(performEffectGA);
        }
        foreach (var effectWrapper in playCardGA.Card.otherEffects)
        {
            List<CombatantView> targets = effectWrapper.TargetMode.GetTargets();
            PerformEffectGameAction performEffectGA = new(effectWrapper.Effect, targets);
            ActionSystem.Instance.AddReaction(performEffectGA);
        }
    }

    //Helpers

    private IEnumerator DrawCard()
    {
        // Defensive pop from drawPile to avoid unknown extension methods and null returns
        if (drawPile.Count == 0)
        {
            Debug.LogWarning("CardSystem.DrawCard: drawPile empty when trying to draw.");
            yield break;
        }

        int last = drawPile.Count - 1;
        Card card = drawPile[last];
        drawPile.RemoveAt(last);

        if (card == null)
        {
            Debug.LogWarning("CardSystem.DrawCard: popped null Card from drawPile.");
            yield break;
        }

        hand.Add(card);

        // If visuals unavailable (scene not bound) we keep model updated and skip visuals
        if (handView == null || CardViewCreator.Instance == null || !CardViewCreator.Instance.HasPrefab)
            yield break;

        CardView cardView = CardViewCreator.Instance.CreateCardView(card, drawPilePoint.position, drawPilePoint.rotation);
        if (cardView == null)
        {
            Debug.LogWarning($"CardSystem.DrawCard: failed to create CardView for '{card.Title}'.");
            yield break;
        }

        yield return handView.AddCard(cardView);
    }

    private void RefillDeck()
    {
        // Move discard into draw in randomized order if desired; current simplest move
        if (discardPile.Count == 0)
            return;

        // Optional shuffle here if you want randomness
        drawPile.AddRange(discardPile);
        discardPile.Clear();
    }

    public void ResetDeck()
    {

        drawPile.Clear();
        hand.Clear();
        discardPile.Clear();
        handView?.ClearAllCards();
        DOTween.KillAll();
        DOTween.Clear(true);
    }

    private IEnumerator DiscardCard(CardView cardView)
    {
        // safe-guard: cardView may be null if visuals were destroyed — avoid crash
        if (cardView == null || cardView.Card == null)
        {
            yield break;
        }

        discardPile.Add(cardView.Card);

        if (cardView != null)
        {
            cardView.transform.DOScale(Vector3.zero, 0.15f);
            Tween tween = (discardPilePoint != null) ? cardView.transform.DOMove(discardPilePoint.position, 0.15f) : null;
            if (tween != null)
                yield return tween.WaitForCompletion();
            Destroy(cardView.gameObject);
        }
    }
}
