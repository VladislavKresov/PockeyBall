using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameController : MonoBehaviour
{
    [SerializeField] private Ball _ball;
    [SerializeField] private TextMeshPro _coinsText;
    
    private void OnCoinCollected(int coins)
    {
        _coinsText.text = coins.ToString();
    }

    private void OnEnable()
    {
        _ball.CoinCollected += OnCoinCollected;
    }

    private void OnDisable()
    {
        _ball.CoinCollected -= OnCoinCollected;
    }
}
