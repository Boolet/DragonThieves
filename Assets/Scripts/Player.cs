
using UnityEngine;

using UnityEngine.Networking;

public class Coin : NetworkBehaviour {
    int num_coins;
    public GameObject coinPrefab;
    Transform coinLoc;
    
    // Use this for initialization
    void Start () {
        num_coins = 10;
	}
	
	// Update is called once per frame
	void Update () {
		if (num_coins < 10)
        {
         //   coinLoc
            var new_coin = (GameObject)Instantiate(coinPrefab, coinLoc.position, coinLoc.rotation);
        }
	}


}
