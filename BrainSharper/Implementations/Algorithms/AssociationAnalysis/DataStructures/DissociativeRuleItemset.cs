﻿using System.Collections.Generic;
using System.Linq;
using BrainSharper.Abstract.Algorithms.AssociationAnalysis.DataStructures;

namespace BrainSharper.Implementations.Algorithms.AssociationAnalysis.DataStructures
{
    public class DissociativeRuleCandidateItemSet<TValue>
    {
        public DissociativeRuleCandidateItemSet(
            IFrequentItemsSet<TValue> firstSubset, 
            IFrequentItemsSet<TValue> secondSubset, 
            double? support = null,
            double? relativeSupport = null)
        {
            FirstSubset = firstSubset;
            SecondSubset = secondSubset;
            Support = support;
            RelativeSupport = relativeSupport;
            AllItemsSet = new HashSet<TValue>(FirstSubset.ItemsSet.Union(secondSubset.ItemsSet));
        }

        public IFrequentItemsSet<TValue> FirstSubset { get; }
        public IFrequentItemsSet<TValue> SecondSubset { get; }
        public ISet<TValue> AllItemsSet { get; }
        public double? Support { get; set; }
        public double? RelativeSupport { get; set; }
        public bool IsSupportSet => Support.HasValue;
        public bool IsRelativeSet => RelativeSupport.HasValue;

    }
}