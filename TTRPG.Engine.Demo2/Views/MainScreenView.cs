using EmptyKeys.UserInterface.Controls;
using EmptyKeys.UserInterface.Input;
using EmptyKeys.UserInterface.Mvvm;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using TTRPG.Engine.CommandParsing;
using TTRPG.Engine.Demo2.Controls;

namespace TTRPG.Engine.Demo2.Views
{
	internal class MainScreenView : ViewModelBase
	{
		#region Private
		private readonly GameObject _data;
		private readonly TTRPGEngine _engine;

		private ObservableCollection<DragDropItem> _targets;
		private string _textBoxCommand;
		private string _selectedTarget;
		private string _selectedTargetItem;
		private StatDataGridItem _selectedAttribute;
		private string _commandResult;
		private string _statusResult;
		private string _timeResult;
		private string _turnResult;
		private List<StatDataGridItem> _attributes;
		private InventoryDataGridItem _selectedBagItem;
		private List<InventoryDataGridItem> _bagItems;
		private InventoryDataGridItem _selectedInventoryItem;
		private List<InventoryDataGridItem> _inventoryItems;

		public void OnButtonExecuteCommandClick(object command)
		{
			var commandText = !string.IsNullOrWhiteSpace(command as string)
				? command as string
				: TextBoxCommand;
			if (!string.IsNullOrWhiteSpace(commandText))
			{
				try
				{
					var result = _engine.Process(commandText, false);
					AdvanceTimeMessage(result);
					CommandResult = string.Join("\n", result.First().Messages);
					UpdateTargets();
					UpdatePlayerAttributes();
					UpdateStatusResult();
					UpdateInventoryItems();
					UpdateBagItems();
				}
				catch (Exception ex)
				{
					CommandResult = ex.Message;
				}
			}
		}

		private string[] GetLiveTargets() => _data.Roles
				.Where(x => x.Categories.Contains(_selectedTarget, StringComparer.OrdinalIgnoreCase)
					&& int.Parse(x.Attributes["hp"]) > 0)
				.Select(x => x.Name)
				.ToArray();

		private HashSet<string> GetCommodities() => _data.Roles
			.Where(x => x.Categories.Contains("Commodity", StringComparer.OrdinalIgnoreCase))
			.Select(x => x.Attributes["resource"])
			.ToHashSet(StringComparer.OrdinalIgnoreCase);

		private bool TargetsChanged()
		{
			var currentItems = Targets?.Select(x => x.Name).ToArray();
			var updatedItems = GetLiveTargets();
			if (currentItems == null && updatedItems == null) return false;
			if (currentItems == null && updatedItems != null || currentItems != null && updatedItems == null) return true;
			if (currentItems.Length != updatedItems.Length) return true;

			for (int i = 0; i < updatedItems.Length; i++)
			{
				if (updatedItems[i] != currentItems[i])
					return true;
			}

			return false;
		}

		private void UpdateTargets()
		{
			if (string.IsNullOrWhiteSpace(_selectedTarget)) return;
			if (!TargetsChanged()) return;

			Targets = new ObservableCollection<DragDropItem>();
			var liveTarget = GetLiveTargets();
			foreach (var target in liveTarget)
			{
				Targets.Add(new DragDropItem { Name = target });
			}
		}

		private void UpdateTextBoxCommandFromTargetItem()
		{
			if (string.IsNullOrWhiteSpace(_selectedTargetItem))
			{
				TextBoxCommand = "";
				return;
			}
			if (_selectedTarget == "terrain")
			{
				TextBoxCommand = $"MineTerrain [miner:miner,{_selectedTargetItem}:terrain]";
			}
			else if (_selectedTarget == "animal")
			{
				TextBoxCommand = $"Hunt [miner:hunter,{_selectedTargetItem}:defender]";
			}
			else if (_selectedTarget == "commodity")
			{
				TextBoxCommand = $"BuyCommodity [miner:buyer,{_selectedTargetItem}:commodity] {{quantity:1}}";
			}
		}

		private void UpdateTextBoxCommandFromAttribute()
		{
			if (_selectedAttribute == null)
			{
				TextBoxCommand = "";
				return;
			}
			if (GetCommodities().Contains(_selectedAttribute.Attribute) && int.Parse(_selectedAttribute.Value) > 0)
			{
				TextBoxCommand = $"SellCommodity [miner:seller,{_selectedAttribute.Attribute}:commodity] {{quantity:{_selectedAttribute.Value}}}";
			}
		}

		private void UpdateTextCommandFromInventoryItem()
		{
			if (_selectedInventoryItem == null)
			{
				TextBoxCommand = "";
				return;
			}
			TextBoxCommand = $"Unequip [miner] {{itemName:{_selectedInventoryItem.EquipAs}}}";
		}

		private void UpdateTextBoxCommandFromBagItem()
		{
			if (_selectedBagItem == null)
			{
				TextBoxCommand = "";
				return;
			}
			TextBoxCommand = $"Equip [miner] {{itemName:{_selectedBagItem.Name},equipAs:{_selectedBagItem.EquipAs}}}";
		}

