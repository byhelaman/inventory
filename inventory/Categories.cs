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
    public partial class Categories : Form
    {
        public Categories()
        {
            InitializeComponent();

            //Connect.CheckConnection();

            DataTable dt = Connect.ReadRecords("categories");
            DgvCategories.DataSource = dt;

            ConfigureGridColumns();
        }

        private void ConfigureGridColumns()
        {
            DgvCategories.Columns["name"].HeaderText = "Nombre del Producto";
            DgvCategories.Columns["description"].HeaderText = "Descripción";

            DgvCategories.Columns["category_id"].Visible = false;
        }

        private void DgvCategories_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            // Verificamos que la fila seleccionada sea válida (no es un encabezado)
            if (e.RowIndex >= 0)
            {
                DataGridViewRow selectedRow = DgvCategories.Rows[e.RowIndex];

                TxtCategoryName.Text = selectedRow.Cells["name"].Value?.ToString() ?? string.Empty;
                TxtCategoryDescription.Text = selectedRow.Cells["description"].Value?.ToString() ?? string.Empty;
            }

        }

        private void BtnCreate_Click(object sender, EventArgs e)
        {
            // Verificar que todos los campos requeridos están completos
            if (string.IsNullOrEmpty(TxtCategoryName.Text) ||
                string.IsNullOrEmpty(TxtCategoryDescription.Text))
            {
                MessageBox.Show("Por favor, complete todos los campos.");
                return;
            }

            Dictionary<string, object> values = new Dictionary<string, object>
            {
                { "name", TxtCategoryName.Text },
                { "description", TxtCategoryDescription.Text },
            };

            // Insertar el nuevo producto en la base de datos
            int rowsAffected = Connect.CreateRecord("categories", values);

            // Verificar si la inserción fue exitosa
            if (rowsAffected > 0)
            {
                MessageBox.Show("Producto creado correctamente.");

                DataTable dt = Connect.ReadRecords("categories");
                DgvCategories.DataSource = dt;
            }
            else
            {
                MessageBox.Show("No se pudo crear el producto.");
            }

            ClearAllTextBoxes();
        }

        private void BtnUpdate_Click(object sender, EventArgs e)
        {

            int categoryId = Convert.ToInt32(DgvCategories.SelectedRows[0].Cells["category_id"].Value);

            Dictionary<string, object> values = new Dictionary<string, object>
            {
                { "name", TxtCategoryName.Text },
                { "description", TxtCategoryDescription.Text }

            };

            string conditions = "category_id = @categoryId";

            SqlParameter[] parameters = new SqlParameter[]
            {
                new SqlParameter("@categoryId", categoryId)
            };


            // Ejecutar la actualización
            int rowsAffected = Connect.UpdateRecord("categories", values, conditions, parameters);

            if (rowsAffected > 0)
            {
                MessageBox.Show("Producto actualizado correctamente.");


                DataTable dt = Connect.ReadRecords("categories");
                DgvCategories.DataSource = dt;
            }
            else
            {
                MessageBox.Show("No se pudo actualizar el producto.");
            }

            ClearAllTextBoxes();
        }

        private void BtnDelete_Click(object sender, EventArgs e)
        {
            if (DgvCategories.SelectedRows.Count > 0)
            {
                int categoryId = Convert.ToInt32(DgvCategories.SelectedRows[0].Cells["category_id"].Value);

                var result = MessageBox.Show("¿Estás seguro de que deseas eliminar esta categoría?", "Confirmación de Eliminación", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

                if (result == DialogResult.Yes)
                {
                    SqlParameter[] parameters = new SqlParameter[]
                    {
                        new SqlParameter("@categoryId", categoryId)
                    };

                    int rowsAffected = Connect.DeleteRecord("categories", "category_id = @categoryId", parameters);

                    // Verificar si la eliminación fue exitosa
                    if (rowsAffected > 0)
                    {
                        MessageBox.Show("Producto eliminado correctamente.");

                        DataTable dt = Connect.ReadRecords("categories");
                        DgvCategories.DataSource = dt;
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

            ClearAllTextBoxes();
        }

        private void ClearAllTextBoxes()
        {
            foreach (Control control in this.Controls)
            {
                if (control is TextBox box)
                {
                    box.Clear();
                }
            }
        }

    }
}
