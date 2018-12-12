using System;
using System.Collections.Generic;
using System.Text;
using CommandLine;

namespace ReadPath
{
    class Options
    {
        [Option(shortName: 'n', longName: "normalized")]
        public bool Normalized { get; set; }

        [Option(shortName: 'p', longName: "parentname")]
        public bool ParentName { get; set; }

        [Option]
        public bool NormalizedParentName { get; set; }

        [Option(
            shortName: 'f',
            longName: "fullparent",
            HelpText = "equal to '--parentname --normalizedparentname'."
        )]
        public bool FullParent
        {
            get => ParentName && NormalizedParentName;
            set
            {
                ParentName = true;
                NormalizedParentName = true;
            }
        }

        public bool AnyParentOption => ParentName || NormalizedParentName;

        [Option(shortName: 'd', longName: "directory")]
        public bool Directory { get; set; }

        public bool Anything =>
            Normalized
            || ParentName
            || NormalizedParentName
            || Directory;

        public bool Everything
        {
            get => Normalized
                && FullParent
                && Directory;

            set
            {
                Normalized = true;
                FullParent = true;
                Directory = true;
            }
        }

        [Value(0, Min = 1, MetaName = "PATHS", Required = true)]
        public IEnumerable<string> Paths { set; get; }

        public override string ToString()
        {
            var sb = new StringBuilder();

            sb.Append($"Normalized: {Normalized},{Environment.NewLine}");
            sb.Append($"Parent name: {ParentName},{Environment.NewLine}");
            sb.Append($"Normalized parent name: {NormalizedParentName},{Environment.NewLine}");
            sb.Append($"Directory: {Directory},{Environment.NewLine}");
            sb.Append($"Paths:{Environment.NewLine}");
            foreach (var path in Paths)
                sb.Append($"{path.Quoted()}{Environment.NewLine}");

            return sb.ToString();
        }
    }
}
