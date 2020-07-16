Задание.
Создать сайт с функцией сокращения ссылок URL.

1. В качестве платформы нужно использовать ASP.NET Core.
2. Разработать на языке C# функциональность для сокращения ссылок;
3. Организовать переход по короткому URL с подсчетом переходов;
4. На главной странице должен быть размещена таблица со следующими столбцами:
    5.1. Длинный URL;
    5.2. Сокращенный URL;
    5.3. Дата создания;
    5.4. Количество переходов.
6. Реализовать возможность удаления элемента из списка;
7. Создать страницу создания/редактирования элемента на которой будет использоваться функциональность для сокращения ссылок;
8. Хранение данных таблицы реализовать в базе данных MySQL (MariaDB).
9. Сокращенные ссылки URL должны создаваться так, чтобы их нельзя было предугадать.Нельзя создавать ссылки по простому последовательному арифметическому порядку.

При реализации рекомендуется:
    1. Проверить код на работоспособность
    2. Проверить случаи некорректного ввод данных
    3. Предусмотреть автоматические миграции БД