/*
 *
 * Developer => Kelvin Mwangi
 * 
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PieceSpawner : MonoBehaviour
{
    public PieceType type;
    private Piece currentPieceSpawner;
    
    public void Spawn()
    {
        int amtObj = 0;
        switch (type)
        {
            case PieceType.Jump:
                amtObj = LevelManager.Instance.jumps.Count;
                break;
            
            case PieceType.Slide:
                amtObj = LevelManager.Instance.slides.Count;
                break;
            
            case PieceType.LongBlock:
                amtObj = LevelManager.Instance.longBlocks.Count;
                break;
            
            case PieceType.Ramp:
                amtObj = LevelManager.Instance.ramps.Count;
                break;
        }
        
        currentPieceSpawner = LevelManager.Instance.GetPiece(type, Random.Range(0, amtObj)); // ---------- Randomize Later
        currentPieceSpawner.gameObject.SetActive(true);
        currentPieceSpawner.transform.SetParent(transform, false);
    }

    public void DeSpawn()
    {
        currentPieceSpawner.gameObject.SetActive(false);
    }
}
