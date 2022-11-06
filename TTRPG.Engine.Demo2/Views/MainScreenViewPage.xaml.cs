// -----------------------------------------------------------
//  
//  This file was generated, please do not modify.
//  
// -----------------------------------------------------------
namespace EmptyKeys.UserInterface.Generated {
    using System;
    using System.CodeDom.Compiler;
    using System.Collections.ObjectModel;
    using EmptyKeys.UserInterface;
    using EmptyKeys.UserInterface.Charts;
    using EmptyKeys.UserInterface.Data;
    using EmptyKeys.UserInterface.Controls;
    using EmptyKeys.UserInterface.Controls.Primitives;
    using EmptyKeys.UserInterface.Input;
    using EmptyKeys.UserInterface.Interactions.Core;
    using EmptyKeys.UserInterface.Interactivity;
    using EmptyKeys.UserInterface.Media;
    using EmptyKeys.UserInterface.Media.Effects;
    using EmptyKeys.UserInterface.Media.Animation;
    using EmptyKeys.UserInterface.Media.Imaging;
    using EmptyKeys.UserInterface.Shapes;
    using EmptyKeys.UserInterface.Renderers;
    using EmptyKeys.UserInterface.Themes;
    
    
    [GeneratedCodeAttribute("Empty Keys UI Generator", "3.2.0.0")]
    public partial class MainScreenViewPage : UIRoot {
        
        private Grid e_0;
        
        private StackPanel e_1;
        
        private TextBlock e_2;
        
        private TabControl TabControl;
        
        private TextBox textBox;
        
        private Button buttonExecuteCommand;
        
        private Grid e_11;
        
        private TextBlock e_12;
        
        private TextBlock e_13;
        
        private Grid e_14;
        
        private TextBlock e_15;
        
        private TextBlock e_16;
        
        public MainScreenViewPage() : 
                base() {
            this.Initialize();
        }
        
        public MainScreenViewPage(int width, int height) : 
                base(width, height) {
            this.Initialize();
        }
        
        private void Initialize() {
            Style style = RootStyle.CreateRootStyle();
            style.TargetType = this.GetType();
            this.Style = style;
            this.InitializeComponent();
        }
        
