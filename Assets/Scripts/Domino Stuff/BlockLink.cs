using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This is a server-side-active script that helps blocks keep track of which
/// dominos are sitting upon them and which are not.
/// </summary>
public class BlockLink : MonoBehaviour {

    EnvironmentBlock block;

    public void SetLink(EnvironmentBlock onBlock) {
        block = onBlock;
    }

    private void OnDestroy() {
        if (block != null)
            block.RemoveDomino(gameObject);
    }
}
