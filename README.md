# yandexdialogscloud
yandex dialogs cloud response on dotnet core (.net core 3.1)

Опубликовать функцию в облаке:
https://cloud.yandex.ru/docs/functions/tutorials/alice-skill#create-function

формат запроса
https://yandex.ru/dev/dialogs/alice/doc/request.html

формат ответа
https://yandex.ru/dev/dialogs/alice/doc/response.html

известные проблемы: 
1)если передано в запросе nlu YANDEX.NUMBER то парсинг валится с ошибкой
https://yandex.ru/dev/dialogs/alice/doc/naming-entities.html
пофиксено в релизе <a href="https://github.com/koroteeww/yandexdialogscloud/commit/c68a3bfac6f9033bdc99630e6f6eec27db73e067">c68a3bf</a>
      
