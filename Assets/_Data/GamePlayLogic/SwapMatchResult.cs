using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class SwapMatchResult
{
    public Node nodeStart { get; set; }
    public List<Transform> listStart { get; set; }
    public Node nodeEnd { get; set; }
    public List<Transform> listEnd { get; set; }
}


