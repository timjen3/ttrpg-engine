using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using EmptyKeys.UserInterface.Controls;
using EmptyKeys.UserInterface.Input;
using EmptyKeys.UserInterface.Mvvm;
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
		private GoodsDataGridItem _selectedGood;
		private string _commandResult;
		private string _statusResult;
		private string _timeResult;
		private string _turnResult;
		private List<GoodsDataGridItem> _goods;
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
					var result = _engine.Process(commandText);
					UpdateTimeResult();
					CommandResult = string.Join("\n", result.SelectMany(x => x.Messages));
					UpdateTargets();
					UpdateGoods();
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

		private DragDropItem MakeDragDropItem(Role liveTarget)
		{
			if (liveTarget.Categories.Contains("Crop")
				&& liveTarget.Attributes["Age"] == liveTarget.Attributes["Maturity"])
			{
				return new DragDropItem
				{
					Name = $"{liveTarget.Name} (Mature)",
					Code = liveTarget.Name
				};
			}
			return new DragDropItem{
				Name = liveTarget.Name,
				Code = liveTarget.Name
			};
		}

		private DragDropItem[] GetLiveTargets() => _data.Roles
				.Where(x => x.Categories.Contains(_selectedTarget, StringComparer.OrdinalIgnoreCase)
					&& int.Parse(x.Attributes["hp"]) > 0)
				.Select(x => MakeDragDropItem(x))
				.ToArray();

		private HashSet<string> GetCommodities() => _data.Roles
			.Where(x => x.Categories.Contains("Commodity", StringComparer.OrdinalIgnoreCase))
			.Select(x => x.Attributes["resource"])
			.ToHashSet(StringComparer.OrdinalIgnoreCase);

		private bool TargetsChanged()
		{
			var currentItems = Targets?.ToArray();
			var updatedItems = GetLiveTargets();
			if (currentItems == null && updatedItems == null)
				return false;
			if (currentItems == null && updatedItems != null || currentItems != null && updatedItems == null)
				return true;
			if (currentItems.Length != updatedItems.Length)
				return true;

			for (int i = 0; i < updatedItems.Length; i++)
			{
				if (updatedItems[i] != currentItems[i])
					return true;
			}

			return false;
		}

		private void UpdateTargets()
		{
			if (string.IsNullOrWhiteSpace(_selectedTarget))
				return;
			if (!TargetsChanged())
				return;

			Targets = new ObservableCollection<DragDropItem>();
			var liveTarget = GetLiveTargets();
			foreach (var target in liveTarget)
			{
				Targets.Add(target);
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
			else if (_selectedTarget == "crop")
			{
				TextBoxCommand = $"Harvest [miner:farmer,{_selectedTargetItem}:crop]";
			}
		}

		private void UpdateTextBoxCommandFromGood(object purpose)
		{
			if (_selectedGood == null)
			{
				TextBoxCommand = "";
				return;
			}
			if (purpose.ToString() == "buy")
			{
				TextBoxCommand = $"BuyCommodity [miner:buyer,{_selectedGood.Name}:commodity] {{quantity:1}}";
			}
			else
			{
				TextBoxCommand = $"SellCommodity [miner:seller,{_selectedGood.Name}:commodity] {{quantity:{_selectedGood.Amount}}}";
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

		private void UpdateGoods()
		{
			var player = _data.Roles.FirstOrDefault(x => x.Name.Equals("miner", StringComparison.OrdinalIgnoreCase));
			var updatedGoods = player.Attributes
				.Where(x => GetCommodities().Contains(x.Key))
				.Select(x => new GoodsDataGridItem
				{
					Name = x.Key,
					Amount = x.Value
				});
			Goods = updatedGoods.ToList();
		}

		private void UpdateStatusResult()
		{
			var statusResult = _engine.Process("Status [miner:target]");
			StatusResult = statusResult.First().Messages.First();
		}

		private void UpdateBagItems() => BagItems = _data.Roles.Single(x => x.Name.Equals("miner", StringComparison.OrdinalIgnoreCase))
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

		private void UpdateInventoryItems() => 
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

		private void UpdateTimeResult()
		{
			var result = _engine.Process("DisplayTime [time:time]")
				.First();
			TurnResult = result.Messages.First();
			TimeResult = result.Messages.Last();
		}
		#endregion

		public MainScreenView(GameObject data, TTRPGEngine engine)
		{
			_data = data;
			_engine = engine;
			ButtonExecuteCommand = new RelayCommand(new Action<object>(OnButtonExecuteCommandClick));
			TargetItemFocusCommand = new RelayCommand(new Action<object>(TargetItemFocus));
			BagItemFocusCommand = new RelayCommand(new Action<object>(BagItemFocused));
			InventoryItemFocusCommand = new RelayCommand(new Action<object>(InventoryItemFocus));
			CommodityExchange = new RelayCommand(new Action<object>(UpdateTextBoxCommandFromGood));
			RestCommand = new RelayCommand(new Action<object>(SetRestCommand));
			SelectedTarget = new ComboBoxItem { Name = "terrain" };
			UpdateGoods();
			UpdateStatusResult();
			UpdateInventoryItems();
			UpdateBagItems();
			UpdateTimeResult();
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
			get => new() { Name = _selectedTarget };
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
			get => new() { Name = _selectedTargetItem };
			set
			{
				SetProperty(ref _selectedTargetItem, value?.Code);
				UpdateTextBoxCommandFromTargetItem();
			}
		}

		public ICommand TargetItemFocusCommand { get; set; }

		public void TargetItemFocus(object value) => UpdateTextBoxCommandFromTargetItem();

		public GoodsDataGridItem SelectedGood
		{
			get => new() { Name = _selectedGood?.Name, Amount = _selectedGood?.Amount };
			set => SetProperty(ref _selectedGood, value);
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

		public List<GoodsDataGridItem> Goods
		{
			get => _goods;
			set => SetProperty(ref _goods, value);
		}

		public List<InventoryDataGridItem> BagItems
		{
			get => _bagItems;
			set => SetProperty(ref _bagItems, value);
		}

		public InventoryDataGridItem SelectedBagItem
		{
			get => new() { Name = _selectedBagItem?.Name, EquipAs = _selectedBagItem?.EquipAs, Value = _selectedBagItem?.Value };
			set
			{
				SetProperty(ref _selectedBagItem, value);
				UpdateTextBoxCommandFromBagItem();
			}
		}

		public ICommand BagItemFocusCommand { get; set; }

		public void BagItemFocused(object args) => UpdateTextBoxCommandFromBagItem();

		public List<InventoryDataGridItem> InventoryItems
		{
			get => _inventoryItems;
			set => SetProperty(ref _inventoryItems, value);
		}

		public InventoryDataGridItem SelectedInventoryItem
		{
			get => new() { Name = _selectedInventoryItem?.Name, EquipAs = _selectedInventoryItem?.EquipAs, Value = _selectedInventoryItem?.Value };
			set
			{
				SetProperty(ref _selectedInventoryItem, value);
				UpdateTextCommandFromInventoryItem();
			}
		}

		public ICommand InventoryItemFocusCommand { get; set; }

		public void InventoryItemFocus(object args) => UpdateTextCommandFromInventoryItem();

		public ICommand RestCommand { get; set; }

		public void SetRestCommand(object args) => TextBoxCommand = "Rest [miner:sleeper]";

		public ICommand CommodityExchange { get; set; }
	}
}
