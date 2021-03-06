﻿// Copyright (c) Microsoft.  All Rights Reserved.  Licensed under the Apache License, Version 2.0.  See License.txt in the project root for license information.

using System.Collections.Immutable;
using Roslyn.Utilities;

namespace Microsoft.CodeAnalysis.Diagnostics
{
    internal partial class DiagnosticAnalyzerService
    {
        public class AnalysisData
        {
            public static readonly AnalysisData Empty = new AnalysisData(VersionStamp.Default, VersionStamp.Default, ImmutableArray<DiagnosticData>.Empty);

            public readonly VersionStamp TextVersion;
            public readonly VersionStamp DataVersion;
            public readonly ImmutableArray<DiagnosticData> OldItems;
            public readonly ImmutableArray<DiagnosticData> Items;

            public AnalysisData(VersionStamp textVersion, VersionStamp dataVersion, ImmutableArray<DiagnosticData> items)
            {
                this.TextVersion = textVersion;
                this.DataVersion = dataVersion;
                this.Items = items;
            }

            public AnalysisData(VersionStamp textVersion, VersionStamp dataVersion, ImmutableArray<DiagnosticData> oldItems, ImmutableArray<DiagnosticData> newItems) :
                this(textVersion, dataVersion, newItems)
            {
                this.OldItems = oldItems;
            }

            public AnalysisData ToPersistData()
            {
                return new AnalysisData(TextVersion, DataVersion, Items);
            }

            public bool FromCache
            {
                get { return this.OldItems.IsDefault; }
            }
        }

        public struct SolutionArgument
        {
            public readonly Solution Solution;
            public readonly ProjectId ProjectId;
            public readonly DocumentId DocumentId;

            public SolutionArgument(Solution solution, ProjectId projectId, DocumentId documentId)
            {
                this.Solution = solution;
                this.ProjectId = projectId;
                this.DocumentId = documentId;
            }

            public SolutionArgument(Document document) :
                this(document.Project.Solution, document.Id.ProjectId, document.Id)
            { }

            public SolutionArgument(Project project) :
                this(project.Solution, project.Id, null)
            { }
        }

        public struct VersionArgument
        {
            public readonly VersionStamp TextVersion;
            public readonly VersionStamp DataVersion;
            public readonly VersionStamp ProjectVersion;

            public VersionArgument(VersionStamp textVersion, VersionStamp dataVersion) :
                this(textVersion, dataVersion, VersionStamp.Default)
            {
            }

            public VersionArgument(VersionStamp textVersion, VersionStamp dataVersion, VersionStamp projectVersion)
            {
                this.TextVersion = textVersion;
                this.DataVersion = dataVersion;
                this.ProjectVersion = projectVersion;
            }
        }

        public class ArgumentKey
        {
            public readonly int ProviderId;
            public readonly StateType StateTypeId;
            public readonly object Key;

            public ArgumentKey(int providerId, StateType stateTypeId, object key)
            {
                this.ProviderId = providerId;
                this.StateTypeId = stateTypeId;
                this.Key = key;
            }

            public override bool Equals(object obj)
            {
                var other = obj as ArgumentKey;
                if (other == null)
                {
                    return false;
                }

                return ProviderId == other.ProviderId && StateTypeId == other.StateTypeId && Key == other.Key;
            }

            public override int GetHashCode()
            {
                return Hash.Combine(Key, Hash.Combine(ProviderId, (int)StateTypeId));
            }
        }
    }
}
