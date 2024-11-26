using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace inventory
{
    public partial class Products : Form
    {
        public Products()
        {
            InitializeComponent();

            //Connect.CheckConnection();
            //DataTable dt = Connect.ReadRecords("products");

            string query = @"
                SELECT p.product_id, p.name, p.description, p.price, p.stock, p.date_added, p.category_id, c.name AS category
                FROM products p
                JOIN categories c ON p.category_id = c.category_id;
            ";

            DataTable dt = Connect.ExecuteQuery(query, null);
            DgvProducts.DataSource = dt;

            ConfigureGridColumns();
        }

        private void ConfigureGridColumns()
        {
            DgvProducts.Columns["name"].HeaderText = "Nombre del Producto";
            DgvProducts.Columns["description"].HeaderText = "Descripción";
            DgvProducts.Columns["category"].HeaderText = "Categoría";
            DgvProducts.Columns["price"].HeaderText = "Precio";
            DgvProducts.Columns["stock"].HeaderText = "Stock Inicial";
            DgvProducts.Columns["date_added"].HeaderText = "Fecha de Ingreso";

            DgvProducts.Columns["product_id"].Visible = false;
            DgvProducts.Columns["category_id"].Visible = false;

            DgvProducts.Columns["category"].DisplayIndex = 3;

            DgvProducts.CellFormatting += FormatDateAddedColumn;
            LoadCategories();
        }

        private void FormatDateAddedColumn(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (DgvProducts.Columns[e.ColumnIndex].Name == "date_added" && e.Value != null && e.Value is DateTime dateValue)
            {
                e.Value = dateValue.ToString("dd/MM/yyyy");
                e.FormattingApplied = true;
            }
        }

        private void DgvProducts_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            // Verificamos que la fila seleccionada sea válida (no es un encabezado)
            if (e.RowIndex >= 0)
            {
                DataGridViewRow selectedRow = DgvProducts.Rows[e.RowIndex];

                TxtProductName.Text = selectedRow.Cells["name"].Value?.ToString() ?? string.Empty;
                TxtProductDescription.Text = selectedRow.Cells["description"].Value?.ToString() ?? string.Empty;
                TxtProductPrice.Text = selectedRow.Cells["price"].Value?.ToString() ?? string.Empty;

                int categoryId = selectedRow.Cells["category_id"].Value != DBNull.Value
                ? Convert.ToInt32(selectedRow.Cells["category_id"].Value)
                : 0;

                CmbCategory.SelectedItem = CmbCategory.Items.Cast<Category>()
                    .FirstOrDefault(item => item.Value == categoryId);
            }
        }

        private void BtnUpdate_Click(object sender, EventArgs e)
        {

            int productId = Convert.ToInt32(DgvProducts.SelectedRows[0].Cells["product_id"].Value);
            int categoryId = CmbCategory.SelectedItem is Category category ? category.Value : 0;

            if (categoryId == 0)
            {
                MessageBox.Show("Por favor, seleccione una categoría válida.");
                return;
            }

            Dictionary<string, object> values = new Dictionary<string, object>
            {
                { "name", TxtProductName.Text },
                { "description", TxtProductDescription.Text },
                { "price", Convert.ToDecimal(TxtProductPrice.Text) },
                { "category_id", categoryId },

            };

            string conditions = "product_id = @productId";

            SqlParameter[] parameters = new SqlParameter[]
            {
                new SqlParameter("@productId", productId)
            };


            // Ejecutar la actualización
            int rowsAffected = Connect.UpdateRecord("products", values, conditions, parameters);

            if (rowsAffected > 0)
            {
                MessageBox.Show("Producto actualizado correctamente.");


                //DataTable dt = Connect.ReadRecords("products");
                //DgvProducts.DataSource = dt;

                string query = @"
                    SELECT p.product_id, p.name, p.description, p.price, p.stock, p.date_added, p.category_id, c.name AS category
                    FROM products p
                    JOIN categories c ON p.category_id = c.category_id;
                ";

                DataTable dt = Connect.ExecuteQuery(query, null);
                DgvProducts.DataSource = dt;
            }
            else
            {
                MessageBox.Show("No se pudo actualizar el producto.");
            }

            ClearAllTextBoxes();
        }

        private void BtnCreate_Click(object sender, EventArgs e)
        {
            // Verificar que todos los campos requeridos están completos
            if (string.IsNullOrEmpty(TxtProductName.Text) ||
                string.IsNullOrEmpty(TxtProductDescription.Text) ||
                string.IsNullOrEmpty(TxtProductPrice.Text))
            {
                MessageBox.Show("Por favor, complete todos los campos.");
                return;
            }

            int categoryId = CmbCategory.SelectedItem is Category category ? category.Value : 0;

            if (categoryId == 0)
            {
                MessageBox.Show("Por favor, seleccione una categoría válida.");
                return;
            }

            Dictionary<string, object> values = new Dictionary<string, object>
            {
                { "name", TxtProductName.Text },
                { "description", TxtProductDescription.Text },
                { "price", Convert.ToDecimal(TxtProductPrice.Text) },
                { "category_id", categoryId }
            };

            // Insertar el nuevo producto en la base de datos
            int rowsAffected = Connect.CreateRecord("products", values);

            // Verificar si la inserción fue exitosa
            if (rowsAffected > 0)
            {
                MessageBox.Show("Producto creado correctamente.");

                // Actualizar el DataGridView con los nuevos datos
                //DataTable dt = Connect.ReadRecords("products");
                //DgvProducts.DataSource = dt;

                string query = @"
                    SELECT p.product_id, p.name, p.description, p.price, p.stock, p.date_added, p.category_id, c.name AS category
                    FROM products p
                    JOIN categories c ON p.category_id = c.category_id;
                ";

                DataTable dt = Connect.ExecuteQuery(query, null);
                DgvProducts.DataSource = dt;
            }
            else
            {
                MessageBox.Show("No se pudo crear el producto.");
            }

            ClearAllTextBoxes();
        }

        private void BtnDelete_Click(object sender, EventArgs e)
        {
            if (DgvProducts.SelectedRows.Count > 0)
            {
                int productId = Convert.ToInt32(DgvProducts.SelectedRows[0].Cells["product_id"].Value);

                var result = MessageBox.Show("¿Estás seguro de que deseas eliminar este producto?", "Confirmación de Eliminación", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

                if (result == DialogResult.Yes)
                {
                    SqlParameter[] parameters = new SqlParameter[]
                    {
                        new SqlParameter("@productId", productId)
                    };

                    int rowsAffected = Connect.DeleteRecord("products", "product_id = @productId", parameters);

                    // Verificar si la eliminación fue exitosa
                    if (rowsAffected > 0)
                    {
                        MessageBox.Show("Producto eliminado correctamente.");

                        string query = @"
                            SELECT p.product_id, p.name, p.description, p.price, p.stock, p.date_added, p.category_id, c.name AS category
                            FROM products p
                            JOIN categories c ON p.category_id = c.category_id;
                        ";

                        DataTable dt = Connect.ExecuteQuery(query, null);
                        DgvProducts.DataSource = dt;
                    }
                    else
                    {
                        MessageBox.Show("No se pudo eliminar el producto.");
                    }
                }
            }
            else
            {
                MessageBox.Show("Por favor, seleccione un producto para eliminar.");
            }

            // Limpiar los controles de texto (si es necesario)
            ClearAllTextBoxes();
        }

        private void LoadCategories()
        {
            DataTable dt = Connect.ReadRecords("categories");

            CmbCategory.Items.Clear();
            CmbCategory.Items.Add(new Category { Text = "Seleccionar...", Value = 0 });

            foreach (DataRow row in dt.Rows)
            {
                CmbCategory.Items.Add(new Category
                {
                    Value = Convert.ToInt32(row["category_id"]),
                    Text = row["name"].ToString()
                });
            }

            CmbCategory.DisplayMember = "Text";
            CmbCategory.ValueMember = "Value";
            CmbCategory.SelectedIndex = 0;
        }

        private void ClearAllTextBoxes()
        {
            foreach (Control control in this.Controls)
            {
                if (control is TextBox box)
                {
                    box.Clear();
                }

                if (control is ComboBox comboBox)
                {
                    comboBox.SelectedIndex = 0;
                }
            }
        }

        private void CmbCategory_MouseDown(object sender, MouseEventArgs e)
        {
            LoadCategories();
        }
    }

    public class Category
    {
        public int Value { get; set; }
        public string Text { get; set; }
    }
}