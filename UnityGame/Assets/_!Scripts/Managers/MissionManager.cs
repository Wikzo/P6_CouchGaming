using System.Collections.Generic;
using UnityEngine;
using System.Collections;

public class MissionManager : MonoBehaviour
{
    public GameObject[] AvailableMissions;
    public GameObject[] ChosenMissions;

    public IMission[] IAvailableMissions;
    public KillMission[] kills;

 
    // Use this for initialization
    private void Start()
    {
        IAvailableMissions = new IMission[10];
        

        foreach (var m in GetComponents(typeof(IMission)))
        {
            print(m);
        }

        //ChosenMissions = ChooseCardsFromSet(5, AvailableMissions);

        ChosenMissions = new GameObject[10];
        for (int i = 0; i < 10; i++)
        {
            ChosenMissions[i] = ChooseRandomMission(AvailableMissions);
        }
    }

    // Update is called once per frame
    private void Update()
    {
    }

    void ShuffleMissions(GameObject[] missions)
    {
        for (int i = 0; i < missions.Length; i++)
        {
            GameObject temp = missions[i];
            int random = Random.Range(0, missions.Length);
            missions[i] = missions[random];
            missions[random] = temp;
        }
    }

    GameObject ChooseRandomMission(GameObject[] missions)
    {
        return missions[Random.Range(0, missions.Length)];

        // TODO: implement probability into Missions
        /*float total = 0;

        foreach (GameObject c in cards)
            total += c.probability; // add up all probabilities

        float random = Random.value * total; // Random.value returns value between 0.0 and 1.0

        for (int i = 0; i < cards.Length; i++)
        {
            if (cards[i].probability > random)
                return cards[i];
            else
                random -= cards[i].probability;
        }

        return cards[cards.Length - 1];*/
    }

    GameObject[] ChooseCardsFromSet(int howManyToChoose, GameObject[] availableMissions)
    {
        // Example: have 10 available cards to choose from, but only needs to choose 5
        // The probability of the first item being chosen will be 5 / 10 or 0.5
        // If it's chosen then the probability for the second item will be 4 / 9 or 0.44 (ie, four items still needed, nine left to choose from)
        // However, if the first was not chosen then the probability for the second will be 5 / 9 or 0.56 (ie, five still needed, nine left to choose from)
        // This continues until the set contains the five items required.

        GameObject[] result = new GameObject[howManyToChoose];

        int numToChoose = howManyToChoose;

        for (int numLeft = availableMissions.Length; numLeft > 0; numLeft--)
        {
            // Adding 0.0 is simply to cast the integers to float for the division.
            float probability = numToChoose + 0.0f / numLeft + 0.0f;

            if (probability >= Random.value)
            {
                numToChoose--;
                result[numToChoose] = availableMissions[numLeft - 1];
            }

            if (numToChoose == 0)
                break;
        }

        // Note that although the selection is random, items in the chosen set will be
        // in the same order they had in the original array.
        // If the items are to be used one at a time in sequence then the ordering can make
        // them partly predictable, so it may be necessary to shuffle the array before use.
        ShuffleMissions(result);

        return result;
    }
}