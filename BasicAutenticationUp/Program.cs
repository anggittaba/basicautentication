using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

class User
{
    public int Id { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Name { get; set; }
    public string Username { get; set; }
    public string Password { get; set; }
}

class Program
{
    static List<User> users = new List<User>();

    static void Main(string[] args)
    {
        while (true)
        {
            Console.WriteLine($"== BASIC AUTHENTICATION ==\nTanggal dan waktu saat ini: {DateTime.Now} WIT \n");
            Console.WriteLine("1. Buat Pengguna");
            Console.WriteLine("2. Tampilkan Pengguna");
            Console.WriteLine("3. Cari Pengguna");
            Console.WriteLine("4. Masuk sebagai Pengguna");
            Console.WriteLine("5. Keluar");

            Console.Write("Input : ");
            string input = Console.ReadLine();

            switch (input)
            {
                case "1":
                    CreateUser();
                    break;
                case "2":
                    ShowUsers();
                    break;
                case "3":
                    SearchUser();
                    break;
                case "4":
                    LoginUser();
                    break;
                case "5":
                    Environment.Exit(0);
                    break;
                default:
                    Console.WriteLine("Invalid input!");
                    break;
            }

            Console.WriteLine();
        }
    }

    static void CreateUser()
    {
        Console.Write("Masukkan Nama Depan: ");
        string firstName = ValidateNameInput();

        Console.Write("Masukkan Nama Belakang: ");
        string lastName = ValidateNameInput();

        string password = ValidatePasswordInput();

        int id = users.Count + 1;
        string username = $"{firstName.Substring(0, 2)}{lastName.Substring(0, 2)}";
        string name = $"{firstName} {lastName}";

        bool isUsernameTaken = true;
        while (isUsernameTaken)
        {
            int count = users.Count(u => u.Username.StartsWith(username));
            if (count == 0)
            {
                isUsernameTaken = false;
            }
            else
            {
                username += count + 1;
            }
        }

        User user = new User { Id = id, FirstName = firstName, LastName = lastName, Name = name, Username = username, Password = password };
        users.Add(user);

        Console.WriteLine("User berhasil ditambahkan!!");
    }

    static void ShowUsers()
    {
        Console.WriteLine("== SHOW USER ==");

        foreach (User user in users)
        {
            DisplayUser(user);
        }

        Console.WriteLine();
        Console.WriteLine("Menu");
        Console.WriteLine("1. Edit User");
        Console.WriteLine("2. Delete User");
        Console.WriteLine("3. Back");

        Console.Write("Input : ");
        string input = Console.ReadLine();

        switch (input)
        {
            case "1":
                EditUser();
                break;
            case "2":
                DeleteUser();
                break;
            case "3":
                break;
            default:
                Console.WriteLine("Invalid input!");
                break;
        }
    }

    static void EditUser()
    {
        Console.Write("Masukkan ID pengguna yang akan diedit: ");
        int id;
        while (!int.TryParse(Console.ReadLine(), out id))
        {
            Console.WriteLine("Input tidak valid! Silakan masukkan angka.");
            Console.Write("Masukkan ID pengguna yang akan diedit: ");
        }

        User user = users.FirstOrDefault(u => u.Id == id);
        if (user != null)
        {
            Console.Write("Masukkan nama depan baru: ");
            string firstName = ValidateNameInput();

            Console.Write("Masukkan nama belakang baru: ");
            string lastName = ValidateNameInput();

            string password = ValidatePasswordInput();

            user.FirstName = firstName;
            user.LastName = lastName;
            user.Name = $"{firstName} {lastName}";
            user.Password = password;

            string newUsername = $"{firstName.Substring(0, 2)}{lastName.Substring(0, 2)}";
            bool isUsernameTaken = true;
            while (isUsernameTaken)
            {
                int count = users.Count(u => u.Username.StartsWith(newUsername));
                if (count == 0)
                {
                    isUsernameTaken = false;
                }
                else
                {
                    newUsername += count + 1;
                }
            }

            user.Username = newUsername;

            Console.WriteLine("Pengguna berhasil diperbarui!");
        }
        else
        {
            Console.WriteLine("Pengguna tidak ditemukan!");
        }
    }

    static void DeleteUser()
    {
        Console.Write("Masukkan ID pengguna yang akan dihapus: ");
        int id = int.Parse(Console.ReadLine());

        User user = users.FirstOrDefault(u => u.Id == id);
        if (user != null)
        {
            users.Remove(user);
            Console.WriteLine("Pengguna berhasil dihapus!");
        }
        else
        {
            Console.WriteLine("Pengguna tidak ditemukan!");
        }
    }

    static void SearchUser()
    {
        Console.WriteLine("== Cari Akun ==");

        Console.Write("Masukkan Nama : ");
        string name = Console.ReadLine();

        var matchingUsers = users.Where(u => u.Name.ToLower().Contains(name.ToLower()));

        if (matchingUsers.Any())
        {
            foreach (User user in matchingUsers)
            {
                DisplayUser(user);
            }
        }
        else
        {
            Console.WriteLine("User tidak ditemukan!");
        }
    }

    static void LoginUser()
    {
        Console.WriteLine("== LOGIN ==");

        User user = null;
        while (user == null)
        {
            Console.Write("USERNAME : ");
            string username = Console.ReadLine();

            Console.Write("PASSWORD : ");
            string password = Console.ReadLine();

            user = users.FirstOrDefault(u => u.Username == username && u.Password == password);
            if (user == null)
            {
                Console.WriteLine("Username atau password tidak valid!");
                Console.WriteLine("Tekan 'm' untuk kembali ke menu utama atau tombol lain untuk mencoba lagi...");
                ConsoleKeyInfo key = Console.ReadKey();
                if (key.KeyChar == 'm')
                {
                    return;
                }
            }

        }

        Console.WriteLine($"Welcome, {user.Name}!");

        Console.WriteLine("Tekan tombol apa saja untuk melanjutkan...");
        Console.ReadKey();
    }
    static void DisplayUser(User user)
    {
        Console.WriteLine("========================");
        Console.WriteLine($"ID       : {user.Id}");
        Console.WriteLine($"Nama     : {user.Name}");
        Console.WriteLine($"Username : {user.Username}");
        Console.WriteLine($"Password : {user.Password}");
        Console.WriteLine("========================");
    }
    static string ValidateNameInput()
    {
        string input = Console.ReadLine();

        while (string.IsNullOrEmpty(input) || !Regex.IsMatch(input, @"^[a-zA-Z]+$") || input.Length < 2)
        {
            Console.WriteLine("Input tidak valid! Nama harus berisi huruf saja dan minimal 2 karakter.");
            Console.Write("Masukkan nama: ");
            input = Console.ReadLine();
        }

        return input;
    }

    static string ValidatePasswordInput()
    {
        string password;
        while (true)
        {
            Console.Write("Masukkan password (minimal 8 karakter dengan setidaknya satu huruf besar, satu huruf kecil, dan satu angka): ");
            password = Console.ReadLine();

            if (password.Length < 8)
            {
                Console.WriteLine("Password harus memiliki setidaknya 8 karakter!");
            }
            else if (!password.Any(char.IsUpper))
            {
                Console.WriteLine("Password harus memiliki setidaknya satu huruf besar!");
            }
            else if (!password.Any(char.IsLower))
            {
                Console.WriteLine("Password harus memiliki setidaknya satu huruf kecil!");
            }
            else if (!password.Any(char.IsDigit))
            {
                Console.WriteLine("Password harus memiliki setidaknya satu angka!");
            }
            else
            {
                break;
            }
        }

        return password;
    }
}