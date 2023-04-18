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
using System.Net.Http;
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
            //this.WindowState = WindowState.Maximized;
            this.ResizeMode = ResizeMode.NoResize;
            getNotes(stackPanel);
        }

        static async void getNotes(StackPanel stackPanel)
        {

            // Create an HTTP client object
            using (HttpClient client = new HttpClient())
            {
                client.BaseAddress = new Uri("http://localhost:3000");
                client.DefaultRequestHeaders.Add("Authorization", "Bearer "+Token.accessToken);
                var response = await client.GetAsync("/notes");

                // If the response was successful, return the success message
                if (response.IsSuccessStatusCode)
                {
                    string json = await response.Content.ReadAsStringAsync();
                    dynamic data = JsonConvert.DeserializeObject(json);

                    // Define the stack panel that will contain the cards
                    stackPanel.Margin = new Thickness(10);;

                    // Assume that the API data is an array of objects with "name" property
                    foreach (dynamic item in data)
                    {

                        // Define the card data
                        string noteId = item.noteId;
                        string title = item.title;
                        string content = item.content;

                        // Create the card
                        Border card = new Border();
                        card.BorderBrush = System.Windows.Media.Brushes.Black;
                        card.BorderThickness = new Thickness(1);
                        card.Margin = new Thickness(5);
                        card.Padding = new Thickness(15);

                        Grid gridCard = new Grid();
                        gridCard.RowDefinitions.Add(new RowDefinition() { Height = GridLength.Auto });
                        gridCard.RowDefinitions.Add(new RowDefinition() { Height = new GridLength(1, GridUnitType.Star) });
                        gridCard.RowDefinitions.Add(new RowDefinition() { Height = GridLength.Auto });

                        TextBlock titleCard = new TextBlock();
                        titleCard.Text = title;
                        titleCard.FontSize = 20;
                        titleCard.FontWeight = System.Windows.FontWeights.Bold;
                        titleCard.Margin = new Thickness(5);
                        titleCard.TextWrapping = TextWrapping.Wrap;
                        Grid.SetRow(titleCard, 0);
                        Grid.SetColumn(titleCard, 1);

                        TextBlock contentCard = new TextBlock();
                        contentCard.Text = content;
                        contentCard.FontSize = 12;
                        contentCard.Margin = new Thickness(5);
                        contentCard.TextWrapping = TextWrapping.Wrap;
                        Grid.SetRow(contentCard, 1);
                        Grid.SetColumn(contentCard, 1);

                        // Bind the TextBlocks' Width property to the parent element's ActualWidth
                        titleCard.SetBinding(TextBlock.WidthProperty, new Binding("ActualWidth") { Source = card });
                        contentCard.SetBinding(TextBlock.WidthProperty, new Binding("ActualWidth") { Source = card });

                        // Wrap the TextBlocks in a Viewbox to make them responsive
                        Viewbox titleViewbox = new Viewbox();
                        titleViewbox.Child = titleCard;

                        Viewbox contentViewbox = new Viewbox();
                        contentViewbox.Child = contentCard;

                        // Add the Viewboxes to the grid
                        Grid.SetRow(titleViewbox, 0);
                        Grid.SetColumn(titleViewbox, 1);
                        gridCard.Children.Add(titleViewbox);

                        Grid.SetRow(contentViewbox, 1);
                        Grid.SetColumn(contentViewbox, 1);
                        gridCard.Children.Add(contentViewbox);

                        // Create a StackPanel for the buttons
                        StackPanel buttonPanel = new StackPanel();
                        buttonPanel.Orientation = Orientation.Horizontal;
                        buttonPanel.Margin = new Thickness(5);
                        Grid.SetRow(buttonPanel, 2);
                        Grid.SetColumn(buttonPanel, 1);

                        // Create the buttons and add them to the StackPanel
                        Button editButton = new Button();
                        editButton.Content = "Edit";
                        editButton.Margin = new Thickness(5);
                        buttonPanel.Children.Add(editButton);

                        Button deleteButton = new Button();
                        deleteButton.Content = "Delete";
                        deleteButton.Margin = new Thickness(5);
                        buttonPanel.Children.Add(deleteButton);

                        gridCard.Children.Add(buttonPanel);

                        card.Child = gridCard;
                        stackPanel.Children.Add(card);

                        Trace.WriteLine("Card added");
                    }
                }

                else
                {
                    // If the response was not successful, throw an exception with the error message
                    var error = await response.Content.ReadAsStringAsync();
                    throw new Exception(error);
                }
            }
        }

    }
}
