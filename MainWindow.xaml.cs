using Inventario_Hotel.Entities;
using Inventario_Hotel.Views;
using MaterialDesignThemes.Wpf;
using System;
using System.Collections.Generic;
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

namespace Inventario_Hotel
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private string userRole;
        private const string predefinedPasswordSA = "1234";
        private const string predefinedPasswordAd = "M45ft";


        public MainWindow()
        {
            InitializeComponent();
        }

        UsuarioServices services = new UsuarioServices();

        private void btnLogin_Click(object sender, RoutedEventArgs e)
        {
            string password = txtPassword.Password.ToString();

            if (password == predefinedPasswordSA)
            {
                // Simula una respuesta de login
                var response = services.LoginWithPredefinedPassword();

                if (response != null)
                {
                    userRole = response.Papel.Nombre;

                    if (userRole == "Super Administrador")
                    {
                        MessageBox.Show("Sesión Iniciada Como Super Administrador");
                        Productos menu = new Productos();
                        menu.Show();
                        Close();
                    }
                    else if (userRole == "Administrador")
                    {
                        MessageBox.Show("Sesión Iniciada Como Administrador");
                        Productos produc = new Productos();
                        produc.Show();
                        Close();
                    }
                    else
                    {
                        MessageBox.Show("Rol Desconocido o Acceso no Autorizado.");
                    }
                }
                else
                {
                    MessageBox.Show("Error en la respuesta del servicio de login.");
                }
            }
            else
            {
                MessageBox.Show("Contraseña Incorrecta");
            }
        }

        private void Border_MouseDown(object sender, MouseButtonEventArgs e)
        {
            DragMove();
        }

        private void btnExit_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void btnMinimized_Click(object sender, RoutedEventArgs e)
        {
            this.WindowState = WindowState.Minimized;
        }

        private string passwordTemp;

        private void CheckBox_Checked(object sender, RoutedEventArgs e)
        {
            // Mostrar contraseña
            passwordTemp = txtPassword.Password;
            txtPassword.Visibility = Visibility.Collapsed;

            txtTempPassword.Text = passwordTemp;
            txtTempPassword.Visibility = Visibility.Visible;
        }

        private void CheckBox_Unchecked(object sender, RoutedEventArgs e)
        {
            // Ocultar contraseña
            passwordTemp = txtTempPassword.Text;
            txtTempPassword.Visibility = Visibility.Collapsed;

            txtPassword.Password = passwordTemp;
            txtPassword.Visibility = Visibility.Visible;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            // Si está normal lo maximiza, si está maximizado lo regresa a normal.
            if (WindowState == WindowState.Normal)
                WindowState = WindowState.Maximized;
            else if (WindowState == WindowState.Maximized)
                WindowState = WindowState.Normal;
        }

    }
}
