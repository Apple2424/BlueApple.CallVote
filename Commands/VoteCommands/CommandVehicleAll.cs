using BlueApple.CallVote.Utils;
using Rocket.API;
using Rocket.Unturned.Player;
using SDG.Unturned;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BlueApple.CallVote.Commands.VoteCommands
{
    public class CommandVehicleAll : IRocketCommand
    {
        public string Name => "vehicleall";

        public string Help => "Drops a (random) vehicle for everyone";

        public string Syntax => "[vehicleId]";

        public AllowedCaller AllowedCaller => AllowedCaller.Both;

        public List<string> Aliases => new List<string>();

        public List<string> Permissions => new List<string>();

        private static readonly Random random = new Random();

        public void Execute(IRocketPlayer caller, string[] command)
        {
            var vehicles = Assets.find(EAssetType.VEHICLE).Cast<VehicleAsset>().ToArray();
            VehicleAsset vehicle = null;

            if (command.Length > 0 && ushort.TryParse(command[0], out var vehicleId))
            {
                vehicle = vehicles.FirstOrDefault(v => v.id == vehicleId);
            }

            foreach (var player in Provider.clients.Select(UnturnedPlayer.FromSteamPlayer))
            {
                var playerVehicle = vehicle ?? vehicles[random.Next(vehicles.Length)];

                player.GiveVehicle(playerVehicle.id);
                player.SendMessage(Plugin.Instance.Translate("VEHICLE_ALL_RECEIVED", playerVehicle.vehicleName));
            }

            ChatUtil.Broadcast(Plugin.Instance.Translate("VEHICLE_ALL", vehicle != null ?
                vehicle.vehicleName : Plugin.Instance.Translate("RANDOM_VEHICLE")));
        }
    }
}
