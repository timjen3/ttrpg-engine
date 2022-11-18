using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using EmptyKeys.UserInterface.Controls;
using EmptyKeys.UserInterface.Input;
using EmptyKeys.UserInterface.Mvvm;
using TTRPG.Engine.Demo2.Controls;
using TTRPG.Engine.Demo2.Helpers;
using TTRPG.Engine.Roles;

namespace TTRPG.Engine.Demo2.Views;

internal class MainScreenView : ViewModelBase
{
	#region Private
	private readonly GameObject _data;
	private readonly TTRPGEngine _engine;
	private readonly IRoleService _roleService;
	private readonly HashSet<string> _commodityNames;

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

	private Entity Player => _data.Entities.First(x => x.Name.Equals("miner", StringComparison.OrdinalIgnoreCase));

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
				CommandResult = string.Join("\n", result.SelectMany(x => x.Messages));
				UpdateTimeResult();
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

	private DragDropItem[] GetLiveTargets() => _data.Entities
		.FilterToLiveTargets(_selectedTarget)
		.Select(EntityExtensions.MakeDragDropItem)
		.ToArray();

	private void UpdateTargets()
	{
		var liveTargets = GetLiveTargets();
		if (Targets.ItemsChanged(liveTargets))
		{
			Targets = new ObservableCollection<DragDropItem>(liveTargets);
		}
	}

	private void UpdateTextBoxCommandFromTargetItem()
		=> TextBoxCommand = TextBoxCommandHelpers.GetTextBoxCommandForTargetItem(_selectedTarget, _selectedTargetItem);

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
		=> TextBoxCommand = _selectedInventoryItem.MakeUnequipCommand();

	private void UpdateTextBoxCommandFromBagItem()
		=> TextBoxCommand = _selectedBagItem.MakeEquipCommand();

	private void UpdateGoods()
	{
		var updatedGoods = Player.GetGoods(_commodityNames);
		if (Goods.ItemsChanged(updatedGoods))
		{
			Goods = updatedGoods.ToList();
		}
	}

	private void UpdateStatusResult()
	{
		var statusResult = _engine.Process("Status [miner:target]");
		StatusResult = statusResult.First().Messages.First();
	}

	private void UpdateBagItems()
	{
		var updatedItems = Player.GetBagItems();
		if (BagItems.ItemsChanged(updatedItems))
		{
			BagItems = Player.GetBagItems();
		}
	}

	private void UpdateInventoryItems()
	{
		var updatedItems = Player.GetInventoryItems();
		if (InventoryItems.ItemsChanged(updatedItems))
		{
			InventoryItems = Player.GetInventoryItems();
		}
	}

	private void UpdateTimeResult()
	{
		var result = _engine.Process("DisplayTime [time:time]")
			.First();
		TurnResult = result.Messages.First();
		TimeResult = result.Messages.Last();
	}
	#endregion

	public MainScreenView(GameObject data, TTRPGEngine engine, IRoleService roleService)
	{
		_data = data;
		_engine = engine;
		_roleService = roleService;
		// add some random animals
		_roleService.Birth("Bear");
		_roleService.Birth("Bear");
		_roleService.Birth("Bear");
		_commodityNames = data.Entities.GetCommodityNames();
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
