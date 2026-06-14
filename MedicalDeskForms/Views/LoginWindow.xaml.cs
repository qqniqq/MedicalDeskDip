using System.Windows;
using System.Windows.Input;
using MedicalDeskLib.Services;

namespace MedicalDeskForms.Views;

public partial class LoginWindow : Window
{
    private readonly AuthService _authService =
        new();

    public LoginWindow()
    {
        InitializeComponent();

        KeyDown += LoginWindow_KeyDown;
    }

    private void LoginWindow_KeyDown(
        object? sender,
        KeyEventArgs e)
    {
        if (e.Key == Key.Enter)
        {
            btnLogin_Click(
                btnLogin,
                new RoutedEventArgs());
        }
    }

    private void btnLogin_Click(
        object sender,
        RoutedEventArgs e)
    {
        string login =
            txtLogin.Text.Trim();

        string password =
            txtPassword.Password;

        var user =
            _authService.Login(
                login,
                password);

        if (user == null)
        {
            MessageBox.Show(
                "Неверный логин или пароль");

            return;
        }

        MainWindow window =
            new();

        window.Show();

        Close();
    }
}