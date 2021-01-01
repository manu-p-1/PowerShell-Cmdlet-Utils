﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Management.Automation;
using System.Management.Automation.Language;
using System.Text;
using PowerPlug.BaseCmdlets;

namespace PowerPlug.Engines.Byname
{
    public abstract class CreatableBynameCreatorBase : BynameCreatorBase
    {
        protected const string NewAliasCommand = "New-Alias";
        protected const string SetAliasCommand = "Set-Alias";
        protected WritableBynameBase AliasCmdlet { get; }

        protected CreatableBynameCreatorBase(WritableBynameBase cmdlet) : base(cmdlet)
        {
            AliasCmdlet = cmdlet;
        }

        protected StringBuilder RunCommand(string realCommand)
        {
            foreach (var p in RunUnderlyingCommand(realCommand))
            {
                AliasCmdlet.WriteObject(p);
            }

            var sb = new StringBuilder("\n# Autogenerated Profile Alias - Do Not Modify\n");

            if (GetAliasValueType() is FunctionValueType ft)
            {
                sb.Append($"function {AliasCmdlet.Value} {{ {ft.ScriptBlock} }}\n");
            }

            sb.Append("New-Alias")
                .Append($" -Name {AliasCmdlet.Name}")
                .Append($" -Value {AliasCmdlet.Value}")
                .Append($" -Option {AliasCmdlet.Option}")
                .Append($" -Scope {AliasCmdlet.Scope}")
                .AppendIf(" -PassThru", AliasCmdlet.PassThru)
                .AppendIf(" -Force", AliasCmdlet.Force)
                .AppendIf(" -WhatIf", AliasCmdlet.WhatIf)
                .AppendIf(" -Confirm", AliasCmdlet.Confirm)
                .AppendIf($" -Description {AliasCmdlet.Description}", AliasCmdlet.Description != string.Empty);

            return sb;
        }

        private IEnumerable<PSObject> RunUnderlyingCommand(string realCommand)
        {
            using var ps = PowerShell.Create(RunspaceMode.CurrentRunspace);

            var res = ps
                .AddCommand(realCommand)
                .AddParameter("Name", AliasCmdlet.Name)
                .AddParameter("Value", AliasCmdlet.Value)
                .AddParameter("Description", AliasCmdlet.Description)
                .AddParameter("Option", AliasCmdlet.Option)
                .AddParameter("PassThru", AliasCmdlet.PassThru)
                .AddParameter("Scope", AliasCmdlet.Scope)
                .AddParameter("Force", AliasCmdlet.Force)
                .AddParameter("WhatIf", AliasCmdlet.WhatIf)
                .AddParameter("Confirm", AliasCmdlet.Confirm)
                .Invoke();

            if (ps.HadErrors)
            {
                AliasCmdlet.ThrowTerminatingError(ps.Streams.Error[0]);
            }

            return res;
        }
        private CommandAliasValueType GetAliasValueType()
        {
            using var ps = PowerShell.Create(RunspaceMode.CurrentRunspace);
            var gc = ps
                .AddScript($"Get-Command {AliasCmdlet.Name} | select *")
                .Invoke<PSObject>();

            var gcProp = gc[0].Properties;
            var resolvedValue = gcProp.FirstOrDefault(e => e.Name == "ResolvedCommand").Value;

            if (resolvedValue == null)
            {
                return new UndefinedValueType(AliasCmdlet);
            }

            var cmd = (resolvedValue as CommandInfo);
            return (cmd.CommandType.ToString()) switch
            {
                "Function" => new FunctionValueType(AliasCmdlet, cmd.Definition.Trim()),
                "Cmdlet" => new CmdletValueType(AliasCmdlet),
                _ => new UndefinedValueType(AliasCmdlet),
            };
        }
    }

    public abstract record CommandAliasValueType
    {
        protected WritableBynameBase AliasCmdlet { get; }

        protected CommandAliasValueType(WritableBynameBase cmdlet)
        {
            AliasCmdlet = cmdlet;
        }
    }

    public sealed record FunctionValueType : CommandAliasValueType
    {
        public string ScriptBlock { get; }

        public FunctionValueType(WritableBynameBase cmdlet, string scriptBlock) : base(cmdlet)
        {
            ScriptBlock = scriptBlock;
        }
    }

    public sealed record CmdletValueType : CommandAliasValueType
    {
        public CmdletValueType(WritableBynameBase cmdlet) : base(cmdlet) { }
    }

    public sealed record UndefinedValueType : CommandAliasValueType
    {
        public UndefinedValueType(WritableBynameBase cmdlet) : base(cmdlet) { }
    }
}
