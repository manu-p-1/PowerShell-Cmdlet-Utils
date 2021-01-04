﻿using PowerPlug.BaseCmdlets;
using PowerPlug.Engines.Byname;
using PowerPlug.Engines.Byname.Base;
using PowerPlug.PowerPlugUtilities.Attributes;
using PowerPlug.PowerPlugUtilities.Cmdlets;
using System.Management.Automation;
using System.Text;
using PowerPlug.PowerPlugUtilities.Extensions;

namespace PowerPlug.Cmdlets
{
    [Cmdlet(VerbsCommon.New, "Byname", HelpUri = "https://docs.microsoft.com/en-us/powershell/module/microsoft.powershell.utility/new-alias?view=powershell-7")]
    [Alias("nbn")]
    [BetaCmdlet(BetaCmdlet.WarningMessage)]
    public class NewBynameCmdlet : WritableByname
    {
        protected override void ProcessRecord()
        {
            using var ps = PowerShell.Create(RunspaceMode.CurrentRunspace);

            ps.AddCommand(WritableBynameCreatorBaseOperation.NewAliasCommand)
                .AddParameter("Name", Name)
                .AddParameter("Value", Value)
                .AddParameter("Description", Description)
                .AddParameter("Option", Option)
                .AddParameter("PassThru", PassThru)
                .AddParameter("Scope", Scope)
                .AddParameter("Force", Force)
                .AddParameter("WhatIf", WhatIf)
                .AddParameter("Confirm", Confirm);

            new BynameCreatorContext(
                new NewBynameCreatorOperation(
                    this,
                    CmdletUtilities.InvokePowershellCommandOrThrowIfUnsuccessful(ps, this)
                )
            ).ExecuteStrategy();
        }

        public override string ToString() =>
            new StringBuilder()
                .Append("New-Alias")
                .Append($" -Name {Name}")
                .Append($" -Value {Value}")
                .Append($" -Option {Option}")
                .Append($" -Scope {Scope}")
                .AppendIf(" -PassThru", PassThru)
                .AppendIf(" -Force", Force)
                .AppendIf(" -WhatIf", WhatIf)
                .AppendIf(" -Confirm", Confirm)
                .AppendIf($" -Description {Description}", Description != string.Empty)
                .ToString();
    }
}