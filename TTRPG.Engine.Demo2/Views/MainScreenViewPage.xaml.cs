namespace EmptyKeys.UserInterface.Generated
{
    using System.CodeDom.Compiler;
    using EmptyKeys.UserInterface;
    using EmptyKeys.UserInterface.Controls;
    using EmptyKeys.UserInterface.Data;
    using EmptyKeys.UserInterface.Media;
    using EmptyKeys.UserInterface.Themes;


    [GeneratedCodeAttribute("Empty Keys UI Generator", "3.2.0.0")]
    public partial class MainScreenViewPage : UIRoot
    {

        private Grid e_0;

        private StackPanel e_1;

        private TextBlock e_2;

        private TabControl TabControl;

        private TextBox textBox;

        private Button buttonExecuteCommand;

        private Grid e_12;

        private TextBlock e_13;

        private TextBlock e_14;

        private Grid e_15;

        private TextBlock e_16;

        private TextBlock e_17;

        public MainScreenViewPage() :
                base()
        {
            this.Initialize();
        }

        public MainScreenViewPage(int width, int height) :
                base(width, height)
        {
            this.Initialize();
        }

        private void Initialize()
        {
            Style style = RootStyle.CreateRootStyle();
            style.TargetType = this.GetType();
            this.Style = style;
            this.InitializeComponent();
        }

        private void InitializeComponent()
        {
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
            // e_12 element
            this.e_12 = new Grid();
            this.e_0.Children.Add(this.e_12);
            this.e_12.Name = "e_12";
            RowDefinition row_e_12_0 = new RowDefinition();
            this.e_12.RowDefinitions.Add(row_e_12_0);
            ColumnDefinition col_e_12_0 = new ColumnDefinition();
            this.e_12.ColumnDefinitions.Add(col_e_12_0);
            ColumnDefinition col_e_12_1 = new ColumnDefinition();
            this.e_12.ColumnDefinitions.Add(col_e_12_1);
            Grid.SetRow(this.e_12, 3);
            Grid.SetColumnSpan(this.e_12, 4);
            // e_13 element
            this.e_13 = new TextBlock();
            this.e_12.Children.Add(this.e_13);
            this.e_13.Name = "e_13";
            this.e_13.Margin = new Thickness(5F, 5F, 5F, 5F);
            this.e_13.HorizontalAlignment = HorizontalAlignment.Stretch;
            this.e_13.VerticalAlignment = VerticalAlignment.Stretch;
            this.e_13.Background = new SolidColorBrush(new ColorW(128, 128, 128, 255));
            Grid.SetColumn(this.e_13, 0);
            Binding binding_e_13_Text = new Binding("CommandResult");
            this.e_13.SetBinding(TextBlock.TextProperty, binding_e_13_Text);
            // e_14 element
            this.e_14 = new TextBlock();
            this.e_12.Children.Add(this.e_14);
            this.e_14.Name = "e_14";
            this.e_14.Margin = new Thickness(5F, 5F, 5F, 5F);
            this.e_14.HorizontalAlignment = HorizontalAlignment.Stretch;
            this.e_14.VerticalAlignment = VerticalAlignment.Stretch;
            this.e_14.Background = new SolidColorBrush(new ColorW(128, 128, 128, 255));
            Grid.SetColumn(this.e_14, 1);
            Binding binding_e_14_Text = new Binding("StatusResult");
            this.e_14.SetBinding(TextBlock.TextProperty, binding_e_14_Text);
            // e_15 element
            this.e_15 = new Grid();
            this.e_0.Children.Add(this.e_15);
            this.e_15.Name = "e_15";
            RowDefinition row_e_15_0 = new RowDefinition();
            this.e_15.RowDefinitions.Add(row_e_15_0);
            ColumnDefinition col_e_15_0 = new ColumnDefinition();
            this.e_15.ColumnDefinitions.Add(col_e_15_0);
            ColumnDefinition col_e_15_1 = new ColumnDefinition();
            this.e_15.ColumnDefinitions.Add(col_e_15_1);
            Grid.SetRow(this.e_15, 4);
            Grid.SetColumnSpan(this.e_15, 4);
            // e_16 element
            this.e_16 = new TextBlock();
            this.e_15.Children.Add(this.e_16);
            this.e_16.Name = "e_16";
            this.e_16.Margin = new Thickness(5F, 5F, 5F, 5F);
            this.e_16.HorizontalAlignment = HorizontalAlignment.Stretch;
            this.e_16.VerticalAlignment = VerticalAlignment.Stretch;
            Grid.SetColumn(this.e_16, 0);
            Binding binding_e_16_Text = new Binding("TimeResult");
            this.e_16.SetBinding(TextBlock.TextProperty, binding_e_16_Text);
            // e_17 element
            this.e_17 = new TextBlock();
            this.e_15.Children.Add(this.e_17);
            this.e_17.Name = "e_17";
            this.e_17.Margin = new Thickness(5F, 5F, 5F, 5F);
            this.e_17.HorizontalAlignment = HorizontalAlignment.Stretch;
            this.e_17.VerticalAlignment = VerticalAlignment.Stretch;
            Grid.SetColumn(this.e_17, 1);
            Binding binding_e_17_Text = new Binding("TurnResult");
            this.e_17.SetBinding(TextBlock.TextProperty, binding_e_17_Text);
            FontManager.Instance.AddFont("Segoe UI", 26.66667F, FontStyle.Bold, "Segoe_UI_20_Bold");
        }

        private static void InitializeElementResources(UIElement elem)
        {
            elem.Resources.MergedDictionaries.Add(Dictionary.Instance);
        }

        private static System.Collections.ObjectModel.ObservableCollection<object> Get_TabControl_Items()
        {
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
            // e_5 element
            ListBox e_5 = new ListBox();
            e_4.Children.Add(e_5);
            e_5.Name = "e_5";
            e_5.VerticalAlignment = VerticalAlignment.Stretch;
            e_5.TabIndex = 2;
            Grid.SetRow(e_5, 1);
            DragDrop.SetIsDragSource(e_5, true);
            DragDrop.SetIsDropTarget(e_5, true);
            Binding binding_e_5_ItemsSource = new Binding("Targets");
            e_5.SetBinding(ListBox.ItemsSourceProperty, binding_e_5_ItemsSource);
            Binding binding_e_5_SelectedItem = new Binding("SelectedTargetItem");
            e_5.SetBinding(ListBox.SelectedItemProperty, binding_e_5_SelectedItem);
            items.Add(e_3);
            // e_6 element
            TabItem e_6 = new TabItem();
            e_6.Name = "e_6";
            e_6.HorizontalContentAlignment = HorizontalAlignment.Stretch;
            e_6.Header = "Goods";
            // e_7 element
            DataGrid e_7 = new DataGrid();
            e_6.Content = e_7;
            e_7.Name = "e_7";
            e_7.VerticalAlignment = VerticalAlignment.Stretch;
            e_7.AutoGenerateColumns = false;
            DataGridTextColumn e_7_Col0 = new DataGridTextColumn();
            e_7_Col0.Header = "Attribute";
            Binding e_7_Col0_b = new Binding("Attribute");
            e_7_Col0.Binding = e_7_Col0_b;
            e_7.Columns.Add(e_7_Col0);
            DataGridTextColumn e_7_Col1 = new DataGridTextColumn();
            e_7_Col1.Header = "Value";
            Style e_7_Col1_e_s = new Style(typeof(DataGridCell));
            Setter e_7_Col1_e_s_S_0 = new Setter(DataGridCell.BackgroundProperty, new SolidColorBrush(new ColorW(128, 128, 128, 255)));
            e_7_Col1_e_s.Setters.Add(e_7_Col1_e_s_S_0);
            Setter e_7_Col1_e_s_S_1 = new Setter(DataGridCell.HorizontalAlignmentProperty, HorizontalAlignment.Center);
            e_7_Col1_e_s.Setters.Add(e_7_Col1_e_s_S_1);
            Setter e_7_Col1_e_s_S_2 = new Setter(DataGridCell.VerticalAlignmentProperty, VerticalAlignment.Center);
            e_7_Col1_e_s.Setters.Add(e_7_Col1_e_s_S_2);
            e_7_Col1.ElementStyle = e_7_Col1_e_s;
            Binding e_7_Col1_b = new Binding("Value");
            e_7_Col1.Binding = e_7_Col1_b;
            e_7.Columns.Add(e_7_Col1);
            Binding binding_e_7_ItemsSource = new Binding("Attributes");
            e_7.SetBinding(DataGrid.ItemsSourceProperty, binding_e_7_ItemsSource);
            Binding binding_e_7_SelectedItem = new Binding("SelectedAttribute");
            e_7.SetBinding(DataGrid.SelectedItemProperty, binding_e_7_SelectedItem);
            items.Add(e_6);
            // e_8 element
            TabItem e_8 = new TabItem();
            e_8.Name = "e_8";
            e_8.HorizontalContentAlignment = HorizontalAlignment.Stretch;
            e_8.Header = "Equipment";
            // e_9 element
            Grid e_9 = new Grid();
            e_8.Content = e_9;
            e_9.Name = "e_9";
            RowDefinition row_e_9_0 = new RowDefinition();
            e_9.RowDefinitions.Add(row_e_9_0);
            RowDefinition row_e_9_1 = new RowDefinition();
            e_9.RowDefinitions.Add(row_e_9_1);
            ColumnDefinition col_e_9_0 = new ColumnDefinition();
            e_9.ColumnDefinitions.Add(col_e_9_0);
            // e_10 element
            DataGrid e_10 = new DataGrid();
            e_9.Children.Add(e_10);
            e_10.Name = "e_10";
            e_10.VerticalAlignment = VerticalAlignment.Stretch;
            e_10.AutoGenerateColumns = false;
            DataGridTextColumn e_10_Col0 = new DataGridTextColumn();
            e_10_Col0.Header = "Name";
            Binding e_10_Col0_b = new Binding("Name");
            e_10_Col0.Binding = e_10_Col0_b;
            e_10.Columns.Add(e_10_Col0);
            DataGridTextColumn e_10_Col1 = new DataGridTextColumn();
            e_10_Col1.Header = "Equipped As";
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
            Grid.SetRow(e_10, 0);
            Binding binding_e_10_ItemsSource = new Binding("InventoryItems");
            e_10.SetBinding(DataGrid.ItemsSourceProperty, binding_e_10_ItemsSource);
            Binding binding_e_10_SelectedItem = new Binding("SelectedInventoryItem");
            e_10.SetBinding(DataGrid.SelectedItemProperty, binding_e_10_SelectedItem);
            // e_11 element
            DataGrid e_11 = new DataGrid();
            e_9.Children.Add(e_11);
            e_11.Name = "e_11";
            e_11.VerticalAlignment = VerticalAlignment.Stretch;
            e_11.AutoGenerateColumns = false;
            DataGridTextColumn e_11_Col0 = new DataGridTextColumn();
            e_11_Col0.Header = "Name";
            Binding e_11_Col0_b = new Binding("Name");
            e_11_Col0.Binding = e_11_Col0_b;
            e_11.Columns.Add(e_11_Col0);
            DataGridTextColumn e_11_Col1 = new DataGridTextColumn();
            e_11_Col1.Header = "Equip As";
            Style e_11_Col1_e_s = new Style(typeof(DataGridCell));
            Setter e_11_Col1_e_s_S_0 = new Setter(DataGridCell.BackgroundProperty, new SolidColorBrush(new ColorW(128, 128, 128, 255)));
            e_11_Col1_e_s.Setters.Add(e_11_Col1_e_s_S_0);
            Setter e_11_Col1_e_s_S_1 = new Setter(DataGridCell.HorizontalAlignmentProperty, HorizontalAlignment.Center);
            e_11_Col1_e_s.Setters.Add(e_11_Col1_e_s_S_1);
            Setter e_11_Col1_e_s_S_2 = new Setter(DataGridCell.VerticalAlignmentProperty, VerticalAlignment.Center);
            e_11_Col1_e_s.Setters.Add(e_11_Col1_e_s_S_2);
            e_11_Col1.ElementStyle = e_11_Col1_e_s;
            Binding e_11_Col1_b = new Binding("EquipAs");
            e_11_Col1.Binding = e_11_Col1_b;
            e_11.Columns.Add(e_11_Col1);
            DataGridTextColumn e_11_Col2 = new DataGridTextColumn();
            e_11_Col2.Header = "Value";
            Binding e_11_Col2_b = new Binding("Value");
            e_11_Col2.Binding = e_11_Col2_b;
            e_11.Columns.Add(e_11_Col2);
            Grid.SetRow(e_11, 1);
            Binding binding_e_11_ItemsSource = new Binding("BagItems");
            e_11.SetBinding(DataGrid.ItemsSourceProperty, binding_e_11_ItemsSource);
            Binding binding_e_11_SelectedItem = new Binding("SelectedBagItem");
            e_11.SetBinding(DataGrid.SelectedItemProperty, binding_e_11_SelectedItem);
            items.Add(e_8);
            return items;
        }

        private static System.Collections.ObjectModel.ObservableCollection<object> Get_comboTargets_Items()
        {
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
            crop.Content = "Crop";
            items.Add(crop);
            // commodity element
            ComboBoxItem commodity = new ComboBoxItem();
            commodity.Name = "commodity";
            commodity.Content = "Commodities";
            items.Add(commodity);
            return items;
        }
    }
}
