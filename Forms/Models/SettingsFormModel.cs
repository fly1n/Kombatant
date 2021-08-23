using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using System.Linq;
using Buddy.Overlay.Commands;
using ff14bot;
using ff14bot.Managers;
using Kombatant.Annotations;
using Kombatant.Settings.Models;
using Kombatant.Helpers;

namespace Kombatant.Forms.Models
{
    /// <summary>
    /// ViewModel for the settings window.
    /// </summary>
    public class SettingsFormModel : INotifyPropertyChanged
    {
        private static SettingsFormModel _settingsFormModel;
        internal static SettingsFormModel Instance => _settingsFormModel ?? (_settingsFormModel = new SettingsFormModel());

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private SettingsFormModel()
        {

        }

        /// <summary>
        /// Reference for the BotBase settings.
        /// Placed here, so we only have to deal with one model.
        /// </summary>
        public Settings.BotBase BotBase
        {
            get => Settings.BotBase.Instance;

            set
            {
                Settings.BotBase.Overwrite(value);
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Reference for the Hotkeys settings.
        /// Placed here, so we only have to deal with one model.
        /// </summary>
        public Settings.Hotkeys Hotkeys
        {
            get => Settings.Hotkeys.Instance;

            set
            {
                Settings.Hotkeys.Overwrite(value);
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Pops open the combat routine selector.
        /// </summary>
        public ICommand SelectCombatRoutine
        {
            get
            {
                return new RelayCommand(s =>
                {
                    RoutineManager.PreferedRoutine = @"";
                    RoutineManager.PickRoutine();
                });
            }
        }

        /// <summary>
        /// Adds the currently selected target to the targeting whitelist.
        /// </summary>
        public ICommand AddToTargetWhitelist
        {
            get
            {
                return new RelayCommand(s =>
                {
                    if (!Core.Me.HasTarget || BotBase.TargetWhitelist.Any(whiteListEntry => whiteListEntry.NpcId == Core.Target.NpcId))
                        return;

                    BotBase.TargetWhitelist.Add(new TargetObject(Core.Target.NpcId, Core.Target.Name));
                    LogHelper.Instance.Log($"Adding target {Core.Target.Name} to whitelist...");
                });
            }
        }

        public ICommand RemoveSelectedEntry
        {
            get
            {
                return new RelayCommand(s =>
                {
                    if (BotBase.SelectedTargetRow != null)
                    {
                        BotBase.TargetWhitelist.Remove(BotBase.SelectedTargetRow);
                    }
                });
            }
        }
    }
}