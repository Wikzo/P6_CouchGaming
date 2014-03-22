using UnityEngine;
using System.Collections;

// from https://docs.unity3d.com/Documentation/Manual/RandomNumbers.html

public struct Card
{
    public int number;
    public float probability;
}

public class GettingRandomStuff : MonoBehaviour
{

    void Start()
    {
        Card[] availableCards = new Card[10];
        for (int i = 0; i < availableCards.Length; i++)
        {
            availableCards[i].number = i;
        }

        availableCards[0].probability = 0.9f;
        availableCards[1].probability = 0.9f;
        availableCards[2].probability = 0.9f;
        availableCards[3].probability = 0.9f;
        availableCards[4].probability = 0.2f;


        ShuffleCards(availableCards);
        /*foreach (Card card in availableCards)
        {
            print(card.number);
        }*/

        Card[] setCards = new Card[5];
        setCards = ChooseCardsFromSet(setCards.Length, availableCards);
        foreach (Card card in setCards)
        {
            print(card.number);
        }

    }

    Card ChooseRandomCard(Card[] cards)
    {
        float total = 0;

        foreach (Card c in cards)
            total += c.probability; // add up all probabilities

        float random = Random.value*total; // Random.value returns value between 0.0 and 1.0

        for (int i = 0; i < cards.Length; i++)
        {
            if (cards[i].probability > random)
                return cards[i];
            else
                random -= cards[i].probability;
        }

        return cards[cards.Length - 1];
    }

    void ShuffleCards(Card[] cards)
    {
        for (int i = 0; i < cards.Length; i++)
        {
            Card temp = cards[i];
            int random = Random.Range(0, cards.Length);
            cards[i] = cards[random];
            cards[random] = temp;
        }
    }

    Card[] ChooseCardsFromSet(int howManyToChoose, Card[] availableCards)
    {
        // Example: have 10 available cards to choose from, but only needs to choose 5
        // The probability of the first item being chosen will be 5 / 10 or 0.5
        // If it's chosen then the probability for the second item will be 4 / 9 or 0.44 (ie, four items still needed, nine left to choose from)
        // However, if the first was not chosen then the probability for the second will be 5 / 9 or 0.56 (ie, five still needed, nine left to choose from)
        // This continues until the set contains the five items required.

        Card[] result = new Card[howManyToChoose];

        int numToChoose = howManyToChoose;

        for (int numLeft = availableCards.Length; numLeft > 0; numLeft--)
        {
            // Adding 0.0 is simply to cast the integers to float for the division.
            float probability = numToChoose + 0.0f / numLeft + 0.0f;

            if (probability >= Random.value)
            {
                numToChoose--;
                result[numToChoose] = availableCards[numLeft - 1];
            }

            if (numToChoose == 0)
                break;
        }

        // Note that although the selection is random, items in the chosen set will be
        // in the same order they had in the original array.
        // If the items are to be used one at a time in sequence then the ordering can make
        // them partly predictable, so it may be necessary to shuffle the array before use.
        ShuffleCards(result);

        return result;
    }
}