﻿using BrainSharper.Abstract.Algorithms.DecisionTrees;

namespace BrainSharper.Implementations.Algorithms.DecisionTrees
{
    public class DecisionTreeLeaf<TDecisionValue> : IDecisionTreeLeaf<TDecisionValue>
    {
        public DecisionTreeLeaf(string decisionFeatureName, TDecisionValue leafValue)
        {
            DecisionFeatureName = decisionFeatureName;
            LeafValue = leafValue;
        }

        public bool IsLeaf => true;
        public string DecisionFeatureName { get; }
        public TDecisionValue LeafValue { get; }
    }
}