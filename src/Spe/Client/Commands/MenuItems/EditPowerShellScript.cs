﻿using System;
using Sitecore.Configuration;
using Sitecore.Data;
using Sitecore.Shell.Framework.Commands;
using Sitecore.Text;
using Spe.Core.Extensions;

namespace Spe.Client.Commands.MenuItems
{
    [Serializable]
    public class EditPowerShellScript : Command
    {
        public override CommandState QueryState(CommandContext context)
        {
            if (context.Items.Length != 1)
                return CommandState.Hidden;

            return context.Items[0].IsPowerShellScript()
                ? CommandState.Enabled
                : CommandState.Hidden;
        }

        public override void Execute(CommandContext context)
        {
            var itemId = context.Items[0].ID.ToString();
            var itemDb = context.Items[0].Database.Name;
            var item = Factory.GetDatabase(itemDb).GetItem(new ID(itemId));
            var title = item.DisplayName;

            var urlString = new UrlString();
            urlString.Append("id", item.ID.ToString());
            urlString.Append("db", itemDb);
            if (!string.IsNullOrEmpty(context.Parameters["frameName"]))
            {
                urlString.Add("pfn", context.Parameters["frameName"]);
            }

            var appItem = Database.GetDatabase("core").GetItem("/sitecore/content/Applications/PowerShell/PowerShellIse");
            var icon = appItem.Appearance.Icon;
            Sitecore.Shell.Framework.Windows.RunApplication(appItem, icon, title, urlString.Query);
        }
    }
}