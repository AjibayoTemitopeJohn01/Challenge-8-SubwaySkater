using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

public class LevelManager : MonoBehaviour
{
    public static LevelManager Instance
    {
        get;
        set;
    }

    [FormerlySerializedAs("ShowCollider")] public bool showCollider = true;
    
    // Leve Spawning
    private const float DistanceBeforeSpawn = 100.0f;
    private const int InitialSegments = 10;
    private const int InitialTransitionSegments = 2;
    private const int MaxSegmentsOnScreen = 15;
    private Transform cameraContainer;
    private int amountOfActiveSegments;
    private int continuousSegments;
    private int currentSpawnZ;
    private int currentLevel;
    private int y1, y2, y3;
    
    
    
    // Level Spawning
    
    // list of Pieces
    public List<Piece> ramps = new List<Piece>();
    public List<Piece> longBlocks = new List<Piece>();
    public List<Piece> jumps = new List<Piece>();
    public List<Piece> slides = new List<Piece>();
    [HideInInspector]
    public List<Piece> pieces = new List<Piece>(); // All Pieces in the pool
    
    // List of Segments
    public List<Segment> availableSegments = new List<Segment>();
    public List<Segment> availableTransitions = new List<Segment>();
    [HideInInspector]
    public List<Segment> segments = new List<Segment>();
    
    // GamePlay
    private bool isMoving = false;

    private void Awake()
    {
        Instance = this;
        cameraContainer = Camera.main.transform;
        currentSpawnZ = 0;
        currentLevel = 0;
    }

    private void Start()
    {
        for (int i = 0; i < InitialSegments; i++)
        {
            if (i < InitialTransitionSegments)
            {
                SpawnTransition();
            }
            else
            {
                GenerateSegment();
            }
        }
    }

    private void Update()
    {
        if (currentSpawnZ - cameraContainer.position.z < DistanceBeforeSpawn)
        {
            GenerateSegment();

            if (amountOfActiveSegments >= MaxSegmentsOnScreen)
            {
                segments[amountOfActiveSegments - 1].DeSpawn();
                amountOfActiveSegments--;
            }
        }
    }

    private void GenerateSegment()
    {
        SpawnSegment();

        if (Random.Range(0f, 1f) < (continuousSegments * 0.25f))
        {
            // Spawn Transition seg
            continuousSegments = 0;
            SpawnTransition();
        }
        else
        {
            continuousSegments++;
        }
    }
    
    private void SpawnSegment()
    {
        List<Segment> possibleSeg = availableSegments.FindAll(x => x.beginY1 == y1 || x.beginY2 == y2 || x.beginY3 == y3);
        int id = Random.Range(0, possibleSeg.Count);
        
        // Segment s = possibleSeg[id];
        Segment s = GetSegment(id, false);
        
        y1 = s.endY1;
        y2 = s.endY2;
        y3 = s.endY3;
        
        s.transform.SetParent(transform);
        s.transform.localPosition = Vector3.forward * currentSpawnZ;

        currentSpawnZ += s.length;
        amountOfActiveSegments++;
        s.Spawn();

    }

    private void SpawnTransition()
    {
        List<Segment> possibleTransition = availableTransitions.FindAll(x => x.beginY1 == y1 || x.beginY2 == y2 || x.beginY3 == y3);
        int id = Random.Range(0, possibleTransition.Count);
        
        // Segment s = possibleSeg[id];
        Segment s = GetSegment(id, true);
        
        y1 = s.endY1;
        y2 = s.endY2;
        y3 = s.endY3;
        
        s.transform.SetParent(transform);
        s.transform.localPosition = Vector3.forward * currentSpawnZ;

        currentSpawnZ += s.length;
        amountOfActiveSegments++;
        s.Spawn();
    }

    public Segment GetSegment(int id, bool transition)
    {
        Segment s = null;
        s = segments.Find(x => x.SegId == id && x.transition == transition && !x.gameObject.activeSelf);

        if (s == null)
        {
            GameObject go = Instantiate((transition) ? availableTransitions[id].gameObject : availableSegments[id].gameObject) as GameObject;
            s = go.GetComponent<Segment>();

            s.SegId = id;
            s.transition = transition;
            
            segments.Insert(0, s);
        }
        else
        {
            segments.Remove(s);
            segments.Insert(0, s);
        }

        return s;
    }

    public Piece GetPiece(PieceType pt, int visualIndex)
    {
        Piece p = pieces.Find(x => x.type == pt && x.visualIndex == visualIndex && !x.gameObject.activeSelf);

        if (p == null)
        {
            GameObject go = null;
            if (pt == PieceType.Ramp)
            {
                go = ramps[visualIndex].gameObject;
            }
            else if (pt == PieceType.Jump)
            {
                go = jumps[visualIndex].gameObject;
            }
            else if (pt == PieceType.LongBlock)
            {
                go = longBlocks[visualIndex].gameObject;
            }
            else if (pt == PieceType.Slide)
            {
                go = slides[visualIndex].gameObject;
            }

            go = Instantiate(go);
            p = go.GetComponent<Piece>();
            pieces.Add(p);
        }
        
        return p;
    }
}














