using System.Data.Entity;
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
using WpfApppliction.db;
using System.Data;
using System.Text.RegularExpressions;

namespace WpfApppliction
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private InventoryDBContext dbContext;

        public MainWindow()
        {
            try 
            { 
            InitializeComponent();
            dbContext = new InventoryDBContext(); // Ensure this is properly initialized
            LoadData();
        
            }
            catch (Exception ex) 
            {
                MessageBox.Show($"Error: {ex.Message}\n{ex.StackTrace}"); }
            
        }

        // Load all items
        private void LoadData()
        {
            dataGridItems.ItemsSource = dbContext.SearchItems("");
        }

        // Add new item
        private void AddItem_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                // Create a new item object
                Item newItem = new Item
                {
                    ItemName = txtItemName.Text,
                    UnitPrice = decimal.TryParse(txtUnitPrice.Text, out decimal unitPrice) ? unitPrice : 0,
                    CasePerUnit = int.TryParse(txtCasePerUnit.Text, out int casePerUnit) ? casePerUnit : 0,
                    Supplier = txtSupplier.Text,
                    Date = dpDate.SelectedDate // Date is nullable
                };

                // Validate the item
                if (!newItem.Validate(out string errorMessage))
                {
                    MessageBox.Show(errorMessage, "Validation Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                // Call AddItem method to insert new item into database
                dbContext.AddItem(newItem);

                // Reload the data to reflect the new item
                LoadData();

                // Clear input fields
                txtItemName.Clear();
                txtUnitPrice.Clear();
                txtCasePerUnit.Clear();
                txtSupplier.Clear();
                dpDate.SelectedDate = null;

                MessageBox.Show("Item added successfully!");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}\n{ex.StackTrace}");
            }
        }

        // Search items
        private void SearchItem_Click(object sender, RoutedEventArgs e)
        {
            string searchTerm = txtItemName.Text;
            dataGridItems.ItemsSource = dbContext.SearchItems(searchTerm);
        }

        // Update item
        private void UpdateItem_Click(object sender, RoutedEventArgs e)
        {

            if (dataGridItems.SelectedItem is Item selectedItem)
            {
                try {
                    selectedItem.ItemName = txtItemName.Text;
                    selectedItem.UnitPrice = Convert.ToDecimal(txtUnitPrice.Text);
                    selectedItem.CasePerUnit = Convert.ToInt32(txtCasePerUnit.Text);
                    selectedItem.Supplier = txtSupplier.Text;
                    selectedItem.Date = dpDate.SelectedDate ?? DateTime.Now;


                    dbContext.UpdateItem(selectedItem);
                    LoadData();

                    MessageBox.Show("Item updated successfully!");
                   }
                catch (Exception ex)
                {
                    //MessageBox.Show($"Error: {ex.Message}\n{ex.StackTrace}");
                }

            }
            else
            {
                MessageBox.Show("Please select an item to update.");
            }


        }

        // Delete item
        private void DeleteItem_Click(object sender, RoutedEventArgs e)
        {
            if (dataGridItems.SelectedItem is Item selectedItem)
            {
                dbContext.DeleteItem(selectedItem.ItemID);
                LoadData();
            }
        }
    }
}