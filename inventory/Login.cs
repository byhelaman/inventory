using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace inventory
{
    public partial class Login : Form
    {
        public Login()
        {
            InitializeComponent();
        }

        private void BtnLogin_Click(object sender, EventArgs e)
        {
            string username = TxtUser.Text.Trim();
            string password = TxtPass.Text;

            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
            {
                MessageBox.Show("Por favor ingresa tanto el nombre de usuario como la contraseña.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // Llamar al método AuthenticateUser para validar las credenciales
            bool isAuthenticated = AuthenticateUser(username, password);

            // Si la autenticación es exitosa
            if (isAuthenticated)
            {
                MessageBox.Show("¡Login exitoso!", "Bienvenido", MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.Close();
            }
            else
            {
                // Si la autenticación falla
                MessageBox.Show("Nombre de usuario o contraseña incorrectos. Intenta nuevamente.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        // Método para autenticar un usuario
        public static bool AuthenticateUser(string username, string password)
        {
            string query = "SELECT password_hash FROM users WHERE username = @username";
            SqlParameter[] parameters = {
                new SqlParameter("@username", username)
            };

            string storedHash = (string)Connect.ExecuteScalar(query, parameters);

            if (storedHash != null)
            {
                return VerifyPassword(password, storedHash);
            }
            return false;
        }

        // Método para crear un nuevo usuario
        public static int RegisterUser(string username, string password, string email, string fullName)
        {
            string passwordHash = HashPassword(password);

            // Creamos un diccionario con los datos del nuevo usuario
            var values = new Dictionary<string, object>
            {
                { "username", username },
                { "password_hash", passwordHash },
                { "email", email },
                { "full_name", fullName }
            };

            return Connect.CreateRecord("users", values);
        }

        // Método para generar un hash de la contraseña utilizando SHA256
        private static string HashPassword(string password)
        {
            using (SHA256 sha256 = SHA256.Create())
            {
                byte[] passwordBytes = Encoding.UTF8.GetBytes(password);
                byte[] hashBytes = sha256.ComputeHash(passwordBytes);
                return BitConverter.ToString(hashBytes).Replace("-", "").ToLower(); // Devuelve el hash en formato hexadecimal
            }
        }

        private static bool VerifyPassword(string password, string storedHash)
        {
            string passwordHash = HashPassword(password);
            return passwordHash == storedHash;
        }
    
    }
}
