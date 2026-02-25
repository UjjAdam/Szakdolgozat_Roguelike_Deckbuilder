using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Splines;

public class HandView : MonoBehaviour
{
    [SerializeField] private SplineContainer splineContainer;
    private readonly List<CardView> handcards = new();

    public IEnumerator AddCard(CardView cardView)
    {
        handcards.Add(cardView);
        yield return UpdateCardPosition(0.15f);
    }

    public CardView RemoveCard(Card card)
    {
        CardView cardView = GetCardView(card);
        if (cardView == null)
        {
            return null;
        }
        handcards.Remove(cardView);
        StartCoroutine(UpdateCardPosition(0.15f));
        return cardView;
    }

    private CardView GetCardView(Card card)
    {
        return handcards.Where(cardView => cardView.Card == card).FirstOrDefault();
    }

    private IEnumerator UpdateCardPosition(float duration)
    {
        if (handcards.Count == 0)
        {
            yield break;
        }
        float cardSpacing = 1f / 10f; //10f is the maximum number of cards allowed in hand
        float firstCardPosition = 0.5f - (handcards.Count-1) * cardSpacing/2;
        Spline spline = splineContainer.Spline;
        for (int i = 0; i < handcards.Count; i++) //positions each card in the right position
        {
            float p = firstCardPosition + i * cardSpacing;
            Vector3 splinePosition = spline.EvaluatePosition(p);
            Vector3 forward = spline.EvaluateTangent(p);
            Vector3 up = spline.EvaluateUpVector(p);
            Quaternion rotation = Quaternion.LookRotation(-up, Vector3.Cross(-up, forward).normalized);
            handcards[i].transform.DOMove(splinePosition + transform.position + 0.01f * i * Vector3.back,duration);
            handcards[i].transform.DORotate(rotation.eulerAngles, duration);

        }
        yield return new WaitForSeconds(duration);
    }

    //Helper

    public void ClearAllCards()
    {
        foreach (var cardView in handcards)
        {
            if (cardView != null)
                Destroy(cardView.gameObject);
        }
        handcards.Clear();
    }
}