		private void UpdatePlayerAttributes()
		{
			var player = _data.Roles.FirstOrDefault(x => x.Name.Equals("miner", StringComparison.OrdinalIgnoreCase));
			var updatedAttributes = player.Attributes
				.Where(x => GetCommodities().Contains(x.Key))
				.Select(x => new StatDataGridItem
				{
					Attribute = x.Key,
					Value = x.Value
				});
			Attributes = updatedAttributes.ToList();
		}

		private void UpdateStatusResult()
		{
			var statusResult = _engine.Process("Status [miner:target]", false);
			StatusResult = statusResult.First().Messages.First();
		}

		private void UpdateBagItems()
		{
			BagItems = _data.Roles.Single(x => x.Name.Equals("miner", StringComparison.OrdinalIgnoreCase))
				.Bag
				.Where(x => x.Attributes.ContainsKey("equipAs"))
				.Select(x => new InventoryDataGridItem
				{
					Name = x.Name,
					EquipAs = x.Attributes["EquipAs"],
					Value = x.Attributes["Value"]
				})
				.OrderBy(x => x.Value)
				.OrderBy(x => x.EquipAs)
				.ToList();
		}

		private void UpdateInventoryItems()
		{
			InventoryItems = _data.Roles.Single(x => x.Name.Equals("miner", StringComparison.OrdinalIgnoreCase))
				.InventoryItems
				.Where(x => x.Value.Attributes.ContainsKey("equipAs"))
				.Select(x => new InventoryDataGridItem
				{
					Name = x.Value.Name,
					EquipAs = x.Value.Attributes["EquipAs"],
					Value = x.Value.Attributes["Value"]
				})
				.OrderBy(x => x.Value)
				.OrderBy(x => x.EquipAs)
				.ToList();
		}

		private void AdvanceTimeMessage(IEnumerable<ProcessedCommand> processed)
		{
			foreach (var command in processed)
			{
				if (command.Source.MainCommand == "AdvanceTime")
				{
					TurnResult = command.Messages.First();
					TimeResult = command.Messages.Last();
				}
			}
		}
		#endregion

		public MainScreenView(GameObject data, TTRPGEngine engine)
		{
			_data = data;
			_engine = engine;
			ButtonExecuteCommand = new RelayCommand(new Action<object>(OnButtonExecuteCommandClick));
			UpdatePlayerAttributes();
			SelectedTarget = new ComboBoxItem { Name = "terrain" };
			UpdateStatusResult();
			UpdateInventoryItems();
			UpdateBagItems();
			TimeResult = "Day: 0 Time: 0";
			TurnResult = "Turn 0";
		}

		public ObservableCollection<DragDropItem> Targets
		{
			get => _targets;
			set => SetProperty(ref _targets, value);
		}

		public string TextBoxCommand
		{
			get => _textBoxCommand;
			set => SetProperty(ref _textBoxCommand, value);
		}

		public ComboBoxItem SelectedTarget
		{
			get => new ComboBoxItem() { Name = _selectedTarget };
			set
			{
				if (_selectedTarget != value?.Name)
				{
					SetProperty(ref _selectedTarget, value?.Name);
					UpdateTargets();
				}
			}
		}

		public DragDropItem SelectedTargetItem
		{
			get => null;
			set
			{
				SetProperty(ref _selectedTargetItem, value?.Name);
				UpdateTextBoxCommandFromTargetItem();
			}
		}

		public StatDataGridItem SelectedAttribute
		{
			get => null;
			set
			{
				SetProperty(ref _selectedAttribute, value);
				UpdateTextBoxCommandFromAttribute();
			}
		}

		public ICommand ButtonExecuteCommand { get; set; }

		public string CommandResult
		{
			get => _commandResult;
			set => SetProperty(ref _commandResult, value);
		}

		public string StatusResult
		{
			get => _statusResult;
			set => SetProperty(ref _statusResult, value);
		}

		public string TimeResult
		{
			get => _timeResult;
			set => SetProperty(ref _timeResult, value);
		}

		public string TurnResult
		{
			get => _turnResult;
			set => SetProperty(ref _turnResult, value);
		}

		public List<StatDataGridItem> Attributes
		{
			get { return _attributes; }
			set { SetProperty(ref _attributes, value); }
		}

		public List<InventoryDataGridItem> BagItems
		{
			get { return _bagItems; }
			set { SetProperty(ref _bagItems, value); }
		}

		public InventoryDataGridItem SelectedBagItem
		{
			get => null;
			set
			{
				SetProperty(ref _selectedBagItem, value);
				UpdateTextBoxCommandFromBagItem();
			}
		}

		public List<InventoryDataGridItem> InventoryItems
		{
			get { return _inventoryItems; }
			set { SetProperty(ref _inventoryItems, value); }
		}

		public InventoryDataGridItem SelectedInventoryItem
		{
			get => null;
			set
			{
				SetProperty(ref _selectedInventoryItem, value);
				UpdateTextCommandFromInventoryItem();
			}
		}
	}
}