        private void InitializeComponent() {
            InitializeElementResources(this);
            // e_0 element
            this.e_0 = new Grid();
            this.Content = this.e_0;
            this.e_0.Name = "e_0";
            RowDefinition row_e_0_0 = new RowDefinition();
            row_e_0_0.Height = new GridLength(30F, GridUnitType.Pixel);
            this.e_0.RowDefinitions.Add(row_e_0_0);
            RowDefinition row_e_0_1 = new RowDefinition();
            row_e_0_1.MinHeight = 350F;
            row_e_0_1.MaxHeight = 350F;
            this.e_0.RowDefinitions.Add(row_e_0_1);
            RowDefinition row_e_0_2 = new RowDefinition();
            row_e_0_2.MinHeight = 50F;
            row_e_0_2.MaxHeight = 50F;
            this.e_0.RowDefinitions.Add(row_e_0_2);
            RowDefinition row_e_0_3 = new RowDefinition();
            this.e_0.RowDefinitions.Add(row_e_0_3);
            RowDefinition row_e_0_4 = new RowDefinition();
            row_e_0_4.MinHeight = 50F;
            row_e_0_4.MaxHeight = 50F;
            this.e_0.RowDefinitions.Add(row_e_0_4);
            ColumnDefinition col_e_0_0 = new ColumnDefinition();
            this.e_0.ColumnDefinitions.Add(col_e_0_0);
            ColumnDefinition col_e_0_1 = new ColumnDefinition();
            this.e_0.ColumnDefinitions.Add(col_e_0_1);
            ColumnDefinition col_e_0_2 = new ColumnDefinition();
            this.e_0.ColumnDefinitions.Add(col_e_0_2);
            ColumnDefinition col_e_0_3 = new ColumnDefinition();
            this.e_0.ColumnDefinitions.Add(col_e_0_3);
            // e_1 element
            this.e_1 = new StackPanel();
            this.e_0.Children.Add(this.e_1);
            this.e_1.Name = "e_1";
            this.e_1.Background = new SolidColorBrush(new ColorW(0, 0, 0, 255));
            Grid.SetRow(this.e_1, 0);
            Grid.SetColumnSpan(this.e_1, 4);
            // e_2 element
            this.e_2 = new TextBlock();
            this.e_1.Children.Add(this.e_2);
            this.e_2.Name = "e_2";
            this.e_2.HorizontalAlignment = HorizontalAlignment.Center;
            this.e_2.VerticalAlignment = VerticalAlignment.Center;
            this.e_2.Foreground = new SolidColorBrush(new ColorW(211, 211, 211, 255));
            this.e_2.TextWrapping = TextWrapping.Wrap;
            this.e_2.FontFamily = new FontFamily("Segoe UI");
            this.e_2.FontSize = 26.66667F;
            this.e_2.FontStyle = FontStyle.Bold;
            this.e_2.SetResourceReference(TextBlock.TextProperty, "TitleResource");
            // TabControl element
            this.TabControl = new TabControl();
            this.e_0.Children.Add(this.TabControl);
            this.TabControl.Name = "TabControl";
            this.TabControl.Margin = new Thickness(10F, 10F, 10F, 10F);
            this.TabControl.VerticalAlignment = VerticalAlignment.Stretch;
            this.TabControl.ItemsSource = Get_TabControl_Items();
            Grid.SetColumn(this.TabControl, 0);
            Grid.SetRow(this.TabControl, 1);
            Grid.SetColumnSpan(this.TabControl, 4);
            // textBox element
            this.textBox = new TextBox();
            this.e_0.Children.Add(this.textBox);
            this.textBox.Name = "textBox";
            this.textBox.Margin = new Thickness(5F, 5F, 5F, 5F);
            this.textBox.HorizontalAlignment = HorizontalAlignment.Stretch;
            KeyBinding textBox_IB_0 = new KeyBinding();
            textBox_IB_0.Gesture = new KeyGesture(KeyCode.Return, ModifierKeys.None, "");
            Binding binding_textBox_IB_0_Command = new Binding("ButtonExecuteCommand");
            textBox_IB_0.SetBinding(KeyBinding.CommandProperty, binding_textBox_IB_0_Command);
            Binding binding_textBox_IB_0_CommandParameter = new Binding("TextBoxCommand");
            textBox_IB_0.SetBinding(KeyBinding.CommandParameterProperty, binding_textBox_IB_0_CommandParameter);
            textBox.InputBindings.Add(textBox_IB_0);
            textBox_IB_0.Parent = textBox;
            this.textBox.TabIndex = 3;
            this.textBox.SelectionBrush = new SolidColorBrush(new ColorW(255, 0, 0, 255));
            this.textBox.UndoLimit = 20;
            Grid.SetColumn(this.textBox, 0);
            Grid.SetRow(this.textBox, 2);
            Grid.SetColumnSpan(this.textBox, 3);
            Binding binding_textBox_Text = new Binding("TextBoxCommand");
            this.textBox.SetBinding(TextBox.TextProperty, binding_textBox_Text);
            // buttonExecuteCommand element
            this.buttonExecuteCommand = new Button();
            this.e_0.Children.Add(this.buttonExecuteCommand);
            this.buttonExecuteCommand.Name = "buttonExecuteCommand";
            this.buttonExecuteCommand.Margin = new Thickness(5F, 5F, 5F, 5F);
            this.buttonExecuteCommand.HorizontalAlignment = HorizontalAlignment.Stretch;
            this.buttonExecuteCommand.TabIndex = 4;
            this.buttonExecuteCommand.Content = "Execute Command";
            Grid.SetColumn(this.buttonExecuteCommand, 3);
            Grid.SetRow(this.buttonExecuteCommand, 2);
            Binding binding_buttonExecuteCommand_Command = new Binding("ButtonExecuteCommand");
            this.buttonExecuteCommand.SetBinding(Button.CommandProperty, binding_buttonExecuteCommand_Command);
            Binding binding_buttonExecuteCommand_CommandParameter = new Binding("TextBoxCommand");
            this.buttonExecuteCommand.SetBinding(Button.CommandParameterProperty, binding_buttonExecuteCommand_CommandParameter);
            // e_11 element
            this.e_11 = new Grid();
            this.e_0.Children.Add(this.e_11);
            this.e_11.Name = "e_11";
            RowDefinition row_e_11_0 = new RowDefinition();
            this.e_11.RowDefinitions.Add(row_e_11_0);
            ColumnDefinition col_e_11_0 = new ColumnDefinition();
            this.e_11.ColumnDefinitions.Add(col_e_11_0);
            ColumnDefinition col_e_11_1 = new ColumnDefinition();
            this.e_11.ColumnDefinitions.Add(col_e_11_1);
            ColumnDefinition col_e_11_2 = new ColumnDefinition();
            this.e_11.ColumnDefinitions.Add(col_e_11_2);
            ColumnDefinition col_e_11_3 = new ColumnDefinition();
            this.e_11.ColumnDefinitions.Add(col_e_11_3);
            Grid.SetRow(this.e_11, 3);
            Grid.SetColumnSpan(this.e_11, 4);
            // e_12 element
            this.e_12 = new TextBlock();
            this.e_11.Children.Add(this.e_12);
            this.e_12.Name = "e_12";
            this.e_12.Margin = new Thickness(5F, 5F, 5F, 5F);
            this.e_12.HorizontalAlignment = HorizontalAlignment.Stretch;
            this.e_12.VerticalAlignment = VerticalAlignment.Stretch;
            this.e_12.Background = new SolidColorBrush(new ColorW(128, 128, 128, 255));
            Grid.SetColumn(this.e_12, 0);
            Grid.SetColumnSpan(this.e_12, 3);
            Binding binding_e_12_Text = new Binding("CommandResult");
            this.e_12.SetBinding(TextBlock.TextProperty, binding_e_12_Text);
            // e_13 element
            this.e_13 = new TextBlock();
            this.e_11.Children.Add(this.e_13);
            this.e_13.Name = "e_13";
            this.e_13.Margin = new Thickness(5F, 5F, 5F, 5F);
            this.e_13.HorizontalAlignment = HorizontalAlignment.Stretch;
            this.e_13.VerticalAlignment = VerticalAlignment.Stretch;
            MouseBinding e_13_IB_0 = new MouseBinding();
            e_13_IB_0.Gesture = new MouseGesture(MouseAction.LeftClick, ModifierKeys.None);
            Binding binding_e_13_IB_0_Command = new Binding("RestCommand");
            e_13_IB_0.SetBinding(MouseBinding.CommandProperty, binding_e_13_IB_0_Command);
            e_13.InputBindings.Add(e_13_IB_0);
            e_13_IB_0.Parent = e_13;
            this.e_13.Background = new SolidColorBrush(new ColorW(128, 128, 128, 255));
            Grid.SetColumn(this.e_13, 3);
            Binding binding_e_13_Text = new Binding("StatusResult");
            this.e_13.SetBinding(TextBlock.TextProperty, binding_e_13_Text);
            // e_14 element
            this.e_14 = new Grid();
            this.e_0.Children.Add(this.e_14);
            this.e_14.Name = "e_14";
            RowDefinition row_e_14_0 = new RowDefinition();
            this.e_14.RowDefinitions.Add(row_e_14_0);
            ColumnDefinition col_e_14_0 = new ColumnDefinition();
            this.e_14.ColumnDefinitions.Add(col_e_14_0);
            ColumnDefinition col_e_14_1 = new ColumnDefinition();
            this.e_14.ColumnDefinitions.Add(col_e_14_1);
            Grid.SetRow(this.e_14, 4);
            Grid.SetColumnSpan(this.e_14, 4);
            // e_15 element
            this.e_15 = new TextBlock();
            this.e_14.Children.Add(this.e_15);
            this.e_15.Name = "e_15";
            this.e_15.Margin = new Thickness(5F, 5F, 5F, 5F);
            this.e_15.HorizontalAlignment = HorizontalAlignment.Stretch;
            this.e_15.VerticalAlignment = VerticalAlignment.Stretch;
            Grid.SetColumn(this.e_15, 0);
            Binding binding_e_15_Text = new Binding("TimeResult");
            this.e_15.SetBinding(TextBlock.TextProperty, binding_e_15_Text);
            // e_16 element
            this.e_16 = new TextBlock();
            this.e_14.Children.Add(this.e_16);
            this.e_16.Name = "e_16";
            this.e_16.Margin = new Thickness(5F, 5F, 5F, 5F);
            this.e_16.HorizontalAlignment = HorizontalAlignment.Stretch;
            this.e_16.VerticalAlignment = VerticalAlignment.Stretch;
            Grid.SetColumn(this.e_16, 1);
            Binding binding_e_16_Text = new Binding("TurnResult");
            this.e_16.SetBinding(TextBlock.TextProperty, binding_e_16_Text);
            FontManager.Instance.AddFont("Segoe UI", 26.66667F, FontStyle.Bold, "Segoe_UI_20_Bold");
        }
        
