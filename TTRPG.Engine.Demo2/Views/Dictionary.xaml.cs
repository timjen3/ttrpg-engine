namespace EmptyKeys.UserInterface.Generated
{
    using System;
    using System.CodeDom.Compiler;
    using EmptyKeys.UserInterface;
    using EmptyKeys.UserInterface.Controls;
    using EmptyKeys.UserInterface.Data;


    [GeneratedCodeAttribute("Empty Keys UI Generator", "3.2.0.0")]
    public sealed class Dictionary : ResourceDictionary
    {

        private static Dictionary singleton = new Dictionary();

        public Dictionary()
        {
            this.InitializeResources();
        }

        public static Dictionary Instance
        {
            get
            {
                return singleton;
            }
        }

        private void InitializeResources()
        {
            // Resource - [DataTemplateKey(TTRPG.Engine.Demo2.Controls.DragDropItem)] DataTemplate
            Func<UIElement, UIElement> r_0_dtFunc = r_0_dtMethod;
            this.Add(typeof(TTRPG.Engine.Demo2.Controls.DragDropItem), new DataTemplate(typeof(TTRPG.Engine.Demo2.Controls.DragDropItem), r_0_dtFunc));
            // Resource - [TitleResource] String
            this.Add("TitleResource", "Survival Game");
        }

        private static UIElement r_0_dtMethod(UIElement parent)
        {
            // e_0 element
            TextBlock e_0 = new TextBlock();
            e_0.Parent = parent;
            e_0.Name = "e_0";
            e_0.Margin = new Thickness(5F, 5F, 5F, 5F);
            Binding binding_e_0_Text = new Binding("Name");
            e_0.SetBinding(TextBlock.TextProperty, binding_e_0_Text);
            return e_0;
        }
    }
}
