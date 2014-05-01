using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using XInputDotNetPure;

public class MorseCode : ControllerTesterRumble
{
    public MorseCode(List<ControllerPlayer> controllerPlayers, ControllerTesterManager controllerManager) : base(controllerPlayers, controllerManager)
    {
        timer = manager.RumbleTimer;
    }

    // A = 0, B = 1, X = 2, Y = 3

    // A "B" (- . . .)
    int[] nodesA = { 1, 0, 1, 0, 1, 0, 1, 0 }; // vibrations
    float[] durationsA = { 0.2f, 1, 0.2f, 0.4f, 0.2f, 0.4f, 0.2f, 0.4f };

    // X "C" (- . - .)
    int[] nodesX = { 1, 0, 1, 0, 1, 0, 1, 0 }; // vibrations
    float[] durationsX = { 0.2f, 1, 0.2f, 0.4f, 0.2f, 1f, 0.2f, 0.4f };

    // B "R" (. - .)
    int[] nodesB = { 1, 0, 1, 0, 1, 0, 1, 0}; // vibrations
    float[] durationsB = { 0.2f, 0.4f, 0.2f, 1f, 0.2f, 0.4f,};

    // Y "U" (. . -)
    int[] nodesY = { 1, 0, 1, 0, 1, 0, 1, 0}; // vibrations
    float[] durationsY = { 0.2f, 0.4f, 0.2f, 0.4f, 0.2f, 1f};

    int currentNode = 0;

    float timer = 0;

    //manager.RumbleTimer

    void PlayPattern(int[] nodes, float[] durations)
    {
        if (manager.RumbleTimer - timer >= durations[currentNode])
        {
            timer = manager.RumbleTimer;
            foreach (ControllerPlayer player in players)
                GamePad.SetVibration(player.Index, nodes[currentNode], nodes[currentNode]);

            if (currentNode + 1 < durations.Length)
                currentNode++;
            else
                StopRumble();
        }
    }

    public override void UpdateRumble()
    {
        switch ((int)pattern)
        {
            case 0: // A
                PlayPattern(nodesA, durationsA);
                break;

            case 1: // B
                PlayPattern(nodesB, durationsB);
                break;

            case 2: // X
                PlayPattern(nodesX, durationsX);
                break;

            case 3: // Y
                PlayPattern(nodesY, durationsY);
                break;

        }
    }
}