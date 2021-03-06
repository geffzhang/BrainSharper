﻿namespace BrainSharper.Implementations.Algorithms.DecisionTrees.DataStructures.BinaryDecisionTrees
{
    #region

    using System.Collections.Generic;
    using System.Linq;

    using BrainSharper.Abstract.Algorithms.DecisionTrees.DataStructures;
    using BrainSharper.Abstract.Algorithms.DecisionTrees.DataStructures.BinaryTrees;

    #endregion

    public class BinaryDecisionTreeParentNode : DecisionTreeParentNode, IBinaryDecisionTreeParentNode
    {
        public BinaryDecisionTreeParentNode(
            bool isLeaf, 
            string decisionFeatureName, 
            IDictionary<IDecisionTreeLink, IDecisionTreeNode> linksToChildren, 
            object decisionValue, 
            bool isSplitValueNumeric)
            : base(isLeaf, decisionFeatureName, linksToChildren)
        {
            IsValueNumeric = isSplitValueNumeric;
            DecisionValue = decisionValue;
            TestResultsWithChildren = linksToChildren.ToDictionary(
                kvp => kvp.Key as IBinaryDecisionTreeLink, 
                kvp => kvp.Value);
            foreach (var link in linksToChildren)
            {
                var binaryTreeLink = link.Key as IBinaryDecisionTreeLink;
                if (binaryTreeLink != null)
                {
                    if (!binaryTreeLink.LogicalTestResult)
                    {
                        LeftChild = link.Value;
                        LeftChildLink = binaryTreeLink;
                    }
                    else
                    {
                        RightChild = link.Value;
                        RightChildLink = binaryTreeLink;
                    }
                }
            }
        }

        public IDictionary<IBinaryDecisionTreeLink, IDecisionTreeNode> TestResultsWithChildren { get; }

        public IDecisionTreeNode LeftChild { get; }

        public IBinaryDecisionTreeLink LeftChildLink { get; }

        public IDecisionTreeNode RightChild { get; }

        public IBinaryDecisionTreeLink RightChildLink { get; }

        public object DecisionValue { get; }

        public bool IsValueNumeric { get; }
    }
}