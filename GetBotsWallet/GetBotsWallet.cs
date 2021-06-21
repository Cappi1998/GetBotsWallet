//ArchiSteamFarm-5.1.0.9
using ArchiSteamFarm.Core;
using ArchiSteamFarm.Localization;
using ArchiSteamFarm.Plugins.Interfaces;
using ArchiSteamFarm.Steam;
using ArchiSteamFarm.Steam.Interaction;
using System;
using System.Collections.Generic;
using System.Composition;
using System.Threading.Tasks;

namespace GetBotsWallet
{
    [Export(typeof(IPlugin))]
    class GetBotsWallet : IBotCommand
    {
        public string Name => nameof(GetBotsWallet);
        public Version Version => typeof(GetBotsWallet).Assembly.GetName().Version ?? throw new InvalidOperationException(nameof(Version));

        public async Task<string> OnBotCommand(Bot bot, ulong steamID, string message, string[] args)
        {
            switch (args[0].ToUpperInvariant())
            {
                case "WALLET" when args.Length > 1:
                    {
                        return await GetWallet(Utilities.GetArgsAsText(args, 1, ",")).ConfigureAwait(false);
                    }
                    
                case "WALLET":
                    {
                        return await GetWallet(bot.BotName).ConfigureAwait(false);
                    }
                    
                default:
                    {
                        return null;
                    }
            }
        }

        private static async Task<string> GetWallet(string botNames)
        {
            HashSet<Bot> bots = Bot.GetBots(botNames);
            if ((bots == null) || (bots.Count == 0))
            {
                return Commands.FormatStaticResponse(string.Format(Strings.BotNotFound, botNames));
            }

            List<string> Wallets = new List<string>();

            foreach (var bot in bots)
            {
                if (bot.IsConnectedAndLoggedOn)
                {
                    string responsebot = $"<{bot.BotName}> - Wallet:{bot.WalletBalance} - Currency:{bot.WalletCurrency}";
                    Wallets.Add(responsebot);
                }
                else
                {
                    string responsebot = $"<{bot.BotName}> - this bot is not connected to the steam network!";
                    Wallets.Add(responsebot);
                }
            }

            return string.Join(Environment.NewLine, Wallets);
        }

        public void OnLoaded()
        {
            ASF.ArchiLogger.LogGenericInfo("GetBotsWallet Plugin By Cappi_1998 is loaded!");
        }
    }
}
