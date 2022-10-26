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
        
        private Grid e_8;
        
        private TextBlock e_9;
        
        private TextBlock e_10;
        
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
            // e_8 element
            this.e_8 = new Grid();
            this.e_0.Children.Add(this.e_8);
            this.e_8.Name = "e_8";
            RowDefinition row_e_8_0 = new RowDefinition();
            this.e_8.RowDefinitions.Add(row_e_8_0);
            ColumnDefinition col_e_8_0 = new ColumnDefinition();
            this.e_8.ColumnDefinitions.Add(col_e_8_0);
            ColumnDefinition col_e_8_1 = new ColumnDefinition();
            this.e_8.ColumnDefinitions.Add(col_e_8_1);
            Grid.SetRow(this.e_8, 3);
            Grid.SetColumnSpan(this.e_8, 4);
            // e_9 element
            this.e_9 = new TextBlock();
            this.e_8.Children.Add(this.e_9);
            this.e_9.Name = "e_9";
            this.e_9.Margin = new Thickness(5F, 5F, 5F, 5F);
            this.e_9.HorizontalAlignment = HorizontalAlignment.Stretch;
            this.e_9.VerticalAlignment = VerticalAlignment.Stretch;
            this.e_9.Background = new SolidColorBrush(new ColorW(128, 128, 128, 255));
            Grid.SetColumn(this.e_9, 0);
            Binding binding_e_9_Text = new Binding("CommandResult");
            this.e_9.SetBinding(TextBlock.TextProperty, binding_e_9_Text);
            // e_10 element
            this.e_10 = new TextBlock();
            this.e_8.Children.Add(this.e_10);
            this.e_10.Name = "e_10";
            this.e_10.Margin = new Thickness(5F, 5F, 5F, 5F);
            this.e_10.HorizontalAlignment = HorizontalAlignment.Stretch;
            this.e_10.VerticalAlignment = VerticalAlignment.Stretch;
            this.e_10.Background = new SolidColorBrush(new ColorW(128, 128, 128, 255));
            Grid.SetColumn(this.e_10, 1);
            Binding binding_e_10_Text = new Binding("StatusResult");
            this.e_10.SetBinding(TextBlock.TextProperty, binding_e_10_Text);
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
            e_6.Header = "Player";
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
            items.Add(e_6);
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
            // commodity element
            ComboBoxItem commodity = new ComboBoxItem();
            commodity.Name = "commodity";
            commodity.Content = "Commodities";
            items.Add(commodity);
            return items;
        }
    }
}
