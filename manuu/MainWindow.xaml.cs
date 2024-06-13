using System;
using System.Collections.Generic;
using System.Data.SqlClient;
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

namespace manuu
{
    public partial class MainWindow : Window
    {
        // Строка подключения к базе данных
        private string connectionString = "data source=huaweii;initial catalog=master;trusted_connection=true";
        private object productErrorLabel;
        private object productUnitsTextBox;
        private readonly object productNameTextBox;




        // Списки для хранения данных
        public List<Dish> Dishes { get; set; }
        public List<Ingredient> Ingredients { get; set; }
        public List<Receptura> Recipes { get; set; }

        public MainWindow(Dolj authUser)
        {
            InitializeComponent();
           

            // Загрузка данных при запуске
            LoadDishesData();
            LoadIngredientsData();

            productNameTextBox = (TextBox)this.FindName("productNameTextBox");
            productUnitsTextBox = (TextBox)this.FindName("productUnitsTextBox");

        }
        private void MainFrame_ContentRendering(object sender, EventArgs e)
        {
         
        }

        public MainWindow()
        {
        }





        // Метод для загрузки данных о блюдах
        private void LoadDishesData()
        {
            Dishes = new List<Dish>();
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string query = "SELECT * FROM Bludo";
                SqlCommand command = new SqlCommand(query, connection);
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    Dishes.Add(new Dish
                    {
                        Id = (int)reader["id_bludo"],
                        NameBludo = reader["name_bludo"].ToString(),
                        VesPorc = (int)reader["ves_porc"],
                        Kalariynost = (int)reader["kalariynost"],
                        Price = (int)reader["price"]
                    });
                }
                connection.Close();
            }
        }

        // Метод для загрузки данных об ингредиентах
        private void LoadIngredientsData()
        {
            Ingredients = new List<Ingredient>();
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string query = "SELECT * FROM Product";
                SqlCommand command = new SqlCommand(query, connection);
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    Ingredients.Add(new Ingredient
                    {
                        Id = (int)reader["id_product"],
                        NameProduct = reader["name_product"].ToString(),
                        UnitsOfMeasurement = reader["units_of_measurement"].ToString()
                    });
                }
                connection.Close();
            }

            // Обновление ListBox для ингредиентов
            ingredientsListBox.ItemsSource = Ingredients;
        }

        // Обработчик события для сохранения блюда
        private void SaveDishButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string nameBludo = dishNameTextBox.Text;
                int vesPorc = int.Parse(dishWeightTextBox.Text);
                int kalariynost = int.Parse(dishCaloriesTextBox.Text);
                int price = int.Parse(dishPriceTextBox.Text);

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    string query = "INSERT INTO Bludo (name_bludo, ves_porc, kalariynost, price) VALUES (@nameBludo, @vesPorc, @kalariynost, @price)";
                    SqlCommand command = new SqlCommand(query, connection);
                    command.Parameters.AddWithValue("@nameBludo", nameBludo);
                    command.Parameters.AddWithValue("@vesPorc", vesPorc);
                    command.Parameters.AddWithValue("@kalariynost", kalariynost);
                    command.Parameters.AddWithValue("@price", price);
                    connection.Open();
                    command.ExecuteNonQuery();
                    connection.Close();
                }

                // Обновление данных в ListBox
                LoadDishesData();

                // Очистка текстовых полей
                dishNameTextBox.Clear();
                dishWeightTextBox.Clear();
                dishCaloriesTextBox.Clear();
                dishPriceTextBox.Clear();
            }
            catch (Exception ex)
            {
                dishErrorLabel.Content = ex.Message;
            }
        }

        // Обработчик события для сохранения продукта
        private void SaveIngredientButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (sender is Button button)
                {
                    // Получение текста из textbox-ов
                    string nameProduct = ((TextBox)productNameTextBox).Text;
                    string unitsOfMeasurement = ((TextBox)productUnitsTextBox).Text;
                    // Работа с базой данных
                    using (SqlConnection connection = new SqlConnection(connectionString))
                    {
                        string query = "INSERT INTO Product (name_product, units_of_measurement) VALUES (@nameProduct, @unitsOfMeasurement)";
                        SqlCommand command = new SqlCommand(query, connection);
                        command.Parameters.AddWithValue("@nameProduct", nameProduct);
                        command.Parameters.AddWithValue("@unitsOfMeasurement", unitsOfMeasurement);
                        connection.Open();
                        command.ExecuteNonQuery();
                        connection.Close();
                    }

                    LoadIngredientsData();

                    // Find the parent control and child elements
                    Button saveButton = (Button)sender;
                    Control parentControl = (Control)saveButton.Parent;
                    TextBox nameTextBox = (TextBox)parentControl.FindName("productNameTextBox");
                    TextBox unitsTextBox = (TextBox)parentControl.FindName("productUnitsTextBox");
                    Label errorLabel = (Label)parentControl.FindName("productErrorLabel");

                    // Clear the text boxes and update the error label
                    nameTextBox.Clear();
                    unitsTextBox.Clear();
                    errorLabel.Content = ""; // Clear any previous error message
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

            // Обработчик события для добавления ингредиента в рецепт

        }
        private void AddIngredientButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (ingrediensListBox.SelectedItem != null)
                {

                    Ingredient selectedIngredient = (Ingredient)ingrediensListBox.SelectedItem;
                    int quantity = int.Parse(quantityTextBox.Text);
                    string sposobObrab = processingMethodTextBox.Text;

                    using (SqlConnection connection = new SqlConnection(connectionString))
                    {
                        string query = "INSERT INTO Receptura (id_bludo, id_product, kolichestvo, sposob_obrab) VALUES (@idBludo, @idProduct, @kolichestvo, @sposobObrab)";
                        SqlCommand command = new SqlCommand(query, connection);

                        command.Parameters.AddWithValue("@idProduct", selectedIngredient.Id);
                        command.Parameters.AddWithValue("@kolichestvo", quantity);
                        command.Parameters.AddWithValue("@sposobObrab", sposobObrab);
                        connection.Open();
                        command.ExecuteNonQuery();
                        connection.Close();
                    }



                    // Очистка текстовых полей
                    quantityTextBox.Clear();
                    processingMethodTextBox.Clear();
                }
                else
                {
                    MessageBox.Show("Выберите продукт, способ обработки и количество!");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void SearchButton_Click(object sender, RoutedEventArgs e)
        {

        }

        // Классы для данных
        public class Dish
        {
            public int Id { get; set; }
            public string NameBludo { get; set; }
            public int VesPorc { get; set; }
            public int Kalariynost { get; set; }
            public int Price { get; set; }
        }

        public class Ingredient
        {
            public int Id { get; set; }
            public string NameProduct { get; set; }
            public string UnitsOfMeasurement { get; set; }
        }

        public partial class Receptura
        {
            public int Id { get; set; }
            public int IdBludo { get; set; }
            public int IdProduct { get; set; }
            public int Kolichestvo { get; set; }
            public string SposobObrab { get; set; }
        }

        private void AnalyzeRecipesButton_Click(object sender, RoutedEventArgs e)
        {

        }

        public static implicit operator MainWindow(Auth v)
        {
            throw new NotImplementedException();
        }

        private void Auth_Click(object sender, RoutedEventArgs e)
        {
            Manager.MainFrame.Navigate(new Auth());
        }
    }
}
