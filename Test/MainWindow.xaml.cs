using AdaptiveCards;
using AdaptiveCards.Rendering;
using AdaptiveCards.Rendering.Wpf;
using Microsoft.AspNetCore.Components.RenderTree;
using Newtonsoft.Json;
using OxyPlot;
using System;
using System.Collections.Generic;
using System.Diagnostics;
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

            // Define a list of card data
            List<CardData> cardDataList = new List<CardData>()
            {
                new CardData("Title 1", "Content 1"),
                new CardData("Title 2", "Content 2"),
                new CardData("Title 3", "Content 3"),
                new CardData("Title 4", "Content 4")
            };

            foreach (CardData cardData in cardDataList)
            {
                string title = "Title of the card";
                string content = "Content of the card";
                // Create the JSON content for the Adaptive Card
                var cardJson = $@"
                {{
                    ""$schema"": ""http://adaptivecards.io/schemas/adaptive-card.json"",
                    ""type"": ""AdaptiveCard"",
                    ""version"": ""1.0"",
                    ""body"": [
                        {{
                            ""type"": ""TextBlock"",
                            ""text"": ""{title}"",
                            ""weight"": ""bolder"",
                            ""size"": ""medium""
                        }},
                        {{
                            ""type"": ""TextBlock"",
                            ""text"": ""{content}"",
                            ""wrap"": true
                        }}
                    ]
                }}";

                // Parse JSON string into Adaptive Card object
                AdaptiveCard card = AdaptiveCard.FromJson(cardJson).Card;

                // Render Adaptive Card object in a WPF container
                FrameworkElement renderedCard = Render(card);

                // Add rendered card to StackPanel
                MainGrid.Children.Add(renderedCard);
                Trace.WriteLine("Card added");
            }
        }
        private FrameworkElement Render(AdaptiveCard card)
        {
            // Create a renderer
            var renderer = new AdaptiveCardRenderer();

            // Render the card
            var renderedCard = renderer.RenderCard(card);

            // Get the WPF container
            var container = renderedCard.FrameworkElement;

            // Set the container width to the desired width
            container.Width = MainGrid.Width;

            return container;
        }
        private class CardData
        {
            public string Title { get; set; }
            public string Content { get; set; }

            public CardData(string title, string content)
            {
                Title = title;
                Content = content;
            }
        }

    }
}