        private static void InitializeElementResources(UIElement elem) {
            elem.Resources.MergedDictionaries.Add(Dictionary.Instance);
        }
        
        private static System.Collections.ObjectModel.ObservableCollection<object> Get_TabControl_Items() {
            System.Collections.ObjectModel.ObservableCollection<object> items = new System.Collections.ObjectModel.ObservableCollection<object>();
            // e_3 element
            TabItem e_3 = new TabItem();
            e_3.Name = "e_3";
            e_3.VerticalAlignment = VerticalAlignment.Stretch;
            e_3.HorizontalContentAlignment = HorizontalAlignment.Stretch;
            e_3.Header = "Area";
            // e_4 element
            Grid e_4 = new Grid();
            e_3.Content = e_4;
            e_4.Name = "e_4";
            RowDefinition row_e_4_0 = new RowDefinition();
            row_e_4_0.Height = new GridLength(50F, GridUnitType.Pixel);
            e_4.RowDefinitions.Add(row_e_4_0);
            RowDefinition row_e_4_1 = new RowDefinition();
            e_4.RowDefinitions.Add(row_e_4_1);
            ColumnDefinition col_e_4_0 = new ColumnDefinition();
            e_4.ColumnDefinitions.Add(col_e_4_0);
            // comboTargets element
            ComboBox comboTargets = new ComboBox();
            e_4.Children.Add(comboTargets);
            comboTargets.Name = "comboTargets";
            comboTargets.Width = 300F;
            comboTargets.Margin = new Thickness(5F, 5F, 5F, 5F);
            comboTargets.TabIndex = 1;
            comboTargets.ItemsSource = Get_comboTargets_Items();
            comboTargets.SelectedIndex = 0;
            Grid.SetRow(comboTargets, 0);
            Binding binding_comboTargets_SelectedItem = new Binding("SelectedTarget");
            comboTargets.SetBinding(ComboBox.SelectedItemProperty, binding_comboTargets_SelectedItem);
            // AreaList element
            ListBox AreaList = new ListBox();
            e_4.Children.Add(AreaList);
            AreaList.Name = "AreaList";
            AreaList.VerticalAlignment = VerticalAlignment.Stretch;
            EventTriggerBehavior AreaList_BEH_0 = new EventTriggerBehavior();
            Interaction.GetBehaviors(AreaList).Add(AreaList_BEH_0);
            InvokeCommandAction AreaList_BEH_0_ACT_0 = new InvokeCommandAction();
            AreaList_BEH_0.Actions.Add(AreaList_BEH_0_ACT_0);
            Binding binding_AreaList_BEH_0_ACT_0_Command = new Binding("TargetItemFocusCommand");
            binding_AreaList_BEH_0_ACT_0_Command.Source = AreaList_BEH_0;
            AreaList_BEH_0_ACT_0.SetBinding(InvokeCommandAction.CommandProperty, binding_AreaList_BEH_0_ACT_0_Command);
            AreaList_BEH_0.EventName = "GotFocus";
            AreaList.TabIndex = 2;
            Grid.SetRow(AreaList, 1);
            DragDrop.SetIsDragSource(AreaList, true);
            DragDrop.SetIsDropTarget(AreaList, true);
            Binding binding_AreaList_ItemsSource = new Binding("Targets");
            AreaList.SetBinding(ListBox.ItemsSourceProperty, binding_AreaList_ItemsSource);
            Binding binding_AreaList_SelectedItem = new Binding("SelectedTargetItem");
            AreaList.SetBinding(ListBox.SelectedItemProperty, binding_AreaList_SelectedItem);
            items.Add(e_3);
            // e_5 element
            TabItem e_5 = new TabItem();
            e_5.Name = "e_5";
            e_5.HorizontalContentAlignment = HorizontalAlignment.Stretch;
            e_5.Header = "Goods";
            // e_6 element
            Grid e_6 = new Grid();
            e_5.Content = e_6;
            e_6.Name = "e_6";
            RowDefinition row_e_6_0 = new RowDefinition();
            e_6.RowDefinitions.Add(row_e_6_0);
            RowDefinition row_e_6_1 = new RowDefinition();
            row_e_6_1.Height = new GridLength(75F, GridUnitType.Pixel);
            e_6.RowDefinitions.Add(row_e_6_1);
            ColumnDefinition col_e_6_0 = new ColumnDefinition();
            e_6.ColumnDefinitions.Add(col_e_6_0);
            ColumnDefinition col_e_6_1 = new ColumnDefinition();
            e_6.ColumnDefinitions.Add(col_e_6_1);
            // GoodsTable element
            DataGrid GoodsTable = new DataGrid();
            e_6.Children.Add(GoodsTable);
            GoodsTable.Name = "GoodsTable";
            GoodsTable.VerticalAlignment = VerticalAlignment.Stretch;
            GoodsTable.AutoGenerateColumns = false;
            DataGridTextColumn GoodsTable_Col0 = new DataGridTextColumn();
            GoodsTable_Col0.Width = 150F;
            GoodsTable_Col0.Header = "Name";
            Binding GoodsTable_Col0_b = new Binding("Name");
            GoodsTable_Col0.Binding = GoodsTable_Col0_b;
            GoodsTable.Columns.Add(GoodsTable_Col0);
            DataGridTextColumn GoodsTable_Col1 = new DataGridTextColumn();
            GoodsTable_Col1.Width = 75F;
            GoodsTable_Col1.Header = "Amount";
            Style GoodsTable_Col1_e_s = new Style(typeof(DataGridCell));
            Setter GoodsTable_Col1_e_s_S_0 = new Setter(DataGridCell.BackgroundProperty, new SolidColorBrush(new ColorW(128, 128, 128, 255)));
            GoodsTable_Col1_e_s.Setters.Add(GoodsTable_Col1_e_s_S_0);
            Setter GoodsTable_Col1_e_s_S_1 = new Setter(DataGridCell.HorizontalAlignmentProperty, HorizontalAlignment.Center);
            GoodsTable_Col1_e_s.Setters.Add(GoodsTable_Col1_e_s_S_1);
            Setter GoodsTable_Col1_e_s_S_2 = new Setter(DataGridCell.VerticalAlignmentProperty, VerticalAlignment.Center);
            GoodsTable_Col1_e_s.Setters.Add(GoodsTable_Col1_e_s_S_2);
            GoodsTable_Col1.ElementStyle = GoodsTable_Col1_e_s;
            Binding GoodsTable_Col1_b = new Binding("Amount");
            GoodsTable_Col1.Binding = GoodsTable_Col1_b;
            GoodsTable.Columns.Add(GoodsTable_Col1);
            Grid.SetRow(GoodsTable, 0);
            Grid.SetColumnSpan(GoodsTable, 2);
            Binding binding_GoodsTable_ItemsSource = new Binding("Goods");
            GoodsTable.SetBinding(DataGrid.ItemsSourceProperty, binding_GoodsTable_ItemsSource);
            Binding binding_GoodsTable_SelectedItem = new Binding("SelectedGood");
            GoodsTable.SetBinding(DataGrid.SelectedItemProperty, binding_GoodsTable_SelectedItem);
            // buttonBuyCommodity element
            Button buttonBuyCommodity = new Button();
            e_6.Children.Add(buttonBuyCommodity);
            buttonBuyCommodity.Name = "buttonBuyCommodity";
            buttonBuyCommodity.Margin = new Thickness(5F, 5F, 5F, 5F);
            buttonBuyCommodity.HorizontalAlignment = HorizontalAlignment.Stretch;
            buttonBuyCommodity.TabIndex = 4;
            buttonBuyCommodity.Content = "Buy";
            buttonBuyCommodity.CommandParameter = "buy";
            Grid.SetColumn(buttonBuyCommodity, 0);
            Grid.SetRow(buttonBuyCommodity, 1);
            Binding binding_buttonBuyCommodity_Command = new Binding("CommodityExchange");
            buttonBuyCommodity.SetBinding(Button.CommandProperty, binding_buttonBuyCommodity_Command);
            // buttonSellCommodity element
            Button buttonSellCommodity = new Button();
            e_6.Children.Add(buttonSellCommodity);
            buttonSellCommodity.Name = "buttonSellCommodity";
            buttonSellCommodity.Margin = new Thickness(5F, 5F, 5F, 5F);
            buttonSellCommodity.HorizontalAlignment = HorizontalAlignment.Stretch;
            buttonSellCommodity.TabIndex = 4;
            buttonSellCommodity.Content = "Sell";
            buttonSellCommodity.CommandParameter = "sell";
            Grid.SetColumn(buttonSellCommodity, 1);
            Grid.SetRow(buttonSellCommodity, 1);
            Binding binding_buttonSellCommodity_Command = new Binding("CommodityExchange");
            buttonSellCommodity.SetBinding(Button.CommandProperty, binding_buttonSellCommodity_Command);
            items.Add(e_5);
            // e_7 element
            TabItem e_7 = new TabItem();
            e_7.Name = "e_7";
            e_7.HorizontalContentAlignment = HorizontalAlignment.Stretch;
            e_7.Header = "Equipment";
            // e_8 element
            Grid e_8 = new Grid();
            e_7.Content = e_8;
            e_8.Name = "e_8";
            RowDefinition row_e_8_0 = new RowDefinition();
            e_8.RowDefinitions.Add(row_e_8_0);
            RowDefinition row_e_8_1 = new RowDefinition();
            e_8.RowDefinitions.Add(row_e_8_1);
            ColumnDefinition col_e_8_0 = new ColumnDefinition();
            e_8.ColumnDefinitions.Add(col_e_8_0);
            // e_9 element
            DataGrid e_9 = new DataGrid();
            e_8.Children.Add(e_9);
            e_9.Name = "e_9";
            e_9.VerticalAlignment = VerticalAlignment.Stretch;
            EventTriggerBehavior e_9_BEH_0 = new EventTriggerBehavior();
            Interaction.GetBehaviors(e_9).Add(e_9_BEH_0);
            InvokeCommandAction e_9_BEH_0_ACT_0 = new InvokeCommandAction();
            e_9_BEH_0.Actions.Add(e_9_BEH_0_ACT_0);
            Binding binding_e_9_BEH_0_ACT_0_Command = new Binding("InventoryItemFocusCommand");
            binding_e_9_BEH_0_ACT_0_Command.Source = e_9_BEH_0;
            e_9_BEH_0_ACT_0.SetBinding(InvokeCommandAction.CommandProperty, binding_e_9_BEH_0_ACT_0_Command);
            e_9_BEH_0.EventName = "GotFocus";
            e_9.AutoGenerateColumns = false;
            DataGridTextColumn e_9_Col0 = new DataGridTextColumn();
            e_9_Col0.Width = 200F;
            e_9_Col0.Header = "Name";
            Binding e_9_Col0_b = new Binding("Name");
            e_9_Col0.Binding = e_9_Col0_b;
            e_9.Columns.Add(e_9_Col0);
            DataGridTextColumn e_9_Col1 = new DataGridTextColumn();
            e_9_Col1.Width = 150F;
            e_9_Col1.Header = "Equipped As";
            Style e_9_Col1_e_s = new Style(typeof(DataGridCell));
            Setter e_9_Col1_e_s_S_0 = new Setter(DataGridCell.BackgroundProperty, new SolidColorBrush(new ColorW(128, 128, 128, 255)));
            e_9_Col1_e_s.Setters.Add(e_9_Col1_e_s_S_0);
            Setter e_9_Col1_e_s_S_1 = new Setter(DataGridCell.HorizontalAlignmentProperty, HorizontalAlignment.Center);
            e_9_Col1_e_s.Setters.Add(e_9_Col1_e_s_S_1);
            Setter e_9_Col1_e_s_S_2 = new Setter(DataGridCell.VerticalAlignmentProperty, VerticalAlignment.Center);
            e_9_Col1_e_s.Setters.Add(e_9_Col1_e_s_S_2);
            e_9_Col1.ElementStyle = e_9_Col1_e_s;
            Binding e_9_Col1_b = new Binding("EquipAs");
            e_9_Col1.Binding = e_9_Col1_b;
            e_9.Columns.Add(e_9_Col1);
            Grid.SetRow(e_9, 0);
            Binding binding_e_9_ItemsSource = new Binding("InventoryItems");
            e_9.SetBinding(DataGrid.ItemsSourceProperty, binding_e_9_ItemsSource);
            Binding binding_e_9_SelectedItem = new Binding("SelectedInventoryItem");
            e_9.SetBinding(DataGrid.SelectedItemProperty, binding_e_9_SelectedItem);
            // e_10 element
            DataGrid e_10 = new DataGrid();
            e_8.Children.Add(e_10);
            e_10.Name = "e_10";
            e_10.VerticalAlignment = VerticalAlignment.Stretch;
            EventTriggerBehavior e_10_BEH_0 = new EventTriggerBehavior();
            Interaction.GetBehaviors(e_10).Add(e_10_BEH_0);
            InvokeCommandAction e_10_BEH_0_ACT_0 = new InvokeCommandAction();
            e_10_BEH_0.Actions.Add(e_10_BEH_0_ACT_0);
            Binding binding_e_10_BEH_0_ACT_0_Command = new Binding("BagItemFocusCommand");
            binding_e_10_BEH_0_ACT_0_Command.Source = e_10_BEH_0;
            e_10_BEH_0_ACT_0.SetBinding(InvokeCommandAction.CommandProperty, binding_e_10_BEH_0_ACT_0_Command);
            e_10_BEH_0.EventName = "GotFocus";
            e_10.AutoGenerateColumns = false;
            DataGridTextColumn e_10_Col0 = new DataGridTextColumn();
            e_10_Col0.Width = 200F;
            e_10_Col0.Header = "Name";
            Binding e_10_Col0_b = new Binding("Name");
            e_10_Col0.Binding = e_10_Col0_b;
            e_10.Columns.Add(e_10_Col0);
            DataGridTextColumn e_10_Col1 = new DataGridTextColumn();
            e_10_Col1.Width = 150F;
            e_10_Col1.Header = "Equip As";
            Style e_10_Col1_e_s = new Style(typeof(DataGridCell));
            Setter e_10_Col1_e_s_S_0 = new Setter(DataGridCell.BackgroundProperty, new SolidColorBrush(new ColorW(128, 128, 128, 255)));
            e_10_Col1_e_s.Setters.Add(e_10_Col1_e_s_S_0);
            Setter e_10_Col1_e_s_S_1 = new Setter(DataGridCell.HorizontalAlignmentProperty, HorizontalAlignment.Center);
            e_10_Col1_e_s.Setters.Add(e_10_Col1_e_s_S_1);
            Setter e_10_Col1_e_s_S_2 = new Setter(DataGridCell.VerticalAlignmentProperty, VerticalAlignment.Center);
            e_10_Col1_e_s.Setters.Add(e_10_Col1_e_s_S_2);
            e_10_Col1.ElementStyle = e_10_Col1_e_s;
            Binding e_10_Col1_b = new Binding("EquipAs");
            e_10_Col1.Binding = e_10_Col1_b;
            e_10.Columns.Add(e_10_Col1);
            DataGridTextColumn e_10_Col2 = new DataGridTextColumn();
            e_10_Col2.Header = "Value";
            Binding e_10_Col2_b = new Binding("Value");
            e_10_Col2.Binding = e_10_Col2_b;
            e_10.Columns.Add(e_10_Col2);
            Grid.SetRow(e_10, 1);
            Binding binding_e_10_ItemsSource = new Binding("BagItems");
            e_10.SetBinding(DataGrid.ItemsSourceProperty, binding_e_10_ItemsSource);
            Binding binding_e_10_SelectedItem = new Binding("SelectedBagItem");
            e_10.SetBinding(DataGrid.SelectedItemProperty, binding_e_10_SelectedItem);
            items.Add(e_7);
            return items;
        }
        
        private static System.Collections.ObjectModel.ObservableCollection<object> Get_comboTargets_Items() {
            System.Collections.ObjectModel.ObservableCollection<object> items = new System.Collections.ObjectModel.ObservableCollection<object>();
            // terrain element
            ComboBoxItem terrain = new ComboBoxItem();
            terrain.Name = "terrain";
            terrain.Content = "Terrain";
            terrain.IsSelected = true;
            items.Add(terrain);
            // animal element
            ComboBoxItem animal = new ComboBoxItem();
            animal.Name = "animal";
            animal.Content = "Animals";
            items.Add(animal);
            // crop element
            ComboBoxItem crop = new ComboBoxItem();
            crop.Name = "crop";
            crop.Content = "Crops";
            items.Add(crop);
            return items;
        }
    }
}
