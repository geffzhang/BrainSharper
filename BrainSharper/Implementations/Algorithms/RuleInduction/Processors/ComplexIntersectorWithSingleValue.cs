﻿namespace BrainSharper.Implementations.Algorithms.RuleInduction.Processors
{
    using System.Collections.Generic;
    using System.Linq;

    using Abstract.Algorithms.RuleInduction.DataStructures;
    using Abstract.Algorithms.RuleInduction.Processors;

    using Abstract.Data;
    using DataStructures;

    public class ComplexIntersectorWithSingleValue<TValue> : IComplexesIntersector<TValue>
    {
        public IDictionary<string, ISet<IComplex<TValue>>> PrepareFeatureDomains(
            IDataFrame dataFrame,
            string dependentFeatureName)
        {
            var results = (from columnName in dataFrame.ColumnNames.Where(col => !col.Equals(dependentFeatureName))
                           let columnVector =
                               dataFrame.GetColumnVector<TValue>(columnName).Values.Distinct()
                           let singleValSelectors =
                                columnVector.Select(
                                    val =>
                                    new Complex<TValue>(
                                        selectors: new DisjunctiveSelector<TValue>(columnName, val)))
                           group singleValSelectors by columnName
                           into grp
                           select grp).ToDictionary(grp => grp.Key, grp => new HashSet<IComplex<TValue>>(grp.SelectMany(e => e)) as ISet<IComplex<TValue>>);

            return results;
        }

        public IList<IComplex<TValue>> IntersectComplexesWithFeatureDomains(
            IList<IComplex<TValue>> complexesToIntersect,
            IDictionary<string, ISet<IComplex<TValue>>> featureDomains)
        {
            var results = new List<IComplex<TValue>>();

            //TODO: AAA !!! optimize this maybe later, to not include redundant intersection operations - add helper dictionary to exclude values already covered

            foreach (var featureWithDomain in featureDomains)
            {
                var featureName = featureWithDomain.Key;
                var domain = featureWithDomain.Value;
                foreach (var singleDomainValueComplex in domain)
                {
                    foreach (var complexToIntersect in complexesToIntersect)
                    {
                        var intersectedComplex = complexToIntersect.Intersect(singleDomainValueComplex);
                        if (!intersectedComplex.IsEmpty && 
                            !intersectedComplex.Equals(complexToIntersect) && 
                            complexToIntersect.IsMoreGeneralThan(intersectedComplex))
                        {
                            results.Add(intersectedComplex);
                        }
                    }
                }
            }

            return results;
        }
    }
}