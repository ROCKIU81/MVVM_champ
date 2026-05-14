# World Cup MVVM - Приложение для управления данными чемпионатов

Приложение WPF на C# для управления данными о чемпионатах, матчах, игроках и тренерах с использованием паттерна MVVM и PostgreSQL.

## Структура проекта

```
MVVM_Champ/
├── models/                 # Модели данных и интерфейсы Repository
│   ├── Country.cs
│   ├── Person.cs
│   ├── Championship.cs
│   ├── Match.cs
│   ├── Goal.cs
│   ├── GoalType.cs
│   ├── PlayerSquad.cs
│   ├── CoachMatch.cs
│   └── I*Repository.cs     # Интерфейсы для Repository
├── services/               # Реализация Repository для PostgreSQL
│   ├── CountryRepository.cs
│   ├── PersonRepository.cs
│   ├── ChampionshipRepository.cs
│   ├── MatchRepository.cs
│   ├── GoalRepository.cs
│   ├── GoalTypeRepository.cs
│   ├── PlayerSquadRepository.cs
│   └── CoachMatchRepository.cs
├── viewmodels/             # ViewModel классы
│   ├── ViewModelBase.cs
│   ├── RelayCommand.cs
│   ├── CountryViewModel.cs
│   ├── PersonViewModel.cs
│   ├── ChampionshipViewModel.cs
│   ├── MatchViewModel.cs
│   ├── GoalViewModel.cs
│   ├── GoalTypeViewModel.cs
│   ├── PlayerSquadViewModel.cs
│   ├── CoachMatchViewModel.cs
│   └── MainViewModel.cs
├── view/                   # XAML представления
│   └── MainWindow.xaml
├── App.xaml.cs            # Инициализация приложения
├── ServiceContainer.cs    # DI контейнер
└── database_init.sql      # SQL скрипт для создания БД
```

## Требования

- .NET 9.0 или выше
- PostgreSQL 12 или выше
- Visual Studio 2022 или выше (опционально)

## Установка

### 1. Установка зависимостей

Проект использует следующие NuGet пакеты:
- `Npgsql` - драйвер для PostgreSQL
- `Microsoft.Extensions.DependencyInjection` - DI контейнер

Они автоматически установятся при восстановлении пакетов.

### 2. Создание БД

1. Откройте PostgreSQL и создайте новую БД:
```sql
CREATE DATABASE world_cup;
```

2. Выполните SQL скрипт `database_init.sql`:
```bash
psql -U postgres -d world_cup -f database_init.sql
```

### 3. Настройка строки подключения

Отредактируйте `App.xaml.cs` и обновите строку подключения:

```csharp
string connectionString = "Host=localhost;Port=5432;Database=world_cup;Username=postgres;Password=your_password";
```

Замените:
- `your_password` на пароль вашего пользователя PostgreSQL
- `localhost` на адрес сервера (если необходимо)
- `5432` на порт PostgreSQL (если используется другой)

### 4. Запуск приложения

```bash
dotnet run
```

## Использование

Приложение имеет табулированный интерфейс с 8 вкладками:

1. **Страны** - управление странами
2. **Чемпионаты** - управление чемпионатами
3. **Матчи** - управление матчами
4. **Люди** - управление игроками и тренерами
5. **Голы** - управление забитыми голами
6. **Типы голов** - управление типами голов
7. **Состав игроков** - управление составом игроков на матч
8. **Тренеры матчей** - управление тренерами на матч

### Основные операции

- **Загрузить** - загрузить данные из БД
- **Удалить** - удалить выбранную запись
- Выбор записи в таблице для работы с ней

## Архитектура

### MVVM паттерн

- **Model** - классы в папке `models/` (Country, Person, Match и т.д.)
- **View** - XAML файлы в папке `view/`
- **ViewModel** - классы в папке `viewmodels/` для управления логикой

### Repository паттерн

Каждая сущность имеет:
- Интерфейс `I*Repository` в папке `models/`
- Реализацию в папке `services/` для работы с PostgreSQL

### Dependency Injection

Используется `Microsoft.Extensions.DependencyInjection` для управления зависимостями. Инициализация происходит в `ServiceContainer.cs`.

## Примеры SQL запросов

### Получить все страны
```sql
SELECT id, название FROM страна ORDER BY id;
```

### Получить матчи чемпионата
```sql
SELECT * FROM матч WHERE id_чемпионата = 1;
```

### Получить голы матча
```sql
SELECT * FROM гол WHERE id_матча = 1 ORDER BY минута;
```

## Расширение функциональности

### Добавление новой операции

1. Добавьте метод в интерфейс `I*Repository`
2. Реализуйте метод в `*Repository`
3. Добавьте команду в `*ViewModel`
4. Добавьте кнопку в XAML

Пример для добавления страны:

```csharp
// В CountryViewModel
public ICommand AddCommand { get; }

public CountryViewModel(ICountry countryService)
{
    AddCommand = new RelayCommand(() => AddCountry());
}

private void AddCountry()
{
    // Логика добавления
}
```

## Решение проблем

### Ошибка подключения к БД
- Проверьте, запущен ли PostgreSQL
- Проверьте строку подключения в `App.xaml.cs`
- Убедитесь, что БД `world_cup` создана

### Таблицы не найдены
- Выполните SQL скрипт `database_init.sql`
- Проверьте, что вы подключены к правильной БД

### Данные не загружаются
- Нажмите кнопку "Загрузить" на нужной вкладке
- Проверьте логи ошибок в Output окне Visual Studio

## Лицензия

MIT
