using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class SlotMachine : MonoBehaviour
{
    public TMP_Text txtReel1, txtReel2, txtReel3;
    public TMP_Text txtBetResult, txtCredits;
    public TMP_InputField inputFieldBet;

    private int credits = 1000;

    public void Spin() {
    int bet;
        if (int.TryParse(inputFieldBet.text, out bet) && bet > 0 && bet <= credits) {
            credits -= bet;
            UpdateCreditsText();

            string reel1 = GetWeightedSymbol();
            string reel2 = GetWeightedSymbol();
            string reel3 = GetWeightedSymbol();

            // Check for a near miss scenario
            if (reel1 == reel2 && reel1 != reel3) {
                reel3 = GetNearMissSymbol(reel1); // Force a near miss
            }

            txtReel1.text = reel1;
            txtReel2.text = reel2;
            txtReel3.text = reel3;

            CheckWin(reel1, reel2, reel3, bet);
        } else {
            txtBetResult.text = "Insert a valid bet!";
        }
    }

    // Method to get a symbol with weighted probabilities
    private string GetWeightedSymbol() {
        int randomValue = Random.Range(0, 100); // 0 to 99
        if (randomValue < 50) {
            return "X"; // 50% chance
        } else if (randomValue < 70) {
            return "O"; // 20% chance
        } else if (randomValue < 85) {
            return "Z"; // 15% chance
        } else if (randomValue < 95) {
            return "Y"; // 10% chance
        } else {
            return "S"; // 5% chance
        }
    }

    // Method to return a near miss symbol
    private string GetNearMissSymbol(string matchingSymbol) {
        // Return a symbol that is not the matching one
        string[] symbols = { "X", "O", "Z", "Y", "S" };
        string nearMissSymbol;
        do {
            nearMissSymbol = symbols[Random.Range(0, symbols.Length)];
        } while (nearMissSymbol == matchingSymbol);

        return nearMissSymbol;
    }

    private void CheckWin(string reel1, string reel2, string reel3, int bet) {
        if (reel1 == reel2 && reel2 == reel3) {
            int winAmount = bet * 500;
            credits += winAmount;
            txtBetResult.text = $"You win! Amount: {winAmount}";
        } else {
            txtBetResult.text = $"You lose! Bet: {bet}";
        }
        UpdateCreditsText();
    }

    private void UpdateCreditsText() {
        txtCredits.text = $"Credits: {credits}";
        if (credits <= 0) {
            txtBetResult.text = "GAME OVER";
        }
    }
}
