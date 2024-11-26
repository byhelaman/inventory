using System.Drawing;

namespace inventory
{
    partial class Home
    {
        /// <summary>
        /// Variable del diseñador necesaria.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Limpiar los recursos que se estén usando.
        /// </summary>
        /// <param name="disposing">true si los recursos administrados se deben desechar; false en caso contrario.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Código generado por el Diseñador de Windows Forms

        /// <summary>
        /// Método necesario para admitir el Diseñador. No se puede modificar
        /// el contenido de este método con el editor de código.
        /// </summary>
        private void InitializeComponent()
        {
            this.TabControl = new System.Windows.Forms.TabControl();
            this.TabLogin = new System.Windows.Forms.TabPage();
            this.TabProducts = new System.Windows.Forms.TabPage();
            this.TabCategories = new System.Windows.Forms.TabPage();
            this.TabSuppliers = new System.Windows.Forms.TabPage();
            this.TabControl.SuspendLayout();
            this.SuspendLayout();
            // 
            // TabControl
            // 
            this.TabControl.Controls.Add(this.TabLogin);
            this.TabControl.Controls.Add(this.TabProducts);
            this.TabControl.Controls.Add(this.TabCategories);
            this.TabControl.Controls.Add(this.TabSuppliers);
            this.TabControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.TabControl.Font = new System.Drawing.Font("Verdana", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.TabControl.Location = new System.Drawing.Point(0, 0);
            this.TabControl.Name = "TabControl";
            this.TabControl.SelectedIndex = 0;
            this.TabControl.Size = new System.Drawing.Size(1208, 669);
            this.TabControl.TabIndex = 2;
            // 
            // TabLogin
            // 
            this.TabLogin.Location = new System.Drawing.Point(4, 25);
            this.TabLogin.Name = "TabLogin";
            this.TabLogin.Size = new System.Drawing.Size(1200, 640);
            this.TabLogin.TabIndex = 0;
            this.TabLogin.Text = "Iniciar Sesión";
            this.TabLogin.UseVisualStyleBackColor = true;
            // 
            // TabProducts
            // 
            this.TabProducts.Location = new System.Drawing.Point(4, 25);
            this.TabProducts.Name = "TabProducts";
            this.TabProducts.Size = new System.Drawing.Size(1200, 640);
            this.TabProducts.TabIndex = 1;
            this.TabProducts.Text = "Productos";
            this.TabProducts.UseVisualStyleBackColor = true;
            // 
            // TabCategories
            // 
            this.TabCategories.Location = new System.Drawing.Point(4, 25);
            this.TabCategories.Name = "TabCategories";
            this.TabCategories.Size = new System.Drawing.Size(1200, 640);
            this.TabCategories.TabIndex = 2;
            this.TabCategories.Text = "Categorias";
            this.TabCategories.UseVisualStyleBackColor = true;
            // 
            // TabSuppliers
            // 
            this.TabSuppliers.Location = new System.Drawing.Point(4, 25);
            this.TabSuppliers.Name = "TabSuppliers";
            this.TabSuppliers.Size = new System.Drawing.Size(1200, 640);
            this.TabSuppliers.TabIndex = 3;
            this.TabSuppliers.Text = "Proveedores";
            this.TabSuppliers.UseVisualStyleBackColor = true;
            // 
            // Home
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1208, 669);
            this.Controls.Add(this.TabControl);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.Name = "Home";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Form1";
            this.TabControl.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.TabControl TabControl;
        private System.Windows.Forms.TabPage TabLogin;
        private System.Windows.Forms.TabPage TabProducts;
        private System.Windows.Forms.TabPage TabCategories;
        private System.Windows.Forms.TabPage TabSuppliers;
    }
}

