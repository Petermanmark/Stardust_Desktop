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
                new CardData("Title 1", "Content 1"),
                new CardData("Title 2", "Content 2"),
                new CardData("Title 3", "Content 3"),
                new CardData("Title 1", "Content 1"),
                new CardData("Title 2", "Content 2"),
                new CardData("Title 3", "Content 3"),
                new CardData("Title 1", "Content 1"),
                new CardData("Title 2", "Content 2"),
                new CardData("Title 3", "Content 3"),
                new CardData("Title 1", "Content 1"),
                new CardData("Title 2", "Content 2"),
                new CardData("Title 3", "Content 3"),
                new CardData("Title 1", "Content 1"),
                new CardData("Title 2", "Content 2"),
                new CardData("Title 3", "Content 3"),
                new CardData("Title 4", "Content 4")
            };

            foreach (CardData cardData in cardDataList)
            {
                string title = cardData.Title;
                string content = cardData.Content;

                // Create a StackPanel control with vertical orientation
                stackPanel.Orientation = Orientation.Vertical;

                // Create the first card
                Border card = new Border();
                card.BorderBrush = System.Windows.Media.Brushes.Black;
                card.BorderThickness = new Thickness(1);
                card.Margin = new Thickness(5);

                Grid gridCard = new Grid();
                gridCard.RowDefinitions.Add(new RowDefinition() { Height = GridLength.Auto });
                gridCard.RowDefinitions.Add(new RowDefinition() { Height = new GridLength(1, GridUnitType.Star) });

                TextBlock titleCard = new TextBlock();
                titleCard.Text = title;
                titleCard.FontSize = 18;
                titleCard.FontWeight = System.Windows.FontWeights.Bold;
                titleCard.Margin = new Thickness(5);
                Grid.SetRow(titleCard, 0);
                Grid.SetColumn(titleCard, 1);
                gridCard.Children.Add(titleCard);

                TextBlock contentCard = new TextBlock();
                contentCard.Text = content;
                contentCard.Margin = new Thickness(5);
                Grid.SetRow(contentCard, 1);
                Grid.SetColumn(contentCard, 1);
                gridCard.Children.Add(contentCard);

                card.Child = gridCard;
                stackPanel.Children.Add(card);

                Trace.WriteLine("Card added");
            }
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
