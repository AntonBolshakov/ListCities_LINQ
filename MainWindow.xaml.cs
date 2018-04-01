using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.IO;
using Cities;

namespace AppWithLINQ
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private RowDefinition[] ListCityRows;
        private ColumnDefinition[] ListCityColumn;
        private TextBox[,] Text;
        private ScrollViewer Scroll;

        private List<ListCities> Cities;
        private int Count;
        private City city;

        public MainWindow()
        {
            InitializeComponent();
            
            city = new City();

            string PathToFile = System.IO.Path.GetFullPath("CitiesInfo.xml");
            
            city.OutXML(PathToFile);
            StartValue();

            CreateTableForCities();
        }

        public void StartValue()
        {
            Count = city.GetCount;
            Cities = new List<ListCities>();
            Cities = city.GetListCities;
        }

        public void CreateTableForCities()
        {
            CityList.RowDefinitions.Clear();
            CityList.Children.Clear();

            int i, j;
            ListCityRows = new RowDefinition[Count+1];
            Text = new TextBox[Count + 1, 5];

            for (i = 0; i <= Count; i++)
            {
                ListCityRows[i] = new RowDefinition();
                ListCityRows[i].Height = new GridLength(30);
                CityList.RowDefinitions.Add(ListCityRows[i]);

                for (j = 0; j < 5; j++)
                {
                    Text[i, j] = new TextBox();
                    Text[i, j].Style = (Style)this.Resources["TextBox1"];
                    CityList.Children.Add(Text[i, j]);
                    Grid.SetColumn(Text[i, j], j);
                    Grid.SetRow(Text[i, j], i);
                }

                if (i > 0)
                {
                    for (j = 0; j < 5; j++)
                    {
                        switch (j)
                        {
                            case 0:
                                Text[i, j].Text = Convert.ToString(Cities[i - 1].Id);
                                break;
                            case 1:
                                Text[i, j].Text = Convert.ToString(Cities[i - 1].Name);
                                break;
                            case 2:
                                Text[i, j].Text = Convert.ToString(Cities[i - 1].CountryCode);
                                break;
                            case 3:
                                Text[i, j].Text = Convert.ToString(Cities[i - 1].District);
                                break;
                            case 4:
                                Text[i, j].Text = Convert.ToString(Cities[i - 1].Population);
                                break;
                            default:
                                break;
                        }
                    }
                }
                else
                {
                    Text[0, 0].Text = "Id";
                    Text[0, 0].Background = new SolidColorBrush(Color.FromArgb(15, 70, 7, 60));
                    Text[0, 1].Text = "Name";
                    Text[0, 1].Background = new SolidColorBrush(Color.FromArgb(15, 70, 7, 60));
                    Text[0, 2].Text = "Country code";
                    Text[0, 2].Background = new SolidColorBrush(Color.FromArgb(15, 70, 7, 60));
                    Text[0, 3].Text = "District";
                    Text[0, 3].Background = new SolidColorBrush(Color.FromArgb(15, 70, 7, 60));
                    Text[0, 4].Text = "Population";
                    Text[0, 4].Background = new SolidColorBrush(Color.FromArgb(15, 70, 7, 60));
                }
            }

            CityList.UpdateLayout();
        }

        public void Filter_CountryCode(string _value)
        {
            var Query = from x in Cities where x.CountryCode == _value select x;
            var QueryToList = Query.ToList();

            Cities = new List<ListCities>();
            Cities = QueryToList;
            Count = Cities.Count;
        }

        public void Filter_NameAndDistrict(string _value)
        {
            if (_value == "Similar")
            {
                var Query = from x in Cities where x.Name == x.District select x; 
                var QueryToList = Query.ToList();
                Cities = new List<ListCities>();
                Cities = QueryToList;
            }
            else
            {
                var Query = from x in Cities where x.Name != x.District select x;
                var QueryToList = Query.ToList();
                Cities = new List<ListCities>();
                Cities = QueryToList;
            }

            Count = Cities.Count;
        }

        public void Filter_District(string _value)
        {
            if (_value == "Similar")
            {
                var ListDistrict = Cities.GroupBy(x => x.District).Where(x => x.Count() > 1).Select(x => x.Key).ToList();
                var Query = from x in Cities join y in ListDistrict on x.District equals y select x;
                var QueryToList = Query.ToList();
                Cities = new List<ListCities>();
                Cities = QueryToList;
            }
            else
            {
                var ListDistrict = Cities.GroupBy(x => x.District).Where(x => x.Count() == 1).Select(x => x.Key).ToList();
                var Query = from x in Cities join y in ListDistrict on x.District equals y select x;
                var QueryToList = Query.ToList();
                Cities = new List<ListCities>();
                Cities = QueryToList;
            }

            Count = Cities.Count;
        }

        public void Filter_Population(string _operation, int _value)
        {
            if (_operation == "> or =")
            {
                var Query = from x in Cities where x.Population >= _value select x;
                var QueryToList = Query.ToList();
                Cities = new List<ListCities>();
                Cities = QueryToList;
            }
            else
            {
                var Query = from x in Cities where x.Population <= _value select x;
                var QueryToList = Query.ToList();
                Cities = new List<ListCities>();
                Cities = QueryToList;
            }

            Count = Cities.Count;
        }

        public void OrderBy()
        {
            switch (OrderByItem.Text)
            {
                case "Id":
                    var OrderById = from x in Cities orderby x.Id select x;
                    var OrderById_ToList = OrderById.ToList();
                    Cities = new List<ListCities>();
                    Cities = OrderById_ToList;
                    break;
                case "Name":
                    var OrderByName = from x in Cities orderby x.Name select x;
                    var OrderByName_ToList = OrderByName.ToList();
                    Cities = new List<ListCities>();
                    Cities = OrderByName_ToList;
                    break;
                case "Country code":
                    var OrderByCountryCode = from x in Cities orderby x.CountryCode select x;
                    var OrderByCountryCode_ToList = OrderByCountryCode.ToList();
                    Cities = new List<ListCities>();
                    Cities = OrderByCountryCode_ToList;
                    break;
                case "District":
                    var OrderByDistrict = from x in Cities orderby x.District select x;
                    var OrderByDistrict_ToList = OrderByDistrict.ToList();
                    Cities = new List<ListCities>();
                    Cities = OrderByDistrict_ToList;
                    break;
                case "Population":
                    var OrderByPopulation = from x in Cities orderby x.Population select x;
                    var OrderByPopulation_ToList = OrderByPopulation.ToList();
                    Cities = new List<ListCities>();
                    Cities = OrderByPopulation_ToList;
                    break;
                default:
                    break;
            }
        }

        private void CountPopulation_Click(object sender, RoutedEventArgs e)
        {
            var QueryPopulation = Cities.Select(x => x.Population).ToList();
            var Query = QueryPopulation.Aggregate(0, (acc, i) => acc + i);
            MessageBox.Show("Total number of people in cities that meet the requirements: " + Convert.ToString(Query), "Count population", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void ApplyFilter_Click(object sender, RoutedEventArgs e)
        {
            StartValue();
            CreateTableForCities();

            if (ComboBoxDistrict.Text != "")
            {
                Filter_District(ComboBoxDistrict.Text);
            }

            if (ComboBoxCountryCode.Text != "")
            {
                Filter_CountryCode(ComboBoxCountryCode.Text);
            }

            if (ComboBoxPopulation.Text != "" && ValuePopulation.Text != "")
            {
                Filter_Population(ComboBoxPopulation.Text, Convert.ToInt32(ValuePopulation.Text));
            }

            if (ComboBoxName_District.Text != "")
            {
                Filter_NameAndDistrict(ComboBoxName_District.Text);
            }

            OrderBy();

            CreateTableForCities();
        }

        private void TextInput_IntegerNumber(object sender, TextCompositionEventArgs e)
        {
            if (!Char.IsDigit(e.Text, 0)) e.Handled = true;
        }


        private void AboutProgram_Click(object sender, RoutedEventArgs e)
        {
            string Header = "About program";
            string Content = "This application filters cities, according to the specified criteria.";
            MessageBox.Show(Content, Header, MessageBoxButton.OK, MessageBoxImage.Information);
            
        }

        private void AboutAuthor_Click(object sender, RoutedEventArgs e)
        {
            string Header = "About author";
            string Content = "Application author: Bolshakov Anton Alexandrovich";
            MessageBox.Show(Content, Header, MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void ExitToProgram_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
