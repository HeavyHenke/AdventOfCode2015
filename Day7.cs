using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace ConsoleApplication5
{
    public class Day7
    {
        private Dictionary<string, Node> _nodes;
        private Regex _parser;

        public void Calculate()
        {
            string[] regExParts = {
                @"^(?<command>NOT) (?<input>\w*) -> (?<output>\w*)$",
                @"^(?<input1>\w*) (?<command>OR) (?<input2>\w*) -> (?<output>\w*)$",
                @"^(?<input1>\w*) (?<command>AND) (?<input2>\w*) -> (?<output>\w*)$",
                @"^(?<command>)(?<input>\w*) -> (?<output>\w*)$",
                @"^(?<input1>\w*) (?<command>RSHIFT) (?<input2>\w*) -> (?<output>\w*)$",
                @"^(?<input1>\w*) (?<command>LSHIFT) (?<input2>\w*) -> (?<output>\w*)$",
            };

            _parser = new Regex(string.Join("|", regExParts), RegexOptions.Compiled);

            var lines = File.ReadAllLines("day7.txt");
            _nodes = new Dictionary<string, Node>();

            foreach (var line in lines)
                CreateNode(line);

            _nodes["b"] = new Fixed(_nodes, "b", "16076");

            int iterations = 0;
            while (_nodes.Values.Any(v => v.Doit()))
                iterations++;

            Console.Write("Node a = " + _nodes["a"].Value);
        }

        private void CreateNode(string input)
        {
            var match = _parser.Match(input);

            if(match.Success == false)
                throw new Exception("Unable to parse line: " + input);

            Node node;
            switch (match.Groups["command"].Value)
            {
                case "NOT":
                    node = new Not(_nodes, match.Groups["output"].Value, match.Groups["input"].Value);
                    _nodes[node.Name] = node;
                    break;
                case "OR":
                    node = new Or(_nodes, match.Groups["output"].Value, match.Groups["input1"].Value, match.Groups["input2"].Value);
                    _nodes[node.Name] = node;
                    break;
                case "AND":
                    node = new And(_nodes, match.Groups["output"].Value, match.Groups["input1"].Value, match.Groups["input2"].Value);
                    _nodes[node.Name] = node;
                    break;
                case "RSHIFT":
                    node = new Rshift(_nodes, match.Groups["output"].Value, match.Groups["input1"].Value, match.Groups["input2"].Value);
                    _nodes[node.Name] = node;
                    break;
                case "LSHIFT":
                    node = new Lshift(_nodes, match.Groups["output"].Value, match.Groups["input1"].Value, match.Groups["input2"].Value);
                    _nodes[node.Name] = node;
                    break;
                case "":
                    node = new Fixed(_nodes, match.Groups["output"].Value, match.Groups["input"].Value);
                    _nodes[node.Name] = node;
                    break;
                default:
                    throw new Exception("Unable to parse line: " + input);
            }
        }

        class Input
        {
            private readonly Dictionary<string, Node> _nodes;
            private readonly ushort? _fixed;
            private readonly string _inputRef;

            public Input(Dictionary<string, Node> nodes, string val)
            {
                _nodes = nodes;
                ushort parsed;
                if (ushort.TryParse(val, out parsed))
                    _fixed = parsed;
                else
                    _inputRef = val;
            }

            public bool HasValue
            {
                get { return _fixed.HasValue || _nodes[_inputRef].HasValue; }
            }

            public ushort Value
            {
                get
                {
                    if (_fixed.HasValue)
                        return _fixed.Value;
                    var node = _nodes[_inputRef];
                    return node.Value;
                }
            }
        }


        abstract class Node
        {
            private readonly Dictionary<string, Node> _nodes;
            private ushort? _value;

            public string Name { get; }
            public ushort Value => _value.Value;
            public bool HasValue => _value.HasValue;

            protected Node(Dictionary<string, Node> nodes, string name)
            {
                _nodes = nodes;
                Name = name;
            }

            protected bool SetValue(ushort value)
            {
                if (_value == value)
                    return false;
                _value = value;
                return true;
            }

            public abstract bool Doit();
        }

        class Not : Node
        {
            private readonly Input _input;

            public Not(Dictionary<string, Node> nodes, string name, string input)
                : base(nodes, name)
            {
                _input = new Input(nodes, input);
            }

            public override bool Doit()
            {
                if (_input.HasValue)
                {
                    return SetValue((ushort) ~_input.Value);
                }
                return false;
            }
        }
        class Rshift : Node
        {
            private readonly Input _input;
            private readonly int _steps;

            public Rshift(Dictionary<string, Node> nodes, string name, string input, string steps)
                : base(nodes, name)
            {
                _input = new Input(nodes, input);
                _steps = int.Parse(steps);
            }

            public override bool Doit()
            {
                if (_input.HasValue)
                    return SetValue((ushort) (_input.Value >> _steps));
                return false;
            }
        }
        class Lshift : Node
        {
            private readonly Input _input;
            private readonly int _steps;

            public Lshift(Dictionary<string, Node> nodes, string name, string input, string steps)
                : base(nodes, name)
            {
                _input = new Input(nodes, input);
                _steps = int.Parse(steps);
            }

            public override bool Doit()
            {
                if (_input.HasValue)
                    return SetValue((ushort)(_input.Value << _steps));
                return false;
            }
        }

        class Or : Node
        {
            private readonly Input _input1;
            private readonly Input _input2;

            public Or(Dictionary<string, Node> nodes, string name, string input1, string input2)
                : base(nodes, name)
            {
                _input1 = new Input(nodes, input1);
                _input2 = new Input(nodes, input2);
            }

            public override bool Doit()
            {
                if (_input1.HasValue && _input2.HasValue)
                    return SetValue((ushort) (_input1.Value | _input2.Value));
                return false;
            }
        }

        class And : Node
        {
            private readonly Input _input1;
            private readonly Input _input2;

            public And(Dictionary<string, Node> nodes, string name, string input1, string input2)
                : base(nodes, name)
            {
                _input1 = new Input(nodes, input1);
                _input2 = new Input(nodes, input2);
            }

            public override bool Doit()
            {
                if (_input1.HasValue && _input2.HasValue)
                    return SetValue((ushort)(_input1.Value & _input2.Value));
                return false;
            }
        }

        class Fixed : Node
        {
            private readonly Input _input;

            public Fixed(Dictionary<string, Node> nodes, string name, string input) 
                : base(nodes, name)
            {
                _input = new Input(nodes, input);
            }

            public override bool Doit()
            {
                if (_input.HasValue)
                    return SetValue(_input.Value);
                return false;
            }
        }
    }
}