using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using inventory.Properties;

namespace inventory
{
    public partial class Home : Form
    {

        public Home()
        {
            InitializeComponent();
            InitializeLoginForm();
        }

        private void InitializeLoginForm()
        {
            Login login = new Login();
            Functions.ShowForm(TabLogin, login);

            HideTabsForLogin();

            login.FormClosed += OnLoginFormClosed;
        }

        private void OnLoginFormClosed(object sender, FormClosedEventArgs e)
        {
            TabControl.TabPages.Remove(TabLogin);
            ShowTabsAfterLogin();
        }

        private void HideTabsForLogin()
        {
            TabControl.TabPages.Remove(TabProducts);
            TabControl.TabPages.Remove(TabCategories);
            TabControl.TabPages.Remove(TabSuppliers);
        }

        private void ShowTabsAfterLogin()
        {
            TabControl.TabPages.Add(TabProducts);
            TabControl.TabPages.Add(TabCategories);
            TabControl.TabPages.Add(TabSuppliers);

            Products products = new Products();
            Functions.ShowForm(TabProducts, products);

            Categories categories = new Categories();
            Functions.ShowForm(TabCategories, categories);

            Suppliers supplies = new Suppliers();
            Functions.ShowForm(TabSuppliers, supplies);
        }

    }
}
