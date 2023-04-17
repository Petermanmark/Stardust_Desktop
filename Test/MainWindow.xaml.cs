using AdaptiveCards;
using AdaptiveCards.Rendering;
using AdaptiveCards.Rendering.Wpf;
using Microsoft.AspNetCore.Components.RenderTree;
using Newtonsoft.Json;
using OxyPlot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Test
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            Loaded += MainWindow_Loaded;
        }

        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            // Create the JSON content for the Adaptive Card
            var cardJson = @"
            {
                ""$schema"": ""http://adaptivecards.io/schemas/adaptive-card.json"",
                ""type"": ""AdaptiveCard"",
                ""version"": ""1.0"",
                ""body"": [
                    {
                        ""type"": ""TextBlock"",
                        ""text"": ""Title of the card"",
                        ""weight"": ""bolder"",
                        ""size"": ""medium""
                    },
                    {
                        ""type"": ""TextBlock"",
                        ""text"": ""Content of the card"",
                        ""wrap"": true
                    }
                ]
            }";

            // Parse the JSON content into an AdaptiveCard object
            var card = AdaptiveCard.FromJson(cardJson).Card;

            // Render the Adaptive Card into a WPF container
            var renderer = new AdaptiveCardRenderer();
            var uiElement = renderer.RenderCard(card).FrameworkElement;

            // Add the Adaptive Card to the window
            MainGrid.Children.Add(uiElement);
        }

    }
}
