﻿namespace BrainSharper.Abstract.Algorithms.DecisionTrees.DataStructures.MultiValueTrees
{
    public interface IMultiValueRegressionLink<TDecisionValue> : IDecisionTreeLink
    {
        TDecisionValue LinkDecisionValue { get; }
        double Variance { get; }
        double Mean { get; }
    }
}
