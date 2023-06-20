# ProjektNaZajeciaDotNet

### Uruchamianie
1. W pierwszej kolejności należy sklonować repozytorium
2. Otworzyć solucję
    1. Utworzyć bazę danych MSSQL LocalDb
    2. Podstawić jej connection string w pliku ```appsettings.json``` (pole DefaultConnection)
3. W Nuget Package Manager wpisać Update-Database
4. **OPCJONALNE** - Aby móc odbierać maile (np. z resetowaniem hasła lub potwierdzeniem konta) należy uruchomić dowolny serwer smtp (ja skorzystałem z [Papercut](https://github.com/ChangemakerStudios/Papercut-SMTP)). Konfigurację hosta i portu smtp oraz adresu email z którego wysyłane są wiadomości, można skonfigurować w pliku ```program.cs``` konfigurując opcje ```smtpOptions```.
5. Uruchomić aplikację **bez debugowania** - w innym wypadku aplikacja może się wyłączyć przy zamknięciu okna wyboru pliku. Skąd debugger wie, że jakieś okno w przeglądarce zostało otwarte? Nie wiem


Przy pierwszym uruchomieniu apliakacji, baza danych zostanie zaseedowana.

Dane domyślnie utworzonych użytkowników:

| Login                    | Hasło      |
|--------------------------|------------|
| admin@skupzywca.pl       | Haslo1234! |
| user@hehe.pl             | Haslo1234! |
| vateusz@ojciecmateusz.pl | Haslo1234! |

Pierwsze konto jest kontem administratora.
